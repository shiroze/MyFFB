using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAttd.Helpers;
using MyAttd.Models;
using MyAttd.Services;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Threading.Tasks;

namespace MyAttd.Controllers
{
    public class HomeController : Controller
    {
        protected readonly IWebHostEnvironment HostEnvironment;
        public HomeController(IWebHostEnvironment hostEnv)
        {
            this.HostEnvironment = hostEnv;
        }

        const string SessionUserName = "_UserName_ATTD";
        const string SessionUserID = "_UserID_ATTD";
        const string SessionName = "_Name_ATTD";
        const string SessionRoleID = "_RoleID_ATTD";
        const string SessionRoleDesc = "_RoleDesc_ATTD";
        const string SessionToggleStyle = "_ToggleStyle_ATTD";
        const string SessionConStr = "_ConStr_ATTD";
        const string SessionJsonConStr = "_JsonConStr_ATTD";
        const string SessionJsonUserID = "_JsonUserID_ATTD";
        const string SessionJsonRoleID = "_JsonRoleID_ATTD";

        [Authorize]
        public IActionResult KCIndex(T_User objUser)
        {
            ClaimsPrincipal currentUser = this.User;
            //Get username, for keycloak you need to regex this to get the clean username
            var userEmail = currentUser.FindFirst(ClaimTypes.Email).Value;

            //localDB
            string constr = "ATTD";

            string encryptconstr = Startup.Configuration.GetSection("ConnectionStrings").GetSection(constr).Value;

            string resaacp = DALSecurity.Decrypter(encryptconstr, "ATTDMDN");
            var obj = SUser.User(encryptconstr, constr).KeycloakUserLogin(userEmail);

            if (obj.MsgResult == "OK")
            {
                HttpContext.Session.SetString(SessionConStr, constr);
                HttpContext.Session.SetInt32(SessionUserID, obj.FCUserID);
                HttpContext.Session.SetString(SessionUserName, obj.FCUserName);
                HttpContext.Session.SetString(SessionName, obj.FCName);
                HttpContext.Session.SetInt32(SessionRoleID, obj.FCRoleID);
                HttpContext.Session.SetString(SessionRoleDesc, obj.Role.FCRoleDesc);
                //HttpContext.Session.SetString(SessionToggleStyle, " sidebar-collapse ");
                HttpContext.Session.SetString(SessionToggleStyle, " sidebar ");

                HttpContext.Session.Set(SessionJsonConStr, constr);
                HttpContext.Session.Set(SessionJsonRoleID, obj.FCRoleID);
                HttpContext.Session.Set(SessionJsonUserID, obj.FCUserID);

                return View("Dashboard");
            }
            else if(obj.MsgResult == "USER NOT FOUND")
            {
                return RedirectToAction("AuthenticationStatus", new { MsgResult = obj.MsgResult } );
            }
            else
            {
                if (this.HostEnvironment.IsDevelopment())
                {
                    Response.WriteAsync(obj.MsgResult);
                }
                ViewBag.Message = obj.MsgResult;
                return View("Dashboard", objUser);
            }
        }

