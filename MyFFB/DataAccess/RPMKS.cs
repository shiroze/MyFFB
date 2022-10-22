using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyAttd.DataAccess
{
    public class RPMKS : IRPMKS
    {
        public RPMKS(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        //public int Add(T_PMKS obj)
        public string Add(T_PMKS obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                DynamicParameters p = new DynamicParameters();
                //p.Add("@FCSAPID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.AddDynamicParams(new { action = "ADD",
                                        PMKSID = obj.PMKSID,
                                        AreaOperational = obj.AreaOperational,
                                        Regional = obj.Regional,
                                        PMKSName = obj.PMKSName,
                                        Company = obj.Company,
                                        KomidelMin = obj.KomidelMin,
                                        HargaMin = obj.HargaMin,
                                        KomidelPlus = obj.KomidelPlus,
                                        HargaPlus = obj.HargaPlus,
                                        CompanyCode = obj.CompanyCode,
                                        BusinessArea = obj.BusinessArea,
                                        BusinessAreaCoP = obj.BusinessAreaCoP,
                                        PMKSGroup = obj.PMKSGroup,
                                        HouseBank = obj.HouseBank,
                                        PaySaturday = obj.PaySaturday,
                                        KomidelMin1 = obj.KomidelMin1,
                                        HargaMin1 = obj.HargaMin1,
                                        KomidelPlus1 = obj.KomidelPlus1,
                                        HargaPlus1 = obj.HargaPlus1,
                                        KomidelMin2 = obj.KomidelMin2,
                                        HargaMin2 = obj.HargaMin2,
                                        KomidelPlus2 = obj.KomidelPlus2,
                                        HargaPlus2 = obj.HargaPlus2,
                                        AccessID = obj.AccessID,
                                       });
                db.Execute("sp_CRUDPMKS", p, commandType: CommandType.StoredProcedure);
                return p.Get<string>("@PMKSID");
            }
        }

        //public bool Defunct(T_Menu obj, bool stsDefunct)
        //{
        //    using (IDbConnection db = DALSecurity.GetSqlConnection)
        //    {
        //        if (db.State == ConnectionState.Closed)
        //            db.Open();

        //        string act;
        //        if (stsDefunct)
        //            act = "DEFUNCT";
        //        else
        //            act = "UNDEFUNCT";

        //        int result = db.Execute("sp_CRUDMenu", new { action = act, FCMENUID = obj.FCMenuID, FCMENUCODE = obj.FCMenuCode, FCMENUDESC = obj.FCMenuDesc, FCMENUPID = obj.FCMenuPID, FCMENULINK = obj.FCMenuLink, FCORDERNO = obj.FCOrderNo, FCICON = obj.FCIcon, FCHIDDEN = obj.FCHidden, ACCESSID = obj.AccessID }, commandType: CommandType.StoredProcedure);
        //        return result != 0;
        //    }
        //}

        public bool Delete(T_PMKS obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDPMKS", new { action = "DELETE",
                                                                PMKSID = obj.PMKSID,
                                                                AreaOperational = obj.AreaOperational,
                                                                Regional = obj.Regional,
                                                                PMKSName = obj.PMKSName,
                                                                Company = obj.Company,
                                                                KomidelMin = obj.KomidelMin,
                                                                HargaMin = obj.HargaMin,
                                                                KomidelPlus = obj.KomidelPlus,
                                                                HargaPlus = obj.HargaPlus,
                                                                CompanyCode = obj.CompanyCode,
                                                                BusinessArea = obj.BusinessArea,
                                                                BusinessAreaCoP = obj.BusinessAreaCoP,
                                                                PMKSGroup = obj.PMKSGroup,
                                                                HouseBank = obj.HouseBank,
                                                                PaySaturday = obj.PaySaturday,
                                                                KomidelMin1 = obj.KomidelMin1,
                                                                HargaMin1 = obj.HargaMin1,
                                                                KomidelPlus1 = obj.KomidelPlus1,
                                                                HargaPlus1 = obj.HargaPlus1,
                                                                KomidelMin2 = obj.KomidelMin2,
                                                                HargaMin2 = obj.HargaMin2,
                                                                KomidelPlus2 = obj.KomidelPlus2,
                                                                HargaPlus2 = obj.HargaPlus2,
                                                                AccessID = obj.AccessID,
                                                            }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_PMKS obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDPMKS", 
                                new { action = "EDIT",
                                PMKSID = obj.PMKSID,
                                AreaOperational = obj.AreaOperational,
                                Regional = obj.Regional,
                                PMKSName = obj.PMKSName,
                                Company = obj.Company,
                                KomidelMin = obj.KomidelMin,
                                HargaMin = obj.HargaMin,
                                KomidelPlus = obj.KomidelPlus,
                                HargaPlus = obj.HargaPlus,
                                CompanyCode = obj.CompanyCode,
                                BusinessArea = obj.BusinessArea,
                                BusinessAreaCoP = obj.BusinessAreaCoP,
                                PMKSGroup = obj.PMKSGroup,
                                HouseBank = obj.HouseBank,
                                PaySaturday = obj.PaySaturday,
                                KomidelMin1 = obj.KomidelMin1,
                                HargaMin1 = obj.HargaMin1,
                                KomidelPlus1 = obj.KomidelPlus1,
                                HargaPlus1 = obj.HargaPlus1,
                                KomidelMin2 = obj.KomidelMin2,
                                HargaMin2 = obj.HargaMin2,
                                KomidelPlus2 = obj.KomidelPlus2,
                                HargaPlus2 = obj.HargaPlus2,
                                AccessID = obj.AccessID,
                            }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Approve(T_PMKS obj, bool stsApprove)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                string act;
                if (stsApprove)
                    act = "APPROVE";
                else
                    act = "UNAPPROVE";
                int result = db.Execute("sp_CRUDPMKS", new
                {
                    action = act,
                    PMKSID = obj.PMKSID,
                    AreaOperational = obj.AreaOperational,
                    Regional = obj.Regional,
                    PMKSName = obj.PMKSName,
                    Company = obj.Company,
                    KomidelMin = obj.KomidelMin,
                    HargaMin = obj.HargaMin,
                    KomidelPlus = obj.KomidelPlus,
                    HargaPlus = obj.HargaPlus,
                    CompanyCode = obj.CompanyCode,
                    BusinessArea = obj.BusinessArea,
                    BusinessAreaCoP = obj.BusinessAreaCoP,
                    PMKSGroup = obj.PMKSGroup,
                    HouseBank = obj.HouseBank,
                    PaySaturday = obj.PaySaturday,
                    KomidelMin1 = obj.KomidelMin1,
                    HargaMin1 = obj.HargaMin1,
                    KomidelPlus1 = obj.KomidelPlus1,
                    HargaPlus1 = obj.HargaPlus1,
                    KomidelMin2 = obj.KomidelMin2,
                    HargaMin2 = obj.HargaMin2,
                    KomidelPlus2 = obj.KomidelPlus2,
                    HargaPlus2 = obj.HargaPlus2,
                    AccessID = obj.AccessID,
                }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        //public T_PMKS FindPMKS(string AreaOperational, string Regional, string PMKSID)
        public T_PMKS FindPMKS(string PMKSID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT * FROM tblPMKS
                                WHERE 1=1 ";


                if (PMKSID != null)
                    que += "  AND PMKSID=@PMKSID ";

                return db.Query<T_PMKS>(que, new { PMKSID= PMKSID }, commandType: CommandType.Text).SingleOrDefault();
            }
        }

        public List<T_PMKS> GetListPMKS(bool? defunct, int FCUserID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" DECLARE @setpmksid varchar(max),@pos int, @name varchar(25)
                                SELECT @setpmksid=SetPMKSID from t_user WHERE FCUserID=@FCUserID

                                SELECT * FROM tblPMKS a
                                LEFT join t_User b ON a.FCCreatedBy=b.FCUserID
                                LEFT join t_User b1 ON a.FCUpdatedBy=b1.FCUserID
                                WHERE (a.PMKSID in (select * from dbo.splitstring(@setpmksid)) or isnull(@setpmksid,'')='')";
                //if (defunct != null)
                //    que += " AND ISNULL(a.FCDEFUNCTIND,0)=@FCDEFUNCTIND ";

                //if (PMKSID != null)
                //    que += " AND a.PMKSID=@PMKSID ";
                //if (AreaOperational != null && AreaOperational !="")
                //    que += " AND a.AreaOperational=@AreaOperational";
                //if (Regional != null && Regional !="")
                //    que += " AND a.Regional=@Regional";


                que += " ORDER BY a.PMKSID";


                var result = db.Query<T_PMKS,T_User, T_User, T_PMKS>(que, (a,b,c) =>
               {
                   //a.RootMenu = b;
                   a.CreatedBy = b;
                   a.UpdatedBy = c;
                   return a;
               }, splitOn: "FCUserID,FCUserID", param: new { FCDEFUNCTIND = defunct, FCUserID=FCUserID }).ToList();
                return result;
            }
        }
    }
}
