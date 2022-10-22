using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyAttd.DataAccess
{
    public class RBudget : IRBudget
    {
        public RBudget(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        public T_Budget FindBudget(string Periode,string PMKSID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que =
                    @"
                    SELECT a.*,ISNULL(u.FCName,'') as FCName,ISNULL(u1.FCName,'') as FCName
                    FROM tblBudget a
	                    LEFT JOIN t_User u ON a.FCCreatedBy=u.FCUserID
	                    LEFT JOIN t_User u1 ON a.FCUpdatedBy=u1.FCUserID
                    WHERE a.Periode=@Periode and a.PMKSID=@PMKSID
                    ";

                var result = db.Query<T_Budget, T_User, T_User, T_Budget>(que, (a, b, c) =>
                {
                    a.CreatedBy = b;
                    a.UpdateBy = c;
                    return a;
                }, splitOn: "FCName,FCName", param: new { Periode = Periode, PMKSID = PMKSID }).SingleOrDefault();
                return result;
            }
        }

        public List<T_Budget> GetListBudget(bool? defunct, string Periode, int FCUserID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que =
                    @" 
                    DECLARE @setpmksid varchar(max),@pos int, @name varchar(25)
                    SELECT @setpmksid=SetPMKSID from t_user WHERE FCUserID=@FCUserID
                    SELECT a.*,u.*,u1.*
                    FROM tblBudget a
	                    LEFT JOIN t_User u ON a.FCCreatedBy=u.FCUserID
	                    LEFT JOIN t_User u1 ON a.FCUpdatedBy=u1.FCUserID
                    WHERE Periode=@Periode and (a.PMKSID in (select * from dbo.splitstring(@setpmksid)) or isnull(@setpmksid,'')='')
                    ";

                que += " ORDER BY a.PMKSID";

                var result = db.Query<T_Budget, T_User, T_User, T_Budget>(que, (a, b, c) =>
                {
                    a.CreatedBy = b;
                    a.UpdateBy = c;
                    return a;
                }, splitOn: "FCUserID,FCUserID", param: new { FCDEFUNCTIND = defunct, Periode = Periode, FCUserID = FCUserID }).ToList();
                return result;
            }
        }

        public bool Add(T_Budget obj)
        {
            string coba = obj.Periode.Replace("-", "");
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDBudget",
                        new
                        {
                            action = "ADD",
                            PMKSID = obj.PMKSID ,
                            Periode = obj.Periode,
                            VolumeFFB_KG = obj.VolumeFFB_KG,
                            VolumeCPO_KG = obj.VolumeCPO_KG,
                            VolumePK_KG = obj.VolumePK_KG,
                            OER = obj.OER ,
                            KER = obj.KER ,
                            NetMargin_USD_MT_CPO = obj.NetMargin_USD_MT_CPO,
                            HK = obj.HK ,
                            ExchangeRate = obj.ExchangeRate,
                            Capacity = obj.Capacity,
                            ProduksiCangkang_KG = obj.ProduksiCangkang_KG ,
                            BakarCangkang_KG = obj.BakarCangkang_KG,
                            EBITDA_Cangkang = obj.EBITDA_Cangkang,
                            ProduksiBA_KG = obj.ProduksiBA_KG,
                            Price_BunchAsh = obj.Price_BunchAsh,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Delete(T_Budget obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDBudget",
                        new
                        {
                            action = "DELETE",
                            PMKSID = obj.PMKSID,
                            Periode = obj.Periode,
                            VolumeFFB_KG = obj.VolumeFFB_KG,
                            VolumeCPO_KG = obj.VolumeCPO_KG,
                            VolumePK_KG = obj.VolumePK_KG,
                            OER = obj.OER,
                            KER = obj.KER,
                            NetMargin_USD_MT_CPO = obj.NetMargin_USD_MT_CPO,
                            HK = obj.HK,
                            ExchangeRate = obj.ExchangeRate,
                            Capacity = obj.Capacity,
                            ProduksiCangkang_KG = obj.ProduksiCangkang_KG,
                            BakarCangkang_KG = obj.BakarCangkang_KG,
                            EBITDA_Cangkang = obj.EBITDA_Cangkang,
                            ProduksiBA_KG = obj.ProduksiBA_KG,
                            Price_BunchAsh = obj.Price_BunchAsh,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_Budget obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDBudget",
                        new
                        {
                            action = "EDIT",
                            PMKSID = obj.PMKSID,
                            Periode = obj.Periode,
                            VolumeFFB_KG = obj.VolumeFFB_KG,
                            VolumeCPO_KG = obj.VolumeCPO_KG,
                            VolumePK_KG = obj.VolumePK_KG,
                            OER = obj.OER,
                            KER = obj.KER,
                            NetMargin_USD_MT_CPO = obj.NetMargin_USD_MT_CPO,
                            HK = obj.HK,
                            ExchangeRate = obj.ExchangeRate,
                            Capacity = obj.Capacity,
                            ProduksiCangkang_KG = obj.ProduksiCangkang_KG,
                            BakarCangkang_KG = obj.BakarCangkang_KG,
                            EBITDA_Cangkang = obj.EBITDA_Cangkang,
                            ProduksiBA_KG = obj.ProduksiBA_KG,
                            Price_BunchAsh = obj.Price_BunchAsh,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }
    }
}
