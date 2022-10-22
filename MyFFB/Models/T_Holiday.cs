using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_Holiday
    {
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        public int AccessID { get; set; }
    }
}
