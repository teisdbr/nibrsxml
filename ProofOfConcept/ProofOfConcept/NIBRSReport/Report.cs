using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NIBRSXML.Constants;
using NIBRSXML.NIBRSReport;

namespace NIBRSXML.NIBRSReport
{
    [XmlRoot("Report", Namespace = Namespaces.cjisNibrs)]
    public class Report
    {
        public ReportHeader reportHeader { get; set; }

        public Report() { }

        public Report(ReportHeader header)
        {
            this.reportHeader = header;
        }
    }
}
