using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;

namespace NibrsXml.NibrsReport.MessageMetadatas
{
    [XmlRoot("MessageSubmittingOrganization", Namespace = Namespaces.cjis)]
    public class MessageSubmittingOrganization
    {
        [XmlElement("OrganizationAugmentation", Namespace = Namespaces.justice, Order = 1)]
        public OrganizationAugmentation OrganizationAugmentation { get; set; }
    }
}