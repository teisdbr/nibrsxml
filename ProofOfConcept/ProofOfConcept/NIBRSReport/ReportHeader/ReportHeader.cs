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
    public class ReportHeader : ReportElement
    {
        [XmlElement("NIBRSReportCategoryCode", Namespace = Namespaces.cjisNibrs)]
        public string nibrsReportCategoryCode { get; set; }

        [XmlElement("ReportActionCategoryCode", Namespace = Namespaces.cjisNibrs)]
        public string reportActionCategoryCode { get; set; }

        [XmlElement("ReportDate", Namespace = Namespaces.cjisNibrs)]
        public ReportDate reportDate { get; set; }

        [XmlElement("ReportingAgency", Namespace = Namespaces.justice)]
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
