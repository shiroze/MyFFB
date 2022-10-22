using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_GroupSuppDet
    {
        public int NoId { get; set; }
        [Display(Name = "Group Supplier ID")]
        public int GroupSuppID { get; set; }

        [Display(Name = "Group Supplier Name")]
        public string GroupSuppName { get; set; }

        [Display(Name = "PMKS ID")]
        public string PMKSID { get; set; }

        [Display(Name = "PMKS Name")]
        public string PMKSName { get; set; }

        [Display(Name = "Supplier ID")]
        public string SupplierID { get; set; }

        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        [Display(Name = "Regional")]
        public string Regional { get; set; }

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
