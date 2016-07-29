using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Item
{
    [XmlRoot("ItemStatus", Namespace = Namespaces.niemCore)]
    public class ItemStatus
    {
        [XmlElement("ItemStatusCode", Namespace = Namespaces.cjis)]
        public string statusCode { get; set; }

        public ItemStatus() { }

        public ItemStatus(string statusCode)
        {
            this.statusCode = statusCode;
        }
    }
}
