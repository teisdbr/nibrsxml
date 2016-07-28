using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NIBRSXML.Constants;
using NIBRSXML.NIBRSReport.Misc;

namespace NIBRSXML.NIBRSReport.ReportHeader
{
    public class ReportingAgency
    {
        [XmlElement(Namespace = Namespaces.justice)]
        public OrganizationAugmentation OrganizationAugmentation { get; set; }

        public ReportingAgency() { }

        public ReportingAgency(OrganizationAugmentation orgAug)
        {
            this.OrganizationAugmentation = orgAug;
        }
    }
}
