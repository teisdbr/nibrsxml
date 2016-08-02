using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Misc
{
    [XmlRoot("ActivityDate", Namespace = Namespaces.niemCore)]
    public class ActivityDate
    {
        [XmlElement("DateTime", Namespace = Namespaces.niemCore)]
        public string dateTime { get; set; }

        [XmlElement("Date", Namespace = Namespaces.niemCore)]
        public string date { get; set; }

        public string time { get { return dateTime.Substring(dateTime.IndexOf("T") + 1); } }

        public ActivityDate() { }

        public ActivityDate(string date)
        {
            this.date = date;
        }

        public ActivityDate(string date, string time)
        {
            this.dateTime = date + "T" + time;
        }
    }
}
