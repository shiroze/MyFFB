using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using System;
using System.Data;

namespace MyAttd.DataAccess
{
    public class RSeqMstr : IRSeqMstr
    {
        public RSeqMstr(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        public T_SeqMstr GetAvailCode(string code, string parameter)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                DynamicParameters p = new DynamicParameters();
                p.Add("@FCINDVAL", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@FCGENCODE", dbType: DbType.String, direction: ParameterDirection.Output, size: 120);
                p.Add("@FCREQDT", dbType: DbType.DateTime, direction: ParameterDirection.Output);
                p.AddDynamicParams(new { FCCODE = code, LPARAMS = parameter });
                db.Execute("sp_GenerateSeq", p, commandType: CommandType.StoredProcedure);
                T_SeqMstr obj = new T_SeqMstr();
                obj.FCCode = code;
                obj.FCIndVal = p.Get<int>("@FCINDVAL");
                obj.FCGenCode = p.Get<string>("@FCGENCODE");
                obj.FCRequestDT = p.Get<DateTime>("@FCREQDT");

                return obj;
            }
        }

        public bool UpdateTSeqMstr(T_SeqMstr seqMstr)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_EditSeq", new { FCCODE = seqMstr.FCCode, FCREQDT = seqMstr.FCRequestDT, FCINDVAL = seqMstr.FCIndVal, FCGENCODE = seqMstr.FCGenCode }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }
    }
}
