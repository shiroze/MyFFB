using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_SetReport
    {
        [Display(Name = "Report ID")]
        public string ReportID { get; set; }

        [Display(Name = "User ID")]
        public int FCUserID { get; set; }

        [Display(Name = "Name 1")]
        public string Name1 { get; set; }

        [Display(Name = "Name 2")]
        public string Name2 { get; set; }

        [Display(Name = "Name 3")]
        public string Name3 { get; set; }

        [Display(Name = "Name 4")]
        public string Name4 { get; set; }

        [Display(Name = "Name 5")]
        public string Name5 { get; set; }

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
