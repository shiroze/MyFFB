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
    public class TFFBController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, capprove;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TFFB";
        //DateTime date1 = DateTime.Today;


        public TFFBController(RequestHandler requestHandler)
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

        // GET: TFFB
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
                var modal = SPMKS.PMKS(econstr, jconstr).GetListPMKS(null, juserid);
                ViewBag.DListPMKS = modal;
                return View(null);
            }
        }
        public ActionResult GridMainTable(string p_tglTimbang, string fil_pmks)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (TempData["Message"] != null)
                    ViewData["Message"] = TempData["Message"].ToString();
                var TotalTRX = SFFB.FFB(econstr, jconstr).TotalFFBHarian(juserid, p_tglTimbang);
                ViewBag.TotalTRX = TotalTRX;
                var model = SFFB.FFB(econstr, jconstr).GetListFFB(false, p_tglTimbang, juserid, fil_pmks);
                return PartialView("_MainTable", model);
            }
        }


        // GET: TFFB/View/5
        public ActionResult View(string p_tglTimbang, string p_ticket)
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
                    var model = SFFB.FFB(econstr, jconstr).FindFFB(p_tglTimbang, p_ticket);
                    return PartialView("_View", model);
                }
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CalculateFFB(string p_tglTimbang)
        {
            int jlh_calculate = 0;
            bool status = false;
            string pesan = "";
            try
            {
                
                if (!cedit)
                {
                    pesan = "Anda tidak memiliki hak untuk 'CALCULATE'.";
                    return Content(JsonConvert.SerializeObject(new { Status = status, Pesan = pesan }));
                }
                // TODO: Add insert logic here
                int AccessID = juserid;
                jlh_calculate=SFFB.FFB(econstr, jconstr).CalculateFFB(AccessID, p_tglTimbang);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }

            status = true;
            pesan = "Total : " + jlh_calculate.ToString() + " Transaction CALCULATED.";
            return Content(JsonConvert.SerializeObject(new { Status = status, Pesan = pesan }));
        }

        public ActionResult DownloadFFB(string p_tglTimbang)
        {
            int jlh_calculate = 0;
            bool status = false;
            string pesan = "";
            try
            {
                if (!cadd)
                {
                    pesan = "Anda tidak memiliki hak untuk 'DOWNLOAD/Tarik Data'.";
                    return Content(JsonConvert.SerializeObject(new { Status = status, Pesan = pesan }));
                }
                // TODO: Add insert logic here
                int AccessID = juserid;
                jlh_calculate = SFFB.FFB(econstr, jconstr).DownloadFFB(AccessID, p_tglTimbang);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }

            status = true;
            pesan = "Total : " + jlh_calculate.ToString() + " Data Downloaded.";
            return Content(JsonConvert.SerializeObject(new { Status = status, Pesan = pesan }));
        }

        public ActionResult Delete(string p_tglTimbang, string p_ticket)
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
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'HAPUS'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Delete");
                }
                else
                {
                    var model = SFFB.FFB(econstr, jconstr).FindFFB(p_tglTimbang, p_ticket);
                    return PartialView("_Delete", model);
                }
            }
        }

        // POST: TCompetitor/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_FFB obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SFFB.FFB(econstr, jconstr).Delete(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Post(string p_tglTimbang, string p_ticket)
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
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'POST'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Post");
                }
                else
                {
                    var model = SFFB.FFB(econstr, jconstr).FindFFB(p_tglTimbang, p_ticket);
                    return PartialView("_Post", model);
                }
            }
        }

        // POST: TCompetitor/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(T_FFB obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SFFB.FFB(econstr, jconstr).Post(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Unpost(string p_tglTimbang, string p_ticket)
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
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'UNPOST'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Unpost");
                }
                else
                {
                    var model = SFFB.FFB(econstr, jconstr).FindFFB(p_tglTimbang, p_ticket);
                    return PartialView("_Unpost", model);
                }
            }
        }

        // POST: TCompetitor/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unpost(T_FFB obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SFFB.FFB(econstr, jconstr).Unpost(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

    }
}