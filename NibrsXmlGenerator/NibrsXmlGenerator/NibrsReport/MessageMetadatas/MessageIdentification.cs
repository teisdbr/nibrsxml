using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.MessageMetadatas
{

   [XmlRoot("MessageIdentification", Namespace = Namespaces.cjis)]
   public  class MessageIdentification
    {
         [XmlElement("IdentificationID", Namespace = Namespaces.niemCore, Order = 1)]
         public string IdentificationID { get; set; }



         public MessageIdentification() { }

    }
}
