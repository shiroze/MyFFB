using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRSAPVendor
    {
        List<T_SAPVendor> GridListSAPVendor();
        List<T_AreaRegional> GetRegion();
        List<T_PMKS> GridListPMKS(string setPMKSIDuser,string setPMKSID);
        List<T_Supplier> GridListSupplierPrice(string dateprice, string setPMKSID);
        List<T_PPN> GridListPajakAdvance(string CC,string VC,string Periode_Akhir);
        List<T_CashAdvance> GridListCashNo(string CC, string VC, string Periode, string setPMKSID);
        List<T_Supplier> GridListSupplier(bool? Approval, string setPMKSID,string filter_PMKSID);
        List<T_Incentive> GridListIncentive(string setPMKSID);
        List<T_Competitor> GridListCompetitor(string PMKSID);
        List<T_SAPVendor.T_GroupPayment> GridListGroupPayment();
    }
}
