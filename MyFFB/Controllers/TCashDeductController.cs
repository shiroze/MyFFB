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
    public class TCashDeductController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, capprove;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TCashAdvance";
        //DateTime date1 = DateTime.Today;


        public TCashDeductController(RequestHandler requestHandler)
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

        // GET: TCashDeduct
        public ActionResult Index(string CashNo, string TanggalTimbang)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (TempData["Message"] != null)
                    ViewData["Message"] = TempData["Message"].ToString();
                ViewBag.CashNo = CashNo;
                ViewBag.TanggalTimbang = TanggalTimbang;
                return View("Index");
            }
        }

        public ActionResult GridMainTable(string TanggalTimbang, string CashNo)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (TempData["Message"] != null)
                    ViewData["Message"] = TempData["Message"].ToString();

                var model = SCashDeduct.CashDeduct(econstr, jconstr).GetListFFB(false, TanggalTimbang, CashNo);
                return PartialView("_MainTable", model);
            }
        }
        public ActionResult GetTotalMain(string CashNo)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                var model = SCashDeduct.CashDeduct(econstr, jconstr).GetTotalMain(CashNo);
                //Content(JsonConvert.SerializeObject(model));
                return Content(model);
            }
        }

        // GET: TCashDeduct/View/5
        public ActionResult Edit(string p_ticket)
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
                    var model = SCashDeduct.CashDeduct(econstr, jconstr).FindFFB(p_ticket);
                    return PartialView("_Edit", model);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_FFB obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SCashDeduct.CashDeduct(econstr, jconstr).Edit(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index", new { PMKSID = obj.PMKSID, SupplierID = obj.Supplier, TanggalTimbang = obj.TanggalTimbang, CashNo = obj.Number });
        }

        //public ActionResult CalculateCashDeduct(string p_tglTimbang)
        //{
        //    int jlh_calculate = 0;
        //    try
        //    {
        //        // TODO: Add insert logic here
        //        int AccessID = juserid;
        //        jlh_calculate=SCashDeduct.CashDeduct(econstr, jconstr).CalculateCashDeduct(AccessID, p_tglTimbang);
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