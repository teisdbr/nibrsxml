using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Incident
{
    [XmlRoot("IncidentExceptionalClearanceDate", Namespace = Namespaces.justice)]
    public class IncidentExceptionalClearanceDate
    {
        [XmlElement("Date", Namespace = Namespaces.niemCore)]
        public string Date { get; set; }

        public IncidentExceptionalClearanceDate() { }

        public IncidentExceptionalClearanceDate(string date)
        {
            this.Date = date;
        }
    }
}
