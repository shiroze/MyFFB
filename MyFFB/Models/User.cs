using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class User
    {
        [Display(Name = "UserID")]
        public int FCUserID { get; set; }

        [Display(Name = "Nama Pengguna")]
        public string FCUserName { get; set; }

        [Display(Name = "Nama")]
        //[Required(ErrorMessage = "Display Name is required")] ---bagian Reset Password bermasalah apabila di aktifkan.
        public string FCName { get; set; }

        [Display(Name = "Kata Sandi Sekarang")]
        [Required(ErrorMessage = "Isi Kata Sandi Sekarang")]
        public string CurrentPassword { get; set; }

        [Display(Name = "Kata Sandi Baru")]
        [Required(ErrorMessage = "Isi Kata Sandi Baru")]
        public string NewPassword { get; set; }

        [Display(Name = "Konfirmasi Kata Sandi Baru")]
        [Required(ErrorMessage = "Isi Konfirmasi Kata Sandi")]
        [Compare("NewPassword", ErrorMessage = "'Kata Sandi Baru' dan 'Konfirmasi Kata Sandi' tidak cocok.")]
        public string ConfirmPassword { get; set; }
    }
}
