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
        [XmlElement(Namespace = Namespaces.justice)]
        public OrganizationAugmentation OrganizationAugmentation { get; set; }

        public ReportingAgency() { }

        public ReportingAgency(OrganizationAugmentation orgAug)
        {
            this.OrganizationAugmentation = orgAug;
        }
    }
}
