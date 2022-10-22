using System;
using MyAttd.Helpers;
using MyAttd.Models;
using MyAttd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Reporting.NETCore;
//using System.Globalization;

namespace MyAttd.Controllers
{
    public class TRptPriceIndicatorController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, capprove;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TRptPriceIndicator";
        //DateTime date1 = DateTime.Today;
        private readonly IWebHostEnvironment _env;


        public TRptPriceIndicatorController(RequestHandler requestHandler, IWebHostEnvironment env)
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



        // GET: TRptPriceIndicator
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
        public ActionResult Print([FromBody] T_Report.RptPriceIndicator ParJson)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                //var coba = DateTime.ParseExact("22/10/2019", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime now = DateTime.Parse(ParJson.dtT);
                string Periode = "Periode Report : " + now.ToString("MMMM") + " " + now.Year;
                //New Add
                DateTime current = DateTime.Now;
                string ReportDate = "Tanggal Penarikan: " + current.ToString();

                /**
                 * Generate Report
                 */
                string contentRootPath = _env.ContentRootPath;
                string path = contentRootPath + "//Report//Rpt_PriceIndicator.rdlc";
                var model = SReport.Report(econstr, jconstr).RptPriceIndicator(juserid, ParJson);
                var parameters = new ReportParameterCollection();
                parameters.Add(new ReportParameter("Par1", "Price Indicator"));
                parameters.Add(new ReportParameter("Par2", Periode));
                parameters.Add(new ReportParameter("Par3", ReportDate));

                if (ParJson.typeFile == "PDF")
                {
                    return File(ModFunction.genRptParam(path, model, ParJson.typeFile, parameters), "application/pdf");
                } else
                {
                    return File(ModFunction.genRptParam(path, model, ParJson.typeFile, parameters), "application/msexcel");
                }

                ////var parameter = JsonConvert.SerializeObject(ParJson);
                //string contentRootPath = _env.ContentRootPath;
                //string path = contentRootPath + "//Report//Rpt_PriceIndicator.rdlc";
                //Dictionary<string, string> parameters = new Dictionary<string, string>();
                //parameters.Add("Par1", "Price Indicator");
                //var model = SReport.Report(econstr, jconstr).RptPriceIndicator(juserid, ParJson);
                //parameters.Add("Par2", Periode);
                //parameters.Add("Par3", ReportDate);
                //LocalReport localReport = new LocalReport(path);
                //localReport.AddDataSource("DataSet1", model);

                //if (ParJson.typeFile == "PDF")
                //{
                //    var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);
                //    return File(result.MainStream, "application/pdf");
                //}
                //else
                //{
                //    var result = localReport.Execute(RenderType.Excel, extension, parameters, mimtype);
                //    return File(result.MainStream, "application/msexcel");
                //}
            }
        }

    }
}