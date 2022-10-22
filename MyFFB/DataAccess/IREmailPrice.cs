using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IREmailPrice
    {
        List<T_EmailPrice> GetListEmailPrice();
        T_EmailPrice FindEmailPrice(string AddressEmail);
        string Add(T_EmailPrice obj);
        bool Edit(T_EmailPrice obj);
        bool Delete(T_EmailPrice obj);
    }
}
