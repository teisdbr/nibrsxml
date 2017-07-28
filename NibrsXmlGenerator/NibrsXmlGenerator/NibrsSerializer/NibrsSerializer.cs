using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport;
using NibrsXml.NibrsReport.Incident;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.NibrsReport.ReportHeader;


namespace NibrsXml.NibrsSerializer
{
    internal sealed class NibrsSerializer : XmlSerializer
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

        // Declare the types of all the objects that shall be serialized by NibrsSerializer
        // The root type is separate from the others because it is its own parameter in the base class's constructor
        private static readonly Type[] NonRootTypes = 
            {
                // NIBRSReport namespace
                typeof(Report), 
                
                // Incident namespace
                typeof(CjisIncidentAugmentation),
                typeof(Incident),
                typeof(IncidentExceptionalClearanceDate),
                typeof(JxdmIncidentAugmentation),

                // Offense namespace
                typeof(Offense),
                typeof(OffenseEntryPoint),
                typeof(OffenseFactor),
                typeof(OffenseForce),

                // Misc namespace
                typeof(ActivityDate),
                typeof(ActivityIdentification),
                typeof(RoleOfPerson),
                typeof(OrganizationAugmentation),
                typeof(OrganizationORIIdentification),
                
                // ReportHeader namespace
                typeof(ReportDate), 
                typeof(ReportHeader), 
                typeof(ReportingAgency)
            };

        // Declare the namespaces that will be used within the report
        private static readonly XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces(
            new[] 
            {
                new XmlQualifiedName(Aliases.nibrs, Constants.Namespaces.cjisNibrs),
                new XmlQualifiedName(Aliases.cjis, Constants.Namespaces.cjis),
                new XmlQualifiedName(Aliases.cjiscodes, Constants.Namespaces.cjisCodes),
                new XmlQualifiedName(Aliases.i, Constants.Namespaces.appInfo),
                new XmlQualifiedName(Aliases.ucr, Constants.Namespaces.fbiUcr),
                new XmlQualifiedName(Aliases.j, Constants.Namespaces.justice),
                new XmlQualifiedName(Aliases.term, Constants.Namespaces.niemTerminology),
                new XmlQualifiedName(Aliases.nc, Constants.Namespaces.niemCore),
                new XmlQualifiedName(Aliases.niemXsd, Constants.Namespaces.niemXsd),
                new XmlQualifiedName(Aliases.s, Constants.Namespaces.niemStructs),
                new XmlQualifiedName(Aliases.xsi, Constants.Namespaces.xsi),
                new XmlQualifiedName(Aliases.xsd, Constants.Namespaces.xsd),
                new XmlQualifiedName(Aliases.nibrscodes, Constants.Namespaces.cjisNibrsCodes)
            });

        public NibrsSerializer(Type type) : base(type, NonRootTypes)
        {
            // Constrain the argument to be either of type Submission or Report
            if (type != typeof(Submission) && type != typeof(Report))
                throw new ArgumentException(
                    "There was an error constructing the NibrsSerializer object." +
                    " Requires first argument to be either typeof(Submission) or typeof(Report)");
        }

        /// <summary>
        /// Writes an XML representation of the given NibrsSerializable object with the statically defined namespaces within this class
        /// </summary>
        /// <param name="serializee">The Submission or Report object to serialize</param>
        /// <returns>An XML representation of the argument object using NIBRS XML schema definitions</returns>
        public string Serialize(NibrsSerializable serializee)
        { 
            var xml = "";
            using (StringWriter xmlWriter = new Utf8StringWriter())
            {
                if (serializee.GetType() == typeof(Submission) || serializee.GetType() == typeof(Report))
                    base.Serialize(xmlWriter, serializee, Namespaces);
                else
                    throw new ArgumentException("The object provided must be of type Submission or Report.");
                xml = xmlWriter + "\r\n";
            }
            return xml;
        }
    }
}
