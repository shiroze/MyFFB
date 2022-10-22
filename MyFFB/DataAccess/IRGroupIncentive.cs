using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRGroupIncentive
    {
        List<T_GroupIncentive> GetListGroupIncentive(bool? defunct);
        List<T_GroupSuppDet> GetListGridGroup(bool? defunct);
        T_GroupIncentive FindGroupIncentive(int GroupIncentiveID);
        int Add(T_GroupIncentive obj);
        bool Edit(T_GroupIncentive obj);
        //bool Defunct(T_Employee1 obj, bool stsDefunct);
        bool Delete(T_GroupIncentive obj);

        bool Approve(T_GroupIncentive obj, bool stsBlock);
    }
}
