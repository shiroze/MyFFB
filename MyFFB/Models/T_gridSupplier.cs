using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_gridSupplier
    {
        [Display(Name = "Supplier ID")]
        public string SupplierID { get; set; }
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }
        [Display(Name = "PMKSD ID")]
        public string PMKSID { get; set; }
        [Display(Name = "PMKS Name")]
        public string PMKSName { get; set; }
        [Display(Name = "Regional")]
        public string Regional { get; set; }
    }
}
