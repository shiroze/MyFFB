using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRSetGeneral
    {
        List<T_SetGeneral> GetListSetGeneral();
        T_SetGeneral FindSetGeneral(int? SetID);
        bool Add(T_SetGeneral obj);
        bool Edit(T_SetGeneral obj);
        bool Delete(T_SetGeneral obj);
    }
}
