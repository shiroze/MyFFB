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
    public class TGroupIncentiveController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, capprove;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TGroupIncentive";
        public TGroupIncentiveController(RequestHandler requestHandler)
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

        //public List<T_Supplier> PopulateSupplier()
        //{
        //    var result = SSupplier.Supplier(econstr, jconstr).GetListSupplier(false, null, null, null);
        //    return result;
        //}

        //public List<T_PMKS> PopulatePMKS()
        //{
        //    var result = SPMKS.PMKS(econstr, jconstr).GetListPMKS(false, null, null, null);
        //    return result;
        //}

        // GET: TGroupIncentive
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

                var model = SGroupIncentive.GroupIncentive(econstr, jconstr).GetListGroupIncentive(false);
                return View(model);
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

                var model = SGroupIncentive.GroupIncentive(econstr, jconstr).GetListGroupIncentive(false);
                return PartialView("_MainTable", model);
            }
        }
        public ActionResult GridGroup()
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (TempData["Message"] != null)
                    ViewData["Message"] = TempData["Message"].ToString();

                var model = SGroupIncentive.GroupIncentive(econstr, jconstr).GetListGridGroup(false);
                return PartialView("_gridGroup", model);
            }
        }

        //GET: TGroupIncentive/Create
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
                    //ViewBag.DListPMKS = PopulatePMKS();
                    //ViewBag.DListSupplier = PopulateSupplier();
                    return PartialView("_Create");
                }
            }
        }

        // POST: TGroupIncentive/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_GroupIncentive obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SGroupIncentive.GroupIncentive(econstr, jconstr).Add(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult View1(int p_no)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                var model = SGroupIncentive.GroupIncentive(econstr, jconstr).FindGroupIncentive(p_no);
                return PartialView("_View", model);
            }
        }
        // GET: TGroupIncentive/Edit/5
        public ActionResult Edit(int p_no)
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
                    //ViewBag.DListPMKS = PopulatePMKS();
                    //ViewBag.DListSupplier = PopulateSupplier();
                    var model = SGroupIncentive.GroupIncentive(econstr, jconstr).FindGroupIncentive(p_no);
                    return PartialView("_Edit", model);
                }
            }
        }

        // POST: TGroupIncentive/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_GroupIncentive obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SGroupIncentive.GroupIncentive(econstr, jconstr).Edit(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TGroupIncentive/Delete/5
        public ActionResult Delete(int p_no)
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
                    //ViewBag.DListPMKS = PopulatePMKS();
                    //ViewBag.DListSupplier = PopulateSupplier();
                    var model = SGroupIncentive.GroupIncentive(econstr, jconstr).FindGroupIncentive(p_no);
                    return PartialView("_Delete", model);
                }
            }
        }

        // POST: TGroupIncentive/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_GroupIncentive obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SGroupIncentive.GroupIncentive(econstr, jconstr).Delete(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TGroupIncentive/Delete/5
        public ActionResult Approve(int p_no)
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
                    //ViewBag.DListPMKS = PopulatePMKS();
                    //ViewBag.DListSupplier = PopulateSupplier();
                    var model = SGroupIncentive.GroupIncentive(econstr, jconstr).FindGroupIncentive(p_no);
                    return PartialView("_Approve", model);
                }
            }
        }

        // POST: TGroupIncentive/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(T_GroupIncentive obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SGroupIncentive.GroupIncentive(econstr, jconstr).Approve(obj,true);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }
    }
}