using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Incident
{
    [XmlRoot("IncidentAugmentation", Namespace = Namespaces.cjis)]
    public class cjisIncidentAugmentation
    {
        [XmlElement("IncidentReportDateIndicator", Namespace = Namespaces.niemCore, Order = 1)]
        public string incidentReportDateIndicator { get; set; }
        
        [XmlElement("OffenseCargoTheftIndicator", Namespace = Namespaces.niemCore, Order = 2)]
        public string offenseCargoTheftIndicator { get; set; }
        
        public cjisIncidentAugmentation() { }

        public cjisIncidentAugmentation(bool reportDateIndicator, bool cargoTheftIndicator)
        {
            this.incidentReportDateIndicator = reportDateIndicator.ToString().ToLower();
            this.offenseCargoTheftIndicator = cargoTheftIndicator.ToString().ToLower();
        }
    }
}
