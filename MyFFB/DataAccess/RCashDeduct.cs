using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyAttd.DataAccess
{
    public class RCashDeduct : IRCashDeduct
    {
        public RCashDeduct(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        public T_FFB FindFFB(string p_ticket)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT * FROM tblFFB
                                WHERE Ticket=@Ticket";

                return db.Query<T_FFB>(que, new {Ticket = p_ticket}, commandType: CommandType.Text).SingleOrDefault();
            }
        }

        public List<T_FFB> GetListFFB(bool? defunct,string p_tglTimbang, string CashNo)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" DECLARE @PMKSID VARCHAR(50),@SupplierID varchar(10),@Tanggal date
                                SELECT @PMKSID=PMKSID,@SupplierID=SupplierID,@Tanggal=Tanggal 
                                    FROM tblCashAdvance where CashNo=@CashNo
                                --SELECT @PMKSID,@SupplierID,@Tanggal
                                SELECT * FROM tblFFB
                                WHERE PMKSID=@PMKSID and Supplier=@SupplierID
                                and CAST(TanggalTimbang AS Date)=@TanggalTimbang
                                --and CAST(TanggalTimbang AS Date)>=@Tanggal
                                --and CAST(TanggalTimbang AS Date)<=EOMONTH(@Tanggal)
                                and (ISNULL(Number,'')='' or Number=@CashNo)
                                ";

                que += " ORDER BY ticket";


                var result = db.Query<T_FFB>(que, param: new { TanggalTimbang= p_tglTimbang, CashNo= CashNo }).ToList();
                return result;
            }
        }


        public bool Edit(T_FFB obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = 
                    db.Execute("sp_CRUDCashDeduct"
                        , new { 
                                action = "EDIT"
                                , Ticket = obj.Ticket
                                , Number = obj.Number
                                , TotalPembayaran=obj.TotalPembayaran
                                , RealisasiPanjarAmount=obj.RealisasiPanjarAmount
                                , ACCESSID = obj.AccessID
                            }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public string GetTotalMain(string CashNo)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                string que = @" SELECT Amount,DeductAmount 
                                FROM tblCashAdvance 
                                WHERE CashNo=@CashNo
                                ";


                var result = db.Query(que, param: new { CashNo = CashNo }).ToList();
                return JsonConvert.SerializeObject(result);
            }
        }
        //public int CalculateCashDeduct(int AccessID, string p_tglTimbang )
        //{
        //    using (IDbConnection db = DALSecurity.GetSqlConnection)
        //    {
        //        if (db.State == ConnectionState.Closed)
        //            db.Open();
        //        int result = db.Execute("sp_CRUDCashDeduct",
        //                        new
        //                        {
        //                            action = "CALCULATE"
        //                            , TanggalTimbang= p_tglTimbang
        //                            , AccessID= AccessID
        //                        }, commandType: CommandType.StoredProcedure);
        //        return result;
        //    }
        //}
    }
}
