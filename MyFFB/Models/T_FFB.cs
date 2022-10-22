using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_FFB
    {
        
        [Display(Name = "Post2Payment")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Post2Payment { get; set; }

        [Display(Name = "Number")]
        public string Number { get; set; }

        [Display(Name = "PMKS ID")]
        public string PMKSID { get; set; }

        [Display(Name = "Supplier ID")]
        public string Supplier { get; set; }

        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        [Display(Name = "Group Payment")]
        public string GroupPayment { get; set; }

        [Display(Name = "Kendaraan")]
        public string Kendaraan { get; set; }

        [Display(Name = "Tanggal Timbang")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime TanggalTimbang { get; set; }

        [Display(Name = "Tanggal POST")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime TanggalPOST { get; set; }

        [Display(Name = "Ticket")]
        public string Ticket { get; set; }

        [Display(Name = "Berat Netto")]
        public int BeratNetto { get; set; }

        [Display(Name = "Potongan")]
        public int Potongan { get; set; }

        [Display(Name = "Netto")]
        public int Netto { get; set; }

        [Display(Name = "Netto Transfeer")]
        public int NettoTransfeer { get; set; }

        [Display(Name = "Janjang")]
        public int Janjang { get; set; }

        [Display(Name = "Komidel")]
        public int Komidel { get; set; }

        [Display(Name = "Harga Beli")]
        public int HargaBeli { get; set; }

        [Display(Name = "Harga")]
        public int Harga { get; set; }

        [Display(Name = "PPH22")]
        public float PPH22 { get; set; }

        [Display(Name = "VAT")]
        public float VAT { get; set; }

        [Display(Name = "Total Pembayaran")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public decimal TotalPembayaran { get; set; }

        [Display(Name = "Realisasi Panjar Amount")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public decimal RealisasiPanjarAmount { get; set; }

        [Display(Name = "PMKS Transfer")]
        public string PMKSTransfeer { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Calculate By")]
        public string CalculateBy { get; set; }

        [Display(Name = "Calculate Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime CalculateDate { get; set; }

        [Display(Name = "UserID")]
        public string UserID { get; set; }

        [Display(Name = "Last Access")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime LastAccess { get; set; }

        [Display(Name = "sap_process")]
        public string sap_process { get; set; }

        public int AccessID { get; set; }
        //public T_User CreatedBy { get; set; }
        //public T_User UpdateBy { get; set; }

        //Untuk Report transaksi PPN
        public string PMKSName { get; set; }
        public string Periode { get; set; }
        public string Company { get; set; }
        public string CompanyCode { get; set; }
        public string Code { get; set; }
    }
}
