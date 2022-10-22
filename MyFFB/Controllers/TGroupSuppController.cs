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
    public class TGroupSuppController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, cundelete;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TGroupSupp";
        public TGroupSuppController(RequestHandler requestHandler)
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
                    cundelete = roledetobj.FCUndelete;
                }
            }
        }

        public List<T_AreaRegional> PopulateRegion()
        {
            var result = SSAPVendor.SAPVendor(econstr, jconstr).GetRegion();
            return result;
        }

        // GET: TGroupSupp
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
                var model = SGroupSupp.GroupSupp(econstr, jconstr).GetListGroupSupp(false);
                
                return View(model);
            }
        }

        // GET: TGroupSupp/Details/5
        public ActionResult Details(int p_GroupSuppID)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (TempData["Message"] != null)
                    ViewData["Message"] = TempData["Message"].ToString();

                var model = SGroupSupp.GroupSupp(econstr, jconstr).GetGroupSuppDet(p_GroupSuppID);
                var m_main= SGroupSupp.GroupSupp(econstr, jconstr).FindByGroupSuppID(p_GroupSuppID,false);
                string[] Header = { m_main.GroupSuppID.ToString(), m_main.GroupSuppName,m_main.Regional };
                ViewBag.Header = Header;
                return View(model);
            }
        }

        // GET: TGroupSupp/Create
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

        // POST: TGroupSupp/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_GroupSupp obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SGroupSupp.GroupSupp(econstr, jconstr).Add(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TGroupSupp/CreateDet
        public ActionResult CreateDet(string p_GroupSuppID, string p_GroupSuppName)
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
                    return PartialView("_CreateDet");
                }
                else
                {
                    string[] Header = { p_GroupSuppID , p_GroupSuppName};
                    ViewBag.Header = Header;
                    return PartialView("_CreateDet");
                }
            }
        }

        // POST: TGroupSupp/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDet(T_GroupSuppDet obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SGroupSupp.GroupSupp(econstr, jconstr).AddDet(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Details", new { p_GroupSuppID = obj.GroupSuppID, p_GroupSuppName = obj.GroupSuppName });
        }

        // GET: TGroupSupp/Edit/5
        public ActionResult Edit(int p_GroupSuppID)
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
                    ViewBag.DListRegional = PopulateRegion();
                    var model = SGroupSupp.GroupSupp(econstr, jconstr).FindByGroupSuppID(p_GroupSuppID);
                    return PartialView("_Edit", model);
                }
            }
        }

        // POST: TGroupSupp/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_GroupSupp obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SGroupSupp.GroupSupp(econstr, jconstr).Edit(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TGroupSupp/EditDetail/5
        public ActionResult EditDet(int p_no)
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
                    var model = SGroupSupp.GroupSupp(econstr, jconstr).FindByGroupSuppIDDet(p_no);
                    return PartialView("_EditDet", model);
                }
            }
        }

        // POST: TGroupSupp/EditDetail/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDet(T_GroupSuppDet obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SGroupSupp.GroupSupp(econstr, jconstr).EditDet(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Details", new { p_GroupSuppID=obj.GroupSuppID, p_GroupSuppName=obj.GroupSuppName});
        }
        // GET: TGroupSupp/DeleteDetail/5
        public ActionResult DeleteDet(int p_no)
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
                    return PartialView("_DeleteDet");
                }
                else
                {
                    var model = SGroupSupp.GroupSupp(econstr, jconstr).FindByGroupSuppIDDet(p_no);
                    return PartialView("_DeleteDet", model);
                }
            }
        }

        // POST: TGroupSupp/DeleteDetail/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDet(T_GroupSuppDet obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SGroupSupp.GroupSupp(econstr, jconstr).DeleteDet(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Details", new { p_GroupSuppID = obj.GroupSuppID, p_GroupSuppName = obj.GroupSuppName });
            //return RedirectToAction("Index");
        }

        // GET: TGroupSupp/Delete/5
        public ActionResult Delete(int p_GroupSuppID)
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
                    var model = SGroupSupp.GroupSupp(econstr, jconstr).FindByGroupSuppID(p_GroupSuppID);
                    return PartialView("_Delete", model);
                }
            }
        }

        // POST: TGroupSupp/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_GroupSupp obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SGroupSupp.GroupSupp(econstr, jconstr).Delete(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TGroupSupp/Undelete/5
        public ActionResult Undelete(int p_GroupSuppID)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (!cundelete)
                {
                    ViewBag.MessageType = "Attention!";
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'BATAL HAPUS'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Undelete");
                }
                else
                {
                    var model = SGroupSupp.GroupSupp(econstr, jconstr).FindByGroupSuppID(p_GroupSuppID, true);
                    return PartialView("_Undelete", model);
                }
            }
        }

        // POST: TGroupSupp/Undelete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Undelete(T_GroupSupp obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SGroupSupp.GroupSupp(econstr, jconstr).Delete(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult GridSupplier(string Regional)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                //ViewBag.DListAreaOperational = PopulateAreaRegional();
                var model = SGroupSupp.GroupSupp(econstr, jconstr).ListSupplierNotGroup(Regional);
                //return PartialView("../TSupplier/_gridSupplier", model);
                return PartialView("_gridSupplier", model);

            }
        }
    }
}