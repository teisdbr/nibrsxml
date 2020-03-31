using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;

namespace NibrsXml.NibrsReport.EnforcementOfficial
{
    [XmlRoot("EnforcementOfficialUnit", Namespace = Namespaces.justice)]
    public class EnforcementOfficialUnit
    {
        public EnforcementOfficialUnit()
        {
        }

        public EnforcementOfficialUnit(OrganizationAugmentation orgAugmentation)
        {
            OrgAugmentation = orgAugmentation;
        }

        [XmlElement("OrganizationAugmentation", Namespace = Namespaces.justice)]
        public OrganizationAugmentation OrgAugmentation { get; set; }
    }
}