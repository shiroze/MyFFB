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
using System.Web;

namespace MyAttd.Controllers
{
    public class TAreaRegionalController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;
        //T_User userobj;
        //string login_AreaOperational, login_Regional;

        string classname = "TAreaRegional";
       public TAreaRegionalController(RequestHandler requestHandler)
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
            //if (juserid!=0)
            //{
            //    userobj = SUser.User(econstr, jconstr).FindUser(juserid, null);
            //    {
            //        login_AreaOperational = userobj.AreaOperational;
            //        login_Regional = userobj.Regional;
            //    }
            //}
            
        }

        public List<T_AreaRegional> PopulateAreaRegional()
        {
            T_AreaRegional obj = new T_AreaRegional();
            var result = SAreaRegional.AreaRegional(econstr, jconstr).GetListAreaRegional(false);
            return result;
        }

        // GET: TAreaRegional
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
                var model = SAreaRegional.AreaRegional(econstr, jconstr).GetListAreaRegional(false);
                return View(model);

            }
        }

        //// GET: TAreaRegional/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //GET: TAreaRegional/Create
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

        //// GET: TPMKS/CreateItem
        //public ActionResult CreateItem()
        //{
        //    if (roledetobj == null)
        //    {
        //        return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
        //    }
        //    else
        //    {
        //        if (!cadd)
        //        {
        //            ViewBag.MessageType = "Attention!";
        //            ViewBag.Message = "Anda tidak memiliki hak untuk 'TAMBAH'.";
        //            ViewBag.MessageCSS = "text-info";
        //            return PartialView("_CreateItem");
        //        }
        //        else
        //        {
        //            ViewBag.DListZeroParentID = PopulateParentID();
        //            return PartialView("_CreateItem");
        //        }
        //    }
        //}

        // POST: TAreaRegional/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_AreaRegional obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SAreaRegional.AreaRegional(econstr, jconstr).Add(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TAreaRegional/Edit/5
        public ActionResult Edit(int p_AreaID)
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
                    //ViewBag.DListZeroParentID = PopulateParentID();
                    var model = SAreaRegional.AreaRegional(econstr, jconstr).FindAreaRegional(p_AreaID);
                    return PartialView("_Edit", model);
                }
            }
        }

        // POST: TAreaRegional/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_AreaRegional obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SAreaRegional.AreaRegional(econstr, jconstr).Edit(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TAreaRegional/Delete/5
        public ActionResult Delete(int p_AreaID)
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
                    //ViewBag.DListZeroParentID = PopulateParentID();
                    var model = SAreaRegional.AreaRegional(econstr, jconstr).FindAreaRegional(p_AreaID);
                    return PartialView("_Delete", model);
                }
            }
        }

        // POST: TAreaRegional/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_AreaRegional obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SAreaRegional.AreaRegional(econstr, jconstr).Delete(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TAreaRegional/Undelete/5
        public ActionResult Undelete(int id)
        {
            return View();
        }

        // POST: TAreaRegional/Undelete/5
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
    }
}