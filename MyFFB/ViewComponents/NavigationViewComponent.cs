using MyAttd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyAttd.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        public NavigationViewComponent()
        {

        }

        public IViewComponentResult Invoke(int id)
        {
            string phasekey = HttpContext.Session.GetString("_ConStr_ATTD");
            string constr = Startup.Configuration.GetSection("ConnectionStrings").GetSection(HttpContext.Session.GetString("_ConStr_ATTD")).Value;
            var model = SRoleDet.RoleDet(constr, phasekey).GetListRoleDet(id, false);
            return View(model);
        }
    }
}
