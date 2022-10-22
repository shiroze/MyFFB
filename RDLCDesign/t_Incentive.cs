namespace RDLCDesign
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_Incentive
    {
        [Key]
        public int IncentiveID { get; set; }

        [Required]
        [StringLength(7)]
        public string Periode { get; set; }

        public int GroupSuppID { get; set; }

        [Required]
        [StringLength(50)]
        public string PMKSID { get; set; }

        [Required]
        [StringLength(15)]
        public string SupplierID { get; set; }

        public double Netto { get; set; }

        public int Incentive { get; set; }

        [StringLength(255)]
        public string Remarks { get; set; }

        public bool? UploadToSAP { get; set; }

        public int? FCApproveBy { get; set; }

        public DateTime? FCApproveDT { get; set; }

        public int? FCCreatedBy { get; set; }

        public DateTime? FCCreatedDT { get; set; }

        public int? FCUpdatedBy { get; set; }

        public DateTime? FCUpdatedDT { get; set; }

        public bool? FCDefunctInd { get; set; }

        public bool? Approval { get; set; }

        public decimal Pembelian { get; set; }

        public decimal PPH22Value { get; set; }

        public decimal Pembayaran { get; set; }

        public string Remarks1 { get; set; }

        public string Remarks2 { get; set; }
    }
}
