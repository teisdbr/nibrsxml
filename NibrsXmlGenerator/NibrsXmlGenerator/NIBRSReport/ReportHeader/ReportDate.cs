using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.ReportHeader
{
    [XmlRoot("ReportDate", Namespace = Namespaces.cjisNibrs)]
    public class ReportDate
    {
        [XmlElement("YearMonthDate", Namespace = Namespaces.niemCore)]
        public string yearMonthDate { get; set; }

        public ReportDate() { }

        public ReportDate(string ymd)
        {
            this.yearMonthDate = ymd;
        }
    }
}
