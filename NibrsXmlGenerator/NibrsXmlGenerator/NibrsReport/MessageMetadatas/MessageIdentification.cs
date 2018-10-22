using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.MessageMetadatas
{
    [XmlRoot("MessageIdentification", Namespace = Namespaces.cjis)]
    public class MessageIdentification
    {
        [XmlElement("IdentificationID", Namespace = Namespaces.niemCore, Order = 1)]
        public string IdentificationId { get; set; }
    }
}