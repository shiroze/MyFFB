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

namespace MyAttd.Controllers
{
    public class TSAPVendorController : Controller
    {
        private readonly RequestHandler _requestHandler;
        string econstr, jconstr;
        int jroleid, juserid;
        T_User userobj;
        string setPMKSID;
        public TSAPVendorController(RequestHandler requestHandler)
        {
            _requestHandler = requestHandler;

            jconstr = _requestHandler.GetSessionConStrRequest();
            jroleid = _requestHandler.GetSessionRoleIDRequest();
            juserid = _requestHandler.GetSessionUserIDRequest();
            econstr = Startup.Configuration.GetSection("ConnectionStrings").GetSection(jconstr).Value;
            //if (jroleid != 0)
            //{
                userobj = SUser.User(econstr, jconstr).FindUser(juserid, null);
                {
                    setPMKSID = userobj.SetPMKSID;
                }
            //}
        }

        public ActionResult GridSupplier()
        {
            var model = SSAPVendor.SAPVendor(econstr, jconstr).GridListSAPVendor();
            return PartialView("_gridSAPVendor", model);
        }

        public ActionResult GridPMKS(string setPMKSIDuser)
        {
            var model = SSAPVendor.SAPVendor(econstr, jconstr).GridListPMKS(setPMKSIDuser, setPMKSID);
            return PartialView("_gridPMKS", model);
        }

        public ActionResult GridSupplierPrice(string dateprice)
        {
            var model = SSAPVendor.SAPVendor(econstr, jconstr).GridListSupplierPrice(dateprice, setPMKSID);
            return PartialView("_gridSupplierPrice", model);
        }

        public ActionResult GridPajakAdvance(string CC,string VC,string Periode_Akhir)
        {
            var model = SSAPVendor.SAPVendor(econstr, jconstr).GridListPajakAdvance(CC,VC, Periode_Akhir);
            return PartialView("_gridPajakAdvance", model);
        }

        public ActionResult GridCashNo(string CC, string VC, string Periode, string setPMKSID)
        {
            var model = SSAPVendor.SAPVendor(econstr, jconstr).GridListCashNo(CC, VC, Periode, setPMKSID);
            return PartialView("_gridCashNO", model);
        }

        public ActionResult GridSupplierID(bool? Approval,string filter_PMKSID)
        {
            var model = SSAPVendor.SAPVendor(econstr, jconstr).GridListSupplier(Approval, setPMKSID,filter_PMKSID);
            return PartialView("_gridSupplier", model);
        }
        public ActionResult GridIncentive()
        {
            var model = SSAPVendor.SAPVendor(econstr, jconstr).GridListIncentive(setPMKSID);
            return PartialView("_gridIncentive", model);
        }

        public ActionResult GridCompetitor(string PMKSID)
        {
            var model = SSAPVendor.SAPVendor(econstr, jconstr).GridListCompetitor(PMKSID);
            return PartialView("_gridCompetitor", model);
        }

        public ActionResult GridGroupPayment()
        {
            var model = SSAPVendor.SAPVendor(econstr, jconstr).GridListGroupPayment();
            return PartialView("_gridGroupPayment", model);
        }
    }
}