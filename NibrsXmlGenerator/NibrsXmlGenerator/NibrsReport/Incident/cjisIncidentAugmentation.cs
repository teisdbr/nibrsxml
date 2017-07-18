using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Incident
{
    [XmlRoot("IncidentAugmentation", Namespace = Namespaces.cjis)]
    public class CjisIncidentAugmentation
    {
        [XmlElement("IncidentReportDateIndicator", Namespace = Namespaces.cjis, Order = 1)]
        public string IncidentReportDateIndicator { get; set; }
        
        [XmlElement("OffenseCargoTheftIndicator", Namespace = Namespaces.justice, Order = 2)]
        public string OffenseCargoTheftIndicator { get; set; }
        
        public CjisIncidentAugmentation() { }

        public CjisIncidentAugmentation(bool reportDateIndicator, bool cargoTheftIndicator)
        {
            this.IncidentReportDateIndicator = reportDateIndicator.ToString().ToLower();
            this.OffenseCargoTheftIndicator = cargoTheftIndicator.ToString().ToLower();
        }
    }
}
