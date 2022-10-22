using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_Menu
    {
        [Display(Name = "Menu ID")]
        public int FCMenuID { get; set; }

        [Display(Name = "Kode")]
        [Required(ErrorMessage = "Isi Kode Menu")]
        public string FCMenuCode { get; set; }

        [Display(Name = "Deskripsi")]
        [Required(ErrorMessage = "Isi Deskripsi")]
        public string FCMenuDesc { get; set; }

        [Display(Name = "Parent ID")]
        [Required(ErrorMessage = "Isi ParentID")]
        public int FCMenuPID { get; set; }

        [Display(Name = "Parent Menu")]
        public bool FCMenuLink { get; set; }

        [Display(Name = "No. Urut")]
        public int FCOrderNo { get; set; }

        [Display(Name = "Ikon")]
        public string FCIcon { get; set; }

        [Display(Name = "Sembunyikan")]
        public bool FCHidden { get; set; }

        [Display(Name = "Dibuat Oleh")]
        public int FCCreatedBy { get; set; }

        [Display(Name = "Tanggal Pembuatan")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FCCreatedDT { get; set; }

        [Display(Name = "Status Hapus")]
        public bool FCDefunctInd { get; set; }

        [Display(Name = "Root")]
        public string Root { get; set; }
        public int AccessID { get; set; }

        public T_Menu RootMenu { get; set; }

        public T_User CreatedBy { get; set; }
        //public T_User UpdatedBy { get; set; }
    }

    public class minmaxpass
    {
        public int SetID { get; set; }
        public string SetName { get; set; }
        public int value { get; set; }

    }
}
