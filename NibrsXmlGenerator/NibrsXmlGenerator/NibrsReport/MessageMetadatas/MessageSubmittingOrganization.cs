using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.MessageMetadatas;


namespace NibrsXml.NibrsReport.MessageMetadatas
{
    [XmlRoot("MessageSubmittingOrganization", Namespace = Namespaces.cjis)]
    public class MessageSubmittingOrganization
    {

        [XmlElement("OrganizationAugmentation", Namespace = Namespaces.justice, Order = 1)]
        public NibrsXml.NibrsReport.Misc.OrganizationAugmentation OrganizationAugmentation { get; set; }

        public MessageSubmittingOrganization() { }
    }
}
