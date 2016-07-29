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
    public class Incident : ReportElement
    {
        [XmlElement("ActivityIdentification", Namespace = Namespaces.niemCore)]
        public ActivityIdentification activityId { get; set; }

        [XmlElement("ActivityDate", Namespace = Namespaces.niemCore)]
        public ActivityDate activityDate { get; set; }

        [XmlElement("IncidentAugmentation", Namespace = Namespaces.cjis)]
        public cjisIncidentAugmentation cjisIncidentAugmentation { get; set; }

        [XmlElement("IncidentAugmentation", Namespace = Namespaces.justice)]
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
