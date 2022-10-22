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
    public class TEmailPriceController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, capprove;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TEmailPrice";
        public TEmailPriceController(RequestHandler requestHandler)
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

        //public List<T_AreaRegional> PopulateBagianEmail()
        //{
        //    var result = SAreaRegional.AreaRegional(econstr, jconstr).GetListAreaRegional(false);
        //    return result;
        //}

        // GET: TEmailPrice
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
                var model = SEmailPrice.EmailPrice(econstr, jconstr).GetListEmailPrice();
                //var test = GlobalVariable.GV_AreaOperational;
                //string json = JsonConvert.SerializeObject(model);
                return View(model);
            }
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
                {

                    //ViewBag.DListAreaOperational = PopulateAreaRegional();
                    return PartialView("_Create");
                    //return PartialView("_Create");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_EmailPrice obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SEmailPrice.EmailPrice(econstr, jconstr).Add(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult View1(string AddressEmail)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                var model = SEmailPrice.EmailPrice(econstr, jconstr).FindEmailPrice(AddressEmail);
                return PartialView("_View", model);
            }
        }


        public ActionResult Edit(string AddressEmail)
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
                    var model = SEmailPrice.EmailPrice(econstr, jconstr).FindEmailPrice(AddressEmail);
                    return PartialView("_Edit", model);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_EmailPrice obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SEmailPrice.EmailPrice(econstr, jconstr).Edit(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string AddressEmail)
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
                    var model = SEmailPrice.EmailPrice(econstr, jconstr).FindEmailPrice(AddressEmail);
                    return PartialView("_Delete", model);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_EmailPrice obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SEmailPrice.EmailPrice(econstr, jconstr).Delete(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }
    }
}