using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_RoleDet
    {
        [Display(Name = "Role")]
        public int FCRoleID { get; set; }

        [Display(Name = "Menu")]
        public int FCMenuID { get; set; }

        [Display(Name = "Add")]
        public bool FCAdd { get; set; }

        [Display(Name = "Edit")]
        public bool FCEdit { get; set; }

        [Display(Name = "Delete")]
        public bool FCDelete { get; set; }

        [Display(Name = "Undelete")]
        public bool FCUndelete { get; set; }

        [Display(Name = "Approval")]
        public bool FCApprove { get; set; }

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

        public T_Menu Menu { get; set; }
    }
}
