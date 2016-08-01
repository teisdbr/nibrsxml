using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Misc
{
    [XmlRoot("RoleOfPerson", Namespace = Namespaces.niemCore)]
    public class RoleOfPerson
    {
        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string personRef { get; set; }

        public RoleOfPerson() { }

        public RoleOfPerson(string personRef)
        {
            this.personRef = personRef;
        }
    }
}
