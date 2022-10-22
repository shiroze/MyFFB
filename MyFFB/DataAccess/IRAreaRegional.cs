using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRAreaRegional
    {
        List<T_AreaRegional> GetListAreaRegional(bool? defunct);
        //List<T_AreaRegional> GetListAreaOperational(bool? defunct,string AreaOperational,string Regional);
        T_AreaRegional FindAreaRegional(int AreaID);
        int Add(T_AreaRegional obj);
        bool Edit(T_AreaRegional obj);
        bool Defunct(T_AreaRegional obj, bool stsDefunct);
        bool Delete(T_AreaRegional obj);
    }
}
