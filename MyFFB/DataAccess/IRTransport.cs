using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRTransport
    {
        List<T_Transport> GetListTransport(bool? defunct, string Periode);
        T_Transport FindTransport(string p_date, string p_product, string p_pmks);
        bool Add(T_Transport obj);
        bool Edit(T_Transport obj);
        bool Delete(T_Transport obj);
        bool TarikData(string Date, bool? pilih, int ACCESSID);
    }
}
