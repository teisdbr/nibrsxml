using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Misc
{
    [XmlRoot("OrganizationAugmentation", Namespace = Namespaces.justice)]
    public class OrganizationAugmentation
    {
        [XmlElement("OrganizationORIIdentification", Namespace = Namespaces.justice)]
        public OrganizationORIIdentification OrgOriId { get; set; }

        public OrganizationAugmentation() { }

        public OrganizationAugmentation(OrganizationORIIdentification orgOriId)
        {
            this.OrgOriId = orgOriId;
        }
    }
}
