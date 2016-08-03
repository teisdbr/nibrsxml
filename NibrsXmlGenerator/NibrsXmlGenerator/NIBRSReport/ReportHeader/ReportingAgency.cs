using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;

namespace NibrsXml.NibrsReport.ReportHeader
{
    public class ReportingAgency
    {
        [XmlElement("OrganizationAugmentation", Namespace = Namespaces.justice)]
        public OrganizationAugmentation orgAugmentation { get; set; }

        public ReportingAgency() { }

        public ReportingAgency(OrganizationAugmentation orgAugmentation)
        {
            this.orgAugmentation = orgAugmentation;
        }
    }
}
