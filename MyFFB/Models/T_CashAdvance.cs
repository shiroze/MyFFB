using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_CashAdvance
    {
        [Display(Name = "Cash No")]
        public string CashNo { get; set; }

        [Display(Name = "Periode")]
        public string Period { get; set; }

        [Display(Name = "PMKS ID")]
        public string PMKSID { get; set; }

        [Display(Name = "Supplier ID")]
        public string SupplierID { get; set; }

        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        [Display(Name = "Tanggal")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Tanggal { get; set; }

        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Amount")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public Decimal Amount { get; set; }

        [Display(Name = "Deduct Amount")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public Decimal DeductAmount { get; set; }

        [Display(Name = "Total Amount")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public Decimal TotalAmount { get; set; }

        [Display(Name = "Total PPN")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public Decimal TotalPPN { get; set; }

        [Display(Name = "Status Approval")]
        public string StatusApproval { get; set; }

        [Display(Name = "Week")]
        public string Week { get; set; }
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

        public int AccessID { get; set; }

        public T_User CreatedBy { get; set; }
        public T_User UpdatedBy { get; set; }
    }

}
