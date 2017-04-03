using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.ReportHeader
{
    [XmlRoot("ReportHeader", Namespace = Namespaces.cjisNibrs)]
    public class ReportHeader
    {
        [XmlElement("NIBRSReportCategoryCode", Namespace = Namespaces.cjisNibrs, Order = 1)]
        public string NibrsReportCategoryCode { get; set; }

        [XmlElement("ReportActionCategoryCode", Namespace = Namespaces.cjisNibrs, Order = 2)]
        public string ReportActionCategoryCode { get; set; }

        [XmlElement("ReportDate", Namespace = Namespaces.cjisNibrs, Order = 3)]
        public ReportDate ReportDate { get; set; }

        [XmlElement("ReportingAgency", Namespace = Namespaces.cjisNibrs, Order = 4)]
        public ReportingAgency ReportingAgency { get; set; }

        public ReportHeader() { }

        public ReportHeader(string nibrsCode, string actionCode, ReportDate date, ReportingAgency agency)
        {
            this.NibrsReportCategoryCode = nibrsCode;
            this.ReportActionCategoryCode = actionCode;
            this.ReportDate = date;
            this.ReportingAgency = agency;
        }
    }
}
