using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Offense
{
    [XmlRoot("OffenseFactor", Namespace = Namespaces.justice)]
    public class OffenseFactor
    {
        [XmlElement("OffenseFactorCode", Namespace = Namespaces.justice)]
        public string Code { get; set; }

        public OffenseFactor() { }

        public OffenseFactor(string code)
        {
            this.Code = code;
        }
    }
}
