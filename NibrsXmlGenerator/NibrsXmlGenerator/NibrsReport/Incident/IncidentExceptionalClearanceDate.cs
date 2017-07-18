using System;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Incident
{
    [XmlRoot("IncidentExceptionalClearanceDate", Namespace = Namespaces.justice)]
    public class IncidentExceptionalClearanceDate
    {
        [XmlElement("Date", Namespace = Namespaces.niemCore)]
        public string Date { get; set; }

        [XmlIgnore]
        public string YearMonthDate
        {
            get
            {
                DateTime dt;
                if (DateTime.TryParse(this.Date, out dt))
                    return dt.ToString("yyyy-MM");
                return null;
            }
        }

        public IncidentExceptionalClearanceDate() { }

        public IncidentExceptionalClearanceDate(string date)
        {
            this.Date = date;
        }
    }
}
