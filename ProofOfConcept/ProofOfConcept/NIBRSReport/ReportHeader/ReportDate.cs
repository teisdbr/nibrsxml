using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NIBRSXML.Constants;

namespace NIBRSXML.NIBRSReport.ReportHeader
{
    public class ReportDate
    {
        [XmlElement(Namespace = Namespaces.niemCore)]
        public string YearMonthDate { get; set; }

        public ReportDate() { }

        public ReportDate(string ymd)
        {
            this.YearMonthDate = ymd;
        }
    }
}
