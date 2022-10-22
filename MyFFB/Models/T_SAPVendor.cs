using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_SAPVendor
    {
        [Display(Name = "Vendor Code")]
        public String vendor_code { get; set; }

        [Display(Name = "Vendor Name")]
        public String vendor_name { get; set; }




        public class T_GroupPayment
        {
            [Display(Name = "Group Payment")]
            public String GroupPayment { get; set; }

            [Display(Name = "Term")]
            public String Term { get; set; }
        }

    }

    
}
