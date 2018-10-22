using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Incident
{
    [XmlRoot("IncidentAugmentation", Namespace = Namespaces.cjis)]
    public class CjisIncidentAugmentation
    {
        public CjisIncidentAugmentation()
        {
        }

        public CjisIncidentAugmentation(bool reportDateIndicator, bool? cargoTheftIndicator)
        {
            IncidentReportDateIndicator = reportDateIndicator.ToString().ToLower();

            if (cargoTheftIndicator.HasValue)
                OffenseCargoTheftIndicator = cargoTheftIndicator.Value.ToString().ToLower();
        }

        [XmlElement("IncidentReportDateIndicator", Namespace = Namespaces.cjis, Order = 1)]
        public string IncidentReportDateIndicator { get; set; }

        [XmlElement("OffenseCargoTheftIndicator", Namespace = Namespaces.justice, Order = 2)]
        public string OffenseCargoTheftIndicator { get; set; }
    }
}