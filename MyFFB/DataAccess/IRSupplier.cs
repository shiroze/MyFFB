using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRSupplier
    {
        List<T_Supplier> GetListSupplier(bool? defunct, int FCUserID, bool? Approve, string fil_pmks,bool? Status_Active);
        List<T_Supplier> GetListCategory();
        List<T_Supplier.ListVAT> GetListVAT();

        T_Supplier FindSupplier(string p_SupplierID, string p_SupplierName, string p_PMKSDid);

        bool Edit(T_Supplier obj);
        bool Add(T_Supplier obj);
        // bool Defunct(T_Employee1 obj, bool stsDefunct);
        bool Delete(T_Supplier obj);

        bool Approve(T_Supplier obj, bool stsBlock);
    }
}
