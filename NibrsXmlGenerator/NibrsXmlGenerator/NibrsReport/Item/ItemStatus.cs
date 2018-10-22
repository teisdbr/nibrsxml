using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Item
{
    [XmlRoot("ItemStatus", Namespace = Namespaces.niemCore)]
    public class ItemStatus
    {
        public ItemStatus()
        {
        }

        public ItemStatus(string code)
        {
            Code = code;
        }

        [XmlElement("ItemStatusCode", Namespace = Namespaces.cjis)]
        public string Code { get; set; }
    }
}