using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRGroupSupp
    {
        List<T_GroupSupp> GetListGroupSupp(bool? defunct);
        //T_Role GetRoleInfo(int roleid);
        T_GroupSupp FindByGroupSuppID (int groupsuppid, bool defunct = false);
        T_GroupSuppDet FindByGroupSuppIDDet(int no);
        List<T_GroupSuppDet> GetGroupSuppDet(int groupsuppid);
        List<T_gridSupplier> ListSupplierNotGroup(string Regional);
        int Add(T_GroupSupp obj);
        bool Edit(T_GroupSupp obj);
        //bool Defunct(T_GroupSupp obj, bool stsDefunct);
        bool Delete(T_GroupSupp obj);

        int AddDet(T_GroupSuppDet obj);
        bool EditDet(T_GroupSuppDet obj);
        bool DeleteDet(T_GroupSuppDet obj);
    }
}
