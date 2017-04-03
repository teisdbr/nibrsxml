using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Arrest
{
    [XmlRoot("ArrestCharge", Namespace = Namespaces.justice)]
    public class ArrestCharge
    {
        [XmlElement("ChargeUCRCode", Namespace = Namespaces.cjisNibrs)]
        public string UcrCode { get; set; }

        public ArrestCharge() { }

        public ArrestCharge(string ucrCode)
        {
            this.UcrCode = ucrCode;
        }
    }
}
