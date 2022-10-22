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

namespace MyAttd.Controllers
{
    public class TSetReportController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, capprove;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TSetReport";
        //DateTime date1 = DateTime.Today;


        public TSetReportController(RequestHandler requestHandler)
        {
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
        public List<T_AreaRegional> PopulateRegion()
        {
            var result = SSAPVendor.SAPVendor(econstr, jconstr).GetRegion();
            return result;
        }


        // GET: TSetReport
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
        public ActionResult GridMainTable()
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (TempData["Message"] != null)
                    ViewData["Message"] = TempData["Message"].ToString();

                var model = SSetReport.SetReport(econstr, jconstr).GetListSetReport(false, juserid);
                return PartialView("_MainTable", model);
            }
        }

        public ActionResult View1(string ReportID)
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
                    var model = SSetReport.SetReport(econstr, jconstr).FindSetReport(juserid, ReportID);
                    return PartialView("_View", model);
                }
            }
        }
        //[HttpPost]
        public ActionResult Delete(string ReportID)
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
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'Delete'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Delete");
                }
                else
                {
                    var model = SSetReport.SetReport(econstr, jconstr).FindSetReport(juserid, ReportID);
                    return PartialView("_Delete", model);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_SetReport obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SSetReport.SetReport(econstr, jconstr).Delete(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

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
                    ViewBag.DListRegional = PopulateRegion();
                    return PartialView("_Create");
                }
            }
        }

        // POST: T_Holiday/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_SetReport obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SSetReport.SetReport(econstr, jconstr).Add(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string ReportID)
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
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'EDIT'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_EDit");
                }
                else
                {
                    var model = SSetReport.SetReport(econstr, jconstr).FindSetReport(juserid, ReportID);
                    return PartialView("_Edit", model);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_SetReport obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SSetReport.SetReport(econstr, jconstr).Edit(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }


    }
}