using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Item
{
    [XmlRoot("ItemValueAmount", Namespace = Namespaces.niemCore)]
	public class ItemValueAmount
    {
        [XmlElement("Amount", Namespace = Namespaces.niemCore)]
        public String Amount { get; set; }

        public ItemValueAmount() { }

        public ItemValueAmount(int amount)
        {
            this.Amount = amount.ToString();
        }
        public ItemValueAmount(String amount)
        {
            this.Amount = amount;
        }
    }
}
