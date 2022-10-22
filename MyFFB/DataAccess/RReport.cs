using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;
using System.Data.SqlClient;




using System.Threading.Tasks;

using MyAttd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Reporting.NETCore;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MyAttd.DataAccess
{
    public class RReport : IRReport
    {
        public RReport(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        public DataTable RptFFBReceive(int AccessID, T_Report.RptFFBRecap ParJson)
        {
            var connStr = DALSecurity.GetSqlConnection.ConnectionString;
            DataTable dataTable = new DataTable();
            //dynamic json = JsonConvert.DeserializeObject(FBBReceive);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("sp_RptFFBReceive", conn);
                
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@Type", SqlDbType.VarChar);
                //cmd.Parameters["@Type"].Value = ParJson.type;

                cmd.Parameters.Add("@PMKSID", SqlDbType.VarChar);
                cmd.Parameters["@PMKSID"].Value = ParJson.PMKS;

                cmd.Parameters.Add("@DateFrom", SqlDbType.Date);
                cmd.Parameters["@DateFrom"].Value = ParJson.dtF;

                cmd.Parameters.Add("@DateTo", SqlDbType.Date);
                cmd.Parameters["@DateTo"].Value = ParJson.dtT;

                cmd.Parameters.Add("@AccessID", SqlDbType.Int);
                cmd.Parameters["@AccessID"].Value = AccessID;

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

        public DataTable RptFFBRecap(int AccessID, T_Report.RptFFBRecap ParJson)
        {
            var connStr = DALSecurity.GetSqlConnection.ConnectionString;
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("sp_RptFFBRecap", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Type", SqlDbType.VarChar);
                cmd.Parameters["@Type"].Value = ParJson.type;
                    //json.type;

                cmd.Parameters.Add("@PMKSID", SqlDbType.VarChar);
                cmd.Parameters["@PMKSID"].Value = ParJson.PMKS;
                    //json.PMKS;

                cmd.Parameters.Add("@DateFrom", SqlDbType.Date);
                cmd.Parameters["@DateFrom"].Value = ParJson.dtF;
                    //json.dtF;

                cmd.Parameters.Add("@DateTo", SqlDbType.Date);
                cmd.Parameters["@DateTo"].Value = ParJson.dtT;
                //json.dtT;

                cmd.Parameters.Add("@AccessID", SqlDbType.Int);
                cmd.Parameters["@AccessID"].Value = AccessID;

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

        public DataTable RptCashAdvance(int AccessID, T_Report.RptCashAdvance ParJson)
        {
            var connStr = DALSecurity.GetSqlConnection.ConnectionString;
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("sp_RptCashAdvance", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@PMKSID", SqlDbType.VarChar);
                cmd.Parameters["@PMKSID"].Value = ParJson.PMKS;

                cmd.Parameters.Add("@DateFrom", SqlDbType.Date);
                cmd.Parameters["@DateFrom"].Value = ParJson.dtF;

                cmd.Parameters.Add("@DateTo", SqlDbType.Date);
                cmd.Parameters["@DateTo"].Value = ParJson.dtT;

                cmd.Parameters.Add("@AccessID", SqlDbType.Int);
                cmd.Parameters["@AccessID"].Value = AccessID;

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

        public DataTable RptFFBOutStanding(int AccessID, T_Report.RptFFBOutStanding ParJson)
        {
            var connStr = DALSecurity.GetSqlConnection.ConnectionString;
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("sp_RptFFBOutStanding", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@PMKSID", SqlDbType.VarChar);
                cmd.Parameters["@PMKSID"].Value = ParJson.PMKS;

                cmd.Parameters.Add("@DateFrom", SqlDbType.Date);
                cmd.Parameters["@DateFrom"].Value = ParJson.dtF;

                cmd.Parameters.Add("@DateTo", SqlDbType.Date);
                cmd.Parameters["@DateTo"].Value = ParJson.dtT;

                cmd.Parameters.Add("@AccessID", SqlDbType.Int);
                cmd.Parameters["@AccessID"].Value = AccessID;

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
                conn.Close();
                da.Dispose();

                return dataTable;
            }
        }

        public DataTable RptCashAdvanceRequest(int AccessID, T_Report.RptCashAdvanceRequest ParJson)
        {
            //var coba = RptName;
            var connStr = DALSecurity.GetSqlConnection.ConnectionString;
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("sp_RptCashAdvanceRequest", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@PMKSID", SqlDbType.VarChar);
                cmd.Parameters["@PMKSID"].Value = ParJson.PMKS;

                cmd.Parameters.Add("@DateFrom", SqlDbType.VarChar);
                cmd.Parameters["@DateFrom"].Value = ParJson.dtF;

                cmd.Parameters.Add("@AccessID", SqlDbType.Int);
                cmd.Parameters["@AccessID"].Value = AccessID;

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
                conn.Close();
                da.Dispose();

                return dataTable;
            }
        }


        public T_Report.Set_RptName FindSetRptName(int AccessID,string ReportID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que =
                    @" 
                    SELECT * FROM set_RptName WHERE FCUserID=@AccessID and ReportID=@ReportID
                    ";
                var result = db.Query<T_Report.Set_RptName>(que,param: new { AccessID = AccessID, ReportID= ReportID }).SingleOrDefault();
                return result;
            }
        }

        public DataTable RptTransfer(int AccessID, T_Report.RptTransfer ParJson)
        {
            var connStr = DALSecurity.GetSqlConnection.ConnectionString;
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("sp_RptTransfer", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@PMKSID", SqlDbType.VarChar);
                cmd.Parameters["@PMKSID"].Value = ParJson.PMKS;

                cmd.Parameters.Add("@DateFrom", SqlDbType.VarChar);
                cmd.Parameters["@DateFrom"].Value = ParJson.dtF;

                cmd.Parameters.Add("@DateTo", SqlDbType.VarChar);
                cmd.Parameters["@DateTo"].Value = ParJson.dtT;

                cmd.Parameters.Add("@AccessID", SqlDbType.Int);
                cmd.Parameters["@AccessID"].Value = AccessID;

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
                conn.Close();
                da.Dispose();

                return dataTable;
            }
        }

        //public DataTable RptPriceIndicator(int AccessID, T_Report.RptPriceIndicator ParJson)
        //{
        //    var connStr = DALSecurity.GetSqlConnection.ConnectionString;
        //    DataTable dataTable = new DataTable();

        //    using (SqlConnection conn = new SqlConnection(connStr))
        //    {
        //        SqlCommand cmd = new SqlCommand("sp_RptPriceIndicator", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.Add("@PMKSID", SqlDbType.VarChar);
        //        cmd.Parameters["@PMKSID"].Value = ParJson.PMKS;

        //        cmd.Parameters.Add("@DateFrom", SqlDbType.VarChar);
        //        cmd.Parameters["@DateFrom"].Value = ParJson.dtF;

        //        //cmd.Parameters.Add("@DateTo", SqlDbType.VarChar);
        //        //cmd.Parameters["@DateTo"].Value = ParJson.dtT;

        //        cmd.Parameters.Add("@AccessID", SqlDbType.Int);
        //        cmd.Parameters["@AccessID"].Value = AccessID;

        //        conn.Open();
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dataTable);
        //        conn.Close();
        //        da.Dispose();

        //        return dataTable;
        //    }
        //}


        public DataTable RptPriceIndicator(int AccessID, T_Report.RptPriceIndicator ParJson)
        {
            var connStr = DALSecurity.GetSqlConnection.ConnectionString;
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM [dbo].[fn_rpt_price_indicator] (@loc, @date, @AccessID)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("@loc", SqlDbType.VarChar);
                cmd.Parameters["@loc"].Value = ParJson.PMKS;

                //cmd.Parameters.Add("@date", SqlDbType.VarChar);

                //string date_to = string.Concat(ParJson.dtF, "-01");
                //DateTime dt = DateTime.Parse(date_to);
                //DateTime eom = dt.AddMonths(1).AddDays(-1);
                //cmd.Parameters["@date"].Value = eom.ToString("yyyy-MM-dd");

                cmd.Parameters.Add("@date", SqlDbType.VarChar);
                cmd.Parameters["@date"].Value = ParJson.dtT;

                cmd.Parameters.Add("@AccessID", SqlDbType.Int);
                cmd.Parameters["@AccessID"].Value = AccessID;

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
                conn.Close();
                da.Dispose();

                return dataTable;
            }
        }

        public DataTable RptPriceAverage(int AccessID, T_Report.RptPriceAverage ParJson)
        {
            var connStr = DALSecurity.GetSqlConnection.ConnectionString;
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("sp_RptPriceAverage", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Regional", SqlDbType.VarChar);
                cmd.Parameters["@Regional"].Value = ParJson.Regional;

                cmd.Parameters.Add("@DateFrom", SqlDbType.VarChar);
                cmd.Parameters["@DateFrom"].Value = ParJson.dtF;

                cmd.Parameters.Add("@AccessID", SqlDbType.Int);
                cmd.Parameters["@AccessID"].Value = AccessID;

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
                conn.Close();
                da.Dispose();

                return dataTable;
            }
        }

        public DataTable RptDetailListPayment(int AccessID, T_Report.RptDetailListPayment ParJson)
        {
            var connStr = DALSecurity.GetSqlConnection.ConnectionString;
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("sp_RptDetailListPayment", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@PMKSID", SqlDbType.VarChar);
                cmd.Parameters["@PMKSID"].Value = ParJson.PMKSID;

                cmd.Parameters.Add("@SupplierID", SqlDbType.VarChar);
                cmd.Parameters["@SupplierID"].Value = ParJson.SupplierID;

                cmd.Parameters.Add("@DateFrom", SqlDbType.VarChar);
                cmd.Parameters["@DateFrom"].Value = ParJson.dtF;

                cmd.Parameters.Add("@DateTo", SqlDbType.VarChar);
                cmd.Parameters["@DateTo"].Value = ParJson.dtT;

                cmd.Parameters.Add("@AccessID", SqlDbType.Int);
                cmd.Parameters["@AccessID"].Value = AccessID;

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
                conn.Close();
                da.Dispose();

                return dataTable;
            }
        }

        public DataTable RptMenuRole(int FCRoleID)
        {
            var connStr = DALSecurity.GetSqlConnection.ConnectionString;
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("sp_RptMenuRole", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Action", SqlDbType.VarChar);
                cmd.Parameters["@Action"].Value = "MenuRole";

                cmd.Parameters.Add("@FCRoleID", SqlDbType.Int);
                cmd.Parameters["@FCRoleID"].Value = FCRoleID;

                //cmd.Parameters.Add("@AccessID", SqlDbType.Int);
                //cmd.Parameters["@AccessID"].Value = AccessID;

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
                conn.Close();
                da.Dispose();

                return dataTable;
            }
        }

        public DataTable RptUserRole(int FCRoleID)
        {
            var connStr = DALSecurity.GetSqlConnection.ConnectionString;
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("sp_RptMenuRole", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Action", SqlDbType.VarChar);
                cmd.Parameters["@Action"].Value = "UserRole";

                cmd.Parameters.Add("@FCRoleID", SqlDbType.Int);
                cmd.Parameters["@FCRoleID"].Value = FCRoleID;

                //cmd.Parameters.Add("@AccessID", SqlDbType.Int);
                //cmd.Parameters["@AccessID"].Value = AccessID;

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
                conn.Close();
                da.Dispose();

                return dataTable;
            }
        }

    }
}
