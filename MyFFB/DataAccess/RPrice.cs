using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyAttd.DataAccess
{
    public class RPrice : IRPrice
    {
        public RPrice(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        //public int Add(T_Price obj)
        public string Add(T_Price obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                DynamicParameters p = new DynamicParameters();
                //p.Add("@FCSAPID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.AddDynamicParams(new { action = "ADD"
                                        ,SupplierID = obj.SupplierID
                                        ,SupplierName = obj.SupplierName
                                        ,DatePrice = obj.DatePrice
                                        ,PMKSID = obj.PMKSID 
                                        ,Price = obj.Price 
                                        ,PPH22 = obj.PPH22 
                                        ,VAT = obj.VAT 
                                        ,AccessID = obj.AccessID
                                       });
                db.Execute("sp_CRUDPrice", p, commandType: CommandType.StoredProcedure);
                return p.Get<string>("@SupplierID");
            }
        }

        public bool Delete(T_Price obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDPrice", new { action = "DELETE"
                                                            ,SupplierID = obj.SupplierID
                                                            ,SupplierName = obj.SupplierName
                                                            ,DatePrice = obj.DatePrice
                                                            ,PMKSID = obj.PMKSID 
                                                            ,Price = obj.Price 
                                                            ,PPH22 = obj.PPH22 
                                                            ,VAT = obj.VAT 
                                                            ,AccessID = obj.AccessID
                                                            }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_Price obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDPrice", 
                                new { action = "EDIT"
                                        ,SupplierID = obj.SupplierID
                                        ,SupplierName = obj.SupplierName
                                        ,DatePrice = obj.DatePrice
                                        ,PMKSID = obj.PMKSID 
                                        ,Price = obj.Price 
                                        ,PPH22 = obj.PPH22 
                                        ,VAT = obj.VAT 
                                        ,AccessID = obj.AccessID
                            }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool UpHarga(int AccessID, List<T_Price.PriceUp> p_json)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                var json = JsonConvert.SerializeObject(p_json);
                //string coba = JsonConvert.DeserializeObject(json);
                int result = 
                    db.Execute("sp_CRUDPriceUpHarga", 
                        new { 
                            json = json,
                            AccessID = AccessID
                            }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public T_Price FindPrice(string SupplierID, string SupplierName,string PMKSID, string DatePrice)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT * FROM tblPrice
                                WHERE 1=1 ";
                
                if (SupplierID != null)
                    que += "  AND SupplierID=@SupplierID ";
                if (SupplierName != null)
                    que += "  AND SupplierName=@SupplierName ";
                if (PMKSID != null)
                    que += "  AND PMKSID=@PMKSID ";
                if (DatePrice != null)
                    que += "  AND DatePrice=@DatePrice ";

                return db.Query<T_Price>(que, new {SupplierID=SupplierID, SupplierName=SupplierName ,PMKSID= PMKSID, DatePrice=DatePrice }, commandType: CommandType.Text).SingleOrDefault();
            }
        }

        public List<T_Price> GetListPrice(bool? defunct, string Dateprice, int FCUserID, string fil_pmks)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" DECLARE @setpmksid varchar(max),@pos int, @name varchar(25)
                                SELECT @setpmksid=SetPMKSID from t_user WHERE FCUserID=@FCUserID

                                SELECT a.*,b.*,b1.*,
                                     s.Approval,s.SupplierID,s.SupplierName,s.PMKSID,s.PMKSName
	                                ,s.Initial,IIF(ISNULL(s.CashAdvance,'')='Y',1,0) as CashAdvance
	                                ,s.Code,s.NPWP,s.GroupSupplier,s.Category,s.VAT
	                                ,IIF(ISNULL(s.VATCondition,'')='Y',1,0) as VATCondition
	                                ,s.PPH22,IIF(ISNULL(s.PPH22Condition,'')='Y',1,0) as PPH22Condition
	                                ,s.Remarks,s.Remarks1,s.Remarks2
	                                ,IIF(ISNULL(s.StatusActive,'')='Y',1,0) as StatusActive
	                                ,s.UserID,s.LastAccess,s.PaymentTerm
	                                ,IIF(ISNULL(s.KomidelCalculate,'')='Y',1,0) as KomidelCalculate
	                                ,s.ApprovalBy,s.ApprovalDT,IIF(ISNULL(s.BibitTopaz,'')='Y',1,0) as BibitTopaz
	                                ,s.FCCreatedBy,s.FCCreatedDT,s.FCUpdatedBy,s.FCUpdatedDT
	                                ,s.FCApproveBy,s.FCApproveDT,s.FCUnApproveBy,s.FCUnApproveDT
                                    FROM 
                                    (SELECT * FROM tblPrice WHERE DatePrice=@Dateprice) a
                                INNER JOIN tblSupplier s ON a.PMKSID=s.PMKSID AND a.SupplierID=s.SupplierID AND a.SupplierName=s.SupplierName
                                LEFT JOIN t_User b ON a.FCCreatedBy=b.FCUserID
                                LEFT JOIN t_User b1 ON a.FCUpdatedby=b1.FCUserID
                                WHERE 1=1 and (a.PMKSID in (select * from dbo.splitstring(@setpmksid)) or isnull(@setpmksid,'')='')";

                //if (Dateprice != null && Dateprice != "")
                //    que += " AND a.DatePrice=@DatePrice";
                //else
                //    que += " AND a.DatePrice=CAST( GETDATE() AS Date )";
                if (fil_pmks != null && fil_pmks != "")
                    que += " AND a.PMKSID=@fil_pmks";


                que += " ORDER BY a.SupplierID, a.SupplierName, a.PMKSID";


                var result = db.Query<T_Price,T_User, T_User, T_Supplier, T_Price>(que, (a,b,c,d) =>
               {
                   //a.RootMenu = b;
                   a.CreatedBy = b;
                   a.UpdatedBy = c;
                   a.Supplier = d;
                   return a;
               }, splitOn: "FCUserID,FCUserID,Approval", param: new { FCDEFUNCTIND = defunct, Dateprice=Dateprice, FCUserID= FCUserID, fil_pmks= fil_pmks }).ToList();
                return result;
            }
        }
    }
}
