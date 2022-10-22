using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRRole
    {
        List<T_Role> GetListRole(bool? defunct);
        T_Role GetRoleInfo(int roleid);
        T_Role FindByRoleID(int roleid, bool defunct = false);
        int Add(T_Role obj);
        bool Edit(T_Role obj);
        bool Defunct(T_Role obj, bool stsDefunct);
        bool Delete(T_Role obj);
    }
}
