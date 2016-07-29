using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Item
{
    [XmlRoot("ItemValue", Namespace = Namespaces.niemCore)]
    public class ItemValue
    {
        [XmlElement("ItemValueAmount", Namespace = Namespaces.niemCore, Order = 1)]
        public ItemValueAmount amount { get; set; }

        [XmlElement("ItemValueDate", Namespace = Namespaces.niemCore, Order = 2)]
        public ItemValueDate date { get; set; }

        public ItemValue() { }

        public ItemValue(ItemValueAmount amount, ItemValueDate date)
        {
            this.amount = amount;
            this.date = date;
        }
    }
}
