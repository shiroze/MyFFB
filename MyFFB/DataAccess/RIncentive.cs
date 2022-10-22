using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;
using System.Data.SqlClient;

namespace MyAttd.DataAccess
{
    public class RIncentive : IRIncentive
    {
        public RIncentive(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        public T_Incentive FindIncentive(string IncentiveID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT 
	                                gs.GroupSuppID,gs.GroupSuppName,inc.Periode,pmks.CompanyCode,pmks.Regional,s.Code as VendorCode,inc.SupplierID
	                                ,s.SupplierName,inc.PMKSID,inc.NETTO,inc.Incentive,inc.PPH22,inc.Remarks,inc.UploadToSAP
	                                ,inc.FCApproveBy,inc.FCApproveDT,inc.FCCreatedBy,inc.FCCreatedDT,inc.FCUpdatedBy,inc.FCUpdatedDT
	                                ,inc.FCDefunctInd,ISNULL(u.FCName,'') as FCName,isnull(u1.FCName,'') as FCName,ISNULL(U2.FCName,'') as FCName
                                FROM t_Incentive inc
	                                LEFT JOIN
	                                (
		                                SELECT * from t_GroupSupp --where ISNULL(FCDefunctInd,0)=0
	                                ) gs ON inc.GroupSuppID=gs.GroupSuppID
	                                LEFT JOIN
	                                (
		                                SELECT * FROM tblPMKS --WHERE ISNULL(FCDefunctInd,0)=0
	                                ) pmks ON inc.PMKSID=pmks.PMKSID
	                                LEFT JOIN
	                                (
		                                SELECT * FROM tblSupplier --WHERE ISNULL(FCDefunctInd,0)=0
	                                ) s ON inc.SupplierID=s.SupplierID and inc.PMKSID=s.PMKSID
	                                LEFT JOIN t_user u ON u.FCUserID=inc.FCCreatedBy
	                                LEFT JOIN t_user u1 ON u1.FCUserID=inc.FCUpdatedBy
	                                LEFT JOIN t_user u2 ON u2.FCUserID=inc.FCApproveBy
                                WHERE inc.IncentiveID=@IncentiveID
                              ";

                //return db.Query<T_Incentive, T_User, T_User, T_User, T_Incentive>(que,(a,b,c,d), 
                //    new { Periode =Periode, GroupSuppID=GroupSuppID, PMKSID=PMKSID,SupplierID=SupplierID}, 
                //    commandType: CommandType.Text).SingleOrDefault();

                var result = db.Query<T_Incentive, T_User, T_User, T_User, T_Incentive>(que, (a, b, c, d) =>
                {
                    //a.RootMenu = b;
                    a.CreatedBy = b;
                    a.UpdateBy = c;
                    a.ApproveBy = d;
                    return a;
                }, splitOn: "FCName,FCName,FCName", param: new { IncentiveID= IncentiveID }).SingleOrDefault();
                return result;
            }
        }
        //public List<T_Incentive> PrintIncentive(int AccessID,string IncentiveID)
        //{
        //    using (IDbConnection db = DALSecurity.GetSqlConnection)
        //    {
        //        if (db.State == ConnectionState.Closed)
        //            db.Open();
        //        string query =
        //        @" SELECT 
	       //             dbo.GeneratePeriode(inc.Periode) as Periode,inc.SupplierID,s.SupplierName,inc.Netto,inc.PPH22,
	       //             inc.Incentive, inc.Netto*inc.Incentive as Pembelian,
	       //             Round(inc.Netto*inc.Incentive*inc.pph22/100,0) as PPH22Value,
	       //             inc.Netto*inc.Incentive-Round(inc.Netto*inc.Incentive*inc.pph22/100,0) as Pembayaran,
	       //             inc.Approval,s.Remarks,s.Remarks1,s.Remarks2,srn.FcName as Name1,u.FCName as Name2
        //            FROM t_Incentive inc
        //            LEFT JOIN (SELECT * from t_GroupSupp --where ISNULL(FCDefunctInd,0)=0
        //                        ) gs
	       //             ON inc.GroupSuppID=gs.GroupSuppID
        //            LEFT JOIN(SELECT * FROM tblPMKS --WHERE ISNULL(FCDefunctInd,0)=0
        //                        ) pmks
	       //             ON inc.PMKSID=pmks.PMKSID
        //            LEFT JOIN(SELECT * FROM tblSupplier --WHERE ISNULL(FCDefunctInd,0)=0
        //                        ) s
	       //             ON inc.SupplierID=s.SupplierID and inc.PMKSID=s.PMKSID
        //            LEFT JOIN t_User srn on srn.FCUserID=@AccessID
        //            LEFT JOIN t_User u on u.FCUserID=inc.FCApproveBy
        //            WHERE inc.IncentiveID=@IncentiveID
        //        ";
        //        var result = db.Query<T_Incentive>(query, param: new { AccessID= AccessID,IncentiveID = IncentiveID }).ToList();
        //        return result;
        //    }
        //    //var connStr = DALSecurity.GetSqlConnection.ConnectionString;
        //    //DataTable dataTable = new DataTable();
        //    //using (SqlConnection conn = new SqlConnection(connStr))
        //    //{
        //    //    string query =
        //    //        @" select inc.*
        //    //             //inc.Periode,inc.SupplierID,s.SupplierName,inc.Netto,s.PPH22,
        //    //             //inc.Incentive, inc.Netto*inc.Incentive as Pembelian,
        //    //             //Round(inc.Netto*inc.Incentive*s.pph22/100,0) as PPH22Value,
        //    //             //inc.Netto*inc.Incentive-Round(inc.Netto*inc.Incentive*s.pph22/100,0) as Pembayaran,
        //    //             //s.Remarks,s.Remarks1,s.Remarks2
        //    //            FROM t_Incentive inc
        //    //            LEFT JOIN
        //    //            (
        //    //            SELECT * from t_GroupSupp where ISNULL(FCDefunctInd,0)=0
        //    //            ) gs ON inc.GroupSuppID=gs.GroupSuppID
        //    //            LEFT JOIN
        //    //            (
        //    //            SELECT * FROM tblPMKS WHERE ISNULL(FCDefunctInd,0)=0
        //    //            ) pmks ON inc.PMKSID=pmks.PMKSID
        //    //            LEFT JOIN
        //    //            (
        //    //            SELECT * FROM tblSupplier WHERE ISNULL(FCDefunctInd,0)=0
        //    //            ) s ON inc.SupplierID=s.SupplierID and inc.PMKSID=s.PMKSID
        //    //            WHERE inc.IncentiveID=@IncentiveID
        //    //        ";

