using System.Xml.Serialization;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.EnforcementOfficial
{
    [XmlRoot("EnforcementOfficialUnit", Namespace = Namespaces.justice)]
    public class EnforcementOfficialUnit
    {
        [XmlElement("OrganizationAugmentation", Namespace = Namespaces.justice)]
        public OrganizationAugmentation OrgAugmentation { get; set; }

        public EnforcementOfficialUnit() { }

        public EnforcementOfficialUnit(OrganizationAugmentation orgAugmentation)
        {
            this.OrgAugmentation = orgAugmentation;
        }
    }
}
