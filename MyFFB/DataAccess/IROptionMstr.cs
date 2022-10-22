using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IROptionMstr
    {
        List<MyEnum> GetListOptionByGroup(string group="", string subgroup="");
    }
}
