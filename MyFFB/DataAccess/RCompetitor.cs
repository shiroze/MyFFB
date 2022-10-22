using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyAttd.DataAccess
{
    public class RCompetitor : IRCompetitor
    {
        public RCompetitor(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        public string Add(T_Competitor obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                DynamicParameters p = new DynamicParameters();
                //p.Add("@FCSAPID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.AddDynamicParams(new { action = "ADD"
                                        ,CompetitorID=obj.CompetitorID
                                        ,CompetitorName = obj.CompetitorName
                                        ,CompetitorLocation = obj.CompetitorLocation
                                        ,Capacity = obj.Capacity
                                        ,CompetitorGroup=obj.CompetitorGroup
                                        ,PMKSID = obj.PMKSID
                                        ,AccessID = obj.AccessID
                                       });
                db.Execute("sp_CRUDCompetitor", p, commandType: CommandType.StoredProcedure);
                return p.Get<string>("@CompetitorName");
            }
        }

        public bool Delete(T_Competitor obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDCompetitor",
                                new { action = "DELETE"
                                    ,CompetitorID=obj.CompetitorID
                                    ,CompetitorName = obj.CompetitorName
                                    ,CompetitorLocation = obj.CompetitorLocation
                                    ,Capacity = obj.Capacity
                                    ,CompetitorGroup=obj.CompetitorGroup
                                    ,PMKSID = obj.PMKSID
                                    ,AccessID = obj.AccessID
                                    }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_Competitor obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDCompetitor", 
                                new { action = "EDIT"
                                    ,CompetitorID=obj.CompetitorID
                                    ,CompetitorName = obj.CompetitorName
                                    ,CompetitorLocation = obj.CompetitorLocation
                                    ,Capacity = obj.Capacity
                                    ,CompetitorGroup=obj.CompetitorGroup
                                    ,PMKSID = obj.PMKSID
                                    ,AccessID = obj.AccessID
                                    }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public T_Competitor FindCompetitor(int CompetitorID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT * FROM tblCompetitor
                                WHERE CompetitorID=@CompetitorID ";

                //if (CompetitorName != null)
                //    que += "  AND CompetitorName=@CompetitorName ";
                //if (PMKSID != null)
                //    que += "  AND PMKSID=@PMKSID ";

                return db.Query<T_Competitor>(que, new { CompetitorID = CompetitorID }
                                            , commandType: CommandType.Text).SingleOrDefault();
            }
        }

        public List<T_Competitor> GetListCompetitor(bool? defunct, int FCUserID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" DECLARE @setpmksid varchar(max),@pos int, @name varchar(25)
                                SELECT @setpmksid=SetPMKSID from t_user WHERE FCUserID=@FCUserID

                                SELECT * FROM tblCompetitor a
                                LEFT JOIN t_User b ON a.FCCreatedBy=b.FCUserID
                                LEFT JOIN t_User b1 ON a.FCUpdatedBy=b1.FCUserID
                                WHERE 1=1 and (a.PMKSID in (select * from dbo.splitstring(@setpmksid)) or isnull(@setpmksid,'')='')";
                if (defunct != null)
                    que += " AND ISNULL(a.FCDEFUNCTIND,0)=@FCDEFUNCTIND ";

                //if (CompetitorName != null)
                //    que += " AND a.CompetitorName=@CompetitorName ";
                //if (PMKSID != null && PMKSID != "")
                //    que += " AND a.PMKSID=@PMKSID";

                que += " ORDER BY a.CompetitorName, a.PMKSID";


                var result = db.Query<T_Competitor,T_User, T_User, T_Competitor>(que, (a,b,c) =>
               {
                   //a.RootMenu = b;
                   a.CreatedBy = b;
                   a.UpdatedBy = c;
                   return a;
               }, splitOn: "FCUserID,FCUserID", param: new { FCDEFUNCTIND = defunct, FCUserID= FCUserID }).ToList();
                return result;
            }
        }
    }
}
