using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyAttd.DataAccess
{
    public class RCashAdvance : IRCashAdvance
    {
        public RCashAdvance(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        public T_CashAdvance FindCashAdvance(string p_CashNo)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT * FROM tblCashAdvance
                                WHERE CashNo=@CashNO
                             ";

                return db.Query<T_CashAdvance>(que, new { CashNO= p_CashNo}, commandType: CommandType.Text).SingleOrDefault();
            }
        }

        public List<T_CashAdvance> GetListCashAdvance(bool? defunct, string p_Period, int FCUserID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" DECLARE @setpmksid varchar(max),@pos int, @name varchar(25)
                                SELECT @setpmksid=SetPMKSID from t_user WHERE FCUserID=@FCUserID

                                SELECT * FROM tblCashAdvance
                                WHERE [Period]=@Period and (PMKSID in (select * from dbo.splitstring(@setpmksid)) or isnull(@setpmksid,'')='')";

                que += " ORDER BY CashNo";


                var result = db.Query<T_CashAdvance>(que, param: new { Period= p_Period, FCUserID=FCUserID }).ToList();
                return result;
            }
        }

        public List<T_CashAdvance> GetListWeek()
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT * FROM t_ListWeek
                             ";

                que += " ORDER BY Week";


                var result = db.Query<T_CashAdvance>(que, null).ToList();
                return result;
            }
        }

        public string Add(T_CashAdvance obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                DynamicParameters p = new DynamicParameters();
                //p.Add("@CashNo", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.AddDynamicParams(
                    new { action = "ADD"
                        , CashNo = obj.CashNo
                        , Period = obj.Period.Replace("-","")
                        , Tanggal=obj.Tanggal
                        , PMKSID = obj.PMKSID
                        , SupplierID = obj.SupplierID
                        , SupplierName = obj.SupplierName
                        , Code = obj.Code
                        , Description=obj.Description
                        , Amount=obj.Amount
                        , DeductAmount=obj.DeductAmount
                        , Week = obj.Week
                        , ACCESSID = obj.AccessID });
                db.Execute("sp_CRUDCashAdvance", p, commandType: CommandType.StoredProcedure);
                return p.Get<string>("@CashNo");
            }
        }

        public bool Edit(T_CashAdvance obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = 
                    db.Execute("sp_CRUDCashAdvance"
                        , new { 
                                action = "EDIT"
                                , CashNo = obj.CashNo
                                , Period = obj.Period.Replace("-","")
                                , Tanggal=obj.Tanggal
                                , PMKSID = obj.PMKSID
                                , SupplierID = obj.SupplierID
                                , SupplierName = obj.SupplierName
                                , Code = obj.Code
                                , Description=obj.Description
                                , Amount=obj.Amount
                                , DeductAmount=obj.DeductAmount
                                , Week = obj.Week
                                , ACCESSID = obj.AccessID
                            }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Delete(T_CashAdvance obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDCashAdvance"
                        , new
                        {
                            action = "DELETE",
                            CashNo = obj.CashNo,
                            Period = obj.Period.Replace("-", ""),
                            Tanggal = obj.Tanggal,
                            PMKSID = obj.PMKSID,
                            SupplierID = obj.SupplierID,
                            SupplierName = obj.SupplierName,
                            Code = obj.Code,
                            Description = obj.Description,
                            Amount = obj.Amount,
                            DeductAmount = obj.DeductAmount,
                            Week = obj.Week,
                            ACCESSID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        //public int CalculateCashAdvance(int AccessID, string p_tglTimbang )
        //{
        //    using (IDbConnection db = DALSecurity.GetSqlConnection)
        //    {
        //        if (db.State == ConnectionState.Closed)
        //            db.Open();
        //        int result = db.Execute("sp_CRUDCashAdvance",
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
