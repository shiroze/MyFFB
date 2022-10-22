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

namespace MyAttd.Controllers
{
    public class TSupplierController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, capprove;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TSupplier";
        public TSupplierController(RequestHandler requestHandler)
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

        //public List<T_AreaRegional> PopulateAreaRegional()
        //{
        //    //T_PMKS obj = new T_PMKS();
        //    var result = SAreaRegional.AreaRegional(econstr, jconstr).GetListAreaRegional(false);
        //    return result;
        //}

        // GET: TSupplier
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
                //var model = SSupplier.Supplier(econstr, jconstr).GetListSupplier(false, juserid, true);
                var modal = SPMKS.PMKS(econstr, jconstr).GetListPMKS(null, juserid);
                ViewBag.DListPMKS = modal;
                return View();
            }
        }

        //// GET: TPMKS/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //GET: TSupplier/Create
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
                    var VAT = SSupplier.Supplier(econstr, jconstr).GetListVAT();
                    ViewBag.DListVAT = VAT;
                    var result = SSupplier.Supplier(econstr, jconstr).GetListCategory();
                    ViewBag.DListCategory = result;
                    return PartialView("_Create");
                    //return PartialView("_Create");
                }
            }
        }

        // POST: TSupplier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_Supplier obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SSupplier.Supplier(econstr, jconstr).Add(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult View(string p_SupplierID, string p_SupplierName, string p_PMKSDid)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                //var result = SSupplier.Supplier(econstr, jconstr).GetListCategory();
                //ViewBag.DListCategory = result;
                var model = SSupplier.Supplier(econstr, jconstr).FindSupplier(p_SupplierID, p_SupplierName, p_PMKSDid);
                return PartialView("_View", model);
            }
        }



        // GET: TSupplier/Edit/5
        public ActionResult Edit(string p_SupplierID, string p_SupplierName, string p_PMKSDid)
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
                    //ViewBag.DListAreaOperational = PopulateAreaRegional();
                    var result = SSupplier.Supplier(econstr, jconstr).GetListCategory();
                    ViewBag.DListCategory = result;
                    var VAT = SSupplier.Supplier(econstr, jconstr).GetListVAT();
                    ViewBag.DListVAT = VAT;
                    var model = SSupplier.Supplier(econstr, jconstr).FindSupplier(p_SupplierID, p_SupplierName, p_PMKSDid);
                    return PartialView("_Edit", model);
                }
            }
        }

        // POST: TSupplier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_Supplier obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SSupplier.Supplier(econstr, jconstr).Edit(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TSupplier/Delete/5
        public ActionResult Delete(string p_SupplierID, string p_SupplierName, string p_PMKSDid)
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
                    //ViewBag.DListAreaOperational = PopulateAreaRegional();
                    var model = SSupplier.Supplier(econstr, jconstr).FindSupplier(p_SupplierID, p_SupplierName, p_PMKSDid);
                    return PartialView("_Delete", model);
                }
            }
        }

        // POST: TSupplier/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_Supplier obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SSupplier.Supplier(econstr, jconstr).Delete(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        //// GET: TSupplier/Undelete/5
        //public ActionResult Undelete(int id)
        //{
        //    return View();
        //}

        //// POST: TSupplier/Undelete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Undelete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        public ActionResult Approve(string p_SupplierID, string p_SupplierName, string p_PMKSDid)
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
                    //ViewBag.DListAreaOperational = PopulateAreaRegional();
                    var model = SSupplier.Supplier(econstr, jconstr).FindSupplier(p_SupplierID, p_SupplierName, p_PMKSDid);
                    return PartialView("_Approve", model);
                }
            }
        }

        // POST: TPMKS/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(T_Supplier obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SSupplier.Supplier(econstr, jconstr).Approve(obj, true);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult UnApprove(string p_SupplierID, string p_SupplierName, string p_PMKSDid)
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
                    return PartialView("_UnApprove");
                }
                else
                {
                    //ViewBag.DListAreaOperational = PopulateAreaRegional();
                    var model = SSupplier.Supplier(econstr, jconstr).FindSupplier(p_SupplierID, p_SupplierName, p_PMKSDid);
                    return PartialView("_UnApprove", model);
                }
            }
        }

        // POST: TSupplier/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnApprove(T_Supplier obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SSupplier.Supplier(econstr, jconstr).Approve(obj, false);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult GridSupplier()
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                //ViewBag.DListAreaOperational = PopulateAreaRegional();
                var model = SSupplier.Supplier(econstr, jconstr).GetListSupplier(false, juserid, true,"",null);
                return PartialView("_gridSupplier", model);

            }
        }

        public ActionResult GridMainTable(bool? Approve,string fil_pmks,bool? Status_Active)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                //ViewBag.DListAreaOperational = PopulateAreaRegional();
                //if (p_txtSearch != null && p_txtSearch != "")
                //{
                //    p_txtSearch = "'%" + p_txtSearch + "%'";
                //}
                var model = SSupplier.Supplier(econstr, jconstr).GetListSupplier(false, juserid, Approve, fil_pmks, Status_Active);
                return PartialView("_MainTable", model);

            }
        }


    }
}