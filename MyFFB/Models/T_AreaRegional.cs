using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_AreaRegional
    {
        [Display(Name = "ID Area")]
        public int AreaID { get; set; }

        [Display(Name = "Area Operational")]
        public string AreaOperational { get; set; }

        [Display(Name = "Regional")]
        public string Regional { get; set; }

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

        [Display(Name = "Status Hapus")]
        public bool FCDefunctInd { get; set; }

        public int AccessID { get; set; }

        public T_User CreatedBy { get; set; }
    }
}
