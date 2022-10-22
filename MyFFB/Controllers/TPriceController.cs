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
    public class TPriceController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, capprove;
        T_RoleDet roledetobj;
        int jroleid, juserid;
        string econstr, jconstr;

        string classname = "TPrice";
        //DateTime date1 = DateTime.Today;


        public TPriceController(RequestHandler requestHandler)
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

        //public List<T_Supplier> PopulateSupplier()
        //{
        //    var result = SSupplier.Supplier(econstr, jconstr).GetListSupplier(false, null, null, null);
        //    return result;
        //}

        //public List<T_PMKS> PopulatePMKS()
        //{
        //    var result = SPMKS.PMKS(econstr, jconstr).GetListPMKS(false, null, null, null);
        //    return result;
        //}

        // GET: TPrice
        public ActionResult Index(string p_datepick)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (TempData["Message"] != null)
                    ViewData["Message"] = TempData["Message"].ToString();

                if (cedit)
                {
                    ViewBag.can_edit_data = 1;
                }
                else
                {
                    ViewBag.can_edit_data = 0;
                }

                //var model = SPrice.Price(econstr, jconstr).GetListPrice(false, p_datepick, juserid);
                //ViewBag.Tanggal = GlobalVariable.Tanggal;
                //return View(model);
                //return PartialView("_MainTable", model);
                var modal = SPMKS.PMKS(econstr, jconstr).GetListPMKS(null, juserid);
                ViewBag.DListPMKS = modal;
                return View();
;            }
        }
        public ActionResult GridMainTable(string p_datepick, string fil_pmks)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (TempData["Message"] != null)
                    ViewData["Message"] = TempData["Message"].ToString();

                var model = SPrice.Price(econstr, jconstr).GetListPrice(false, p_datepick, juserid, fil_pmks);
                return PartialView("_MainTable", model);
            }
        }

        //GET: TPrice/Create
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
                    //ViewBag.DListPMKS = PopulatePMKS();
                    //ViewBag.DListSupplier = PopulateSupplier();
                    return PartialView("_Create");
                }
            }
        }

        // POST: TPrice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_Price obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SPrice.Price(econstr, jconstr).Add(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TPrice/Edit/5
        public ActionResult Edit(string p_SupplierID, string p_SupplierName, String p_PMKSID, string p_DatePrice)
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
                    //Check Role untuk OPERATOR akses tidak boleh diatas jam 10
                    //jroleid==5 = operator, 4 = Supervisor
                    var Tanggal = DateTime.Parse(p_DatePrice);
                    var Tanggal_Now = DateTime.UtcNow.Date;
                    if (jroleid == 5 && DateTime.Now.Hour> 10)
                    {
                        ViewBag.MessageType = "Attention!";
                        ViewBag.Message = "SUDAH LEWAT JAM 10 PAGI.";
                        ViewBag.MessageCSS = "text-info";
                        return PartialView("_Edit");
                    }
                    else if (Tanggal < Tanggal_Now && jroleid!=4)
                    {
                        ViewBag.MessageType = "Attention!";
                        ViewBag.Message = "Tanggal Sudah Lewat.";
                        ViewBag.MessageCSS = "text-info";
                        return PartialView("_UpHargaInfo");
                    }
                    var model = SPrice.Price(econstr, jconstr).FindPrice(p_SupplierID, p_SupplierName, p_PMKSID, p_DatePrice);
                    return PartialView("_Edit", model);
                }
            }
        }

        // POST: TPrice/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_Price obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SPrice.Price(econstr, jconstr).Edit(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            //return RedirectToAction("GridMainTable", new { p_datepick = obj.DatePrice });
            //GlobalVariable.Tanggal = obj.DatePrice.ToString("yyyy-MM-dd");
            return RedirectToAction("Index");
        }

        // GET: TPrice/Delete/5
        public ActionResult Delete(string p_SupplierID, string p_SupplierName, String p_PMKSID, string p_DatePrice)
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
                    //ViewBag.DListPMKS = PopulatePMKS();
                    //ViewBag.DListSupplier = PopulateSupplier();
                    var model = SPrice.Price(econstr, jconstr).FindPrice(p_SupplierID, p_SupplierName, p_PMKSID, p_DatePrice);
                    return PartialView("_Delete", model);
                }
            }
        }

        // POST: TPrice/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_Price obj)
        {
            try
            {
                // TODO: Add insert logic here
                obj.AccessID = juserid;
                SPrice.Price(econstr, jconstr).Delete(obj);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TPrice/Approve/5
        public ActionResult Approve(string p_SupplierID, string p_SupplierName, String p_PMKSID, string p_DatePrice)
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
                    ViewBag.Message = "Anda tidak memiliki hak untuk 'Approve'.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Approve");
                }
                else
                {
                    var model = SPrice.Price(econstr, jconstr).FindPrice(p_SupplierID, p_SupplierName, p_PMKSID, p_DatePrice);
                    return PartialView("_Approve", model);
                }
            }
        }

        public ActionResult PanggilGridSupplier()
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                var model = SSupplier.Supplier(econstr, jconstr).GetListSupplier(false,juserid,true,"",null);
                return PartialView("_panggilgridSupplier", model);

            }
        }

        //public ActionResult UpHarga(string p_json)
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

        //            var model = SPrice.Price(econstr, jconstr).UpHarga(juserid,p_json);
        //            return ;

        //        }
        //    }
        //}

        //public void UpHarga(List<PriceUp> p_json)
        //public ActionResult UpHarga(string p_json)

        public ActionResult UpHarga([FromBody]List<T_Price.PriceUp> p_json)
        {
            try
            {
                SPrice.Price(econstr, jconstr).UpHarga(juserid, p_json);
                return Content("Proses Up Harga Sukses.");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
                return Content("Proses Up Harga Gagal : \n" + ex.Message.ToString());
            }
        }

        public ActionResult UpHargaInfo([FromBody] List<T_Price.PriceUpInfo> p_json)
        {
            try
            {
                var parameter = JsonConvert.SerializeObject(p_json);
                var jsonObj = JsonConvert.DeserializeObject<List<T_Price.PriceUpInfo>>(parameter).FirstOrDefault();

                var Tanggal = DateTime.Parse(jsonObj.DatePrice);
                var Tanggal_Now = DateTime.UtcNow.Date;

                //jroleid==5 = operator, 4 = Supervisor
                if (jroleid == 5 && DateTime.Now.Hour > 10)
                {
                    ViewBag.MessageType = "Attention!";
                    ViewBag.Message = "SUDAH LEWAT JAM 10 PAGI.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_UpHargaInfo");
                }
                else if (Tanggal < Tanggal_Now && jroleid != 4)
                {
                    ViewBag.MessageType = "Attention!";
                    ViewBag.Message = "Tanggal Sudah Lewat.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_UpHargaInfo");
                }
                //SPrice.Price(econstr, jconstr).UpHarga(juserid, p_json);
                return PartialView("_UpHargaInfo", p_json);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
                return Content("Proses Up Harga Gagal : \n" + ex.Message.ToString());
            }
        }
    }
}