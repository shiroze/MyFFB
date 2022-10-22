using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRSetReport
    {
        List<T_SetReport> GetListSetReport(bool? defunct, int AccessID);
        T_SetReport FindSetReport(int AccessID, string ReportID);
        bool Add(T_SetReport obj);
        bool Edit(T_SetReport obj);
        bool Delete(T_SetReport obj);
    }
}
