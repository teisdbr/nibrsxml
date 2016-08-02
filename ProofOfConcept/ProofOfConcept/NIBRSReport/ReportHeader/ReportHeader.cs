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
        public string nibrsReportCategoryCode { get; set; }

        [XmlElement("ReportActionCategoryCode", Namespace = Namespaces.cjisNibrs, Order = 2)]
        public string reportActionCategoryCode { get; set; }

        [XmlElement("ReportDate", Namespace = Namespaces.cjisNibrs, Order = 3)]
        public ReportDate reportDate { get; set; }

        [XmlElement("ReportingAgency", Namespace = Namespaces.cjisNibrs, Order = 4)]
        public ReportingAgency reportingAgency { get; set; }

        public ReportHeader() { }

        public ReportHeader(string nibrsCode, string actionCode, ReportDate date, ReportingAgency agency)
        {
            this.nibrsReportCategoryCode = nibrsCode;
            this.reportActionCategoryCode = actionCode;
            this.reportDate = date;
            this.reportingAgency = agency;
        }
    }
}
