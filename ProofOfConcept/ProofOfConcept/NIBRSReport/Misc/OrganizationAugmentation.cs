using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Misc
{
    public class OrganizationAugmentation
    {
        public OrganizationORIIdentification OrganizationORIIdentification { get; set; }

        public OrganizationAugmentation() { }

        public OrganizationAugmentation(OrganizationORIIdentification id)
        {
            this.OrganizationORIIdentification = id;
        }
    }
}
