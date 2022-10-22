using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_EmailPrice
    {
        [Display(Name = "Email ID")]
        public int EmailID { get; set; }

        [Display(Name = "Addresss Email")]
        public string AddressEmail { get; set; }

        [Display(Name = "Bagian")]
        public string ListSend { get; set; }

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

        //public T_User t_userArea { get; set; }
        //public T_User t_userRegional { get; set; }
        public T_User CreatedBy { get; set; }
        public T_User UpdatedBy { get; set; }
    }
}
