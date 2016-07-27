using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NIBRSXML.Constants;

namespace NIBRSReport
{
    [XmlRoot(Namespace = Namespaces.cjisNibrs)]
    public class Report
    {
        public ReportHeader ReportHeader {get; set;}

        public Report() { }

        public Report(ReportHeader header)
        {
            this.ReportHeader = header;
        }
    }
}
