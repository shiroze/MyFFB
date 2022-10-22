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
    public class TPMKSController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, capprove;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TPMKS";
        public TPMKSController(RequestHandler requestHandler)
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

        public List<T_AreaRegional> PopulateAreaRegional()
        {
            //T_PMKS obj = new T_PMKS();
            var result = SAreaRegional.AreaRegional(econstr, jconstr).GetListAreaRegional(false);
            return result;
        }

        // GET: TPMKS
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
                var model = SPMKS.PMKS(econstr, jconstr).GetListPMKS(false, juserid);
                //var test = GlobalVariable.GV_AreaOperational;
                //string json = JsonConvert.SerializeObject(model);
                return View(model);
            }
        }

        //GET: TPMKS/Create
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

                    ViewBag.DListAreaOperational = PopulateAreaRegional();
                    return PartialView("_Create");
                    //return PartialView("_Create");
                }
            }
        }

        // POST: TPMKS/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_PMKS obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SPMKS.PMKS(econstr, jconstr).Add(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult View1(string p_PMKSDid)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                ViewBag.DListAreaOperational = PopulateAreaRegional();
                var model = SPMKS.PMKS(econstr, jconstr).FindPMKS(p_PMKSDid);
                return PartialView("_View", model);
            }
        }


        // GET: TPMKS/Edit/5
        //public ActionResult Edit(string AreaOperational, string Regional, string PMKSID)
        public ActionResult Edit(string p_PMKSDid)
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
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'UBAH'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Edit");
                }
                else
                {
                    ViewBag.DListAreaOperational = PopulateAreaRegional();
                    var model = SPMKS.PMKS(econstr, jconstr).FindPMKS(p_PMKSDid);
                    return PartialView("_Edit", model);
                }
            }
        }

        // POST: TPMKS/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_PMKS obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SPMKS.PMKS(econstr, jconstr).Edit(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TPMKS/Delete/5
        public ActionResult Delete(string p_PMKSDid)
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
                    ViewBag.DListAreaOperational = PopulateAreaRegional();
                    var model = SPMKS.PMKS(econstr, jconstr).FindPMKS(p_PMKSDid);
                    return PartialView("_Delete", model);
                }
            }
        }

        // POST: TPMKS/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_PMKS obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SPMKS.PMKS(econstr, jconstr).Delete(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TPMKS/Undelete/5
        public ActionResult Undelete(int id)
        {
            return View();
        }

        // POST: TPMKS/Undelete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Undelete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Approve(string p_PMKSDid)
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
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'Approve'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Approve");
                }
                else
                {
                    ViewBag.DListAreaOperational = PopulateAreaRegional();
                    var model = SPMKS.PMKS(econstr, jconstr).FindPMKS(p_PMKSDid);
                    return PartialView("_Approve", model);
                }
            }
        }

        // POST: TPMKS/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(T_PMKS obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SPMKS.PMKS(econstr, jconstr).Approve(obj,true);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult UnApprove(string p_PMKSDid)
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
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'UnApprove'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_UnApprove");
                }
                else
                {
                    ViewBag.DListAreaOperational = PopulateAreaRegional();
                    var model = SPMKS.PMKS(econstr, jconstr).FindPMKS(p_PMKSDid);
                    return PartialView("_UnApprove", model);
                }
            }
        }

        // POST: TPMKS/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnApprove(T_PMKS obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SPMKS.PMKS(econstr, jconstr).Approve(obj,false);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult PanggilGrid()
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                ViewBag.DListAreaOperational = PopulateAreaRegional();
                var model = SAreaRegional.AreaRegional(econstr, jconstr).GetListAreaRegional(false);
                return PartialView("_panggilgrid", model);

            }
        }
        public ActionResult GridPMKS()
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                ViewBag.DListAreaOperational = PopulateAreaRegional();
                var model = SPMKS.PMKS(econstr, jconstr).GetListPMKS(false,juserid);
                return PartialView("_gridPMKS", model);

            }
        }
    }
}