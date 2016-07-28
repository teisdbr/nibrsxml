using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NIBRSXML.Constants;

namespace NIBRSXML.NIBRSReport.Misc
{
    [XmlRoot("ActivityDate", Namespace = Namespaces.niemCore)]
    public class ActivityDate
    {
        [XmlElement("DateTime", Namespace = Namespaces.niemCore)]
        public string dateTime { get; set; }

        public ActivityDate() { }

        public ActivityDate(string dateTime)
        {
            this.dateTime = dateTime;
        }
    }
}
