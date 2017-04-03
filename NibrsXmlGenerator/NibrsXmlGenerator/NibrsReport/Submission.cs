using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using NibrsXml.Constants;
using System.IO;
using System.Xml;
using LoadBusinessLayer;
using NibrsXml.Builder;

namespace NibrsXml.NibrsReport
{
    /// <summary>
    /// In the NibrsReport namespace, all XML elements that must be serialized must be given public access in order for
    /// NibrsSerializer to print them accordingly. This also gives full freedom for NibrsReportBuilder to build reports
    /// however it sees fit.
    /// </summary>
    [XmlRoot("Submission", Namespace = Namespaces.cjisNibrs)]
    public class Submission : NibrsSerializable
    {
        [XmlAttribute("schemaLocation", Namespace = System.Xml.Schema.XmlSchema.InstanceNamespace)]
        public string XsiSchemaLocation = NibrsXml.Constants.Misc.schemaLocation;

        [XmlElement("Report")]
        public List<Report> Reports = new List<Report>();

        [XmlIgnore]
        private static readonly NibrsSerializer.NibrsSerializer serializer = new NibrsSerializer.NibrsSerializer(typeof(Submission));

        [XmlIgnore]
        public string Xml { get { return serializer.Serialize(this); } }

        public Submission() { }

        public Submission(params Report[] reports)
        {
            foreach (Report r in reports)
            {
                this.Reports.Add(r);
            }
        }

        public static Submission Deserialize(string filepath)
        {
            // Retrieve the XML file
            FileStream xmlFile = new FileStream(filepath, FileMode.Open);
            XmlReader xmlReader = XmlReader.Create(xmlFile);

            // Deserialize the XML file
            Submission sub;
            try
            {

                //When deserializing, associations and persons do not have the full context of their complex elements.
                //Deserialization of XML nodes does not cross check the other XML nodes to give the original full context of the data involved;
                //It will create objects for only what is present within the node being deserialized.

                //For example, if you deseriablize an OffenseVictimAssociation you only have the context of the IDs of the associated offense and victim.
                //Further, you would not have the full context of the victim either because the victim is composed of a person, so you need to use the victim's ID
                //and retrieve the person data for that victim.

                sub = (Submission)serializer.Deserialize(xmlReader);
                foreach (var report in sub.Reports) report.RebuildCrossReferencedRelationships();
            }
            catch (Exception e)
            {
                throw new Exception("There was an error deserializing a submission.", e);
            }

            // Close the file and return
            xmlFile.Close();
            return sub;
        }

        /// <summary>
        /// Use to write NIBRS XML for a single agency/WinLIBRS runnumber.
        /// </summary>
        /// <param name="list">Incident data to be used</param>
        /// <param name="fileName">Complete file name with path prefixed</param>
        public static void WriteXml(IncidentList list, string fileName)
        {
            WriteXml(new List<IncidentList> { list }, fileName);
        }

        /// <summary>
        /// Use to write NIBRS XML for multiple agencies/WinLIBRS runnumbers.
        /// </summary>
        /// <param name="lists">Incident data to be used</param>
        /// <param name="fileName">Complete file name with path prefixed</param>
        public static void WriteXml(List<IncidentList> lists, string fileName, string nibrsSchemaLocation = NibrsXml.Constants.Misc.schemaLocation)
        {
            var submission = SubmissionBuilder.Build(lists);
            //Allows overriding of the location, primarily for individual ORI xmls at this point.  /ORI/NIBRS
            submission.XsiSchemaLocation = nibrsSchemaLocation;
            var xdoc = new XmlDocument();
            xdoc.LoadXml(submission.Xml);
            xdoc.Save(fileName);
        }
    }
}
