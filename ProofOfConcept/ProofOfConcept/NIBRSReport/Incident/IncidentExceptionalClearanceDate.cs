using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Incident
{
    [XmlRoot("ActivityIdentification", Namespace = Namespaces.justice)]
    public class IncidentExceptionalClearanceDate
    {
        [XmlElement("Date", Namespace = Namespaces.niemCore)]
        public string date { get; set; }

        public IncidentExceptionalClearanceDate() { }

        public IncidentExceptionalClearanceDate(string date)
        {
            this.date = date;
        }
    }
}
