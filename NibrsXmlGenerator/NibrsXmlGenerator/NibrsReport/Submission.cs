using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using LoadBusinessLayer;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NibrsXml.NibrsReport.NibrsSubmissionEnvelope;
using NibrsXml.Builder;
using NibrsXml.Constants;
using NibrsInterface;

namespace NibrsXml.NibrsReport
{
    /// <summary>
    ///     In the NibrsReport namespace, all XML elements that must be serialized must be given public access in order for
    ///     NibrsSerializer to print them accordingly. This also gives full freedom for NibrsReportBuilder to build reports
    ///     however it sees fit.
    /// </summary>
    [XmlRoot("Submission", Namespace = Namespaces.cjisNibrs)]
    public class Submission : INibrsSerializable
    {
        [BsonIgnore] [XmlIgnore]
        private static readonly NibrsSerializer.NibrsSerializer Serializer =
            new NibrsSerializer.NibrsSerializer(typeof(Submission));

        [XmlElement("MessageMetadata", Namespace = Namespaces.cjis)]
        public MessageMetadata MessageMetadata = new MessageMetadata();

        [BsonIgnore] [XmlIgnore] public List<Report> RejectedReports = new List<Report>();


        [XmlElement("Report")] public List<Report> Reports = new List<Report>();

        [XmlAttribute("schemaLocation", Namespace = XmlSchema.InstanceNamespace)]
        public string XsiSchemaLocation = Constants.Misc.schemaLocation;

        public Submission()
        {
            Id = ObjectId.GenerateNewId();
        }

        public Submission(params Report[] reports)
        {
            Id = ObjectId.GenerateNewId();

            foreach (var r in reports) Reports.Add(r);
        }

        [BsonId] [XmlIgnore] public ObjectId Id { get; set; }

        [XmlIgnore] public string Runnumber { get; set; }

        [XmlIgnore]
        public string Xml
        {
            get { return Serializer.Serialize(this); }

        }

        

        public static Submission Deserialize(string filepath)
        {
            // Retrieve the XML file
            var xmlFile = new FileStream(filepath, FileMode.Open);
            var xmlReader = XmlReader.Create(xmlFile);

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

                sub = (Submission) Serializer.Deserialize(xmlReader);
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
        ///     Use to write NIBRS XML for a single agency/WinLIBRS runnumber.
        /// </summary>
        /// <param name="list">Incident data to be used</param>
        /// <param name="fileName">Complete file name with path prefixed</param>
        public static void WriteXml(IncidentList list, string fileName)
        {
            WriteXml(new List<IncidentList> {list}, fileName);
        }

        /// <summary>
        ///     Use to write NIBRS XML for multiple agencies/WinLIBRS runnumbers.
        /// </summary>
        /// <param name="lists">Incident data to be used</param>
        /// <param name="fileName">Complete file name with path prefixed</param>
        /// <param name="nibrsSchemaLocation"></param>
        public static Submission WriteXml(List<IncidentList> lists, string fileName,
            string nibrsSchemaLocation = Constants.Misc.schemaLocation)
        {
            var submissions = SubmissionBuilder.BuildMultipleSubmission(lists);
            //Allows overriding of the location, primarily for individual ORI xmls at this point.  /ORI/NIBRS


            foreach (var submission in submissions)
            {
                // Save locally
                submission.XsiSchemaLocation = nibrsSchemaLocation;
                var xdoc = new XmlDocument();
                xdoc.LoadXml(submission.Xml);
                xdoc.Save(fileName.Replace(".xml", Guid.NewGuid() + ".xml"));

                // Call method to create soap envelop
                SubmissionEnvelopeSerializer serializer = new SubmissionEnvelopeSerializer();
                string envelope =  serializer.Serialize(submission.Xml);

                // Call Send report method to get response from FBI
                if(! string.IsNullOrWhiteSpace(envelope))
                {
                    string response = NibrsSubmitter.Sendreport(envelope);
                }

                // Save response to MongoDB


            }

            //Return submission created above
            return submissions[0];
        }
    }
}