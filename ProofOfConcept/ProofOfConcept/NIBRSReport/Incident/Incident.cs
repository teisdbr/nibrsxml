using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NIBRSXML.Constants;
using NIBRSXML.NIBRSReport.Misc;

namespace NIBRSXML.NIBRSReport.Incident
{
    [XmlRoot("Incident", Namespace = Namespaces.niemCore)]
    public class Incident : IReportElement
    {
        public ActivityIdentification activityId { get; set; }
        public ActivityDate activityDate { get; set; }
        public cjisIncidentAugmentation cjisIncidentAugmentation { get; set; }
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
