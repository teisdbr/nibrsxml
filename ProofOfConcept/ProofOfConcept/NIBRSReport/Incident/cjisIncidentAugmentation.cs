using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NIBRSXML.Constants;

namespace NIBRSXML.NIBRSReport.Incident
{
    [XmlRoot("IncidentAugmentation", Namespace = Namespaces.cjis)]
    public class cjisIncidentAugmentation
    {
        [XmlElement("IncidentReportDateIndication", Namespace = Namespaces.niemCore)]
        public string incidentReportDateIndicator { get; set; }
        
        [XmlElement("OffenseCargoTheftIndicator", Namespace = Namespaces.niemCore)]
        public string offenseCargoTheftIndicator { get; set; }
        
        public cjisIncidentAugmentation() { }

        public cjisIncidentAugmentation(bool reportDateIndicator, bool cargoTheftIndicator)
        {
            this.incidentReportDateIndicator = reportDateIndicator ? "true" : "false";
            this.offenseCargoTheftIndicator = cargoTheftIndicator ? "true" : "false";
        }
    }
}
