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
using System.Text.RegularExpressions;

namespace MyAttd.Controllers
{
    public class TUserController : Controller
    {
        private readonly RequestHandler _requestHandler;
        bool cadd, cedit, cdelete, cundelete;
        T_RoleDet roledetobj;
        T_User userobj;
        int jroleid, juserid;
        string econstr, jconstr;
        string setPMKSID;

        string classname = "TUser";
        private readonly IWebHostEnvironment _env;
        public TUserController(RequestHandler requestHandler, IWebHostEnvironment env)
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
                    cundelete = roledetobj.FCUndelete;
                }
                userobj = SUser.User(econstr, jconstr).FindUser(juserid, null);
                {
                    setPMKSID = userobj.SetPMKSID;
                }
            }
        }

        public List<T_Role> PopulateRole()
        {
            var result = SRole.Role(econstr, jconstr).GetListRole(false);
            return result;
        }

        
        //public List<T_StaffAnalysis> PopulateStaff()
        //{
        //    var result = SStaffAnalysis.StaffAnalysis(econstr, jconstr).GetListStaff(false);
        //    return result;
        //}

        //public object GetStaff(DataSourceLoadOptions loadOptions)
        //{
        //    List<T_StaffAnalysis> result = PopulateStaff();
        //    return DataSourceLoader.Load(result, loadOptions);
        //}

        // GET: TUser
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

                var model = SUser.User(econstr, jconstr).GetListUser(null, null, null);
                return View(model);
            }
        }

        // GET: TUser/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TUser/Create
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
                    ViewBag.DListRole = PopulateRole();
                    return PartialView("_Create");
                }
            }
        }

        // POST: TUser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_User user)
        {
            try
            {
                // TODO: Add insert logic here
                user.AccessID = juserid;
                int userid = SUser.User(econstr, jconstr).Add(user);
                
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TUser/Edit/5
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
                    var model = SUser.User(econstr, jconstr).FindUser(id);
                    
                    ViewBag.DListRole = PopulateRole();
                    return PartialView("_Edit", model);
                }
            }
        }

        // POST: TUser/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_User user)
        {
            try
            {
                // TODO: Add update logic here
                user.AccessID = juserid;
                bool res = SUser.User(econstr, jconstr).Edit(user);
                                
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TUser/Delete/5
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
                    var model = SUser.User(econstr, jconstr).FindUser(id);

                    ViewBag.DListRole = PopulateRole();
                    return PartialView("_Delete", model);
                }
            }
        }

        // POST: TUser/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(T_User user)
        {
            try
            {
                // TODO: Add delete logic here
                user.AccessID = juserid;
                SUser.User(econstr, jconstr).Defunct(user, true);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TUser/Undelete/5
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
                    var model = SUser.User(econstr, jconstr).FindUser(id);

                    ViewBag.DListRole = PopulateRole();
                    return PartialView("_Undelete", model);
                }
            }
        }

        // POST: TUser/Undelete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Undelete(T_User user)
        {
            try
            {
                // TODO: Add delete logic here
                user.AccessID = juserid;
                SUser.User(econstr, jconstr).Defunct(user, false);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TUser/Reset/5
        public ActionResult Reset(int id)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (cedit && cdelete)
                {
                    var model = SUser.User(econstr, jconstr).FindUser(id);
                    return PartialView("_Reset", model);
                }
                else
                {
                    ViewBag.MessageType = "Attention!";
                    ViewBag.Message = "Anda tidak memiliki hak akses.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Reset");
                }
            }
        }

        // POST: TUser/Reset/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reset(T_User user)
        {
            try
            {
                // TODO: Add update logic here
                user.AccessID = juserid;
                SUser.User(econstr, jconstr).Reset(user);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult FirstLogin(T_User user)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here
        //        user.AccessID = juserid;
        //        SUser.User(econstr, jconstr).FirstLogin(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Message"] = ex.Message.ToString();
        //    }
        //    return RedirectToAction("Index");
        //}

        // GET: TUser/Block/5
        public ActionResult Block(int id)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (cedit && cdelete)
                {
                    var model = SUser.User(econstr, jconstr).FindUser(id);
                    ViewBag.DListRole = PopulateRole();
                    return PartialView("_Block", model);
                }
                else
                {
                    ViewBag.MessageType = "Attention!";
                    ViewBag.Message = "Anda tidak memiliki hak akses.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Block");
                }
            }
        }

        // POST: TUser/Block/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Block(T_User user)
        {
            try
            {
                // TODO: Add update logic here
                user.AccessID = juserid;
                SUser.User(econstr, jconstr).Block(user, true);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TUser/Unblock/5
        public ActionResult Unblock(int id)
        {
            if (roledetobj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (cedit && cdelete)
                {
                    var model = SUser.User(econstr, jconstr).FindUser(id);
                    ViewBag.DListRole = PopulateRole();
                    return PartialView("_Unblock", model);
                }
                else
                {
                    ViewBag.MessageType = "Attention!";
                    ViewBag.Message = "Anda tidak memiliki hak akses.";
                    ViewBag.MessageCSS = "text-info";
                    return PartialView("_Unblock");
                }
            }
        }

        // POST: TUser/Unblock/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unblock(T_User user)
        {
            try
            {
                // TODO: Add update logic here
                user.AccessID = juserid;
                SUser.User(econstr, jconstr).Block(user, false);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }

        // GET: TUser/EditPassword/5
        public ActionResult EditPassword()
        {
            if (jroleid == 0)
            {
                return RedirectToAction("Login", new RouteValueDictionary(new { controller = "Home", action = "Login" }));
            }
            else
            {
                T_User tuser = SUser.User(econstr, jconstr).FindUser(juserid);
                User model = new User();
                if (tuser != null)
                {
                    model.FCUserID = tuser.FCUserID;
                    model.FCUserName = tuser.FCUserName;
                    model.FCName = tuser.FCName;

                    return View(model);
                }
                else
                {
                    ViewBag.Error = "Not Found";
                    return View();
                }
            }
        }

        // POST: TUser/EditPassword/5
        [HttpPost]
        public ActionResult EditPassword(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            try
            {

                string constr = "ATTD";
                string encryptconstr = Startup.Configuration.GetSection("ConnectionStrings").GetSection(constr).Value;

                var input = user.NewPassword;

                var MatchUser = Regex.Match(input.ToLower(), user.FCUserName.ToLower());
                if (MatchUser.Success == true)
                {
                    ViewBag.Message = "Maaf Password tidak boleh mengandung unsur User ID.";
                    return View(user);
                }
                //1 setid db passmin,  2 setid db passmax
                var PassMin = SMenu.Menu(encryptconstr, constr).SizePass(1);
                var PassMax = SMenu.Menu(encryptconstr, constr).SizePass(2);
                if (input.Length < PassMin.value)
                {
                    ViewBag.Message = "Maaf Password Minimal Harus berjumlah " + PassMin.value + " karakter.";
                    return View(user);
                }
                if (input.Length > PassMax.value)
                {
                    ViewBag.Message = "Maaf Password Maksimal berjumlah " + PassMax.value + " karakter.";
                    return View(user);
                }

                var hasNumber = new Regex(@"[0-9]+");
                var hasUpperChar = new Regex(@"[A-Z]+");
                var hasLowerChar = new Regex(@"[a-z]+");



                var hasMinimum8Chars = new Regex(@".{" + PassMin.value + "," + PassMax.value + "}");
                var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

                var isValidatedNumber = hasNumber.IsMatch(input);
                var isValidatedUpperChar = hasUpperChar.IsMatch(input);
                var isValidatedLowerChar = hasLowerChar.IsMatch(input);
                var isValidatedSymbols = hasSymbols.IsMatch(input);


                if (isValidatedNumber != true)
                {
                    ViewBag.Message = "Maaf Password harus mengandung angka.";
                    return View(user);
                }
                else if (isValidatedUpperChar != true)
                {
                    ViewBag.Message = "Maaf Password harus mengandung Huruf Kapital.";
                    return View(user);
                }
                else if (isValidatedLowerChar != true)
                {
                    ViewBag.Message = "Maaf Password harus mengandung Huruf Kecil.";
                    return View(user);
                }
                else if (isValidatedSymbols != true)
                {
                    ViewBag.Message = "Maaf Password harus mengandung Symbols.";
                    return View(user);
                }
                else
                {
                    // TODO: Add update logic here
                    T_User tuser = SUser.User(econstr, jconstr).LoginUser(user.FCUserName, user.CurrentPassword);
                    if (tuser.MsgResult == "PASSWORD EXPIRED" || tuser.MsgResult == "OK")
                    {
                        T_User usr = new T_User();
                        usr.FCUserName = user.FCUserName;
                        usr.Password = user.NewPassword;
                        usr.FCUserID = tuser.FCUserID;
                        usr.FCName = tuser.FCName;
                        usr.FCRoleID = tuser.FCRoleID;
                        usr.AccessID = tuser.FCUserID;

                        //var MatchUser = Regex.Match(input.ToLower(), usr.FCUserName.ToLower());
                        //if (MatchUser.Success == true)
                        //{
                        //    ViewBag.Message = "Maaf Password tidak boleh mengandung unsur User ID.";
                        //    return View(user);
                        //}

                        bool res = SUser.User(econstr, jconstr).EditPassword(usr);
                        if (res)
                        {
                            return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                        }
                        else
                        {
                            ViewBag.Message = "Tidak dapat mengganti kata sandi.";
                            return View(user);
                        }
                    }
                    else
                    {
                        ViewBag.Message = tuser.MsgResult;
                        return View(user);
                    }
                }
                
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
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

                parameters.Add(new ReportParameter("Par1", "Menu Role"));

                return File(ModFunction.genRptParam(path, model, "PDF", parameters), "application/pdf");

            }
        }
    }
}