        //    //    SqlCommand cmd = new SqlCommand(query, conn);
        //    //    cmd.Parameters.Add("@IncentiveID", SqlDbType.VarChar);
        //    //    cmd.Parameters["@IncentiveID"].Value = IncentiveID;
        //    //    conn.Open();

        //    //    // create data adapter
        //    //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    //    // this will query your database and return the result to your datatable
        //    //    da.Fill(dataTable);
        //    //    conn.Close();
        //    //    da.Dispose();
        //    //}
        //    //var coba = JsonConvert.SerializeObject(dataTable);
        //    //return "coba";
        //}

        public DataTable PrintIncentive(int AccessID, string IncentiveID)
        {
            var connStr = DALSecurity.GetSqlConnection.ConnectionString;
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                string query =
                @" SELECT 
                        dbo.GeneratePeriode(inc.Periode) as Periode,inc.SupplierID,s.SupplierName,inc.Netto,inc.PPH22,
                        inc.Incentive, inc.Netto*inc.Incentive as Pembelian,
                        Round(inc.Netto*inc.Incentive*inc.pph22/100,0) as PPH22Value,
                        inc.Netto*inc.Incentive-Round(inc.Netto*inc.Incentive*inc.pph22/100,0) as Pembayaran,
                        inc.Approval,s.Remarks,s.Remarks1,s.Remarks2,srn.FcName as Name1,u.FCName as Name2
                    FROM t_Incentive inc
                    LEFT JOIN (SELECT * from t_GroupSupp --where ISNULL(FCDefunctInd,0)=0
                                ) gs
                        ON inc.GroupSuppID=gs.GroupSuppID
                    LEFT JOIN(SELECT * FROM tblPMKS --WHERE ISNULL(FCDefunctInd,0)=0
                                ) pmks
                        ON inc.PMKSID=pmks.PMKSID
                    LEFT JOIN(SELECT * FROM tblSupplier --WHERE ISNULL(FCDefunctInd,0)=0
                                ) s
                        ON inc.SupplierID=s.SupplierID and inc.PMKSID=s.PMKSID
                    LEFT JOIN t_User srn on srn.FCUserID=@AccessID
                    LEFT JOIN t_User u on u.FCUserID=inc.FCApproveBy
                    WHERE inc.IncentiveID=@IncentiveID
                ";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@AccessID", SqlDbType.Int);
                cmd.Parameters["@AccessID"].Value = AccessID;
                cmd.Parameters.Add("@IncentiveID", SqlDbType.VarChar);
                cmd.Parameters["@IncentiveID"].Value = IncentiveID;
                conn.Open();

                // create data adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // this will query your database and return the result to your datatable
                da.Fill(dataTable);
                conn.Close();
                da.Dispose();

                //string json = JsonConvert.SerializeObject(dataTable);

