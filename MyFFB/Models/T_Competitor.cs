using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_Competitor
    {
        [Display(Name = "Competitor ID")]
        public int CompetitorID { get; set; }

        [Required]
        [Display(Name = "Competitor Name")]
        public string CompetitorName { get; set; }

        [Required]
        [Display(Name = "Competitor Location")]
        public string CompetitorLocation { get; set; }

        [Required]
        [Display(Name = "Capacity")]
        public int Capacity { get; set; }

        [Required]
        [Display(Name = "Competitor Group")]
        public string CompetitorGroup { get; set; }

        [Required]
        [Display(Name = "PMKS ID")]
        public string PMKSID { get; set; }

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
    }
}
