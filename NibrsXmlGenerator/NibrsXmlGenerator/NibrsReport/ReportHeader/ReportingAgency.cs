using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;

namespace NibrsXml.NibrsReport.ReportHeader
{
    public class ReportingAgency
    {
        public ReportingAgency()
        {
        }

        public ReportingAgency(OrganizationAugmentation orgAugmentation)
        {
            OrgAugmentation = orgAugmentation;
        }

        [XmlElement("OrganizationAugmentation", Namespace = Namespaces.justice)]
        public OrganizationAugmentation OrgAugmentation { get; set; }
    }
}