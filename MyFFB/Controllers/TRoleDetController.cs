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
    public class TRoleDetController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TRole";
        public TRoleDetController(RequestHandler requestHandler)
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
                }
            }
        }

        public List<T_Menu> PopulateMenuNotInRoleID(int roleid)
        {
            var result = SMenu.Menu(econstr, jconstr).GetListMenuNotInRole(roleid);
            return result;
        }

        // GET: TRoleDet
        public ActionResult Index()
        {
            return View();
        }

        // GET: TRoleDet/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TRoleDet/Create
        public ActionResult Create(int id)
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
                    T_Role role = SRole.Role(econstr, jconstr).FindByRoleID(id);
                    ViewBag.RoleID = role.FCRoleID;
                    ViewBag.RoleDesc = role.FCRoleDesc;
                    ViewBag.DListNotZeroParentID = PopulateMenuNotInRoleID(id);
                    return PartialView("_Create");
                }
            }
        }

        // POST: TRoleDet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_RoleDet roledet, int roleID)
        {
            try
            {
                roledet.AccessID = juserid;
                roledet.FCRoleID = roleID;
                SRoleDet.RoleDet(econstr, jconstr).Add(roledet);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Details", new RouteValueDictionary(new { controller = classname, action = "Details", id = roledet.FCRoleID }));
        }

        // GET: TRoleDet/Edit/5
        public ActionResult Edit(int rid, int mid)
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
                    var model = SRoleDet.RoleDet(econstr, jconstr).FindRoleDet(rid, mid);
                    T_Role role = SRole.Role(econstr, jconstr).FindByRoleID(rid);

                    ViewBag.RoleDesc = role.FCRoleDesc;
                    ViewBag.DListNotZeroParentID = PopulateMenuNotInRoleID(rid);
                    return PartialView("_Edit", model);
                }
            }
        }

        // POST: TRoleDet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_RoleDet roledet)
        {
            try
            {
                // TODO: Add update logic here
                roledet.AccessID = juserid;
                SRoleDet.RoleDet(econstr, jconstr).Edit(roledet);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Details", new RouteValueDictionary(new { controller = classname, action = "Details", Id = roledet.FCRoleID }));
        }

        // GET: TRoleDet/Delete/5
        public ActionResult Delete(int rid, int mid)
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
                    var model = SRoleDet.RoleDet(econstr, jconstr).FindRoleDet(rid, mid);
                    T_Role role = SRole.Role(econstr, jconstr).FindByRoleID(rid);

                    ViewBag.RoleDesc = role.FCRoleDesc;
                    return PartialView("_Delete", model);
                }
            }
        }

        // POST: TRoleDet/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_RoleDet roledet)
        {
            try
            {
                // TODO: Add delete logic here
                roledet.AccessID = juserid;
                SRoleDet.RoleDet(econstr, jconstr).Delete(roledet);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Details", new RouteValueDictionary(new { controller = classname, action = "Details", Id = roledet.FCRoleID }));
        }
    }
}