using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRTender
    {
        List<T_Tender> GetListTender(bool? defunct, string Periode);
        T_Tender FindTender(string p_date,string p_product,string p_region);
        bool Add(T_Tender obj);
        bool Edit(T_Tender obj);
        bool Delete(T_Tender obj);
        bool TarikData(string Date,bool? pilih, int ACCESSID);
    }
}
