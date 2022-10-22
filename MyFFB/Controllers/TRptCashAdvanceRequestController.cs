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
using System.Text;
using Microsoft.Net.Http.Headers;
using System.Globalization;
//using System.Globalization;

namespace MyAttd.Controllers
{
    public class TRptCashAdvanceRequestController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, capprove;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TRptCashAdvanceRequest";
        //DateTime date1 = DateTime.Today;
        private readonly IWebHostEnvironment _env;


        public TRptCashAdvanceRequestController(RequestHandler requestHandler, IWebHostEnvironment env)
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

 

        // GET: TRptCashAdvanceRequest
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

        [HttpPost]
        public ActionResult Print([FromBody] T_Report.RptCashAdvanceRequest ParJson)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                string contentRootPath = _env.ContentRootPath;
                string path = contentRootPath + "//Report//Rpt_CARequest.rdlc";
                var model = SReport.Report(econstr, jconstr).RptCashAdvanceRequest(juserid, ParJson);
                var parameters = new ReportParameterCollection();

                DateTime now = DateTime.Parse(ParJson.dtF);
                string Periode = now.ToString("MMMM") + " " + now.Year;

                Int64 TotalPriceInt = 0;
                if (model.Rows.Count >= 1)
                {
                    TotalPriceInt = Int64.Parse(model.Compute("Sum(Amount)", string.Empty).ToString());
                }
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                string TotTerbilang = MyAttd.Controllers.HomeController.Terbilang1(TotalPriceInt).Replace("  ", " ") + " Rupiah";
                TotTerbilang = textInfo.ToTitleCase(TotTerbilang);

                string Par1 = "IDR " + TotalPriceInt.ToString("0,0", CultureInfo.InvariantCulture) + " (" + TotTerbilang.Trim() + ")";
                parameters.Add(new ReportParameter("Par1", Par1));

                string Par2 = "Di berikan pada Hari Jumat dan akan dipotong pada Hari Senin, Minggu Berikutnya.";
                if (ParJson.type == "Bulanan")
                {
                    Par2 = "Di berikan pada awal bulan " + Periode + " dan akan dipotong pada akhir bulan " + Periode;
                }
                parameters.Add(new ReportParameter("Par2", Par2));

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