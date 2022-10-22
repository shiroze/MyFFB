using MyAttd.Models;
using System.Collections.Generic;
using System.Data;

namespace MyAttd.DataAccess
{
    public interface IRIncentive
    {
        List<T_Incentive> GetListIncentive(bool? defunct,string Periode, int FCUserID);
        T_Incentive FindIncentive(string IncentiveID);
        //List<T_Incentive> PrintIncentive(int AccessID,string IncentiveID);
        DataTable PrintIncentive(int AccessID, string IncentiveID);
        bool Add(T_Incentive obj);
        //bool AddAdvance(T_Incentive obj);
        bool Delete(T_Incentive obj);

        bool Approve(T_Incentive obj, bool stsBlock);
    }
}
