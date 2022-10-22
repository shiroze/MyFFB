using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyAttd.DataAccess
{
    public class RSAPVendor : IRSAPVendor
    {
        public RSAPVendor(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }



        public List<T_SAPVendor> GridListSAPVendor()
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT * FROM tblSAPVendor";


                var result = db.Query<T_SAPVendor>(que, null).ToList();
                return result;
            }
        }

        public List<T_PMKS> GridListPMKS(string setPMKSIDuser, string setPMKSID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT * FROM tblPMKS WHERE 1=1
                            ";
                if (setPMKSIDuser != null)
                    que += "  AND PMKSID not in ((select * from dbo.splitstring(@setPMKSIDuser))) ";
                else
                    que += "  AND (PMKSID in ((select * from dbo.splitstring(@setPMKSID))) or isnull(@setPMKSID,'')='') ";


                var result = db.Query<T_PMKS>(que, param: new { setPMKSIDuser= setPMKSIDuser,setPMKSID = setPMKSID }).ToList();
                return result;
            }
        }

        public List<T_Supplier> GridListSupplierPrice(string dateprice, string setPMKSID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que =
                    @" 
                    SELECT 
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
	                FROM tblSupplier s
		                LEFT JOIN 
                            (SELECT * FROM tblPrice WHERE DatePrice=@DatePrice) p
		                ON s.SupplierID=p.SupplierID and s.PMKSID=p.PMKSID
                    WHERE ISNULL(s.Approval,0)=1 
                        and p.SupplierID IS NULL
                    ";
                if (setPMKSID != null)
                    que += "  AND s.PMKSID in ((select * from dbo.splitstring(@setPMKSID))) ";

                var result = db.Query<T_Supplier>(que, param: new { DatePrice = dateprice, setPMKSID= setPMKSID }).ToList();
                return result;
            }
        }

        public List<T_PPN> GridListPajakAdvance(string CC,string VC, string Periode_Akhir)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT * 
	                                FROM tblppn
                                WHERE sap_company_code=@CC and sap_vendor_code=@VC
	                                and tgl_faktur_pajak<=@Periode_Akhir and ISNULL(ppn_type,'')='A'
                                ORDER BY no_faktur_pajak ASC
                                ";


                var result = db.Query<T_PPN>(que, param: new { @CC = CC, @VC=VC, @Periode_Akhir = Periode_Akhir }).ToList();
                return result;
            }
        }

        public List<T_CashAdvance> GridListCashNo(string CC, string VC, string Periode, string setPMKSID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" Declare @PPN float
                                SELECT @PPN=[Value] FROM t_SettingValue WHERE TitleSetting='PPN'
                                SELECT a.*, a.Amount-a.DeductAmount as TotalAmount
                                        , Round(cast((a.Amount-a.DeductAmount)*@PPN as float),2)/100 as TotalPPN
                                FROM tblCashAdvance a
	                                LEFT JOIN (
		                                SELECT DISTINCT 
			                                a.PMKSID,b.SupplierID,a.CompanyCode,b.Code
		                                FROM tblPMKS a
			                                INNER JOIN tblSupplier b
				                                ON a.PMKSID=b.PMKSID
	                                ) b
	                                ON a.PMKSID=b.PMKSID and a.SupplierID=b.SupplierID
                                WHERE b.CompanyCode=@CC and b.code=@VC and a.Period=@Periode
                                ";
                if (setPMKSID != null)
                    que += "  AND a.PMKSID in ((select * from dbo.splitstring(@setPMKSID))) ";

                var result = db.Query<T_CashAdvance>(que, param: new { @CC = CC, @VC = VC, @Periode = Periode, setPMKSID=setPMKSID }).ToList();
                return result;
            }
        }

        public List<T_Supplier> GridListSupplier(bool? Approval, string setPMKSID,string filter_PMKSID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que =
                    @" SELECT 
                        Approval,SupplierID,SupplierName,PMKSID,PMKSName
                        ,Initial,IIF(ISNULL(CashAdvance,'')='Y',1,0) as CashAdvance
                        ,Code,NPWP,GroupSupplier,Category,VAT
                        ,IIF(ISNULL(VATCondition,'')='Y',1,0) as VATCondition
                        ,PPH22,IIF(ISNULL(PPH22Condition,'')='Y',1,0) as PPH22Condition,Remarks,Remarks1,Remarks2
                        ,IIF(ISNULL(StatusActive,'')='Y',1,0) as StatusActive
                        ,UserID,LastAccess,PaymentTerm
                        ,IIF(ISNULL(KomidelCalculate,'')='Y',1,0) as KomidelCalculate
                        ,ApprovalBy,ApprovalDT,IIF(ISNULL(BibitTopaz,'')='Y',1,0) as BibitTopaz,FCCreatedBy
                        ,FCCreatedDT,FCUpdatedBy,FCUpdatedDT--,FCDefunctInd
                        ,FCApproveBy,FCApproveDT,FCUnApproveBy,FCUnApproveDT
                    FROM tblSupplier 
                    WHERE --ISNULL(FCDefunctInd,0)=0 
                    ISNULL(Approval,0)=@Approval
                    ";
                if (setPMKSID != null)
                    que += "  AND (PMKSID in ((select * from dbo.splitstring(@setPMKSID))) or @setPMKSID='') ";
                if (filter_PMKSID != null && filter_PMKSID != "")
                    que += " AND PMKSID=@filter_PMKSID";

                var result = db.Query<T_Supplier>(que, param: new { Approval= Approval, setPMKSID= setPMKSID,filter_PMKSID= filter_PMKSID }).ToList();
                return result;
            }
        }

        public List<T_Incentive> GridListIncentive(string setPMKSID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT a.GroupSuppID,a.GroupSuppName,a.Regional,
		                                b.PMKSID,c.CompanyCode,b.SupplierID,d.SupplierName,d.Code as VendorCode
                                FROM t_GroupSupp a
	                                INNER JOIN t_GroupSuppDet b ON a.GroupSuppID=b.GroupSuppID and ISNULL(b.FCDefunctInd,0)=0
	                                INNER JOIN tblPMKS c ON b.PMKSID=c.PMKSID
	                                INNER JOIN tblSupplier d ON b.PMKSID=d.PMKSID and b.SupplierID=d.SupplierID and d.Approval=1
                                WHERE ISNULL(a.FCDefunctInd,0)=0
                                ";
                if (setPMKSID != null)
                    que += "  AND b.PMKSID in ((select * from dbo.splitstring(@setPMKSID))) ";

                var result = db.Query<T_Incentive>(que, param: new { setPMKSID= setPMKSID }).ToList();
                return result;
            }
        }

        public List<T_Competitor> GridListCompetitor(string PMKSID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT CompetitorID,competitorName,competitorLocation,PMKSID 
                                FROM tblCompetitor 
                                WHERE ISNULL(FCDefunctInd,0)=0 and PMKSID=@PMKSID
                                ";


                var result = db.Query<T_Competitor>(que, param: new { PMKSID=PMKSID}).ToList();
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

        public List<T_SAPVendor.T_GroupPayment> GridListGroupPayment()
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que =
                    @"
                    SELECT * FROM tblGroupPayment
                    ";

                var result = db.Query<T_SAPVendor.T_GroupPayment>(que, param: new { }).ToList();
                return result;
            }
        }
    }
}
