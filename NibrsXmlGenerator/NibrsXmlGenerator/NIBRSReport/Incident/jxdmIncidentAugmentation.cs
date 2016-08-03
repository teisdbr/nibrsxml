using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Incident
{
    [XmlRoot("IncidentAugmentation", Namespace = Namespaces.justice)]
    public class jxdmIncidentAugmentation
    {
        [XmlElement("IncidentExceptionalClearanceCode", Namespace = Namespaces.justice, Order = 1)]
        public string incidentExceptionalClearanceCode { get; set; }

        [XmlElement("IncidentExceptionalClearanceDate", Namespace = Namespaces.justice, Order = 2)]
        public IncidentExceptionalClearanceDate incidentExceptionalClearanceDate { get; set; }

        public jxdmIncidentAugmentation() { }

        public jxdmIncidentAugmentation(string clearanceCode, IncidentExceptionalClearanceDate clearanceDate)
        {
            this.incidentExceptionalClearanceCode = clearanceCode;
            this.incidentExceptionalClearanceDate = clearanceDate;
        }
    }
}
