using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyAttd.DataAccess
{
    public class RGroupIncentive : IRGroupIncentive
    {
        public RGroupIncentive(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        public int  Add(T_GroupIncentive obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                DynamicParameters p = new DynamicParameters();
                p.Add("@NoId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.AddDynamicParams(new { action = "ADD"
                                        ,NoId=obj.NoId
                                        ,GroupSuppID=obj.GroupSuppID
                                        ,Approval = obj.Approval
                                        ,Incentive = obj.Incentive
                                        ,IncentiveQty1 = obj.IncentiveQty1
                                        ,IncentivePrice1 = obj.IncentivePrice1
                                        ,IncentiveQty2 = obj.IncentiveQty2
                                        ,IncentivePrice2 = obj.IncentivePrice2
                                        ,IncentiveQty3 = obj.IncentiveQty3
                                        ,IncentivePrice3 = obj.IncentivePrice3
                                        ,IncentiveQty4 = obj.IncentiveQty4
                                        ,IncentivePrice4 = obj.IncentivePrice4
                                        ,IncentiveQty5 = obj.IncentiveQty5
                                        ,IncentivePrice5 = obj.IncentivePrice5
                                        ,IncentiveQty6 = obj.IncentiveQty6
                                        ,IncentivePrice6 = obj.IncentivePrice6
                                        ,AccessID = obj.AccessID
                                       });;
                db.Execute("sp_CRUDGroupIncentive", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@NoId");
            }
        }

        public bool Delete(T_GroupIncentive obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDGroupIncentive",
                                new { action = "DELETE"
                                    ,NoId=obj.NoId
                                    ,GroupSuppID=obj.GroupSuppID
                                    ,Approval = obj.Approval
                                    ,Incentive = obj.Incentive
                                    ,IncentiveQty1 = obj.IncentiveQty1
                                    ,IncentivePrice1 = obj.IncentivePrice1
                                    ,IncentiveQty2 = obj.IncentiveQty2
                                    ,IncentivePrice2 = obj.IncentivePrice2
                                    ,IncentiveQty3 = obj.IncentiveQty3
                                    ,IncentivePrice3 = obj.IncentivePrice3
                                    ,IncentiveQty4 = obj.IncentiveQty4
                                    ,IncentivePrice4 = obj.IncentivePrice4
                                    ,IncentiveQty5 = obj.IncentiveQty5
                                    ,IncentivePrice5 = obj.IncentivePrice5
                                    ,IncentiveQty6 = obj.IncentiveQty6
                                    ,IncentivePrice6 = obj.IncentivePrice6
                                    ,AccessID = obj.AccessID
                                    }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_GroupIncentive obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDGroupIncentive", 
                                new { action = "EDIT"
                                    ,NoId=obj.NoId
                                    ,GroupSuppID=obj.GroupSuppID
                                    ,Approval = obj.Approval
                                    ,Incentive = obj.Incentive
                                    ,IncentiveQty1 = obj.IncentiveQty1
                                    ,IncentivePrice1 = obj.IncentivePrice1
                                    ,IncentiveQty2 = obj.IncentiveQty2
                                    ,IncentivePrice2 = obj.IncentivePrice2
                                    ,IncentiveQty3 = obj.IncentiveQty3
                                    ,IncentivePrice3 = obj.IncentivePrice3
                                    ,IncentiveQty4 = obj.IncentiveQty4
                                    ,IncentivePrice4 = obj.IncentivePrice4
                                    ,IncentiveQty5 = obj.IncentiveQty5
                                    ,IncentivePrice5 = obj.IncentivePrice5
                                    ,IncentiveQty6 = obj.IncentiveQty6
                                    ,IncentivePrice6 = obj.IncentivePrice6
                                    ,AccessID = obj.AccessID
                                    }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Approve(T_GroupIncentive obj, bool stsApprove)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                string act;
                if (stsApprove)
                    act = "APPROVE";
                else
                    act = "UNAPPROVE";
                int result = db.Execute("sp_CRUDGroupIncentive",
                                            new {
                                                action = act
                                                ,NoId=obj.NoId
                                                ,GroupSuppID=obj.GroupSuppID
                                                ,Approval = obj.Approval
                                                ,Incentive = obj.Incentive
                                                ,IncentiveQty1 = obj.IncentiveQty1
                                                ,IncentivePrice1 = obj.IncentivePrice1
                                                ,IncentiveQty2 = obj.IncentiveQty2
                                                ,IncentivePrice2 = obj.IncentivePrice2
                                                ,IncentiveQty3 = obj.IncentiveQty3
                                                ,IncentivePrice3 = obj.IncentivePrice3
                                                ,IncentiveQty4 = obj.IncentiveQty4
                                                ,IncentivePrice4 = obj.IncentivePrice4
                                                ,IncentiveQty5 = obj.IncentiveQty5
                                                ,IncentivePrice5 = obj.IncentivePrice5
                                                ,IncentiveQty6 = obj.IncentiveQty6
                                                ,IncentivePrice6 = obj.IncentivePrice6
                                                ,AccessID = obj.AccessID
                                            }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public T_GroupIncentive FindGroupIncentive(int NoId)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @"SELECT 
	                            gs.GroupSuppName,a.Regional,gsi.*
	                            INTO #Temp1
                            FROM t_GroupSupp gs
	                            LEFT JOIN (
		                            SELECT DISTINCT 
			                            gsd.GroupSuppID,pmks.Regional
		                            FROM t_GroupSuppDet gsd
			                            INNER JOIN tblPMKS pmks
				                            ON gsd.PMKSID=pmks.PMKSID
		                            where ISNULL(gsd.FCDefunctInd,0)=0 --and ISNULL(pmks.FCDefunctInd,0)=0
	                            ) a
		                            ON a.GroupSuppID=gs.GroupSuppID
	                            LEFT JOIN t_GroupIncentive gsi
		                            on a.GroupSuppID=gsi.GroupSuppID
                            WHERE --ISNULL(gs.FCDefunctInd,0)=0 AND
                                    gsi.GroupSuppID IS NOT NULL

                            SELECT a.*, b.FCUserID, b.FCName, b.FCUserID, b.FCName 
                            FROM #Temp1 a
	                            LEFT JOIN t_User b on a.FCCreatedBy=b.FCUserID
	                            LEFT JOIN t_User c on a.FCUpdatedBy=c.FCUserID
                            WHERE ISNULL(a.FCDEFUNCTIND,0)= 0 and a.NoId=@NoId
                            ORDER BY a.GroupSuppID, a.NoId
                            DROP TABLE #Temp1
                        ";

                return db.Query<T_GroupIncentive>(que, new { NoId = NoId }
                                            , commandType: CommandType.Text).SingleOrDefault();
            }
        }

        public List<T_GroupIncentive> GetListGroupIncentive(bool? defunct)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                //string que = @" SELECT a.*, b.FCUserID, b.FCName, b.FCUserID, b.FCName FROM t_GroupIncentive a
                //                inner join t_User b ON a.FCCreatedBy=b.FCUserID
                //                inner join t_User c ON a.FCCreatedBy=c.FCUserID
                //                WHERE 1=1";

                string que = @"SELECT 
	                            gs.GroupSuppName,a.Regional,gsi.*
	                            INTO #Temp1
                            FROM t_GroupSupp gs
	                            LEFT JOIN (
		                            SELECT DISTINCT 
			                            gsd.GroupSuppID,pmks.Regional
		                            FROM t_GroupSuppDet gsd
			                            INNER JOIN tblPMKS pmks
				                            ON gsd.PMKSID=pmks.PMKSID
		                            where ISNULL(gsd.FCDefunctInd,0)=0 --and ISNULL(pmks.FCDefunctInd,0)=0
	                            ) a
		                            ON a.GroupSuppID=gs.GroupSuppID
	                            LEFT JOIN t_GroupIncentive gsi
		                            on a.GroupSuppID=gsi.GroupSuppID
                            WHERE --ISNULL(gs.FCDefunctInd,0)=0 AND
                                    gsi.GroupSuppID IS NOT NULL

                            SELECT a.*, b.*,c.*,d.*
                            FROM #Temp1 a
	                            LEFT JOIN t_User b on a.FCCreatedBy=b.FCUserID
	                            LEFT JOIN t_User c on a.FCUpdatedBy=c.FCUserID
                                LEFT JOIN t_User d on a.FCApproveBy=d.FCUserID
                        ";
                if (defunct != null)
                    que += " WHERE ISNULL(a.FCDEFUNCTIND,0)=@FCDEFUNCTIND ";

                que += " ORDER BY a.GroupSuppID, a.NoId" +
                        " DROP TABLE #Temp1";


                var result = db.Query<T_GroupIncentive,T_User, T_User, T_User, T_GroupIncentive >(que, (a, b, c,d) =>
               {
                   a.CreatedBy = b;
                   a.UpdatedBy = c;
                   a.ApprovedBy = d;
                   return a;
               }, splitOn: "FCUserID, FCUserID, FCUserID", param: new { FCDEFUNCTIND = defunct }).ToList();
                return result;
            }
        }
        public List<T_GroupSuppDet> GetListGridGroup(bool? defunct)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT 
	                                gs.GroupSuppID,gs.GroupSuppName,a.Regional
                                FROM t_GroupSupp gs
	                                LEFT JOIN (
		                                SELECT DISTINCT 
			                                gsd.GroupSuppID,pmks.Regional
		                                FROM t_GroupSuppDet gsd
			                                INNER JOIN tblPMKS pmks
				                                ON gsd.PMKSID=pmks.PMKSID
		                                where ISNULL(gsd.FCDefunctInd,0)=0 --and ISNULL(pmks.FCDefunctInd,0)=0
	                                ) a
		                                ON a.GroupSuppID=gs.GroupSuppID
	                                LEFT JOIN t_GroupIncentive gsi
		                                on a.GroupSuppID=gsi.GroupSuppID and ISNULL(gsi.FCDefunctInd,0)=0
                                WHERE ISNULL(gs.FCDefunctInd,0)=0 
                                    AND gsi.GroupSuppID IS NULL
                                    AND a.Regional IS NOT NULL
                                ";

                que += " ORDER BY gs.GroupSuppID";


                var result = db.Query<T_GroupSuppDet>(que).ToList();
                return result;
            }
        }
    }
}
