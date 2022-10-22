using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MyAttd.Helpers;
using System.Net;

namespace MyAttd
{
    public class Startup
    {
        public static string ConnectionString { get; private set; }
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = env;
            GlobalVariable.GV_AliasDomain = "FFB";
        }

        public static IConfiguration Configuration { get; private set; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc();

            // Authentication for Keycloak
            services.AddAuthentication(options =>
            {
                //Sets cookie authentication scheme
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(cookie =>
            {
                //Sets the cookie name and maxage, so the cookie is invalidated.
                cookie.Cookie.Name = "keycloak.cookie";
                cookie.Cookie.MaxAge = System.TimeSpan.FromMinutes(30);
                cookie.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                cookie.Cookie.HttpOnly = false;
                cookie.SlidingExpiration = true;
            })
            .AddOpenIdConnect(options =>
            {
                /*
                 * ASP.NET core uses the http://*:5000 and https://*:5001 ports for default communication with the OIDC middleware
                 * The app requires load balancing services to work with :80 or :443
                 * These needs to be added to the keycloak client, in order for the redirect to work.
                 * If you however intend to use the app by itself then,
                 * Change the ports in launchsettings.json, but beware to also change the options.CallbackPath and options.SignedOutCallbackPath!
                 * Use LB services whenever possible, to reduce the config hazzle :)
                */

                //Use default signin scheme
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                //options.ClaimActions.Clear();

                //Keycloak server
                options.Authority = Configuration.GetSection("Keycloak")["ServerRealm"];
                //Keycloak client ID
                options.ClientId = Configuration.GetSection("Keycloak")["ClientId"];
                //Keycloak client secret, bisa di sembunyikan karena tidak di pakai
                options.ClientSecret = Configuration.GetSection("Keycloak")["ClientSecret"];
                //Keycloak .wellknown config origin to fetch config
                options.MetadataAddress = Configuration.GetSection("Keycloak")["Metadata"];
                //Require keycloak to use SSL
                if (Environment.IsDevelopment())
                {
                    options.RequireHttpsMetadata = false;
                }
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("email");
                //Save the token
                options.SaveTokens = true;
                //Token response type, will sometimes need to be changed to IdToken, depending on config.
                options.ResponseType = OpenIdConnectResponseType.Code;
                //SameSite is needed for Chrome/Firefox, as they will give http error 500 back, if not set to unspecified.
                options.NonceCookie.SameSite = SameSiteMode.Unspecified;
                options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;

                ////options.TokenValidationParameters = new TokenValidationParameters
                ////{
                ////    NameClaimType = "name",
                ////    RoleClaimType = ClaimTypes.Role,
                ////    ValidateIssuer = true
                ////};
            });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
                options.IdleTimeout = System.TimeSpan.FromMinutes(30);
            });
            services.AddHttpContextAccessor();
            services.AddSingleton<RequestHandler>();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthorization();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    app.UseHsts();
            //}

            // Kita Gunakan ini untuk memunculkan error jangan membaca environment developement
            app.UseDeveloperExceptionPage();
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
            WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;

            app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules")),
            //    RequestPath = "/node_modules"
            //});

            app.UseHttpsRedirection();

            app.UseCookiePolicy();

            //var reportDirectory = Path.Combine(env.ContentRootPath, "Reports");
            //DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension.RegisterExtensionGlobal(new ReportStorageWebExtension1(reportDirectory));
            //app.UseDevExpressControls();

            app.UseSession();

            AppContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            //app.UsePathBase("/FFB");

            app.UseRouting();

            // for Keycloak Authentication
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");
            });
        }
    }
}













//namespace MyAttd
//{
//    public class Startup
//    {
//        public static string ConnectionString { get; private set; }
//        public Startup(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }

//        public static IConfiguration Configuration { get; private set; }

//        // This method gets called by the runtime. Use this method to add services to the container.
//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddSingleton<IConfiguration>(Configuration);

//            services.Configure<CookiePolicyOptions>(options =>
//            {
//                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
//                options.CheckConsentNeeded = context => false;
//                options.MinimumSameSitePolicy = SameSiteMode.None;
//            });

//            //services.AddDevExpressControls();

//            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
//            services.AddDistributedMemoryCache();
//            services.AddSession(options =>
//            {
//                options.Cookie.IsEssential = true;
//                options.IdleTimeout = System.TimeSpan.FromMinutes(45);
//            });
//            services.AddHttpContextAccessor();
//            services.AddSingleton<RequestHandler>();

//        }

//        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//            }
//            else
//            {
//                app.UseExceptionHandler("/Home/Error");
//                app.UseHsts();
//            }

//            //app.UseFastReport();
//            //app.UseDefaultFiles();
//            //app.UseStaticFiles();

//            app.UseStaticFiles();
//            //app.UseStaticFiles(new StaticFileOptions
//            //{
//            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules")),
//            //    RequestPath = "/node_modules"
//            //});

//            app.UseHttpsRedirection();

//            app.UseCookiePolicy();

//            //var reportDirectory = Path.Combine(env.ContentRootPath, "Reports");
//            //DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension.RegisterExtensionGlobal(new ReportStorageWebExtension1(reportDirectory));
//            //app.UseDevExpressControls();

//            app.UseSession();
//            AppContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());

//            app.UseMvc(routes =>
//            {
//                routes.MapRoute(
//                    name: "default",
//                    template: "{controller=Home}/{action=Index}/{id?}");
//                    //template: "{controller=TIncentive}/{action=print}/{IncentiveID?}");
//            });
//        }
//    }
//}
