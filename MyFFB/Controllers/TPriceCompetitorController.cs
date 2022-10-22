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
    public class TPriceCompetitorController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, capprove;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TPriceCompetitor";
        //DateTime date1 = DateTime.Today;


        public TPriceCompetitorController(RequestHandler requestHandler)
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

        // GET: TPriceCompetitor
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
        public ActionResult GridMainTable(string Date)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (TempData["Message"] != null)
                    ViewData["Message"] = TempData["Message"].ToString();

                var model = SPriceCompetitor.PriceCompetitor(econstr, jconstr).GetListPriceCompetitor(false, Date,juserid);
                return PartialView("_MainTable", model);
            }
        }
        
        public ActionResult TarikData(string Date)
        {
            try
            {
                if (!cadd)
                {
                    return Content("Maaf anda tidak memiliki Hak Akses 'Tarik Data'.");
                }
                else
                {
                    SPriceCompetitor.PriceCompetitor(econstr, jconstr).TarikData(Date, juserid);
                    return Content("Proses Tarik Data Sukses.");
                }
                
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
                return Content("Proses Tarik Data Gagal : \n" + ex.Message.ToString());
                //return RedirectToAction("Index");
            }
            
            
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
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'ADD'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Create");
                }
                else
                {
                    return PartialView("_Create");
                }
            }
        }

        // POST: TPriceCompetitor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_PriceCompetitor obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SPriceCompetitor.PriceCompetitor(econstr, jconstr).Add(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TPriceCompetitor/View/5
        public ActionResult ViewDet(string Date, string PMKSID, string CompetitorName)
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
                    var model = SPriceCompetitor.PriceCompetitor(econstr, jconstr).FindPriceCompetitor(Date,PMKSID,CompetitorName);
                    return PartialView("_View", model);
                }
            }
        }
        public ActionResult Edit(string Date, string PMKSID, string CompetitorName)
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
                    return PartialView("_Edit");
                }
                else
                {
                    var model = SPriceCompetitor.PriceCompetitor(econstr, jconstr).FindPriceCompetitor(Date, PMKSID, CompetitorName);
                    return PartialView("_Edit", model);
                }
            }
        }

        // POST: TPriceCompetitor/EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_PriceCompetitor obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SPriceCompetitor.PriceCompetitor(econstr, jconstr).Edit(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string Date, string PMKSID, string CompetitorName)
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
                    var model = SPriceCompetitor.PriceCompetitor(econstr, jconstr).FindPriceCompetitor(Date, PMKSID, CompetitorName);
                    return PartialView("_Delete", model);
                }
            }
        }

        // POST: TPriceCompetitor/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_PriceCompetitor obj)
        {
            try
            {
                // TODO: Add Delete logic here
                obj.AccessID = juserid;
                SPriceCompetitor.PriceCompetitor(econstr, jconstr).Delete(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }
    }
}