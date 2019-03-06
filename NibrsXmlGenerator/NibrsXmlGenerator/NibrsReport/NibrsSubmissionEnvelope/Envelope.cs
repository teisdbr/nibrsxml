using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using LoadBusinessLayer;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NibrsXml.Builder;
using NibrsXml.Constants;
using NibrsXml.NibrsReport;

namespace NibrsXml.NibrsReport.NibrsSubmissionEnvelope
{
    
    public partial class Envelope 
    {
        [SoapElement(ElementName = "Header")]
        [SoapAttribute(Namespace = Namespaces.soapenv)]
        public string Header { get; set; }
        
        
        //[SoapElement(ElementName = "Body")]
        //[SoapAttribute(Namespace = Namespaces.soapenv)]
        //public Body Body { get; set; } = new Body();

    }

    //[XmlRoot(ElementName = "SubmitNibrsNIEMDocument", Namespace = Namespaces.ws)]
    //public partial class SubmitNibrsNIEMDocument
    //{
    //    [XmlElement(ElementName = "xmlDoc")]
    //    public string XmlDoc { get; set; }
    //}

    //[XmlRoot(ElementName = "Body", Namespace = Namespaces.soapenv)]
    //public partial class Body
    //{
    //    [XmlElement(ElementName = "SubmitNibrsNIEMDocument", Namespace = Namespaces.ws)]
    //    public SubmitNibrsNIEMDocument SubmitNibrsNIEMDocument { get; set; } = new SubmitNibrsNIEMDocument();
    //}
}
