using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Item
{
    [XmlRoot("ItemValueAmount", Namespace = Namespaces.niemCore)]
	public class ItemValueAmount
    {
        [XmlElement("Amount", Namespace = Namespaces.niemCore)]
        public string Amount { get; set; }

        public ItemValueAmount() { }

        public ItemValueAmount(int amount)
        {
            this.Amount = amount.ToString();
        }
        public ItemValueAmount(string amount)
        {
            this.Amount = amount;
        }
    }
}
