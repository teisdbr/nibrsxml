using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;

namespace NibrsXml.NibrsReport.Incident
{
    [XmlRoot("Incident", Namespace = Namespaces.niemCore)]
    public class Incident
    {
        [XmlElement("ActivityIdentification", Namespace = Namespaces.niemCore, Order = 1)]
        public ActivityIdentification ActivityId { get; set; }

        [XmlElement("ActivityDate", Namespace = Namespaces.niemCore, Order = 2)]
        public ActivityDate ActivityDate { get; set; }

        [XmlElement("IncidentAugmentation", Namespace = Namespaces.cjis, Order = 3)]
        public CjisIncidentAugmentation CjisIncidentAugmentation { get; set; }

        [XmlElement("IncidentAugmentation", Namespace = Namespaces.justice, Order = 4)]
        public JxdmIncidentAugmentation JxdmIncidentAugmentation { get; set; }

        public Incident() { }

        public Incident(ActivityIdentification id, ActivityDate date, CjisIncidentAugmentation cjis, JxdmIncidentAugmentation jdxm)
        {
            this.ActivityId = id;
            this.ActivityDate = date;
            this.CjisIncidentAugmentation = cjis;
            this.JxdmIncidentAugmentation = jdxm;
        }
    }
}
