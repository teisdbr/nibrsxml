using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Item
{
    [XmlRoot("Item", Namespace = Namespaces.niemCore)]
    public class Item
    {
        [XmlElement("ItemStatus", Namespace = Namespaces.niemCore, Order = 1)]
        public ItemStatus status { get; set; }

        [XmlElement("ItemValue", Namespace = Namespaces.niemCore, Order = 2)]
        public ItemValue value { get; set; }

        [XmlElement("ItemCategoryNIBRSPropertyCategoryCode", Namespace = Namespaces.justice, Order = 3)]
        public string nibrsPropertyCategoryCode { get; set; }

        [XmlElement("ItemQuantity", Namespace = Namespaces.niemCore, Order = 4)]
        public int quantity { get; set; }

        public Item() { }

        public Item(ItemStatus status, ItemValue value, string nibrsPropertyCategoryCode, int quantity)
        {
            this.status = status;
            this.value = value;
            this.nibrsPropertyCategoryCode = nibrsPropertyCategoryCode;
            this.quantity = quantity;
        }
    }
}
