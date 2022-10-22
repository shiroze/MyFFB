using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Reporting.NETCore;

namespace MyAttd.Helpers
{
    public class ModFunction
    {
        public static IConfiguration configuration;

        public static byte[] genRptParam(string path, DataTable model, string typeFile, ReportParameterCollection parameters)
        {
            // Open RDL File as a Stream
            var reportDefinition = new FileStream(path, FileMode.Open);

            LocalReport report = new LocalReport();
            report.LoadReportDefinition(reportDefinition);
            report.DataSources.Add(new ReportDataSource("DataSet1", model));
            report.SetParameters(parameters);

            if (typeFile == "PDF")
            {
                byte[] pdf = report.Render("PDF");
                return pdf;
            }
            else
            {
                byte[] excel = report.Render("EXCEL");
                return excel;
            }
        }
    }
}
