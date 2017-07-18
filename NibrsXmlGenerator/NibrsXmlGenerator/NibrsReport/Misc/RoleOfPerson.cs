using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Misc
{
    [XmlRoot("RoleOfPerson", Namespace = Namespaces.niemCore)]
    public class RoleOfPerson
    {
        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string PersonId { get; set; }

        public RoleOfPerson() { }

        public RoleOfPerson(string personId)
        {
            this.PersonId = personId;
        }
    }
}
