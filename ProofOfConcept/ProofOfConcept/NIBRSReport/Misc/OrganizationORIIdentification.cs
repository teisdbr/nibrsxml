using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Misc
{
    public class OrganizationORIIdentification
    {
        [XmlElement(Namespace = Namespaces.niemCore)]
        public string IdentificationID;

        public OrganizationORIIdentification() { }

        public OrganizationORIIdentification(string id)
        {
            this.IdentificationID = id;
        }
    }
}
