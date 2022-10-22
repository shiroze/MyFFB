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
    public class TBudgetController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, capprove;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;
        T_User userobj;
        string login_AreaOperational, login_Regional;

        string classname = "TBudget";
        //DateTime date1 = DateTime.Today;


        public TBudgetController(RequestHandler requestHandler)
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
            if (juserid != 0)
            {
                userobj = SUser.User(econstr, jconstr).FindUser(juserid, null);
                {
                    login_AreaOperational = userobj.AreaOperational;
                    login_Regional = userobj.Regional;
                }
            }

        }

        // GET: TBudget
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
                return View(null);
            }
        }
        public ActionResult GridMainTable(string Periode)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (TempData["Message"] != null)
                    ViewData["Message"] = TempData["Message"].ToString();

                var model = SBudget.Budget(econstr, jconstr).GetListBudget(false, Periode,juserid);
                return PartialView("_MainTable", model);
            }
        }


        // GET: TBudget/View/5
        public ActionResult View(string Periode, string PMKSID)
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
                    var model = SBudget.Budget(econstr, jconstr).FindBudget(Periode, PMKSID);
                    return PartialView("_View", model);
                }
            }
        }

        public ActionResult Delete(string Periode, string PMKSID)
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
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'Delete'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Delete");
                }
                else
                {
                    var model = SBudget.Budget(econstr, jconstr).FindBudget(Periode, PMKSID);
                    return PartialView("_Delete", model);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_Budget obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SBudget.Budget(econstr, jconstr).Delete(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

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
                    return PartialView("_Create");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_Budget obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SBudget.Budget(econstr, jconstr).Add(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string Periode, string PMKSID)
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
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'EDIT'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_EDit");
                }
                else
                {
                    var model = SBudget.Budget(econstr, jconstr).FindBudget(Periode, PMKSID);
                    return PartialView("_Edit", model);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_Budget obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SBudget.Budget(econstr, jconstr).Edit(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }


    }
}