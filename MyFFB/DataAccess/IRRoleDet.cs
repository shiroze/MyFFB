using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRRoleDet
    {
        List<T_RoleDet> GetListRoleDet(int? roleid, bool? defunct);
        T_RoleDet FindRoleDet(int? roleid, int? menuid, string menucd = "");
        bool Add(T_RoleDet obj);
        bool Edit(T_RoleDet obj);
        //bool Defunct(T_RoleDet obj, bool stsDefunct);
        bool Delete(T_RoleDet obj);
        bool DefunctByRoleID(int? fcroleid, int accessid);
    }
}
