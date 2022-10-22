using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_Role
    {
        [Display(Name = "Role ID")]
        public int FCRoleID { get; set; }

        [Display(Name = "Role Desc")]
        [Required(ErrorMessage = "Description is required")]
        public string FCRoleDesc { get; set; }

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

        public List<T_RoleDet> RoleDetail { get; set; }
        public T_User CreatedBy { get; set; }
        public T_User UpdatedBy { get; set; }
    }
}
