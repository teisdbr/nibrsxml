using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Incident
{
    [XmlRoot("IncidentAugmentation", Namespace = Namespaces.justice)]
    public class JxdmIncidentAugmentation
    {
        [XmlElement("IncidentExceptionalClearanceCode", Namespace = Namespaces.justice, Order = 1)]
        public string IncidentExceptionalClearanceCode { get; set; }

        [XmlElement("IncidentExceptionalClearanceDate", Namespace = Namespaces.justice, Order = 2)]
        public IncidentExceptionalClearanceDate IncidentExceptionalClearanceDate { get; set; }

        public JxdmIncidentAugmentation() { }

        public JxdmIncidentAugmentation(string clearanceCode, IncidentExceptionalClearanceDate clearanceDate)
        {
            this.IncidentExceptionalClearanceCode = clearanceCode;
            this.IncidentExceptionalClearanceDate = clearanceDate;
        }
    }
}
