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
using Microsoft.Reporting.NETCore;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;

namespace MyAttd.Controllers
{
    public class TMenuController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TMenu";
        private readonly IWebHostEnvironment _env;
        public TMenuController(RequestHandler requestHandler, IWebHostEnvironment env)
        {
            _env = env;
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

        public List<T_Menu> PopulateParentID()
        {
            T_Menu obj = new T_Menu();
            var result = SMenu.Menu(econstr, jconstr).GetListMenu(false, 0);
            return result;
        }

        // GET: TMenu
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
                var model = SMenu.Menu(econstr, jconstr).GetListMenu(null, null);
                return View(model);
            }
        }

        // GET: TMenu/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TMenu/Create
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

        // GET: TMenu/CreateItem
        public ActionResult CreateItem()
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
                    return PartialView("_CreateItem");
                }
                else
                {
                    ViewBag.DListZeroParentID = PopulateParentID();
                    return PartialView("_CreateItem");
                }
            }
        }

        // POST: TMenu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_Menu menu)
        {
            try
            {
                // TODO: Add insert logic here
                menu.AccessID = juserid;
                SMenu.Menu(econstr, jconstr).Add(menu);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TMenu/Edit/5
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
                    ViewBag.DListZeroParentID = PopulateParentID();
                    var model = SMenu.Menu(econstr, jconstr).FindMenu(id);
                    return PartialView("_Edit", model);
                }
            }
        }

        // POST: TMenu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_Menu menu)
        {
            try
            {
                // TODO: Add insert logic here
                menu.AccessID = juserid;
                SMenu.Menu(econstr, jconstr).Edit(menu);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TMenu/EditParent/5
        public ActionResult EditParent(int id)
        {
            if (TempData["Message"] != null)
                ViewData["Message"] = TempData["Message"].ToString();
            var model = SMenu.Menu(econstr, jconstr).GetListMenu(null, id);
            return View(model);
            //if (roledetobj == null)
            //{
            //    return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            //}
            //else
            //{
            //    if (!cedit)
            //    {
            //        ViewBag.MessageType = "Attention!";
            //        ViewBag.Message = "Anda tidak memiliki hak untuk 'UBAH'.";
            //        ViewBag.MessageCSS = "text-info";
            //        return View("Index");
            //    }
            //    else
            //    {
            //        if (TempData["Message"] != null)
            //            ViewData["Message"] = TempData["Message"].ToString();
            //        var model = SMenu.Menu(econstr, jconstr).GetListMenu(null, id);
            //        return View(model);
            //    }
            //}
        }

        // GET: TMenu/Delete/5
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
                    ViewBag.DListZeroParentID = PopulateParentID();
                    var model = SMenu.Menu(econstr, jconstr).FindMenu(id);
                    return PartialView("_Delete", model);
                }
            }
        }

        // POST: TMenu/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_Menu menu)
        {
            try
            {
                // TODO: Add insert logic here
                menu.AccessID = juserid;
                SMenu.Menu(econstr, jconstr).Delete(menu);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TMenu/Undelete/5
        public ActionResult Undelete(int id)
        {
            return View();
        }

        // POST: TMenu/Undelete/5
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

        public ActionResult Print([FromBody] T_Role ParJson)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                string contentRootPath = _env.ContentRootPath;
                string path = contentRootPath + "//Report//Rpt_MenuRole.rdlc";
                var model = SReport.Report(econstr, jconstr).RptMenuRole(ParJson.FCRoleID);

                var parameters = new ReportParameterCollection();

                return File(ModFunction.genRptParam(path, model, "PDF", parameters), "application/pdf");

            }
        }
    }
}