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
        public ActivityIdentification activityId { get; set; }

        [XmlElement("ActivityDate", Namespace = Namespaces.niemCore, Order = 2)]
        public ActivityDate activityDate { get; set; }

        [XmlElement("IncidentAugmentation", Namespace = Namespaces.cjis, Order = 3)]
        public cjisIncidentAugmentation cjisIncidentAugmentation { get; set; }

        [XmlElement("IncidentAugmentation", Namespace = Namespaces.justice, Order = 4)]
        public jxdmIncidentAugmentation jxdmIncidentAugmentation { get; set; }

        public Incident() { }

        public Incident(ActivityIdentification id, ActivityDate date, cjisIncidentAugmentation cjis, jxdmIncidentAugmentation jdxm)
        {
            this.activityId = id;
            this.activityDate = date;
            this.cjisIncidentAugmentation = cjis;
            this.jxdmIncidentAugmentation = jdxm;
        }
    }
}
