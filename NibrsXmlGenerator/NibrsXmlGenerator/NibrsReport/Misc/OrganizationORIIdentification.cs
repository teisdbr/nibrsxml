using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Misc
{
    [XmlRoot("OrganizationORIIdentification", Namespace = Namespaces.justice)]
    public class OrganizationORIIdentification
    {
        public OrganizationORIIdentification()
        {
        }

        public OrganizationORIIdentification(string id)
        {
            Id = id;
        }

        [XmlElement("IdentificationID", Namespace = Namespaces.niemCore)]
        public string Id { get; set; }
    }
}