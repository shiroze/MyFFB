using System;
using MyAttd.Helpers;
using MyAttd.Models;
using MyAttd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Reporting.NETCore;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;

namespace MyAttd.Controllers
{
    public class TIncentiveController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, capprove;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TIncentive";
        public string WebRootPath { get; set; }
        //DateTime date1 = DateTime.Today;

        private readonly IWebHostEnvironment _env;

        public TIncentiveController(RequestHandler requestHandler, IWebHostEnvironment env)
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
        

        // GET: TIncentive
        public ActionResult Index(string p_date)
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
        public ActionResult GridMainTable(string Periode)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (TempData["Message"] != null)
                    ViewData["Message"] = TempData["Message"].ToString();

                var model = SIncentive.Incentive(econstr, jconstr).GetListIncentive(false,Periode,juserid);
                return PartialView("_MainTable", model);
            }
        }

        //GET: TIncentive/Create
        public ActionResult Create()
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (!cadd)
                {
                    ViewBag.MessageType = "Attention!";
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'TAMBAH'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Create");
                }
                else
                {
                    return PartialView("_Create");
                }
            }
        }

        // POST: TIncentive/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_Incentive obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SIncentive.Incentive(econstr, jconstr).Add(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }



        // GET: TIncentive/View/5
        public ActionResult ViewDet(string IncentiveID)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (!cedit)
                {
                    ViewBag.MessageType = "Attention!";
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'VIEW'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_View");
                }
                else
                {
                    var model = SIncentive.Incentive(econstr, jconstr).FindIncentive(IncentiveID);
                    return PartialView("_View", model);
                }
            }
        }

        public ActionResult Delete(string IncentiveID)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (!cdelete)
                {
                    ViewBag.MessageType = "Attention!";
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'DELETE'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Delete");
                }
                else
                {
                    var model = SIncentive.Incentive(econstr, jconstr).FindIncentive(IncentiveID);
                    return PartialView("_Delete", model);
                }
            }
        }

        // POST: TIncentive/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_Incentive obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SIncentive.Incentive(econstr, jconstr).Delete(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Print(string IncentiveID)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (!cedit)
                {
                    ViewBag.MessageType = "Attention!";
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'VIEW'.";
                    ViewBag.MessageCSS = "text-info";
                    return (null);
                }
                else
                {
                    /**
                     * Generate Report
                     */

                    string contentRootPath = _env.ContentRootPath;
                    string path = contentRootPath + "//Report//Rpt_Incentive.rdlc";
                    using var rs = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
                    var model = SIncentive.Incentive(econstr, jconstr).PrintIncentive(juserid, IncentiveID);
                    var parameters = new ReportParameterCollection();

                    ModFunction.genRptParam(path, model, "PDF", parameters);

                    return File(ModFunction.genRptParam(path, model, "PDF", parameters), "application/pdf");
                }
            }
        }

        public ActionResult Approve(string IncentiveID)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (!capprove)
                {
                    ViewBag.MessageType = "Attention!";
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'Approve / UnApprove'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Approve");
                }
                else
                {
                    var model = SIncentive.Incentive(econstr, jconstr).FindIncentive(IncentiveID);
                    return PartialView("_Approve", model);
                }
            }
        }

        // POST: TIncentive/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(T_Incentive obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SIncentive.Incentive(econstr, jconstr).Approve(obj, true);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

    }
}