using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataContext.Models
{
    [Table("PMKS", Schema = "dbo")]
    public class m_PMKS
    {
        [Display(Name = "Appproval")]
        public bool Approval { get; set; }

        [Display(Name = "Area Operational")]
        public string AreaOperational { get; set; }

        [Display(Name = "Regional")]
        public string Regional { get; set; }

        [Display(Name = "Urut")]
        public int Urut { get; set; }

        [Display(Name = "PMKS ID")]
        public string PMKSID { get; set; }

        [Display(Name = "PMKS Name")]
        public string PMKSName { get; set; }

        [Display(Name = "Company")]
        public string Company { get; set; }

        [Display(Name = "Komindel Min")]
        public float KomidelMin { get; set; }

        [Display(Name = "Harga Min")]
        public int HargaMin { get; set; }

        [Display(Name = "Komidel Plus")]
        public float KomidelPlus { get; set; }

        [Display(Name = "Harga Plus")]
        public int HargaPlus { get; set; }

        [Display(Name = "Company Code")]
        public string CompanyCode { get; set; }

        [Display(Name = "Businesss Area")]
        public string BusinessArea { get; set; }

        [Display(Name = "Business Area CoP")]
        public string BusinessAreaCoP { get; set; }

        [Display(Name = "PMKS Group")]
        public string PMKSGroup { get; set; }

        [Display(Name = "House Bank")]
        public string HouseBank { get; set; }

        [Display(Name = "Pay Saturday")]
        public bool PaySaturday { get; set; }

        [Display(Name = "Komindel Min1")]
        public float KomidelMin1 { get; set; }

        [Display(Name = "Harga Min1")]
        public int HargaMin1 { get; set; }

        [Display(Name = "Komindel Plus1")]
        public float KomidelPlus1 { get; set; }

        [Display(Name = "Harga Plus1")]
        public int HargaPlus1 { get; set; }

        [Display(Name = "Komindel Min2")]
        public float KomidelMin2 { get; set; }

        [Display(Name = "Harga Min2")]
        public int HargaMin2 { get; set; }

        [Display(Name = "Komindel Plus2")]
        public float KomidelPlus2 { get; set; }

        [Display(Name = "Harga Plus2")]
        public int HargaPlus2 { get; set; }

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

        //public bool FCApproved { get; set; }

        public int AccessID { get; set; }

        //public T_User t_userArea { get; set; }
        //public T_User t_userRegional { get; set; }
        //public T_User CreatedBy { get; set; }
    }
}
