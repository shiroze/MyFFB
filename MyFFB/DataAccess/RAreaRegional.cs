using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyAttd.DataAccess
{
    public class RAreaRegional : IRAreaRegional
    {
        public RAreaRegional(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        //public int Add(T_PMKS obj)
        public int Add(T_AreaRegional obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                DynamicParameters p = new DynamicParameters();
                p.Add("@AreaID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.AddDynamicParams(new { action = "ADD",
                                        AreaOperational = obj.AreaOperational,
                                        Regional = obj.Regional,
                                        AccessID = obj.AccessID,
                                       });
                db.Execute("sp_CRUDAreaRegional", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@AreaID");
            }
        }

        public bool Defunct(T_AreaRegional obj, bool stsDefunct)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string act;
                if (stsDefunct)
                    act = "DEFUNCT";
                else
                    act = "UNDEFUNCT";

                int result = db.Execute("sp_CRUDAreaRegional",
                                new { action = act,
                                    AreaID = obj.AreaID,
                                    AreaOperational = obj.AreaOperational,
                                    Regional = obj.Regional,
                                    AccessID = obj.AccessID
                                },
                                commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Delete(T_AreaRegional obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDAreaRegional",
                                new { action = "DELETE",
                                    AreaID = obj.AreaID,
                                    AreaOperational = obj.AreaOperational,
                                    Regional = obj.Regional,
                                    AccessID = obj.AccessID
                                },
                                commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_AreaRegional obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDAreaRegional", 
                                new { action = "EDIT",
                                AreaID= obj.AreaID,
                                AreaOperational = obj.AreaOperational,
                                Regional = obj.Regional,
                                AccessID = obj.AccessID
                            }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }
        
        public T_AreaRegional FindAreaRegional(int AreaID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = "SELECT * FROM t_AreaRegional WHERE 1=1 AND AreaID=@AreaID ";

                return db.Query<T_AreaRegional>(que, new { AreaID = AreaID }, commandType: CommandType.Text).SingleOrDefault();
            }
        }

        public List<T_AreaRegional> GetListAreaRegional(bool? defunct)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT * FROM t_AreaRegional a
                                inner join t_User b ON a.FCCreatedBy=b.FCUserID
                                WHERE 1=1 ";

                if (defunct != null)
                    que += " AND ISNULL(a.FCDefunctInd,0)=@FCDEFUNCTIND ";

                que += " ORDER BY a.AreaOperational, a.Regional";


                var result = db.Query<T_AreaRegional,T_User, T_AreaRegional>(que, (a, b) =>
               {
                   //a.RootMenu = b;
                   a.CreatedBy = b;
                   return a;
               }, splitOn: "FCUserID", param: new { FCDEFUNCTIND = defunct }).ToList();
                return result;
            }
        }

    }
}
