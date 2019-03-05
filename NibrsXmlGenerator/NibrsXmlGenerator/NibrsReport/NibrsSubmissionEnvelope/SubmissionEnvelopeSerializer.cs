using NibrsXml.Constants;
using NibrsXml.NibrsSubmissionEnvelope.Envelope;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.NibrsSubmissionEnvelope
{
    class SubmissionEnvelopeSerializer : XmlSerializer
    {
       
             /// <summary>
             /// This class allows the serializer to write in UTF-8 formatting
             /// </summary>
        internal class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get
                {
                    return Encoding.UTF8;
                }
            }
        }

        private static readonly XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces(
           new[]
           {
                new XmlQualifiedName(Aliases.soapenv, Constants.Namespaces.soapenv),
                new XmlQualifiedName(Aliases.ws, Constants.Namespaces.ws)
           });




        public string Serialize(string Nibrsxml)
        {
            var xml = "";

            if (!String.IsNullOrWhiteSpace(Nibrsxml))
            {
                using (StringWriter xmlWriter = new Utf8StringWriter())
                {
                    try{
                        base.Serialize(xmlWriter, BuildEnvelope(Nibrsxml), Namespaces);

                        xml = xmlWriter.ToString();
                    }catch
                    {
                        throw new ArgumentException("Error occured while generating the NIBRS Submission SOAP Envelope xml");
                    }
                }
            }
            return xml;
        }

        public Envelope BuildEnvelope(string Nibrsxml)
        {
            Envelope envelope = new Envelope();

            envelope.Body.SubmitNibrsNIEMDocument.XmlDoc = "![CDATA[" + Nibrsxml + "]]";

            return envelope;
        }

    }
}
