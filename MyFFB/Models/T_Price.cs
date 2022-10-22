using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_Price
    {
        
        [Display(Name = "Appproval")]
        public bool Approval { get; set; }
       

        [Required(ErrorMessage = "Supplier ID is required.")]
        [Display(Name = "Supplier ID")]
        public string SupplierID { get; set; }

        [Required(ErrorMessage = "Supplier Name is required.")]
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }


        //[Display(Name = "Date Price")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime DatePrice { get; set; }

        [Required]
        [Display(Name = "Date Price")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DatePrice { get; set; }

        [Required(ErrorMessage = "PMKS ID is required.")]
        [Display(Name = "PMKS ID")]
        public string PMKSID { get; set; }

        [Required]
        [Display(Name = "Price")]
        public int Price { get; set; }

        [Required]
        [Display(Name = "PPH22")]
        public float PPH22 { get; set; }

        [Required]
        [Display(Name = "VAT")]
        public float VAT { get; set; }

        [Display(Name = "Komidel Calculate")]
        public string KomidelCalculate { get; set; }

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
        public T_User UpdatedBy { get; set; }

        public class PriceUp
        {
            public string SupplierID { get; set; }
            public string PMKSID { get; set; }
            public string DatePrice { get; set; }
            public string UpPrice { get; set; }
        }
        public class PriceUpInfo
        {
            public string SupplierID { get; set; }
            public string PMKSID { get; set; }
            public string DatePrice { get; set; }
            public string Harga { get; set; }
            [Display(Name = "Harga ADJ")]
            public string HargaUp { get; set; }
        }

        public T_Supplier Supplier { get; set; }
    }


}
