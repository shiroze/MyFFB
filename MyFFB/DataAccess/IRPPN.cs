using MyAttd.Models;
using System.Collections.Generic;
using System.Data;

namespace MyAttd.DataAccess
{
    public interface IRPPN
    {
        List<T_PPN> GetListPPN(bool? defunct, string p_tglTimbang);
        T_PPN FindPPN(string p_ticket);

        T_PPN FindPPNProsesHitung(string CC, string VC, string Periode, string awal, string akhir);
        //List<T_FFB> PrintFFB(string CC, string VC, string awal, string akhir);
        DataTable PrintFFB(string noFaktur);
        bool Add(T_PPN obj);
        bool AddAdvance(T_PPN obj);
        bool Delete(T_PPN obj);
        //bool ViewDet(string p_faktur);
    }
}
