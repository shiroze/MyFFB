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
    public class TCashAdvanceController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, capprove;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TCashAdvance";
        //DateTime date1 = DateTime.Today;


        public TCashAdvanceController(RequestHandler requestHandler)
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

        public List<T_CashAdvance> PopulateListWeek()
        {
            //T_PMKS obj = new T_PMKS();
            var result = SCashAdvance.CashAdvance(econstr, jconstr).GetListWeek();
            return result;
        }

        // GET: TCashAdvance
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
        public ActionResult GridMainTable(string p_Period)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (TempData["Message"] != null)
                    ViewData["Message"] = TempData["Message"].ToString();

                var model = SCashAdvance.CashAdvance(econstr, jconstr).GetListCashAdvance(false, p_Period,juserid);
                return PartialView("_MainTable", model);
            }
        }


        // GET: TCashAdvance/View/5
        public ActionResult ViewDet(string p_CashNo)
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
                    return PartialView("_ViewDet");
                }
                else
                {
                    var model = SCashAdvance.CashAdvance(econstr, jconstr).FindCashAdvance(p_CashNo);
                    return PartialView("_ViewDet", model);
                }
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
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'Create'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Create");
                }
                else
                {
                    ViewBag.DListWeek = PopulateListWeek();
                    return PartialView("_Create");
                }
            }
        }

        // POST: TCashAdvance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_CashAdvance obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SCashAdvance.CashAdvance(econstr, jconstr).Add(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string p_CashNo)
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
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'Edit'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Edit");
                }
                else
                {
                    ViewBag.DListWeek = PopulateListWeek();
                    var model = SCashAdvance.CashAdvance(econstr, jconstr).FindCashAdvance(p_CashNo);
                    return PartialView("_Edit", model);
                }
            }
        }

        // POST: TCashAdvance/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_CashAdvance obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SCashAdvance.CashAdvance(econstr, jconstr).Edit(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string p_CashNo)
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
                    ViewBag.DListWeek = PopulateListWeek();
                    var model = SCashAdvance.CashAdvance(econstr, jconstr).FindCashAdvance(p_CashNo);
                    return PartialView("_Delete", model);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_CashAdvance obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SCashAdvance.CashAdvance(econstr, jconstr).Delete(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        //public ActionResult CalculateCashAdvance(string p_tglTimbang)
        //{
        //    int jlh_calculate = 0;
        //    try
        //    {
        //        // TODO: Add insert logic here
        //        int AccessID = juserid;
        //        jlh_calculate=SCashAdvance.CashAdvance(econstr, jconstr).CalculateCashAdvance(AccessID, p_tglTimbang);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Message"] = ex.Message.ToString();
        //    }
        //    //return RedirectToAction("Index");
        //    return Content(jlh_calculate.ToString());
        //}


    }
}