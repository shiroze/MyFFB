using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_Transport
    {
        [Display(Name = "Product ID")]
        public product ProductID { get; set; }

        [Display(Name = "PMKSID")]
        public string PMKSID { get; set; }

        [Display(Name = "Destination")]
        public string Destination { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        [Display(Name = "TransportDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TransportDate { get; set; }

        [Display(Name = "User ID")]
        public string UserID { get; set; }

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
