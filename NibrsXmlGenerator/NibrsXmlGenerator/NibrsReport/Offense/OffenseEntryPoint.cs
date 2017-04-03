using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Offense
{
    [XmlRoot("OffenseEntryPoint", Namespace = Namespaces.justice)]
    public class OffenseEntryPoint
    {
        [XmlElement("PassagePointMethodCode", Namespace = Namespaces.justice)]
        public string PassagePointMethodCode { get; set; }
        
        public OffenseEntryPoint() { }

        public OffenseEntryPoint(string passagePointMethodCode)
        {
            this.PassagePointMethodCode = passagePointMethodCode;
        }
    }
}
