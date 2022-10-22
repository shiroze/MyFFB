using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_SetGeneral
    {
        [Display(Name = "Setting ID")]
        public int SetId { get; set; }

        [Display(Name = "Setting Name")]
        public string SetName { get; set; }

        [Display(Name = "Value")]
        public int value { get; set; }

        public int AccessID { get; set; }
    }
}
