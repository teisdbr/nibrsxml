using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.EnforcementOfficial
{
    [XmlRoot("EnforcementOfficialUnit", Namespace = Namespaces.justice)]
    public class EnforcementOfficialUnit
    {
        [XmlElement("OrganizationAugmentation", Namespace = Namespaces.justice)]
        public OrganizationAugmentation orgAugmentation { get; set; }

        public EnforcementOfficialUnit() { }

        public EnforcementOfficialUnit(OrganizationAugmentation orgAugmentation)
        {
            this.orgAugmentation = orgAugmentation;
        }
    }
}
