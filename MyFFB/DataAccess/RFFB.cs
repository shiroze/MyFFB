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
    public class RFFB : IRFFB
    {
        public RFFB(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        public T_FFB FindFFB(string p_tglTimbang,string p_ticket)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT * FROM tblFFB
                                WHERE CAST(TanggalTimbang AS Date )=@TanggalTimbang and Ticket=@Ticket";

                return db.Query<T_FFB>(que, new { TanggalTimbang=p_tglTimbang,Ticket = p_ticket}, commandType: CommandType.Text).SingleOrDefault();
            }
        }

        public List<T_FFB> GetListFFB(bool? defunct, string p_tglTimbang, int FCUserID, string fil_pmks)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" DECLARE @setpmksid varchar(max)--,@pos int, @name varchar(25)
                                SELECT @setpmksid=SetPMKSID from t_user WHERE FCUserID=@FCUserID

                                SELECT * FROM tblFFB
                                WHERE CAST(TanggalTimbang AS Date )=@TanggalTimbang
                                        AND (PMKSID in (select * from dbo.splitstring(@setpmksid)) or isnull(@setpmksid,'')='')
                                ";
                if (fil_pmks != null && fil_pmks != "")
                    que += " AND PMKSID=@fil_pmks";

                que += " ORDER BY post2payment --PMKSID,Supplier";


                var result = db.Query<T_FFB>(que, param: new { TanggalTimbang= p_tglTimbang, FCUserID= FCUserID, fil_pmks= fil_pmks }).ToList();
                return result;
            }
        }

        public int CalculateFFB(int AccessID, string p_tglTimbang )
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDFFB",
                                new
                                {
                                    action = "CALCULATE", 
                                    TanggalTimbang= p_tglTimbang,
                                    TanggalPOST ="",
                                    Ticket = "", 
                                    AccessID= AccessID
                                }, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        public int DownloadFFB(int AccessID, string p_tglTimbang)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDFFB",
                                new
                                {
                                    action = "DOWNLOADFFB",
                                    TanggalTimbang = p_tglTimbang,
                                    TanggalPOST = "",
                                    Ticket = "",
                                    AccessID = AccessID
                                }, commandType: CommandType.StoredProcedure) ;
                return result;
            }
        }

        public int TotalFFBHarian(int AccessID, string p_tglTimbang)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = 
                    db.Execute("sp_CRUDFFB",
                        new
                        {
                            action = "TotalTRX",
                            TanggalTimbang = p_tglTimbang,
                            TanggalPOST = "",
                            Ticket = "",
                            AccessID = AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public bool Delete(T_FFB obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDFFB",
                                new
                                {
                                    action = "DELETE",
                                    TanggalTimbang = obj.TanggalTimbang.ToShortDateString(),
                                    TanggalPOST = obj.Post2Payment.ToShortDateString(),
                                    Ticket = obj.Ticket,
                                    AccessID = obj.AccessID
                                }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Post(T_FFB obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDFFB",
                                new
                                {
                                    action = "POST",
                                    TanggalTimbang = obj.TanggalTimbang.ToShortDateString(),
                                    TanggalPOST = obj.Post2Payment.ToShortDateString(),
                                    Ticket = obj.Ticket,
                                    AccessID = obj.AccessID
                                }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Unpost(T_FFB obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDFFB",
                                new
                                {
                                    action = "UNPOST",
                                    TanggalTimbang = obj.TanggalTimbang.ToShortDateString(),
                                    TanggalPOST = obj.Post2Payment.ToShortDateString(),
                                    Ticket = obj.Ticket,
                                    AccessID = obj.AccessID
                                }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

    }
}
