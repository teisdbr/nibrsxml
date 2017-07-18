using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;

namespace NibrsXml.NibrsReport.ReportHeader
{
    public class ReportingAgency
    {
        [XmlElement("OrganizationAugmentation", Namespace = Namespaces.justice)]
        public OrganizationAugmentation OrgAugmentation { get; set; }

        public ReportingAgency() { }

        public ReportingAgency(OrganizationAugmentation orgAugmentation)
        {
            this.OrgAugmentation = orgAugmentation;
        }
    }
}
