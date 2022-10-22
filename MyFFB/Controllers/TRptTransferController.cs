using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAttd.Helpers;
using MyAttd.Models;
using MyAttd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Microsoft.Reporting.NETCore;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace MyAttd.Controllers
{
    public class TRptTransferController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, capprove;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TRptTransfer";
        //DateTime date1 = DateTime.Today;
        private readonly IWebHostEnvironment _env;


        public TRptTransferController(RequestHandler requestHandler, IWebHostEnvironment env)
        {
            _env = env;
            _requestHandler = requestHandler;

            jconstr = _requestHandler.GetSessionConStrRequest();
            jroleid = _requestHandler.GetSessionRoleIDRequest();
            juserid = _requestHandler.GetSessionUserIDRequest();
            econstr = Startup.Configuration.GetSection("ConnectionStrings").GetSection(jconstr).Value;
            if (jroleid != 0)
            {
                roledetobj = SRoleDet.RoleDet(econstr, jconstr).FindRoleDet(jroleid, null, classname);
                if (roledetobj != null)
                {
                    cadd = roledetobj.FCAdd;
                    cedit = roledetobj.FCEdit;
                    cdelete = roledetobj.FCDelete;
                    capprove = roledetobj.FCApprove;
                }
            }

        }



        // GET: TRptTransfer
        public ActionResult Index()
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (TempData["Message"] != null)
                    ViewData["Message"] = TempData["Message"].ToString();
                return View(null);
            }
        }

        public ActionResult Print([FromBody] T_Report.RptTransfer ParJson)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                string contentRootPath = _env.ContentRootPath;
                string path = contentRootPath + "//Report//Rpt_MenuRole.rdlc";
                var model = SReport.Report(econstr, jconstr).RptTransfer(juserid, ParJson);
                var parameters = new ReportParameterCollection();

                Int64 TotalPriceInt = 0;

                if (model.Rows.Count >= 1)
                {
                    TotalPriceInt = Int64.Parse(model.Compute("Sum(TotalPembayaran)", string.Empty).ToString());
                }
                string TotTerbilang = MyAttd.Controllers.HomeController.Terbilang1(TotalPriceInt).Replace("  ", " ") + " Rupiah";
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                TotTerbilang = textInfo.ToTitleCase(TotTerbilang).Trim();

                // string TotTerbilang = MyAttd.Controllers.HomeController.Terbilang1(TotalPriceInt).Replace("  ", " ") + " Rupiah";
                //TotTerbilang = textInfo.ToTitleCase(TotTerbilang);

                parameters.Add(new ReportParameter("Par1", TotTerbilang));

                if (ParJson.typeFile == "PDF")
                {
                    return File(ModFunction.genRptParam(path, model, ParJson.typeFile, parameters), "application/pdf");
                }
                else
                {
                    return File(ModFunction.genRptParam(path, model, ParJson.typeFile, parameters), "application/msexcel");
                }
            }
        }


    }
}