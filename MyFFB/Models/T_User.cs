using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_User
    {
        [Display(Name = "UserID")]
        public int FCUserID { get; set; }

        [Display(Name = "Nama Pengguna")]
        [Required(ErrorMessage = "Isi Nama Pengguna")]
        public string FCUserName { get; set; }

        [Display(Name = "Kata Sandi")]
        [Required(ErrorMessage = "Isi Kata Sandi")]
        public string Password { get; set; }

        [Display(Name = "Nama")]
        [Required(ErrorMessage = "Isi Nama")]
        public string FCName { get; set; }
        public string AreaOperational { get; set; }
        public string Regional { get; set; }

        [Display(Name = "Role")]
        public int FCRoleID { get; set; }

        [Display(Name = "Tanggal Exp")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FCPassExpDT { get; set; }

        [Display(Name = "Percobaan Masuk")]
        public int FCLoginAttempt { get; set; }

        [Display(Name = "Blok")]
        public bool FCBlocked { get; set; }

        //[Display(Name = "Approval")]
       // public bool FCApproved { get; set; }

        //[Display(Name = "SV")]
        //public bool FCSeeValue { get; set; }

        //[Display(Name = "RefreshTimer")]
        //public int FCRefreshTimer { get; set; }        

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

        [Display(Name = "First Login")]
        public bool FCFirstLogin { get; set; }

        [Display(Name = "Lokasi Akses")]
        public string[] UserLoc { get; set; }

        [Display(Name = "Set PMKS")]
        public string SetPMKSID { get; set; }

        public int AccessID { get; set; }
        public string MsgResult { get; set; }

        public T_Role Role { get; set; }

        public T_User CreatedBy { get; set; }
        public T_User UpdatedBy { get; set; }
    }
}
