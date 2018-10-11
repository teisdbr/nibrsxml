using NibrsXml.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.NibrsReport;
using NibrsXml.NibrsReport.MessageMetadatas;

namespace NibrsXml.NibrsReport
{
    [XmlRoot("MessageMetadata", Namespace = Namespaces.cjis)]

    public class MessageMetadata : NibrsSerializable

    {

        [XmlElement("MessageDateTime", Namespace = Namespaces.cjis, Order = 1)]
        public String MessageDateTime { get; set; }

        [XmlElement("MessageIdentification", Namespace = Namespaces.cjis, Order = 2)]
        public MessageIdentification MessageIdentification { get; set; }


        [XmlElement("MessageImplementationVersion", Namespace = Namespaces.cjis, Order = 3)]
        public float Version { get; set; }


        [XmlElement("MessageSubmittingOrganization", Namespace = Namespaces.cjis, Order = 4)]
        public MessageSubmittingOrganization MessageSubmittingOrganization { get; set; }



        //public MessageMetadata() {

        //    this.MessageIdentification = new MessageIdentification();
        //    this.MessageSubmittingOrganization = new MessageSubmittingOrganization();
        
        
        
        //}

       





    }
}
