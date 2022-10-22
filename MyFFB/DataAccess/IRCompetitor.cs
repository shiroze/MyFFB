using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRCompetitor
    {
        List<T_Competitor> GetListCompetitor(bool? defunct, int FCUserID);
        T_Competitor FindCompetitor(int CompetitorID);
        string Add(T_Competitor obj);
        bool Edit(T_Competitor obj);
        //bool Defunct(T_Employee1 obj, bool stsDefunct);
        bool Delete(T_Competitor obj);

        //bool Approve(T_Price obj, bool stsBlock);
    }
}
