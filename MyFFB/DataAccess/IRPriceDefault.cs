using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRPriceDefault
    {
        List<T_PriceDefault> GetListPriceDefault();
        T_PriceDefault FindPriceDefault(string p_date, string p_supplier, string p_pmks);
        bool Add(T_PriceDefault obj);
        bool Edit(T_PriceDefault obj);
        bool Delete(T_PriceDefault obj);
    }
}
