using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Associations
{
    [XmlRoot("Victim", Namespace = Namespaces.justice)]
    public class VictimReference
    {
        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string victimId { get; set; }

        public VictimReference() { }

        public VictimReference(Victim.Victim victim)
        {
            //this.victimId = victim.;
        }
    }
}
