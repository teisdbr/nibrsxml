using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Associations
{
    [XmlRoot("Offense", Namespace = Namespaces.justice)]
    public class OffenseReference
    {
        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string offenseId { get; set; }

        public OffenseReference() { }

        public OffenseReference(Offense.Offense offense)
        {
            this.offenseId = offense.offenseId;
        }
    }
}
