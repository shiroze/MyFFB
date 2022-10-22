using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRHoliday
    {
        List<T_Holiday> GetListHoliday(string Periode);
        T_Holiday FindHoliday(string date, string remarks);
        bool Add(T_Holiday obj);
        bool Delete(T_Holiday obj);
    }
}
