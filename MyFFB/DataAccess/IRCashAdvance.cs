using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRCashAdvance
    {
        List<T_CashAdvance> GetListCashAdvance(bool? defunct, string p_Period, int FCUserID);
        T_CashAdvance FindCashAdvance(string p_CashNo);
        List<T_CashAdvance> GetListWeek();
        string Add(T_CashAdvance obj);
        bool Edit(T_CashAdvance obj);
        bool Delete(T_CashAdvance obj);
    }
}
