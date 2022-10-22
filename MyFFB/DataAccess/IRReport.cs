using MyAttd.Models;
using System.Collections.Generic;
using System.Data;

namespace MyAttd.DataAccess
{
    public interface IRReport
    {
        DataTable RptFFBReceive(int AccessID,T_Report.RptFFBRecap FBBReceive);
        DataTable RptFFBRecap(int AccessID, T_Report.RptFFBRecap ParJson);
        DataTable RptCashAdvance(int AccessID, T_Report.RptCashAdvance ParJson);
        DataTable RptFFBOutStanding(int AccessID, T_Report.RptFFBOutStanding ParJson);
        DataTable RptCashAdvanceRequest(int AccessID, T_Report.RptCashAdvanceRequest ParJson);
        //List<T_Report.Set_RptName> FindSetRptName(int AccessID);
        T_Report.Set_RptName FindSetRptName(int AccessID, string ReportID);
        DataTable RptTransfer(int AccessID, T_Report.RptTransfer ParJson);
        DataTable RptPriceIndicator(int AccessID, T_Report.RptPriceIndicator ParJson);
        DataTable RptPriceAverage(int AccessID, T_Report.RptPriceAverage ParJson);
        DataTable RptDetailListPayment(int AccessID, T_Report.RptDetailListPayment ParJson);

        DataTable RptMenuRole(int FCRoleID);
        DataTable RptUserRole(int FCRoleID);
    }
}
