using System;

namespace MyAttd.Models
{
    public class T_SeqMstr
    {
        public string FCCode { get; set; }
        public string FCDesc { get; set; }
        public string FCFormat { get; set; }
        public string FCResetType { get; set; }
        public string FCFormatYear { get; set; }
        public string FCFormatMonth { get; set; }
        public string FCFormatDate { get; set; }
        public string FCIndZeroLen { get; set; }
        public string FCLastDateISO { get; set; }
        public int FCIndVal { get; set; }
        public string FCGenCode { get; set; }
        public bool FCInUse { get; set; }
        public DateTime FCSavedDT { get; set; }
        public DateTime FCRequestDT { get; set; }
    }
}
