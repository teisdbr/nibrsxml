using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Item
{
    [XmlRoot("ItemValue", Namespace = Namespaces.niemCore)]
    public class ItemValue
    {
        public ItemValue()
        {
        }

        public ItemValue(ItemValueAmount valueAmount, ItemValueDate valueDate)
        {
            ValueAmount = valueAmount;
            ValueDate = valueDate;
        }

        public ItemValue(string itemValueAmount, string itemValueDate)
        {
            if (itemValueAmount != null)
                ValueAmount = new ItemValueAmount(itemValueAmount);
            
            if (itemValueDate != null)
                ValueDate = new ItemValueDate(itemValueDate);
        }

        [XmlElement("ItemValueAmount", Namespace = Namespaces.niemCore, Order = 1)]
        public ItemValueAmount ValueAmount { get; set; }

        [XmlElement("ItemValueDate", Namespace = Namespaces.niemCore, Order = 2)]
        public ItemValueDate ValueDate { get; set; }
    }
}