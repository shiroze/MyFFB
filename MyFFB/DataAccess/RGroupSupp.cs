using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyAttd.DataAccess
{
    public class RGroupSupp : IRGroupSupp
    {
        public RGroupSupp(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        public int Add(T_GroupSupp obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                DynamicParameters p = new DynamicParameters();
                p.Add("@GroupSuppID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.AddDynamicParams(new { action = "ADD", GroupSuppName = obj.GroupSuppName, Regional = obj.Regional, ACCESSID = obj.AccessID });
                db.Execute("sp_CRUDGroupSupp", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@GroupSuppID");
            }
        }

        public int AddDet(T_GroupSuppDet obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                DynamicParameters p = new DynamicParameters();
                p.Add("@NoId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.AddDynamicParams(new { action = "ADD", GroupSuppID = obj.GroupSuppID, PMKSID = obj.PMKSID, SupplierID = obj.SupplierID, ACCESSID = obj.AccessID });
                db.Execute("sp_CRUDGroupSuppDet", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@NoId");
            }
        }
        public bool EditDet(T_GroupSuppDet obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDGroupSuppDet", new { action = "EDIT", NoId = obj.NoId, GroupSuppID = obj.GroupSuppID, PMKSID = obj.PMKSID, SupplierID = obj.SupplierID, ACCESSID = obj.AccessID }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool DeleteDet(T_GroupSuppDet obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDGroupSuppDet", new { action = "DELETE", NoId = obj.NoId, GroupSuppID = obj.GroupSuppID, PMKSID = obj.PMKSID, SupplierID = obj.SupplierID, ACCESSID = obj.AccessID }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        //public bool Defunct(T_GroupSupp obj, bool stsDefunct)
        //{
        //    using (IDbConnection db = DALSecurity.GetSqlConnection)
        //    {
        //        if (db.State == ConnectionState.Closed)
        //            db.Open();

        //        string act;
        //        if (stsDefunct)
        //            act = "DEFUNCT";
        //        else
        //            act = "UNDEFUNCT";

        //        int result = db.Execute("sp_CRUDGroupSupp", new { action = act, GroupSuppID = obj.GroupSuppID, GroupSuppName = obj.GroupSuppName, ACCESSID = obj.AccessID }, commandType: CommandType.StoredProcedure);
        //        return result != 0;
        //    }
        //}

        public bool Delete(T_GroupSupp obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDGroupSupp", new { action = "DELETE", GroupSuppID = obj.GroupSuppID, GroupSuppName = obj.GroupSuppName, Regional = obj.Regional, ACCESSID = obj.AccessID }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_GroupSupp obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDGroupSupp", new { action = "EDIT", GroupSuppID = obj.GroupSuppID, GroupSuppName = obj.GroupSuppName, Regional = obj.Regional, ACCESSID = obj.AccessID }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public T_GroupSupp FindByGroupSuppID(int groupsuppid, bool defunct = false)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                string que = "SELECT * FROM t_GroupSupp WHERE ISNULL(FCDefunctInd,0)=@FCDEFUNCTIND AND GroupSuppID=@GroupSuppID";
                return db.Query<T_GroupSupp>(que, new { GroupSuppID = groupsuppid, FCDEFUNCTIND = defunct }, commandType: CommandType.Text).SingleOrDefault();
            }
        }

        public T_GroupSuppDet FindByGroupSuppIDDet(int NoId)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                string que = @"SELECT 
                                    gsd.NoId, gs.GroupSuppID, gs.GroupSuppName, gsd.PMKSID, pmks.PMKSName
                                    , gsd.SupplierID, s.SupplierName, pmks.Regional,gsd.FCCreatedDT, gsd.FCUpdatedDT
                                    , u.FCUserID, u.FCName, u1.FCUserID, u1.FCName
                                FROM t_GroupSupp gs
	                            INNER JOIN t_GroupSuppDet gsd 
		                            ON gs.GroupSuppID=gsd.GroupSuppID and
			                            ISNULL(gs.FCDefunctInd,0)=0 and 
			                            ISNULL(gsd.FCDefunctInd,0)=0
	                            INNER JOIN tblPMKS pmks
		                            ON gsd.PMKSID=pmks.PMKSID
	                            INNER JOIN tblSupplier s
		                            ON gsd.PMKSID=s.PMKSID and gsd.SupplierID=s.SupplierID
                                                   -- and s.Approval=1 --and s.FCDefunctInd=0
	                            LEFT JOIN t_User u
		                            ON gsd.FCCreatedBy=u.FCUserID
                                LEFT JOIN t_User u1
		                            ON gsd.FCUpdatedBy=u1.FCUserID
                               WHERE gsd.NoId=@NoId
                              ";
                return db.Query<T_GroupSuppDet>(que, new { NoId = NoId }, commandType: CommandType.Text).SingleOrDefault();
            }
        }

        public List<T_GroupSupp> GetListGroupSupp(bool? defunct)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT a.*,c.FCUserID,c.FCName,u.FCUserID,u.FCName  
                    FROM t_GroupSupp a 
                    LEFT JOIN t_User c ON a.FCCreatedBy=c.FCUserID
                    LEFT JOIN t_User u ON a.FCUpdatedBy=u.FCUserID
                    WHERE 1=1 ";

                if (defunct != null)
                    que += " AND ISNULL(a.FCDefunctInd,0)=@FCDEFUNCTIND ";

                var result = db.Query<T_GroupSupp, T_User, T_User, T_GroupSupp>(que, (a, b, c) =>
                {
                    a.CreatedBy = b;
                    a.UpdatedBy = c;
                    return a;
                }, splitOn: "FCUserID, FCUserID", param: new { FCDEFUNCTIND = defunct }).ToList();
                return result;
            }
        }

        public List<T_GroupSuppDet> GetGroupSuppDet(int GroupSuppID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                //var dictionary = new Dictionary<string, T_Role>();

                string que = @"SELECT 
                                    gsd.NoId, gs.GroupSuppID, gs.GroupSuppName, gsd.PMKSID, pmks.PMKSName
                                    , gsd.SupplierID, s.SupplierName, pmks.Regional, gsd.FCCreatedDT, gsd.FCUpdatedDT
                                    , u.FCUserID, u.FCName, u1.FCUserID, u1.FCName
                                FROM t_GroupSupp gs
	                            INNER JOIN t_GroupSuppDet gsd 
		                            ON gs.GroupSuppID=gsd.GroupSuppID and
			                            ISNULL(gs.FCDefunctInd,0)=0 and 
			                            ISNULL(gsd.FCDefunctInd,0)=0
	                            INNER JOIN tblPMKS pmks
		                            ON gsd.PMKSID=pmks.PMKSID
	                            INNER JOIN tblSupplier s
		                            ON gsd.PMKSID=s.PMKSID and gsd.SupplierID=s.SupplierID
                                                    --and s.Approval=1 --and s.FCDefunctInd=0
	                            LEFT JOIN t_User u
		                            ON gsd.FCCreatedBy=u.FCUserID
                                LEFT JOIN t_User u1
		                            ON gsd.FCUpdatedBy=u1.FCUserID
                               WHERE gs.GroupSuppID=@GroupSuppID
                              ";

                var result = db.Query<T_GroupSuppDet, T_User, T_User, T_GroupSuppDet>(que, (a, b, c) =>
                {
                    a.CreatedBy = b;
                    a.UpdatedBy = c;
                    return a;
                }, splitOn: "FCUserID, FCUserID", param: new { GroupSuppID = GroupSuppID }).ToList();
                return result;

            }
        }

        public List<T_gridSupplier> ListSupplierNotGroup(string Regional)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @"SELECT 
	                                s.SupplierID, s.SupplierName, s.PMKSID, p.PMKSName, p.Regional
                                FROM tblsupplier s
	                                LEFT JOIN t_GroupSuppDet gsd
		                                ON s.SupplierID=gsd.SupplierID
			                                AND s.PMKSID=gsd.PMKSID AND ISNULL(gsd.FCDefunctInd,0)=0
	                                INNER JOIN tblPMKS p
		                                ON s.PMKSID=p.PMKSID
                                WHERE s.Approval=1 AND gsd.GroupSuppID IS NULL
                              ";
                if (Regional != null)
                    que += " AND p.Regional=@Regional ";

                que += "order by s.SupplierID, s.PMKSID";
                var result = db.Query<T_gridSupplier>(que, param: new { Regional = Regional }).ToList();
                return result;

            }
        }
    }
}