                return dataTable;
            }
        }


        public List<T_Incentive> GetListIncentive(bool? defunct, string Periode, int FCUserID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" DECLARE @setpmksid varchar(max),@pos int, @name varchar(25)
                                SELECT @setpmksid=SetPMKSID from t_user WHERE FCUserID=@FCUserID

                                SELECT 
	                                inc.IncentiveID,gs.GroupSuppID,gs.GroupSuppName,inc.Periode,pmks.CompanyCode,pmks.Regional,s.Code as VendorCode,inc.SupplierID
	                                ,s.SupplierName,inc.PMKSID,inc.NETTO,inc.Incentive,inc.PPH22,inc.Remarks,inc.UploadToSAP
	                                ,inc.FCApproveBy,inc.FCApproveDT,inc.FCCreatedBy,inc.FCCreatedDT,inc.FCUpdatedBy,inc.FCUpdatedDT
	                                ,inc.FCDefunctInd,inc.Approval,u.FCName,u1.FCName,U2.FCName
                                FROM t_Incentive inc
	                                LEFT JOIN
	                                (
		                                SELECT * from t_GroupSupp --where ISNULL(FCDefunctInd,0)=0
	                                ) gs ON inc.GroupSuppID=gs.GroupSuppID
	                                LEFT JOIN
	                                (
		                                SELECT * FROM tblPMKS --WHERE ISNULL(FCDefunctInd,0)=0
	                                ) pmks ON inc.PMKSID=pmks.PMKSID
	                                LEFT JOIN
	                                (
		                                SELECT * FROM tblSupplier --WHERE ISNULL(FCDefunctInd,0)=0
	                                ) s ON inc.SupplierID=s.SupplierID and inc.PMKSID=s.PMKSID
	                                LEFT JOIN t_user u ON u.FCUserID=inc.FCCreatedBy
	                                LEFT JOIN t_user u1 ON u1.FCUserID=inc.FCUpdatedBy
	                                LEFT JOIN t_user u2 ON u2.FCUserID=inc.FCApproveBy
                                WHERE inc.Periode=@Periode and ISNULL(inc.FCDefunctInd,0)=0
                                        and (inc.PMKSID in (select * from dbo.splitstring(@setpmksid)) or isnull(@setpmksid,'')='')
                              ";

                que += " ORDER BY gs.GroupSuppName";

                var result = db.Query<T_Incentive, T_User, T_User, T_User, T_Incentive>(que, (a, b, c, d) =>
                {
                    //a.RootMenu = b;
                    a.CreatedBy = b;
                    a.UpdateBy = c;
                    a.ApproveBy = d;
                    return a;
                }, splitOn: "FCName", param: new { FCDEFUNCTIND = defunct, Periode = Periode.Replace("-", ""), FCUserID=FCUserID }).ToList();
                return result;
            }
        }


        public bool Delete(T_Incentive obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDIncentive",
                                new
                                {
                                    action = "DELETE"
                                    , PERIODE = obj.Periode
                                    , IncentiveID =obj.IncentiveID
                                    , GroupSuppID = obj.GroupSuppID
                                    , GroupSuppName=obj.GroupSuppName
                                    , CompanyCode =obj.CompanyCode
                                    , VendorCode=obj.VendorCode
                                    , PMKSID = obj.PMKSID
                                    , SupplierID = obj.SupplierID
                                    , SupplierName = obj.SupplierName
                                    , AccessID = obj.AccessID
                                }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Add(T_Incentive obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDIncentive",
                                new
                                {
                                    action = "ADD"
                                    , PERIODE = obj.Periode
                                    , IncentiveID =obj.IncentiveID
                                    , GroupSuppID = obj.GroupSuppID
                                    , GroupSuppName=obj.GroupSuppName
                                    , CompanyCode =obj.CompanyCode
                                    , VendorCode=obj.VendorCode
                                    , PMKSID = obj.PMKSID
                                    , SupplierID = obj.SupplierID
                                    , SupplierName = obj.SupplierName
                                    , AccessID = obj.AccessID
                                }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Approve(T_Incentive obj, bool stsApprove)
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
                int result =
                    db.Execute("sp_CRUDIncentive",
                    new
                    {
                        action = act
                        , PERIODE = obj.Periode
                        , IncentiveID =obj.IncentiveID
                        , GroupSuppID = obj.GroupSuppID
                        , GroupSuppName=obj.GroupSuppName
                        , CompanyCode =obj.CompanyCode
                        , VendorCode=obj.VendorCode
                        , PMKSID = obj.PMKSID
                        , SupplierID = obj.SupplierID
                        , SupplierName = obj.SupplierName
                        , AccessID = obj.AccessID
                    }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

    }
}
