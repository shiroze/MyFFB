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

namespace MyAttd.Controllers
{
    public class TRptCashAdvanceController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, capprove;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TRptCashAdvance";
        //DateTime date1 = DateTime.Today;
        private readonly IWebHostEnvironment _env;


        public TRptCashAdvanceController(RequestHandler requestHandler, IWebHostEnvironment env)
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

 

        // GET: TRptCashAdvance
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
        public ActionResult Print([FromBody] T_Report.RptCashAdvance ParJson)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                string contentRootPath = _env.ContentRootPath;
                string path = contentRootPath + "//Report//Rpt_CashAdvance.rdlc";
                var model = SReport.Report(econstr, jconstr).RptCashAdvance(juserid, ParJson);
                var parameters = new ReportParameterCollection();
                parameters.Add(new ReportParameter("Par1", "Cash Advance"));
                parameters.Add(new ReportParameter("Par2", "Dari : " + ParJson.dtF + " s/d " + ParJson.dtT));

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