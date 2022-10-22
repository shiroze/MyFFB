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
    public class RSetReport : IRSetReport
    {
        public RSetReport(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }


        public List<T_SetReport> GetListSetReport(bool? defunct, int AccessID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que =
                    @"
                    SELECT 
	                    a.*,ISNULL(u.FCName,'')as FCName,ISNULL(u1.FCName,'')as FCName
	                    FROM set_RptName a
                    LEFT JOIN t_User u ON a.FCCreatedBy=u.FCUserID
                    LEFT JOIN t_User u1 ON a.FCUpdatedBy=u1.FCUserID
                    WHERE a.FCUserID=@AccessID
                    ";

                var result = db.Query<T_SetReport, T_User, T_User, T_SetReport>(que, (a, b, c) =>
                   {
                       a.CreatedBy = b;
                       a.UpdateBy = c;
                       return a;
                   }, splitOn: "FCName,FCName", param: new { FCDEFUNCTIND = defunct, AccessID = AccessID }).ToList();
                return result;
            }
        }

        public T_SetReport FindSetReport(int AccessID, string ReportID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                string que =
                    @"
                    SELECT 
	                    a.*,ISNULL(u.FCName,'')as FCName,ISNULL(u1.FCName,'')as FCName
	                    FROM set_RptName a
                    LEFT JOIN t_User u ON a.FCCreatedBy=u.FCUserID
                    LEFT JOIN t_User u1 ON a.FCUpdatedBy=u1.FCUserID
                    WHERE a.FCUserID=@AccessID and a.ReportID=@ReportID
                    ";
                var result = db.Query<T_SetReport, T_User, T_User, T_SetReport>(que, (a, b, c) =>
                {
                    a.CreatedBy = b;
                    a.UpdateBy = c;
                    return a;
                }, splitOn: "FCName,FCName", param: new { AccessID = AccessID, ReportID = ReportID }).SingleOrDefault();
                return result;
            }
        }

        public bool Add(T_SetReport obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDSetReport",
                        new
                        {
                            action = "ADD",
                            ReportID = obj.ReportID,
                            Name1 = obj.Name1,
                            Name2 = obj.Name2,
                            Name3 = obj.Name3,
                            Name4 = obj.Name4,
                            Name5 = obj.Name5,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Delete(T_SetReport obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDSetReport",
                        new
                        {
                            action = "DELETE",
                            ReportID = obj.ReportID,
                            Name1 = obj.Name1,
                            Name2 = obj.Name2,
                            Name3 = obj.Name3,
                            Name4 = obj.Name4,
                            Name5 = obj.Name5,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_SetReport obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDSetReport",
                        new
                        {
                            action = "EDIT",
                            ReportID = obj.ReportID,
                            Name1 = obj.Name1,
                            Name2 = obj.Name2,
                            Name3 = obj.Name3,
                            Name4 = obj.Name4,
                            Name5 = obj.Name5,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }
    }
}
