using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRCashDeduct
    {
        List<T_FFB> GetListFFB(bool? defunct, string p_tglTimbang,string CashNo);
        T_FFB FindFFB(string p_ticket);

        bool Edit(T_FFB obj);
        string GetTotalMain(string CashNo);
        //int CalculateCashDeduct(int AccessID, string p_tglTimbang);
    }
}