        [Authorize]
        public IActionResult AuthenticationStatus(string MsgResult)
        {
            ViewBag.Message = MsgResult;
            return View();
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(SessionUserName) != null)
            {
                string constr = HttpContext.Session.GetString(SessionConStr);
                string encryptconstr = Startup.Configuration.GetSection("ConnectionStrings").GetSection(constr).Value;

                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString(SessionUserName) != null)
            {
                string constr = HttpContext.Session.GetString(SessionConStr);
                string encryptconstr = Startup.Configuration.GetSection("ConnectionStrings").GetSection(constr).Value;

                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public IActionResult About()
        {
            if (HttpContext.Session.GetString(SessionUserName) != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public IActionResult Contact()
        {
            if (HttpContext.Session.GetString(SessionUserName) != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Login()
        {
            // Validasi kembali agar tidak login 2x
            ClaimsPrincipal currentUser = this.User;
            if (HttpContext.Session.GetString(SessionUserName) != null || currentUser.FindFirstValue(ClaimTypes.Email) != null)
            {
                var userEmail = currentUser.FindFirst(ClaimTypes.Email).Value;

                //localDB
                string constr = "ATTD";

                string encryptconstr = Startup.Configuration.GetSection("ConnectionStrings").GetSection(constr).Value;

                string resaacp = DALSecurity.Decrypter(encryptconstr, "ATTDMDN");
                var obj = SUser.User(encryptconstr, constr).KeycloakUserLogin(userEmail);

                if (obj.MsgResult == "OK")
                {
                    HttpContext.Session.SetString(SessionConStr, constr);
                    HttpContext.Session.SetInt32(SessionUserID, obj.FCUserID);
                    HttpContext.Session.SetString(SessionUserName, obj.FCUserName);
                    HttpContext.Session.SetString(SessionName, obj.FCName);
                    HttpContext.Session.SetInt32(SessionRoleID, obj.FCRoleID);
                    HttpContext.Session.SetString(SessionRoleDesc, obj.Role.FCRoleDesc);
                    //HttpContext.Session.SetString(SessionToggleStyle, " sidebar-collapse ");
                    HttpContext.Session.SetString(SessionToggleStyle, " sidebar ");

                    HttpContext.Session.Set(SessionJsonConStr, constr);
                    HttpContext.Session.Set(SessionJsonRoleID, obj.FCRoleID);
                    HttpContext.Session.Set(SessionJsonUserID, obj.FCUserID);

                    return View("Dashboard");
                }
                else if (obj.MsgResult == "USER NOT FOUND")
                {
                    return RedirectToAction("AuthenticationStatus", new { MsgResult = obj.MsgResult });
                }

                return RedirectToAction("Dashboard");
                //string constr = HttpContext.Session.GetString(SessionConStr);
                //string encryptconstr = Startup.Configuration.GetSection("ConnectionStrings").GetSection(constr).Value;

                //return RedirectToAction("KCIndex");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(T_User objUser)
        {
            //localDB
            string constr = "ATTD";
            string encryptconstr = Startup.Configuration.GetSection("ConnectionStrings").GetSection(constr).Value;

            
            string resaacp = DALSecurity.Decrypter(encryptconstr, "ATTDMDN");
            //Pada bagian sp code di modify agar user yang emailnya sudah terisi baru bisa login
            var obj = SUser.User(encryptconstr, constr).LoginUser(objUser.FCUserName, objUser.Password);
            //var coba = obj.FCFirstLogin;
            
            if (obj.MsgResult == "OK")
            {
                HttpContext.Session.SetString(SessionConStr, constr);
                HttpContext.Session.SetInt32(SessionUserID, obj.FCUserID);
                HttpContext.Session.SetString(SessionUserName, objUser.FCUserName);
                HttpContext.Session.SetString(SessionName, obj.FCName);
                HttpContext.Session.SetInt32(SessionRoleID, obj.FCRoleID);
                HttpContext.Session.SetString(SessionRoleDesc, obj.Role.FCRoleDesc);
                //HttpContext.Session.SetString(SessionToggleStyle, " sidebar-collapse ");
                HttpContext.Session.SetString(SessionToggleStyle, " sidebar ");

                HttpContext.Session.Set(SessionJsonConStr, constr);
                HttpContext.Session.Set(SessionJsonRoleID, obj.FCRoleID);
                HttpContext.Session.Set(SessionJsonUserID, obj.FCUserID);

                return RedirectToAction("Index");
            }
            else if (obj.MsgResult == "PASSWORD EXPIRED")
            {
                return RedirectToAction("Reset");
            }
            //else if (obj.MsgResult == "FIRST LOGIN")
            //{
            //    return RedirectToAction("FirstLogin");
            //}
            else
            {
                ViewBag.Message = obj.MsgResult;
                return View(objUser);
            }
        }

        public ActionResult Reset()
        {
            return View();
        }
        public ActionResult FirstLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reset(User objUser)
        {
            if (!ModelState.IsValid)
            {
                //AddErrorsFromModel(ModelState.Values);
                return View();
            }
            //string constr = "QCD";
            string constr = "ATTD";
            string encryptconstr = Startup.Configuration.GetSection("ConnectionStrings").GetSection(constr).Value;

            var input = objUser.NewPassword;

            var MatchUser = Regex.Match(input.ToLower(), objUser.FCUserName.ToLower());
            if (MatchUser.Success == true)
            {
                ViewBag.Message = "Maaf Password tidak boleh mengandung unsur User ID.";
                return View(objUser);
            }
            //1 setid db passmin,  2 setid db passmax
            var PassMin = SMenu.Menu(encryptconstr, constr).SizePass(1);
            var PassMax = SMenu.Menu(encryptconstr, constr).SizePass(2);
            if (input.Length<PassMin.value)
            {
                ViewBag.Message = "Maaf Password Minimal Harus berjumlah "+PassMin.value+" karakter.";
                return View(objUser);
            }
            if (input.Length > PassMax.value)
            {
                ViewBag.Message = "Maaf Password Maksimal berjumlah " + PassMax.value + " karakter.";
                return View(objUser);
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");

            

            var hasMinimum8Chars = new Regex(@".{"+ PassMin.value + ","+ PassMax.value + "}");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            var isValidatedNumber = hasNumber.IsMatch(input);
            var isValidatedUpperChar = hasUpperChar.IsMatch(input);
            var isValidatedLowerChar = hasLowerChar.IsMatch(input);
            var isValidatedSymbols = hasSymbols.IsMatch(input);
            

            if (isValidatedNumber != true)
            {
                ViewBag.Message = "Maaf Password harus mengandung angka.";
                return View(objUser);
            }
            else if (isValidatedUpperChar != true)
            {
                ViewBag.Message = "Maaf Password harus mengandung Huruf Kapital.";
                return View(objUser);
            }
            else if (isValidatedLowerChar != true)
            {
                ViewBag.Message = "Maaf Password harus mengandung Huruf Kecil.";
                return View(objUser);
            }
            else if (isValidatedSymbols != true)
            {
                ViewBag.Message = "Maaf Password harus mengandung Symbols.";
                return View(objUser);
            }
            //else if (isMinimum8Chars != true)
            //{
            //    ViewBag.Message = "Maaf Password minimal "+ PassMin.value+" Karakter dan maksimal " +PassMax.value+" karakter.";
            //    return View(objUser);
            //}
            else
            {
                
                var obj = SUser.User(encryptconstr, constr).LoginUser(objUser.FCUserName, objUser.CurrentPassword);
                if (obj.MsgResult == "PASSWORD EXPIRED") /*|| obj.MsgResult == "FIRST LOGIN")*/
                {
                    T_User usr = new T_User();
                    usr.FCUserName = objUser.FCUserName;
                    usr.Password = objUser.NewPassword;
                    usr.FCUserID = obj.FCUserID;
                    usr.FCName = obj.FCName;
                    usr.FCRoleID = obj.FCRoleID;
                    usr.AccessID = obj.FCUserID;
                    bool res;
                    //if (obj.MsgResult == "PASSWORD EXPIRED")
                    //{
                        res = SUser.User(encryptconstr, constr).EditPassword(usr);
                    //}
                    //else
                    //{
                    //    res = SUser.User(encryptconstr, constr).FirstLogin(usr);
                    //}

                    if (res)
                    {
                        HttpContext.Session.SetString(SessionConStr, constr);
                        HttpContext.Session.SetInt32(SessionUserID, obj.FCUserID);
                        HttpContext.Session.SetString(SessionUserName, objUser.FCUserName);
                        HttpContext.Session.SetString(SessionName, obj.FCName);
                        HttpContext.Session.SetInt32(SessionRoleID, obj.FCRoleID);
                        HttpContext.Session.SetString(SessionRoleDesc, obj.Role.FCRoleDesc);
                        //HttpContext.Session.SetString(SessionToggleStyle, " sidebar-collapse ");
                        HttpContext.Session.SetString(SessionToggleStyle, " sidebar ");

                        HttpContext.Session.Set(SessionJsonConStr, constr);
                        HttpContext.Session.Set(SessionJsonRoleID, obj.FCRoleID);
                        HttpContext.Session.Set(SessionJsonUserID, obj.FCUserID);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = obj.MsgResult;
                    return View(objUser);
                }
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public async Task KeycloakLogout()
        {

            ClaimsPrincipal currentUser = this.User;
            /**
             * Works Perfectly
             */
            HttpContext.Session.Clear();

            if(currentUser.FindFirstValue(ClaimTypes.Email) != null)
            {
                await HttpContext.SignOutAsync("Cookies");
                await HttpContext.SignOutAsync("OpenIdConnect");
            }
        }

        public IActionResult SetToggleStyle()
        {
            if (HttpContext.Session.GetString(SessionToggleStyle) == "")
                //HttpContext.Session.SetString(SessionToggleStyle, " sidebar-collapse ");
                HttpContext.Session.SetString(SessionToggleStyle, " sidebar ");
            else
                HttpContext.Session.SetString(SessionToggleStyle, "");

            return Json("OK");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
        public static string Terbilang1(double n)

        {

            string[] k = { "", "satu", "dua", "tiga", "empat", "lima", "enam", "tujuh", "delapan", "sembilan" };

            double[] a = { 1000000000000000, 1000000000000, 1000000000, 1000000, 1000, 100, 10, 1 };

            string[] s = { "kuadriliun", "trilyun", "milyar", "juta", "ribu", "ratus", "puluh", "" };



            int i = 0;

            string x = "";



            while (n > 0)
            {

                double b = a[i];

                int c = (int)Math.Truncate(n / b);

                n -= b * c;

                x += (c >= 10 ? Terbilang1(c) + " " + s[i] + " " : ((c > 0 && c < 10) ? k[c] + " " + s[i] + " " : ""));

                i++;

            }
            x = Regex.Replace(x, @"(?i)satu puluh (\w+)", "$1 belas");

            x = Regex.Replace(x, @"(?i)satu (ribu|ratus|puluh|belas)", "se$1");

            return x;

        }
    }
}
