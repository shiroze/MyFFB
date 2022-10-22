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
    public class RPriceDefault : IRPriceDefault
    {
        public RPriceDefault(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }


        public List<T_PriceDefault> GetListPriceDefault()
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que =
                    @"
                    SELECT 
	                    a.SupplierID,a.SupplierName,a.Efective_Date,a.PMKSID,a.LastAccess,
	                    a.FCCreatedDT,a.FCUpdatedDT,u.*,u1.*
                    FROM tblPriceDefault a
	                    LEFT JOIN t_User u ON a.FCCreatedBy=u.FCUserID
	                    LEFT JOIN t_User u1 ON a.FCUpdatedBy=u1.FCUserID
                    ORDER BY a.Efective_Date,a.SupplierID,a.PMKSID
                    ";

                var result = db.Query<T_PriceDefault>(que,param: new {}).ToList();
                return result;
            }
        }

        public T_PriceDefault FindPriceDefault(string p_date, string p_supplier, string p_pmks)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                string que =
                    @"
                    SELECT 
	                    a.SupplierID,a.SupplierName,a.Efective_Date,a.PMKSID,a.LastAccess,
	                    a.FCCreatedDT,a.FCUpdatedDT,
	                    ISNULL(u.FCName,'') as FCName,ISNULL(u1.FCName,'') as FCName
                    FROM tblPriceDefault a
	                    LEFT JOIN t_User u ON a.FCCreatedBy=u.FCUserID
	                    LEFT JOIN t_User u1 ON a.FCUpdatedBy=u1.FCUserID
                    WHERE a.SupplierID=@SupplierID
	                    AND a.Efective_Date=@Efective_Date
	                    AND PMKSID=@PMKSID
                    ";
                var result = db.Query<T_PriceDefault, T_User, T_User, T_PriceDefault>(que, (a, b, c) =>
                {
                    a.CreatedBy = b;
                    a.UpdateBy = c;
                    return a;
                }, splitOn: "FCName,FCName", param: new { Efective_Date = p_date, SupplierID = p_supplier, PMKSID = p_pmks }).SingleOrDefault();
                return result;
            }
        }

        public List<T_AreaRegional> GetRegion()
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que =
                    @"
                    SELECT DISTINCT Regional
                    FROM t_AreaRegional
                    WHERE ISNULL(FCDefunctInd,0)=0
                    ";

                var result = db.Query<T_AreaRegional>(que, param: new { }).ToList();
                return result;
            }
        }

        public bool Add(T_PriceDefault obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDPriceDefault",
                        new
                        {
                            action = "ADD",
                            SupplierID = obj.SupplierID,
                            SupplierName = obj.SupplierName,
                            Efective_Date = obj.Efective_Date,
                            PMKSID = obj.PMKSID,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Delete(T_PriceDefault obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDPriceDefault",
                        new
                        {
                            action = "DELETE",
                            SupplierID = obj.SupplierID,
                            SupplierName = obj.SupplierName,
                            Efective_Date = obj.Efective_Date,
                            PMKSID = obj.PMKSID,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_PriceDefault obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDPriceDefault",
                        new
                        {
                            action = "EDIT",
                            SupplierID = obj.SupplierID,
                            SupplierName = obj.SupplierName,
                            Efective_Date = obj.Efective_Date,
                            PMKSID = obj.PMKSID,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

    }
}
