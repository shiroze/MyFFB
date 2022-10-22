using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;
//using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace MyAttd.DataAccess
{
    public class RPriceCompetitor : IRPriceCompetitor
    {
        public RPriceCompetitor(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        public T_PriceCompetitor FindPriceCompetitor(string Date, string PMKSID, string CompetitorName)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que =
                    @"
                    SELECT
	                    CAST(A.Date as date) as Date,a.PMKSID,a.CompetitorName,a.Price,
	                    a.FCCreatedBy,a.FCCreatedDT,a.FCUpdatedBy,a.FCUpdatedDT
	                    ,ISNULL(u.FCName,'') as FCName, ISNULL(u1.FCName,'') as FCName 
                    FROM TblPriceCompetitor a
                    LEFT JOIN t_User u
	                    ON a.FCCreatedBy=u.FCUserID
                    LEFT JOIN t_User u1
	                    ON a.FCUpdatedBy=u1.FCUserID
                    WHERE CAST(A.Date as date)=@Date
	                    AND a.PMKSID=@PMKSID
	                    AND a.CompetitorName=@CompetitorName
                    ";

                //return db.Query<T_PriceCompetitor>
                //    (que, new { Periode=Periode,PMKSID = PMKSID}, commandType: CommandType.Text).SingleOrDefault();

                var result = db.Query<T_PriceCompetitor, T_User, T_User, T_PriceCompetitor>(que, (a, b, c) =>
                {
                    a.CreatedBy = b;
                    a.UpdateBy = c;
                    return a;
                }, splitOn: "FCName,FCName", param: new { Date = Date, PMKSID= PMKSID, CompetitorName= CompetitorName }).SingleOrDefault();
                return result;
            }
        }

        public List<T_PriceCompetitor> GetListPriceCompetitor(bool? defunct, string Date, int FCUserID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" DECLARE @setpmksid varchar(max),@pos int, @name varchar(25)
                                SELECT @setpmksid=SetPMKSID from t_user WHERE FCUserID=@FCUserID

                                SELECT
	                                cast(a.Date as date) as Date, a.PMKSID,a.CompetitorName,a.Price,
	                                a.FCCreatedBy,a.FCCreatedDT,a.FCUpdatedBy,a.FCUpdatedDT
	                                ,ISNULL(u.FCName,'') as FCName, ISNULL(u1.FCName,'') as FCName 
                                FROM TblPriceCompetitor a
	                                LEFT JOIN t_User u
		                                ON a.FCCreatedBy=u.FCUserID
	                                LEFT JOIN t_User u1
		                                ON a.FCUpdatedBy=u1.FCUserID
                                WHERE cast(a.Date as date)=@Date
	                                --AND a.PMKSID='PUS' AND CompetitorName='RAMP KARUNIA'
                                    AND (a.PMKSID in (select * from dbo.splitstring(@setpmksid)) or isnull(@setpmksid,'')='')
                            ";

                que += " ORDER BY a.PMKSID";


                //var result = db.Query<T_PriceCompetitor>(que, param: new { Date= Date }).ToList();

                var result = db.Query<T_PriceCompetitor, T_User, T_User, T_PriceCompetitor>(que, (a, b, c) =>
                {
                    a.CreatedBy = b;
                    a.UpdateBy = c;
                    return a;
                }, splitOn: "FCName,FCName", param: new { FCDEFUNCTIND = defunct, Date = Date, FCUserID= FCUserID }).ToList();
                return result;
                //return result;
            }
        }

        public bool Add(T_PriceCompetitor obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDPriceCompetitor",
                                new
                                {
                                    action = "ADD",
                                    //PriceCompetitorID = obj.PriceCompetitorID,
                                    Date = obj.Date,
                                    PMKSID = obj.PMKSID,
                                    //CompetitorID = obj.CompetitorID,
                                    CompetitorName=obj.CompetitorName,
                                    Price = obj.Price,
                                    AccessID = obj.AccessID
                                }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_PriceCompetitor obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDPriceCompetitor",
                                new
                                {
                                    action = "EDIT",
                                    //PriceCompetitorID = obj.PriceCompetitorID,
                                    Date = obj.Date,
                                    PMKSID = obj.PMKSID,
                                    //CompetitorID = obj.CompetitorID,
                                    CompetitorName = obj.CompetitorName,
                                    Price = obj.Price,
                                    AccessID = obj.AccessID
                                }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Delete(T_PriceCompetitor obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDPriceCompetitor",
                                new
                                {
                                    action = "DELETE",
                                    //PriceCompetitorID = obj.PriceCompetitorID,
                                    Date = obj.Date,
                                    PMKSID = obj.PMKSID,
                                    //CompetitorID = obj.CompetitorID,
                                    CompetitorName = obj.CompetitorName,
                                    Price = obj.Price,
                                    AccessID = obj.AccessID
                                }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool TarikData(string Date, int ACCESSID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDPriceCompetitorTarikData",
                                new
                                {
                                    action = "TARIKDATA",
                                    Date = Date,
                                    AccessID = ACCESSID
                                }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }


            //var connStr = DALSecurity.GetSqlConnection.ConnectionString;
            //DataTable dtStrCon = new DataTable();
            //DataTable dataTable = new DataTable();
            //using (SqlConnection conn = new SqlConnection(connStr))
            //{
            //    string query = @" SELECT a.*,b.CompetitorName,ISNULL(u.FCName,'') as FCName, ISNULL(u1.FCName,'') as FCName
            //                FROM t_PriceCompetitor a
            //                INNER JOIN tblCompetitor b
            //                    ON a.CompetitorID=b.CompetitorID
            //                LEFT JOIN t_User u
            //                    ON a.FCCreatedBy=u.FCUserID
            //                LEFT JOIN t_User u1
            //                    ON a.FCUpdatedBy=u1.FCUserID
            //                WHERE a.[Date]=@Date and ISNULL(a.FCDefunctInd,0)=0
            //                ORDER BY a.PMKSID
            //            ";
            //    SqlCommand cmd = new SqlCommand(query, conn);
            //    cmd.Parameters.Add("@Date", SqlDbType.Date);
            //    cmd.Parameters["@Date"].Value = Date;
            //    conn.Open();

            //    // create data adapter
            //    SqlDataAdapter da = new SqlDataAdapter(cmd);
            //    // this will query your database and return the result to your datatable
            //    da.Fill(dataTable);
            //    conn.Close();
            //    da.Dispose();

            //    string queStrCon = @" SELECT Value FROM t_SettingString WHERE TitleSetting='SERVER-DOWNLOAD-FFB'";
            //    cmd = new SqlCommand(queStrCon, conn);
            //    conn.Open();
            //    da = new SqlDataAdapter(cmd);
            //    da.Fill(dtStrCon);
            //    conn.Close();
            //    da.Dispose();
            //}

            //connStr = dtStrCon.Rows[0]["Value"].ToString();
            //using (SqlConnection connection = new SqlConnection(connStr))
            //{
            //    connection.Open();
            //    SqlCommand command = connection.CreateCommand();
            //    SqlTransaction transaction;
            //    transaction = connection.BeginTransaction("SampleTransaction");
            //    command.Connection = connection;
            //    command.Transaction = transaction;
            //    command.Parameters.Add("@PriceCompetitorID", SqlDbType.VarChar);
            //    command.Parameters.Add("@Date", SqlDbType.Date);
            //    command.Parameters.Add("@PMKSID", SqlDbType.VarChar);
            //    command.Parameters.Add("@CompetitorID", SqlDbType.Int);
            //    command.Parameters.Add("@Price", SqlDbType.Int);
            //    try
            //    {
            //        foreach (DataRow row in dataTable.Rows)
            //        {
            //            command.CommandText = @" INSERT INTO t_PriceCompetitor1
            //                                    (PriceCompetitorID, [Date], PMKSID, CompetitorID, Price)
            //                                 VALUES
            //                                    (@PriceCompetitorID, @Date, @PMKSID, @CompetitorID, @Price)
            //                                ";
            //            command.Parameters["@PriceCompetitorID"].Value = row["PriceCompetitorID"];
            //            command.Parameters["@Date"].Value = Date;
            //            command.Parameters["@PMKSID"].Value = row["PMKSID"];
            //            command.Parameters["@CompetitorID"].Value = row["CompetitorID"];
            //            command.Parameters["@Price"].Value = row["Price"];

            //            command.ExecuteNonQuery();
            //        }
            //        transaction.Commit();
            //        return false;
            //    }
            //    catch (Exception ex)
            //    {

            //        transaction.Rollback();
            //        throw ex;
            //    }
            //    finally
            //    {
            //        connection.Close();
            //    }
            //}
        }














        //string que1 = @" SELECT a.*,b.CompetitorName,ISNULL(u.FCName,'') as FCName, ISNULL(u1.FCName,'') as FCName
        //                        FROM t_PriceCompetitor a
        //                        INNER JOIN tblCompetitor b
        //                            ON a.CompetitorID=b.CompetitorID
        //                        LEFT JOIN t_User u
        //                            ON a.FCCreatedBy=u.FCUserID
        //                        LEFT JOIN t_User u1
        //                            ON a.FCUpdatedBy=u1.FCUserID
        //                        WHERE a.[Date]=@Date and ISNULL(a.FCDefunctInd,0)=0";

        //que1 += " ORDER BY a.PMKSID";



        //using (IDbConnection db = DALSecurity.GetSqlConnection)
        //{
        //    if (db.State == ConnectionState.Closed)
        //        db.Open();
        //    int result = db.Execute("sp_CRUDPriceCompetitorTarikData",
        //                    new
        //                    {
        //                        action = "TARIKDATA"
        //                        , Date = Date
        //                        , AccessID = ACCESSID
        //                    }, commandType: CommandType.StoredProcedure);
        //    return result != 0;
        //}

    }
}
