using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_Incentive
    {
        [Display(Name = "Incentive ID")]
        public string IncentiveID { get; set; }

        [Display(Name = "Group Supp Name")]
        public string GroupSuppName { get; set; }

        [Display(Name = "Group Supp ID")]
        public int GroupSuppID { get; set; }

        [Display(Name = "Periode")]
        public string Periode { get; set; }

        [Display(Name = "Company Code")]
        public string CompanyCode { get; set; }

        [Display(Name = "PMKSID")]
        public string PMKSID { get; set; }
        [Display(Name = "Regional")]
        public string Regional { get; set; }

        [Display(Name = "Vendor Code")]
        public string VendorCode { get; set; }

        [Display(Name = "Supplier ID")]
        public string SupplierID { get; set; }

        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        [Display(Name = "NETTO")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Netto { get; set; }

        [Display(Name = "Incentive")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public int Incentive { get; set; }

        [Display(Name = "PPH22")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal pph22 { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

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

        [Display(Name = "Approval")]
        public bool Approval { get; set; }

        [Display(Name = "Diapprove Oleh")]
        public int FCApproveBy { get; set; }

        [Display(Name = "Tanggal Approve")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FCApproveDT { get; set; }

        [Display(Name = "Upload to SAP")]
        public bool UploadToSAP { get; set; }

        [Display(Name = "Status Hapus")]
        public bool FCDefunctInd { get; set; }

        public int AccessID { get; set; }

        public T_User CreatedBy { get; set; }
        public T_User UpdateBy { get; set; }
        public T_User ApproveBy { get; set; }


        //Tambahan Buat Report
        [Display(Name = "Pembelian")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Pembelian { get; set; }

        [Display(Name = "Harga PPH22")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal PPH22Value { get; set; }

        [Display(Name = "Pembayaran")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Pembayaran { get; set; }

        [Display(Name = "Remarks1")]
        public string Remarks1 { get; set; }
        [Display(Name = "Remarks2")]
        public string Remarks2 { get; set; }

        [Display(Name = "Name1")]
        public string Name1 { get; set; }
        [Display(Name = "Name2")]
        public string Name2 { get; set; }

    }
 
}
