using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NIBRSXML.Constants;

namespace NIBRSXML.NIBRSReport.Misc
{
    [XmlRoot("ActivityIdentification", Namespace = Namespaces.niemCore)]
    public class ActivityIdentification
    {
        [XmlElement("IdentificationID", Namespace = Namespaces.niemCore)]
        public string id { get; set; }

        public ActivityIdentification() { }

        public ActivityIdentification(string id)
        {
            this.id = id;
        }
    }
}
