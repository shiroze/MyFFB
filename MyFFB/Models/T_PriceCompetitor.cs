using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_PriceCompetitor
    {
        //[Display(Name = "Price Comptetitor ID")]
        //public string PriceCompetitorID { get; set; }

        [Display(Name = "Tanggal")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "PMKS ID")]
        public string PMKSID { get; set; }

        //[Display(Name = "PMKS Name")]
        //public string PMKSName { get; set; }

        //[Display(Name = "Competitor ID")]
        //public string CompetitorID { get; set; }

        [Display(Name = "Competitor Name")]
        public string CompetitorName { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:0,0}", ApplyFormatInEditMode = true)]
        public Int64 Price { get; set; }

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
}
