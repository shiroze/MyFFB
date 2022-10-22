using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRMenu
    {
        List<T_Menu> GetListMenu(bool? defunct, int? parentid);
        List<T_Menu> GetListMenuNotInRole(int roleid);
        T_Menu FindMenu(int? menuid, string menucd = "");
        minmaxpass SizePass(int? SetID);
        int Add(T_Menu obj);
        bool Edit(T_Menu obj);
        bool Defunct(T_Menu obj, bool stsDefunct);
        bool Delete(T_Menu obj);
    }
}
