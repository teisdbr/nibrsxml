using System;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Item
{
    [XmlRoot("Item", Namespace = Namespaces.niemCore)]
    public class Item : IComparable
    {
        public Item()
        {
        }

        public Item(ItemStatus status, ItemValue value, string nibrsPropertyCategoryCode, int quantity)
        {
            Status = status;
            Value = value;
            NibrsPropertyCategoryCode = nibrsPropertyCategoryCode;
            Quantity = quantity.ToString();
        }

        public Item(string statusCode, string valueAmount, string valueDate, string nibrsPropCategCode, string quantity)
        {
            Status = new ItemStatus(statusCode);
            if (valueAmount != null & valueDate != null)
                Value = new ItemValue(valueAmount, valueDate);
            NibrsPropertyCategoryCode = nibrsPropCategCode;
            Quantity = quantity;
        }

        [XmlElement("ItemStatus", Namespace = Namespaces.niemCore, Order = 1)]
        public ItemStatus Status { get; set; }

        [XmlElement("ItemValue", Namespace = Namespaces.niemCore, Order = 2)]
        public ItemValue Value { get; set; }

        [XmlElement("ItemCategoryNIBRSPropertyCategoryCode", Namespace = Namespaces.justice, Order = 3)]
        public string NibrsPropertyCategoryCode { get; set; }

        [XmlElement("ItemQuantity", Namespace = Namespaces.niemCore, Order = 4)]
        public string Quantity { get; set; }

        public int CompareTo(object b)
        {
            if (b == null)
                throw new ArgumentNullException();

            var otherItem = b as Item;
            if (otherItem != null)
                return Convert.ToInt32(Value.ValueAmount.Amount) - Convert.ToInt32(otherItem.Value.ValueAmount.Amount);
            throw new ArgumentException("Object is not an Item.");
        }
    }
}