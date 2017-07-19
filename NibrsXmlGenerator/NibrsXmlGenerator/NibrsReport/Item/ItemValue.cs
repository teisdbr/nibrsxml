using System;
using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Item
{
    [XmlRoot("ItemValue", Namespace = Namespaces.niemCore)]
    public class ItemValue
    {
        [XmlElement("ItemValueAmount", Namespace = Namespaces.niemCore, Order = 1)]
        public ItemValueAmount ValueAmount { get; set; }

        [XmlElement("ItemValueDate", Namespace = Namespaces.niemCore, Order = 2)]
        public ItemValueDate ValueDate { get; set; }

        public ItemValue() { }

        public ItemValue(ItemValueAmount valueAmount, ItemValueDate valueDate)
        {
            this.ValueAmount = valueAmount;
            this.ValueDate = valueDate;
        }

        public ItemValue(string itemValueAmount, string itemValueDate)
        {
            this.ValueAmount = new ItemValueAmount(itemValueAmount);
            
            if (itemValueDate != null)
                this.ValueDate = new ItemValueDate(itemValueDate);
        }
    }
}
