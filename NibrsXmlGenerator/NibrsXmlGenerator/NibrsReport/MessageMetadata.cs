using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.MessageMetadatas;

namespace NibrsXml.NibrsReport
{
    [XmlRoot("MessageMetadata", Namespace = Namespaces.cjis)]
    public class MessageMetadata : INibrsSerializable

    {
        [XmlElement("MessageDateTime", Namespace = Namespaces.cjis, Order = 1)]
        public string MessageDateTime { get; set; }

        [XmlElement("MessageIdentification", Namespace = Namespaces.cjis, Order = 2)]
        public MessageIdentification MessageIdentification { get; set; }


        [XmlElement("MessageImplementationVersion", Namespace = Namespaces.cjis, Order = 3)]
        public float Version { get; set; }


        [XmlElement("MessageSubmittingOrganization", Namespace = Namespaces.cjis, Order = 4)]
        public MessageSubmittingOrganization MessageSubmittingOrganization { get; set; }
    }
}