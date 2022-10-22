using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyAttd.DataAccess
{
    public class ROptionMstr : IROptionMstr
    {
        public ROptionMstr(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        public List<MyEnum> GetListOptionByGroup(string group = "", string subgroup = "")
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = "SELECT FCOptionName as ID, FCOptionDesc as Value FROM t_OptionMstr WHERE 1=1 ";

                if (!string.IsNullOrEmpty(group))
                    que += " AND FCOptionGroup=@GROUP";

                if (!string.IsNullOrEmpty(subgroup))
                    que += " AND FCOptionSubGroup=@SUBGROUP";

                que += " ORDER BY FCOrderNo, FCOptionDesc ";
                return db.Query<MyEnum>(que, new { GROUP = group, SUBGROUP = subgroup }, commandType: CommandType.Text).ToList();
            }
        }
    }
}
