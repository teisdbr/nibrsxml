using System;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.ReportHeader
{
    [XmlRoot("ReportDate", Namespace = Namespaces.cjisNibrs)]
    public class ReportDate
    {
        public ReportDate()
        {
        }

        public ReportDate(string ymd)
        {
            YearMonthDate = ymd;
        }

        [XmlElement("YearMonthDate", Namespace = Namespaces.niemCore)]
        public string YearMonthDate { get; set; }

        [XmlIgnore]
        public DateTime YearMonthDateTime
        {
            get
            {
                //Attempt to convert date
                DateTime dt;
                DateTime.TryParse(YearMonthDate, out dt);

                return dt;
            }
        }
    }
}