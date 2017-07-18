using System;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.ReportHeader
{
    [XmlRoot("ReportDate", Namespace = Namespaces.cjisNibrs)]
    public class ReportDate
    {
        [XmlElement("YearMonthDate", Namespace = Namespaces.niemCore)]
        public string YearMonthDate { get; set; }

        [XmlIgnore]
        public DateTime YearMonthDateTime
        {
            get
            {
                //Attempt to convert date
                DateTime dt;
                DateTime.TryParse(this.YearMonthDate, out dt);

                return dt;
            }
        }

        public ReportDate() { }

        public ReportDate(string ymd)
        {
            this.YearMonthDate = ymd;
        }
    }
}
