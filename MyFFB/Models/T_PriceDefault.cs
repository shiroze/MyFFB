using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_PriceDefault
    {
        [Display(Name = "Supplier ID")]
        public string SupplierID { get; set; }

        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        [Display(Name = "Efective Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Efective_Date { get; set; }

        [Display(Name = "PMKS ID")]
        public string PMKSID { get; set; }

        [Display(Name = "LastAccess")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LastAccess { get; set; }

        [Display(Name = "Dibuat Oleh")]
        public int FCCreatedBy { get; set; }

        [Display(Name = "Tanggal Pembuatan")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FCCreatedDT { get; set; }

        [Display(Name = "Diubah Oleh")]
        public int FCUpdatedBy { get; set; }

        [Display(Name = "Tanggal Pengubahan")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FCUpdatedDT { get; set; }

        public int AccessID { get; set; }
        public T_User CreatedBy { get; set; }
        public T_User UpdateBy { get; set; }
    }
}
