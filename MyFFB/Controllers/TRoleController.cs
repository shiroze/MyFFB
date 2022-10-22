using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using DevExtreme.AspNet.Data;
//using DevExtreme.AspNet.Mvc;
using MyAttd.Helpers;
using MyAttd.Models;
using MyAttd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MyAttd.Controllers
{
    public class TRoleController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, cundelete;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TRole";
        public TRoleController(RequestHandler requestHandler)
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

        //public object GetRoleDetail(int id, DataSourceLoadOptions loadOptions)
        //{
        //    List<T_RoleDet> result = SRoleDet.RoleDet(econstr, jconstr).GetListRoleDet(id, false);
        //    return DataSourceLoader.Load(result, loadOptions);
        //}

        // GET: TRole
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
                var model = SRole.Role(econstr, jconstr).GetListRole(null);
                 return View(model);
            }
        }

        // GET: TRole/Details/5
        public ActionResult Details(int id)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (TempData["Message"] != null)
                    ViewData["Message"] = TempData["Message"].ToString();

                T_Role model = SRole.Role(econstr, jconstr).FindByRoleID(id);
                if (model == null)
                {
                    ViewData["Message"] = "Data tidak ditemukan, pastikan data tidak terhapus!";
                    model = new T_Role();
                    model.FCRoleID = 0;
                    model.FCRoleDesc = "NOT_FOUND";
                }

                return View(model);
            }
        }

        // GET: TRole/Create
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

        // POST: TRole/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_Role role)
        {
            try
            {
                // TODO: Add insert logic here
                role.AccessID = juserid;
                SRole.Role(econstr, jconstr).Add(role);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TRole/Edit/5
        public ActionResult Edit(int id)
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
                    var model = SRole.Role(econstr, jconstr).FindByRoleID(id);
                    return PartialView("_Edit", model);
                }
            }
        }

        // POST: TRole/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_Role role)
        {
            try
            {
                // TODO: Add insert logic here
                role.AccessID = juserid;
                SRole.Role(econstr, jconstr).Edit(role);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TRole/Delete/5
        public ActionResult Delete(int id)
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
                    var model = SRole.Role(econstr, jconstr).FindByRoleID(id);
                    return PartialView("_Delete", model);
                }
            }
        }

        // POST: TRole/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_Role role)
        {
            try
            {
                // TODO: Add insert logic here
                role.AccessID = juserid;
                SRole.Role(econstr, jconstr).Defunct(role, true);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TRole/Undelete/5
        public ActionResult Undelete(int id)
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
                    var model = SRole.Role(econstr, jconstr).FindByRoleID(id, true);
                    return PartialView("_Undelete", model);
                }
            }
        }

        // POST: TRole/Undelete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Undelete(T_Role role)
        {
            try
            {
                // TODO: Add insert logic here
                role.AccessID = juserid;
                SRole.Role(econstr, jconstr).Defunct(role, false);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }
    }
}