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
    public class RTransport : IRTransport
    {
        public RTransport(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }


        public List<T_Transport> GetListTransport(bool? defunct, string Periode)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que =
                    @"
                    SELECT
                        a.ProductID,a.PMKSID,CAST(TransportDate as DATE) as TransportDate,
                        a.Price,a.Destination,a.UserID,a.LastAccess,
                        a.FCCreatedDT,a.FCUpdatedDT,u.*,u1.*
                    FROM tblTransport a
                        LEFT JOIN t_User u ON a.FCCreatedBy=u.FCUserID
                        LEFT JOIN t_User u1 ON a.FCUpdatedBy=u1.FCUserID
                    WHERE CAST(TransportDate as DATE)=@Periode
                    ORDER BY CAST(TransportDate as DATE),ProductID,PMKSID
                    ";

                var result = db.Query<T_Transport, T_User, T_User, T_Transport>(que, (a, b, c) =>
                {
                    a.CreatedBy = b;
                    a.UpdateBy = c;
                    return a;
                }, splitOn: "FCUserID,FCUserID", param: new { FCDEFUNCTIND = defunct, Periode = Periode }).ToList();
                return result;
            }
        }

        public T_Transport FindTransport(string p_date, string p_product, string p_pmks)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                string que =
                    @"
                    SELECT 
                        a.ProductID,a.PMKSID,CAST(TransportDate as DATE) as TransportDate,
                        a.Price,a.Destination,a.UserID,a.LastAccess,a.FCCreatedDT,a.FCUpdatedDT,
                        ISNULL(u.FCName,'') as FCName,ISNULL(u1.FCName,'') as FCName
                    FROM tblTransport a
                        LEFT JOIN t_User u ON a.FCCreatedBy=u.FCUserID
                        LEFT JOIN t_User u1 ON a.FCUpdatedBy=u1.FCUserID
                    WHERE a.TransportDate=@Periode
                            AND ProductID=@ProductID AND PMKSID=@PMKSID
                    ";
                var result = db.Query<T_Transport, T_User, T_User, T_Transport>(que, (a, b, c) =>
                {
                    a.CreatedBy = b;
                    a.UpdateBy = c;
                    return a;
                }, splitOn: "FCName,FCName", param: new { Periode = p_date, ProductID = p_product, PMKSID = p_pmks }).SingleOrDefault();
                return result;
            }
        }

        public bool Add(T_Transport obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDTransport",
                        new
                        {
                            action = "ADD",
                            TransportDate = obj.TransportDate,
                            ProductID = obj.ProductID.ToString(),
                            PMKSID = obj.PMKSID,
                            Price = obj.Price,
                            Destination = obj.Destination,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Delete(T_Transport obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDTransport",
                        new
                        {
                            action = "DELETE",
                            TransportDate = obj.TransportDate,
                            ProductID = obj.ProductID.ToString(),
                            PMKSID = obj.PMKSID,
                            Price = obj.Price,
                            Destination = obj.Destination,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_Transport obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDTransport",
                        new
                        {
                            action = "EDIT",
                            TransportDate = obj.TransportDate,
                            ProductID = obj.ProductID.ToString(),
                            PMKSID = obj.PMKSID,
                            Price = obj.Price,
                            Destination = obj.Destination,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool TarikData(string Date, bool? Pilih, int ACCESSID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDTransportTarikData",
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
