using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_GroupIncentive
    {
        public int NoId { get; set; }

        [Display(Name = "Group Supplier ID")]
        public string GroupSuppID { get; set; }

        [Display(Name = "Group Supplier Name")]
        public string GroupSuppName { get; set; }

        [Display(Name = "Regional")]
        public string Regional { get; set; }

        [Display(Name = "Approval")]
        public bool Approval { get; set; }

        [Display(Name = "Incentive")]
        public bool Incentive { get; set; }

        [Display(Name = "QTY 1")]
        public float IncentiveQty1 { get; set; }

        [Display(Name = "Price 1")]
        public int IncentivePrice1 { get; set; }

        [Display(Name = "QTY 2")]
        public float IncentiveQty2 { get; set; }

        [Display(Name = "Price 2")]
        public int IncentivePrice2 { get; set; }

        [Display(Name = "QTY 3")]
        public float IncentiveQty3 { get; set; }

        [Display(Name = "Price 3")]
        public int IncentivePrice3 { get; set; }

        [Display(Name = "QTY 4")]
        public float IncentiveQty4 { get; set; }

        [Display(Name = "Price 4")]
        public int IncentivePrice4 { get; set; }

        [Display(Name = "QTY 5")]
        public float IncentiveQty5 { get; set; }

        [Display(Name = "Price 5")]
        public int IncentivePrice5 { get; set; }

        [Display(Name = "QTY 6")]
        public float IncentiveQty6 { get; set; }

        [Display(Name = "Price 6")]
        public int IncentivePrice6 { get; set; }

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

        [Display(Name = "DiApprove Oleh")]
        public int FCApproveBy { get; set; }

        [Display(Name = "Tanggal Approve")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FCApproveDT { get; set; }

        [Display(Name = "Status Hapus")]
        public bool FCDefunctInd { get; set; }

        public int AccessID { get; set; }

        public T_User CreatedBy { get; set; }
        public T_User UpdatedBy { get; set; }
        public T_User ApprovedBy { get; set; }

    }
}
