using NibrsXml.Constants;
using NibrsXml.NibrsReport.NibrsSubmissionEnvelope;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Soap;
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
                    try
                    {
                        //base.Serialize(xmlWriter, BuildEnvelope(Nibrsxml), Namespaces);

                        // Serializes a class named Envelope as a SOAP message.  
                        XmlTypeMapping myTypeMapping =
                            new SoapReflectionImporter().ImportTypeMapping(typeof(Envelope));
                        XmlSerializer mySerializer = new XmlSerializer(myTypeMapping);
                        Envelope NDoc = new Envelope();
                        //NDoc.XmlDoc = "![CDATA[" + Nibrsxml + "]]";
                        //mySerializer.Serialize(xmlWriter, NDoc, Namespaces);
                        FileStream fs = new FileStream("DataFile.soap", FileMode.Create);

                        SoapFormatter formatter = new SoapFormatter();
                        SubmitNibrsNIEMDocument sd = new SubmitNibrsNIEMDocument();

                        formatter.Serialize(fs, sd);
                        xml = xmlWriter.ToString();
                    }
                    catch(Exception ex)
                    {
                        string error = ex.Message;
                        throw new ArgumentException("Error occured while generating the NIBRS Submission SOAP XML Envelope ");
                    }
                }
            }
            return xml;
        }

        public Envelope BuildEnvelope(string Nibrsxml)
        {
            Envelope envelope = new Envelope();

            //envelope.Body.SubmitNibrsNIEMDocument.XmlDoc = "![CDATA[" + Nibrsxml + "]]";

            return envelope;
        }

    }
}
