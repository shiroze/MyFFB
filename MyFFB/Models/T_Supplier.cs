using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_Supplier
    {
        [Display(Name = "Approval")]
        public bool Approval { get; set; }
        [Display(Name = "Supplier ID")]
        public string SupplierID { get; set; }
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }
        [Display(Name = "PMKSD ID")]
        public string PMKSID { get; set; }
        [Display(Name = "PMKS Name")]
        public string PMKSName { get; set; }
        [Display(Name = "Initial")]
        public string Initial { get; set; }
        [Display(Name = "Cash Advance")]
        public bool CashAdvance { get; set; }
        [Display(Name = "Code")]
        public string Code { get; set; }
        [Display(Name = "Code Name")]
        public string CodeName { get; set; }
        [Display(Name = "NPWP")]
        public string NPWP { get; set; }
        [Display(Name = "Group Supplier")]
        public string GroupSupplier { get; set; }
        [Display(Name = "Category")]
        public string Category { get; set; }
        [Display(Name = "VAT")]
        public float VAT { get; set; }
        [Display(Name = "VAT Condition")]
        public bool VATCondition { get; set; }
        [Display(Name = "PPH22")]
        public float PPH22 { get; set; }
        [Display(Name = "PPH2 Condition")]
        public bool PPH22Condition { get; set; }
        [Display(Name = "Bank")]
        public string Remarks { get; set; }
        [Display(Name = "Bank No Acc")]
        public string Remarks1 { get; set; }
        [Display(Name = "Bank Name Acc")]
        public string Remarks2 { get; set; }
        [Display(Name = "Status Active")]
        public bool StatusActive { get; set; }
        [Display(Name = "Payment Term")]
        public string PaymentTerm { get; set; }
        [Display(Name = "Komidel Calculate")]
        public bool KomidelCalculate { get; set; }
        [Display(Name = "Bibit Topaz")]
        public bool BibitTopaz { get; set; }

        [Display(Name = "Dibuat Oleh")]
        public int FCCreatedBy { get; set; }

        [Display(Name = "Tanggal Pembuatan")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FCCreatedDT { get; set; }

        [Display(Name = "Diubah Oleh")]
        public int FCUpdatedBy { get; set; }

        [Display(Name = "Tanggal Pengubahan")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FCUpdatedDT { get; set; }

        [Display(Name = "DiApprove Oleh")]
        public int FCApproveBy { get; set; }

        [Display(Name = "Tanggal Approve")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FCApproveDT { get; set; }

        [Display(Name = "DiUnApprove Oleh")]
        public int FCUnApproveBy { get; set; }

        [Display(Name = "Tanggal UnApprove")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FCUnApproveDT { get; set; }

        [Display(Name = "Status Hapus")]
        public bool FCDefunctInd { get; set; }

        public int AccessID { get; set; }

        public T_User CreatedBy { get; set; }
        public T_User UpdatedBy { get; set; }
        public T_User ApprovedBy { get; set; }

        public class ListVAT
        {
            public float VAT { get; set; }
        }
    }
}


