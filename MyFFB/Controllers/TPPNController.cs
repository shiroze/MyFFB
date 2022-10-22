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
//using Microsoft.Reporting.NETCore;
using Microsoft.Reporting.NETCore;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Data;

namespace MyAttd.Controllers
{
    public class TPPNController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, capprove;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TPPN";
        //DateTime date1 = DateTime.Today;
        private readonly IWebHostEnvironment _env;


        public TPPNController(RequestHandler requestHandler, IWebHostEnvironment env)
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
                    capprove = roledetobj.FCApprove;
                }
            }

        }

        // GET: TPPN
        public ActionResult Index(string p_date)
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
        public ActionResult GridMainTable(string p_periode)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (TempData["Message"] != null)
                    ViewData["Message"] = TempData["Message"].ToString();

                var model = SPPN.PPN(econstr, jconstr).GetListPPN(false, p_periode);
                return PartialView("_MainTable", model);
            }
        }

        public ActionResult DuplicatePFaktur(string p_faktur)
        {
            var model = SPPN.PPN(econstr, jconstr).FindPPN(p_faktur);

            return Content(JsonConvert.SerializeObject(model));
        }

        // GET: TPPN/View/5
        public ActionResult ViewDet(string p_faktur)
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
                    var model = SPPN.PPN(econstr, jconstr).FindPPN(p_faktur);
                    return PartialView("_View", model);
                }
            }
        }
        public ActionResult ViewDetAdvance(string p_faktur)
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
                    return PartialView("_ViewAdvance");
                }
                else
                {
                    var model = SPPN.PPN(econstr, jconstr).FindPPN(p_faktur);
                    return PartialView("_ViewAdvance", model);
                }
            }
        }

        public ActionResult Delete(string p_faktur)
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
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'DELETE'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Delete");
                }
                else
                {
                    var model = SPPN.PPN(econstr, jconstr).FindPPN(p_faktur);
                    return PartialView("_Delete", model);
                }
            }
        }

        public ActionResult DeleteAdvance(string p_faktur)
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
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'DELETE'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_DeleteAdvance");
                }
                else
                {
                    var model = SPPN.PPN(econstr, jconstr).FindPPN(p_faktur);
                    return PartialView("_DeleteAdvance", model);
                }
            }
        }

        // POST: TPPN/Delete Normal+Advance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_PPN obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SPPN.PPN(econstr, jconstr).Delete(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }


        //GET: TPPN/Create
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

        // POST: TPPN/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_PPN obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SPPN.PPN(econstr, jconstr).Add(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult CreateAdvance()
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
                    return PartialView("_CreateAdvance");
                }
                else
                {
                    return PartialView("_CreateAdvance");
                }
            }
        }

        // POST: TPPN/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdvance(T_PPN obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SPPN.PPN(econstr, jconstr).AddAdvance(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        public ActionResult callProses(string CC, string VC, String Periode, string awal, string akhir)
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
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'DELETE'.";
                    ViewBag.MessageCSS = "text-info";
                    return null;
                }
                else
                {
                    var model = SPPN.PPN(econstr, jconstr).FindPPNProsesHitung(CC, VC, Periode.Replace("-",""), awal, akhir);
                    ViewBag.dataproses = model;

                    return Content (JsonConvert.SerializeObject(model));
                }
            }
        }

        //public ActionResult Print(string CC, string VC, string awal, string akhir)
        //{
        //    if (roledetobj == null)
        //    {
        //        return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
        //    }
        //    else
        //    {
        //        if (!cedit)
        //        {
        //            ViewBag.MessageType = "Attention!";
        //            ViewBag.Message = "Anda tidak memiliki hak untuk 'VIEW'.";
        //            ViewBag.MessageCSS = "text-info";
        //            return null;
        //        }
        //        else
        //        {
                    

        //            string mimtype = "";
        //            int extension = 1;

        //            string contentRootPath = _env.ContentRootPath;
        //            //string webRootPath = _env.WebRootPath;
        //            string path = contentRootPath + "//Report//Rpt_PPN-FFB.rdlc";
        //            Dictionary<string, string> parameters = new Dictionary<string, string>();
        //            //parameters.Add("rp1", "Paramater Test 1");
        //            var model = SPPN.PPN(econstr, jconstr).PrintFFB(CC, VC, awal, akhir);
        //            LocalReport localReport = new LocalReport(path);
        //            localReport.AddDataSource("DataSet1", model);
        //            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);

        //            return File(result.MainStream, "application/pdf");
        //        }
        //    }
        //}

        public ActionResult Print(string noFaktur)
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
                    return null;
                }
                else
                {
                    string contentRootPath = _env.ContentRootPath;
                    string path = contentRootPath + "//Report//Rpt_PPN-FFB.rdlc";
                    var model = SPPN.PPN(econstr, jconstr).PrintFFB(noFaktur);
                    var parameters = new ReportParameterCollection();

                    return File(ModFunction.genRptParam(path, model, "PDF", parameters), "application/pdf");
                }
            }
        }


    }
}