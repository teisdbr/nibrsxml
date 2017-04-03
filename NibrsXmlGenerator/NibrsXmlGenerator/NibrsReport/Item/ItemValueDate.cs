using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Item
{
    [XmlRoot("ItemValueDate", Namespace = Namespaces.niemCore)]
    public class ItemValueDate
    {
        [XmlElement("Date", Namespace = Namespaces.niemCore)]
        public string Date { get; set; }

        public ItemValueDate() { }

        public ItemValueDate(string date)
        {
            this.Date = date;
        }
    }
}
