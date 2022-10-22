using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_Budget
    {
        [Display(Name = "PMKS ID")]
        public string PMKSID { get; set; }

        [Display(Name = "Periode")]
        public string Periode { get; set; }

        [Display(Name = "VolumeFFB KG")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public decimal VolumeFFB_KG { get; set; }

        [Display(Name = "VolumeCPO KG")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public decimal VolumeCPO_KG { get; set; }

        [Display(Name = "VolumePK KG")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public decimal VolumePK_KG { get; set; }

        [Display(Name = "OER")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public decimal OER { get; set; }

        [Display(Name = "KER")]
        public float KER { get; set; }

        [Display(Name = "NET Margin")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public decimal NetMargin_USD_MT_CPO { get; set; }

        [Display(Name = "HK")]
        [DisplayFormat(DataFormatString = "{0:0,0}", ApplyFormatInEditMode = true)]
        public int HK { get; set; }

        [Display(Name = "Exchange Rate")]
        public float ExchangeRate { get; set; }

        [Display(Name = "Capacity")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public decimal Capacity { get; set; }

        [Display(Name = "Produksi Cangkang KG")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public decimal ProduksiCangkang_KG { get; set; }

        [Display(Name = "Bakar Cangkang KG")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public decimal BakarCangkang_KG { get; set; }

        [Display(Name = "EBITDA Cangkang")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public decimal EBITDA_Cangkang { get; set; }

        [Display(Name = "ProduksiBA KG")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public decimal ProduksiBA_KG { get; set; }

        [Display(Name = "Price BunchAsh")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public decimal Price_BunchAsh { get; set; }

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

        public int AccessID { get; set; }
        public T_User CreatedBy { get; set; }
        public T_User UpdateBy { get; set; }
    }
}
