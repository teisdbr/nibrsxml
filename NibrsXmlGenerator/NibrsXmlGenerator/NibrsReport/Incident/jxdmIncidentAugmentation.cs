using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Incident
{
    [XmlRoot("IncidentAugmentation", Namespace = Namespaces.justice)]
    public class JxdmIncidentAugmentation
    {
        public JxdmIncidentAugmentation()
        {
        }

        public JxdmIncidentAugmentation(string clearanceCode, IncidentExceptionalClearanceDate clearanceDate)
        {
            IncidentExceptionalClearanceCode = clearanceCode;
            IncidentExceptionalClearanceDate = clearanceDate;
        }

        [XmlElement("IncidentExceptionalClearanceCode", Namespace = Namespaces.justice, Order = 1)]
        public string IncidentExceptionalClearanceCode { get; set; }

        [XmlElement("IncidentExceptionalClearanceDate", Namespace = Namespaces.justice, Order = 2)]
        public IncidentExceptionalClearanceDate IncidentExceptionalClearanceDate { get; set; }
    }
}