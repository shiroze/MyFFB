using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyAttd.DataAccess
{
    public class RSupplier : IRSupplier
    {
        public RSupplier(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        public bool Add(T_Supplier obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDSupplier",
                    new
                    {
                        action = "ADD",
                        SupplierID = obj.SupplierID,
                        SupplierName = obj.SupplierName,
                        PMKSID = obj.PMKSID,
                        PMKSName = obj.PMKSName,
                        Initial = obj.Initial,
                        CashAdvance = (obj.CashAdvance == true) ? 'Y':'N',
                        Code = obj.Code,
                        NPWP = obj.NPWP,
                        GroupSupplier = obj.GroupSupplier,
                        Category = obj.Category,
                        VAT = obj.VAT,
                        VATCondition = (obj.VATCondition == true) ? 'Y' : 'N',
                        PPH22 = obj.PPH22,
                        PPH22Condition = (obj.PPH22Condition == true) ? 'Y' : 'N',
                        Remarks = obj.Remarks,
                        Remarks1 = obj.Remarks1,
                        Remarks2 = obj.Remarks2,
                        PaymentTerm = obj.PaymentTerm,
                        KomidelCalculate = obj.KomidelCalculate,
                        BibitTopaz = (obj.BibitTopaz == true) ? 'Y' : 'N',
                        StatusActive = (obj.StatusActive == true) ? 'Y' : 'N',
                        AccessID = obj.AccessID
                    }, commandType: CommandType.StoredProcedure) ;
                return result != 0;
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

        public bool Delete(T_Supplier obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDSupplier", new
                {
                    action = "DELETE",
                    SupplierID = obj.SupplierID,
                    SupplierName = obj.SupplierName,
                    PMKSID = obj.PMKSID,
                    PMKSName = obj.PMKSName,
                    Initial = obj.Initial,
                    CashAdvance = (obj.CashAdvance == true) ? 'Y' : 'N',
                    Code = obj.Code,
                    NPWP = obj.NPWP,
                    GroupSupplier = obj.GroupSupplier,
                    Category = obj.Category,
                    VAT = obj.VAT,
                    VATCondition = (obj.VATCondition == true) ? 'Y' : 'N',
                    PPH22 = obj.PPH22,
                    PPH22Condition = (obj.PPH22Condition == true) ? 'Y' : 'N',
                    Remarks = obj.Remarks,
                    Remarks1 = obj.Remarks1,
                    Remarks2 = obj.Remarks2,
                    PaymentTerm = obj.PaymentTerm,
                    KomidelCalculate = obj.KomidelCalculate,
                    BibitTopaz = (obj.BibitTopaz == true) ? 'Y' : 'N',
                    StatusActive = (obj.StatusActive == true) ? 'Y' : 'N',
                    AccessID = obj.AccessID
                }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_Supplier obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDSupplier",
                                new
                                {
                                    action = "EDIT",
                                    SupplierID = obj.SupplierID,
                                    SupplierName = obj.SupplierName,
                                    PMKSID = obj.PMKSID,
                                    PMKSName = obj.PMKSName,
                                    Initial = obj.Initial,
                                    CashAdvance = (obj.CashAdvance == true) ? 'Y' : 'N',
                                    Code = obj.Code,
                                    NPWP = obj.NPWP,
                                    GroupSupplier = obj.GroupSupplier,
                                    Category = obj.Category,
                                    VAT = obj.VAT,
                                    VATCondition = (obj.VATCondition == true) ? 'Y' : 'N',
                                    PPH22 = obj.PPH22,
                                    PPH22Condition = (obj.PPH22Condition == true) ? 'Y' : 'N',
                                    Remarks = obj.Remarks,
                                    Remarks1 = obj.Remarks1,
                                    Remarks2 = obj.Remarks2,
                                    PaymentTerm = obj.PaymentTerm,
                                    KomidelCalculate = obj.KomidelCalculate,
                                    BibitTopaz = (obj.BibitTopaz == true) ? 'Y' : 'N',
                                    StatusActive = (obj.StatusActive == true) ? 'Y' : 'N',
                                    AccessID = obj.AccessID
                                }, commandType: CommandType.StoredProcedure); ;
                return result != 0;
            }
        }

        public bool Approve(T_Supplier obj, bool stsApprove)
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
                int result = 
                    db.Execute("sp_CRUDSupplier",
                    new
                    {
                        action = act,
                        SupplierID = obj.SupplierID,
                        SupplierName = obj.SupplierName,
                        PMKSID = obj.PMKSID,
                        PMKSName = obj.PMKSName,
                        Initial = obj.Initial,
                        CashAdvance = (obj.CashAdvance == true) ? 'Y' : 'N',
                        Code = obj.Code,
                        NPWP = obj.NPWP,
                        GroupSupplier = obj.GroupSupplier,
                        Category = obj.Category,
                        VAT = obj.VAT,
                        VATCondition = (obj.VATCondition == true) ? 'Y' : 'N',
                        PPH22 = obj.PPH22,
                        PPH22Condition = (obj.PPH22Condition == true) ? 'Y' : 'N',
                        Remarks = obj.Remarks,
                        Remarks1 = obj.Remarks1,
                        Remarks2 = obj.Remarks2,
                        PaymentTerm = obj.PaymentTerm,
                        KomidelCalculate = obj.KomidelCalculate,
                        BibitTopaz = (obj.BibitTopaz == true) ? 'Y' : 'N',
                        StatusActive = (obj.StatusActive == true) ? 'Y' : 'N',
                        AccessID = obj.AccessID
                    }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public T_Supplier FindSupplier(string p_SupplierID, string p_SupplierName, string p_PMKSDid)
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
	                    ,s.Code,sv.vendor_name as CodeName,s.NPWP,s.GroupSupplier,s.Category,s.VAT
	                    ,IIF(ISNULL(s.VATCondition,'')='Y',1,0) as VATCondition
	                    ,s.PPH22,IIF(ISNULL(s.PPH22Condition,'')='Y',1,0) as PPH22Condition
	                    ,s.Remarks,s.Remarks1,s.Remarks2
	                    ,IIF(ISNULL(s.StatusActive,'')='Y',1,0) as StatusActive
	                    ,s.UserID,s.LastAccess,s.PaymentTerm
	                    ,IIF(ISNULL(s.KomidelCalculate,'')='Y',1,0) as KomidelCalculate
	                    ,s.ApprovalBy,s.ApprovalDT,IIF(ISNULL(s.BibitTopaz,'')='Y',1,0) as BibitTopaz
	                    ,FCCreatedBy,s.FCCreatedDT,s.FCUpdatedBy,s.FCUpdatedDT--,FCDefunctInd
	                    ,s.FCApproveBy,s.FCApproveDT,s.FCUnApproveBy,s.FCUnApproveDT
                    FROM tblSupplier s
                    LEFT JOIN
	                    tblSAPVendor sv
	                    ON s.Code=sv.vendor_code
                    WHERE 1=1 
                    ";

                if (p_PMKSDid != null)
                    que += "  AND s.PMKSID=@PMKSID ";
                if (p_SupplierName != null)
                    que += " AND s.SupplierName=@SupplierName ";
                if (p_SupplierID != null)
                    que += " AND s.SupplierID=@SupplierID ";

                return db.Query<T_Supplier>(que, new { PMKSID = p_PMKSDid, SupplierName = p_SupplierName, SupplierID = p_SupplierID }, commandType: CommandType.Text).SingleOrDefault();
            }
        }

        public List<T_Supplier> GetListSupplier(bool? defunct, int FCUserID,bool? Approve, string fil_pmks,bool? Status_Active)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que =
                    @" 
                    DECLARE @setpmksid varchar(max),@pos int, @name varchar(25)
                    SELECT @setpmksid=SetPMKSID from t_user WHERE FCUserID=@FCUserID

                    SELECT 
	                    a.Approval,a.SupplierID,a.SupplierName,a.PMKSID,a.PMKSName
	                    ,a.Initial,IIF(ISNULL(a.CashAdvance,'')='Y',1,0) as CashAdvance
	                    ,a.Code,sv.vendor_name,a.NPWP,a.GroupSupplier,a.Category,a.VAT
	                    ,IIF(ISNULL(a.VATCondition,'')='Y',1,0) as VATCondition
	                    ,a.PPH22,IIF(ISNULL(a.PPH22Condition,'')='Y',1,0) as PPH22Condition,a.Remarks,a.Remarks1,a.Remarks2
	                    ,IIF(ISNULL(a.StatusActive,'')='Y',1,0) as StatusActive
	                    ,a.UserID,a.LastAccess,a.PaymentTerm
	                    ,IIF(ISNULL(a.KomidelCalculate,'')='Y',1,0) as KomidelCalculate
	                    ,a.ApprovalBy,a.ApprovalDT,IIF(ISNULL(a.BibitTopaz,'')='Y',1,0) as BibitTopaz,a.FCCreatedBy
	                    ,a.FCCreatedDT,a.FCUpdatedBy,a.FCUpdatedDT
	                    ,a.FCApproveBy,a.FCApproveDT,a.FCUnApproveBy,a.FCUnApproveDT, b.*,b1.*,b2.*
                    FROM (SELECT * FROM tblSupplier WHERE PMKSID=@fil_pmks or @fil_pmks='' ) a
	                    LEFT JOIN tblSAPVendor sv ON a.Code=sv.vendor_code
                        LEFT JOIN t_User b ON a.FCCreatedBy=b.FCUserID
                        LEFT JOIN t_User b1 ON a.FCUpdatedBy=b1.FCUserID
                        LEFT JOIN t_User b2 ON a.FCApproveBy=b2.FCUserID
                    WHERE 1=1 AND (a.PMKSID in (select * from dbo.splitstring(@setpmksid)) or isnull(@setpmksid,'')='')
                    ";

                //string que = @" DECLARE @setpmksid varchar(max),@pos int, @name varchar(25)
                //                SELECT @setpmksid=SetPMKSID from t_user WHERE FCUserID=@FCUserID

                //                SELECT 
                //                    a.Approval,a.SupplierID,a.SupplierName,a.PMKSID,a.PMKSName
                //                    ,a.Initial,a.CashAdvance
                //                    ,a.Code,a.NPWP,a.GroupSupplier,a.Category,a.VAT
                //                    ,a.VATCondition
                //                    ,a.PPH22,a.PPH22Condition,a.Remarks,a.Remarks1,a.Remarks2
                //                    ,a.StatusActive
                //                    ,a.UserID,a.LastAccess,a.PaymentTerm
                //                    ,a.KomidelCalculate
                //                    ,a.ApprovalBy,a.ApprovalDT,a.BibitTopaz,a.FCCreatedBy
                //                    ,a.FCCreatedDT,a.FCUpdatedBy,a.FCUpdatedDT,a.FCDefunctInd
                //                    ,a.FCApproveBy,a.FCApproveDT,a.FCUnApproveBy,a.FCUnApproveDT, b.*
                //                FROM tblSupplier a
                //                inner join t_User b ON a.FCCreatedBy=b.FCUserID
                //                WHERE 1=1 and a.approval=1 and (a.PMKSID in (select * from dbo.splitstring(@setpmksid)) or isnull(@setpmksid,'')='')";

                if (Approve != null)
                    que += " AND ISNULL(a.Approval,0)=@Approve ";
                if (Status_Active != null)
                    que += " AND IIF(ISNULL(a.StatusActive,'')='Y',1,0)=@StatusActive ";

                que += " ORDER BY a.SupplierID, a.PMKSID";

                var result = db.Query<T_Supplier, T_User, T_User, T_User, T_Supplier >(que, (a,b,c,d) =>
                {
                   //a.RootMenu = b;
                   a.CreatedBy = b;
                   a.UpdatedBy = c;
                   a.ApprovedBy = d;
                    return a;
                }, splitOn: "FCUserID,FCUserID,FCUserID", 
                    param: new { FCDEFUNCTIND = defunct, FCUserID = FCUserID,Approve=Approve, fil_pmks= fil_pmks, StatusActive= Status_Active }).ToList();
                return result;
            }
        }

        public List<T_Supplier> GetListCategory()
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que =
                    @" SELECT GroupSupplier as Category FROM TblGroupSupplier
                    ";

                var result = db.Query<T_Supplier>(que, param: new { }).ToList();
                return result;
            }
        }

        public List<T_Supplier.ListVAT> GetListVAT()
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que =
                    @" SELECT * FROM t_listVat
                    ";

                var result = db.Query<T_Supplier.ListVAT>(que, param: new { }).ToList();
                return result;
            }
        }
    }
}
