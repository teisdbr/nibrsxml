using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NIBRSXML.Constants;

namespace NIBRSXML.NIBRSReport
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
