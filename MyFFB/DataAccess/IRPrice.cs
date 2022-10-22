using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRPrice
    {
        List<T_Price> GetListPrice(bool? defunct, string DatePrice, int FCUserID,string fil_pmks);
        T_Price FindPrice(string SupplierID, string SupplierName, string PMKSID, string DatePrice);
        string Add(T_Price obj);
        bool Edit(T_Price obj);
       // bool Defunct(T_Employee1 obj, bool stsDefunct);
        bool Delete(T_Price obj);

        bool UpHarga(int AccessID, List<T_Price.PriceUp> p_json);

        //bool Approve(T_Price obj, bool stsBlock);
    }
}
