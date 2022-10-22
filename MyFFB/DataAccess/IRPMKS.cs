using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRPMKS
    {
        List<T_PMKS> GetListPMKS(bool? defunct, int FCUserID);
        //List<T_Employee1> GetListMenuNotInRole(int roleid);
        //T_PMKS FindPMKS(string AreaOperational,string Regional,string PMKSID);
        T_PMKS FindPMKS(string PMKSID);
        string Add(T_PMKS obj);
        bool Edit(T_PMKS obj);
       // bool Defunct(T_Employee1 obj, bool stsDefunct);
        bool Delete(T_PMKS obj);

        bool Approve(T_PMKS obj, bool stsBlock);
    }
}
