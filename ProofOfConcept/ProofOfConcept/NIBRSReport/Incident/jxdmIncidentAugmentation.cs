using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NIBRSXML.Constants;

namespace NIBRSXML.NIBRSReport.Incident
{
    [XmlRoot("IncidentAugmentation", Namespace = Namespaces.justice)]
    public class jxdmIncidentAugmentation
    {
        [XmlElement("IncidentExceptionalClearanceCode", Namespace = Namespaces.justice)]
        public string incidentExceptionalClearanceCode { get; set; }

        public IncidentExceptionalClearanceDate incidentExceptionalClearanceDate { get; set; }

        public jxdmIncidentAugmentation() { }

        public jxdmIncidentAugmentation(string clearanceCode, IncidentExceptionalClearanceDate clearanceDate)
        {
            this.incidentExceptionalClearanceCode = clearanceCode;
            this.incidentExceptionalClearanceDate = clearanceDate;
        }
    }
}
