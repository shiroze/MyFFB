using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRFFB
    {
        List<T_FFB> GetListFFB(bool? defunct, string p_tglTimbang, int FCUserID, string fil_pmks);
        T_FFB FindFFB(string p_tglTimbang, string p_ticket);
        int TotalFFBHarian(int AccessID,string p_tglTimbang);
        int CalculateFFB(int AccessID, string p_tglTimbang);
        int DownloadFFB(int AccessID, string p_tglTimbang);

        bool Delete(T_FFB obj);
        bool Post(T_FFB obj);
        bool Unpost(T_FFB obj);
    }
}
