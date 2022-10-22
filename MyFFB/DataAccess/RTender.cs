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
    public class RTender : IRTender
    {
        public RTender(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }


        public List<T_Tender> GetListTender(bool? defunct, string tgl)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" --DECLARE @Periode date
                                SELECT 
                                    a.ProductID,a.Region,CAST(a.DateTender as DATE) as DateTender
                                    ,a.Remarks,a.Price,a.UserID,a.LastAccess,a.FCCreatedDT,a.FCUpdatedDT,u.*,u1.*
                                FROM tblTender a
                                    LEFT JOIN t_User u ON a.FCCreatedBy=u.FCUserID
                                    LEFT JOIN t_User u1 ON a.FCUpdatedBy=u1.FCUserID
                                WHERE CAST(a.DateTender as DATE)=@tgl
                                ORDER BY CAST(a.DateTender as DATE),a.ProductID,a.Region
                            ";

                var result = db.Query<T_Tender, T_User, T_User, T_Tender>(que, (a, b, c) =>
                   {
                       a.CreatedBy = b;
                       a.UpdateBy = c;
                       return a;
                   }, splitOn: "FCUserID,FCUserID", param: new { FCDEFUNCTIND = defunct, tgl = tgl }).ToList();
                return result;
            }
        }

        public T_Tender FindTender(string p_date, string p_product, string p_region)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                string que =
                    @"
                    SELECT 
                        a.ProductID,a.Region,CAST(a.DateTender as DATE) as DateTender,
                        a.Remarks,a.Price,a.UserID,a.LastAccess,a.FCCreatedDT,a.FCUpdatedDT,
                        ISNULL(u.FCName,'') as FCName,ISNULL(u1.FCName,'') as FCName
                    FROM tblTender a
                        LEFT JOIN t_User u ON a.FCCreatedBy=u.FCUserID
                        LEFT JOIN t_User u1 ON a.FCUpdatedBy=u1.FCUserID
                    WHERE a.DateTender=@Periode
                            AND ProductID=@ProductID AND Region=@Region
                    ";
                var result = db.Query<T_Tender, T_User, T_User, T_Tender>(que, (a, b, c) =>
                {
                    a.CreatedBy = b;
                    a.UpdateBy = c;
                    return a;
                }, splitOn: "FCName,FCName", param: new { Periode = p_date, ProductID = p_product, Region = p_region }).SingleOrDefault();
                return result;
            }
        }

        public bool Add(T_Tender obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDTender",
                        new
                        {
                            action = "ADD",
                            DateTender = obj.DateTender,
                            ProductID = obj.ProductID.ToString(),
                            Region = obj.Region,
                            Price = obj.Price,
                            Remarks = obj.Remarks,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Delete(T_Tender obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDTender",
                        new
                        {
                            action = "DELETE",
                            DateTender = obj.DateTender,
                            ProductID = obj.ProductID.ToString(),
                            Region = obj.Region,
                            Price = obj.Price,
                            Remarks = obj.Remarks,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_Tender obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDTender",
                        new
                        {
                            action = "EDIT",
                            DateTender = obj.DateTender,
                            ProductID = obj.ProductID.ToString(),
                            Region = obj.Region,
                            Price = obj.Price,
                            Remarks = obj.Remarks,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool TarikData(string Date,bool? Pilih, int ACCESSID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDTenderTarikData",
                                new
                                {
                                    action = "TARIKDATA",
                                    Tanggal = Date,
                                    Pilih = Pilih,
                                    AccessID = ACCESSID
                                }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }
    }
}
