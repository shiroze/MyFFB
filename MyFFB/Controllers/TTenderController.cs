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
    public class TTenderController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, capprove;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TTender";
        //DateTime date1 = DateTime.Today;


        public TTenderController(RequestHandler requestHandler)
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
        public List<T_AreaRegional> PopulateRegion()
        {
            var result = SSAPVendor.SAPVendor(econstr, jconstr).GetRegion();
            return result;
        }


        // GET: TTender
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
        public ActionResult GridMainTable(string tgl)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (TempData["Message"] != null)
                    ViewData["Message"] = TempData["Message"].ToString();

                var model = STender.Tender(econstr, jconstr).GetListTender(false, tgl);
                return PartialView("_MainTable", model);
            }
        }

        public ActionResult View(string p_date, string p_product, string p_region)
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
                    var model = STender.Tender(econstr, jconstr).FindTender(p_date, p_product.Trim(),p_region.Trim());
                    return PartialView("_View", model);
                }
            }
        }
        //[HttpPost]
        public ActionResult Delete(string p_date, string p_product, string p_region)
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
                    var model = STender.Tender(econstr, jconstr).FindTender(p_date, p_product,p_region);
                    return PartialView("_Delete", model);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_Tender obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                STender.Tender(econstr, jconstr).Delete(obj);
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
                {
                    ViewBag.DListRegional = PopulateRegion();
                    return PartialView("_Create");
                }
            }
        }

        // POST: T_Holiday/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_Tender obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                STender.Tender(econstr, jconstr).Add(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string p_date, string p_product, string p_region)
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
                    var model = STender.Tender(econstr, jconstr).FindTender(p_date, p_product, p_region);
                    return PartialView("_Edit", model);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_Tender obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                STender.Tender(econstr, jconstr).Edit(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult TarikData(string Date,bool pilih)
        {
            try
            {
                if (!cadd)
                {
                    return Content("Maaf anda tidak memiliki Hak Akses 'Tarik Data'.");
                }
                else
                {
                    STender.Tender(econstr, jconstr).TarikData(Date, pilih, juserid);
                    return Content("Proses Tarik Data Sukses.");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
                return Content("Proses Tarik Data Gagal : \n" + ex.Message.ToString());
            }
        }


    }
}