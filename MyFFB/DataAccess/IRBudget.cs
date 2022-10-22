using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRBudget
    {
        List<T_Budget> GetListBudget(bool? defunct, string Periode, int FCUserID);
        T_Budget FindBudget(string Periode, string PMKSID);

        bool Add(T_Budget obj);
        bool Edit(T_Budget obj);
        bool Delete(T_Budget obj);
    }
}
