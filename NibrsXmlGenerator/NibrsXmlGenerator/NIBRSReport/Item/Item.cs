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
        public ItemStatus Status { get; set; }

        [XmlElement("ItemValue", Namespace = Namespaces.niemCore, Order = 2)]
        public ItemValue Value { get; set; }

        [XmlElement("ItemCategoryNIBRSPropertyCategoryCode", Namespace = Namespaces.justice, Order = 3)]
        public string NibrsPropertyCategoryCode { get; set; }

        [XmlElement("ItemQuantity", Namespace = Namespaces.niemCore, Order = 4)]
        public String Quantity { get; set; }

        public Item() { }

        public Item(ItemStatus status, ItemValue value, string nibrsPropertyCategoryCode, int quantity)
        {
            this.Status = status;
            this.Value = value;
            this.NibrsPropertyCategoryCode = nibrsPropertyCategoryCode;
            this.Quantity = quantity.ToString();
        }

        public Item(String statusCode, String valueAmount, String valueDate, String nibrsPropCategCode, String quantity)
        {
            this.Status = new ItemStatus(statusCode);
            this.Value = new ItemValue(valueAmount, valueDate);
            this.NibrsPropertyCategoryCode = nibrsPropCategCode;
            this.Quantity = quantity;
        }
    }
}
