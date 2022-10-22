using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_PPN
    {
        
        [Display(Name = "Periode")]
        public string Periode { get; set; }

        [Display(Name = "No Faktur Pajak")]
        public string no_faktur_pajak { get; set; }

        [Display(Name = "PPN")]
        public string ppn_type { get; set; }

        [Display(Name = "Tgl Faktur")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime tgl_faktur_pajak { get; set; }

        [Display(Name = "Tgl Posting")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime tgl_posting { get; set; }

        [Display(Name = "SAP Company Name")]
        public string sap_company_name { get; set; }

        [Display(Name = "SAP Company Code")]
        public string sap_company_code { get; set; }

        [Display(Name = "SAP Vendor name")]
        public string sap_vendor_name { get; set; }

        [Display(Name = "SAP Vendor Code")]
        public string sap_vendor_code { get; set; }

        [Display(Name = "Periode Awal")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime periode_awal { get; set; }

        [Display(Name = "Periode Akhir")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime periode_akhir { get; set; }

        [Display(Name = "Disetor Ke")]
        public setor disetorke { get; set; }

        [Display (Name ="PPN Penyelesaian")]
        public bool ppn_penyelesaian { get; set; }

        [Display(Name = "No Faktur Advance")]
        public string no_faktur_pajak_advance { get; set; }

        [Display(Name = "Cash No")]
        public string CashNo { get; set; }

        [Display(Name = "Amount Cash Advance")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public Int64 amount_cash_advance { get; set; }

        [Display(Name = "Total Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public Int64 total_amount { get; set; }

        [Display(Name = "Incentive")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Int64 incentive { get; set; }

        [Display(Name = "PPN")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Int64 ppn { get; set; }

        [Display(Name = "PPN")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Int64 ppn1 { get; set; }

        [Display(Name = "UserID")]
        public string UserID { get; set; }

        [Display(Name = "Last Access")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime LastAccess { get; set; }

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

        [Display(Name = "Status Hapus")]
        public bool FCDefunctInd { get; set; }

        public int AccessID { get; set; }

        public T_User CreatedBy { get; set; }
        public T_User UpdateBy { get; set; }
    }
    public enum setor
    {
        V,
        K
    }
}
