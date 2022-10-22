using System;
using System.ComponentModel.DataAnnotations;

namespace MyAttd.Models
{
    public class T_Report
    {
        public class RptFFBRecap
        {
            public string PMKS { get; set; }
            public string type { get; set; }
            public string dtF { get; set; }
            public string dtT { get; set; }
            public string typeFile { get; set; }
        }

        public class RptCashAdvance
        {
            public string PMKS { get; set; }
            public string dtF { get; set; }
            public string dtT { get; set; }
            public string typeFile { get; set; }
        }

        public class RptFFBOutStanding
        {
            public string PMKS { get; set; }
            public string dtF { get; set; }
            public string dtT { get; set; }
            public string typeFile { get; set; }
        }

        public class RptCashAdvanceRequest
        {
            public string PMKS { get; set; }
            public string type { get; set; }
            public string dtF { get; set; }
            public string typeFile { get; set; }
        }

        public class Set_RptName
        {
            public string ReportID { get; set; }
            public int FCUserID { get; set; }
            public string Name1 { get; set; }
            public string Name2 { get; set; }
            public string Name3 { get; set; }
            public string Name4 { get; set; }
            public string Name5 { get; set; }
        }

        public class RptTransfer
        {
            public string PMKS { get; set; }
            public string type { get; set; }
            public string dtF { get; set; }
            public string dtT { get; set; }
            public string typeFile { get; set; }
        }
        public class RptPriceIndicator
        {
            public string PMKS { get; set; }
            public string type { get; set; }
            //public string dtF { get; set; }
            public string dtT { get; set; }
            public string typeFile { get; set; }
        }

        public class RptPriceAverage
        {
            public string Regional { get; set; }
            public string dtF { get; set; }
            public string typeFile { get; set; }
        }

        public class RptDetailListPayment
        {
            public string PMKSID { get; set; }
            public string SupplierID { get; set; }
            public string dtF { get; set; }
            public string dtT { get; set; }
            public string typeFile { get; set; }
        }
    }
}
