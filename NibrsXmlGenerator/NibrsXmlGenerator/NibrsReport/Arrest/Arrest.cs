using System.Xml.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;

namespace NibrsXml.NibrsReport.Arrest
{
    [XmlRoot("Arrest", Namespace = Namespaces.justice)]
    public class Arrest
    {
        public Arrest()
        {
        }

        public Arrest(string arrestId)
        {
            ArrestRef = arrestId;
        }

        public Arrest(string uniquePrefix, string arrestId, ActivityIdentification activityId, ActivityDate date,
            ArrestCharge charge, string categoryCode, string subjectCountCode)
        {
            Id = uniquePrefix + "Arrest" + arrestId.TrimStart('0') + "-" + activityId.Id.Trim();
            ActivityId = activityId;
            Date = date;
            Charge = charge;
            CategoryCode = categoryCode;
            SubjectCountCode = subjectCountCode;

            //Save the sequence number for matching to arrestee later on
            SequenceNumber = arrestId.TrimStart('0');
        }

        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string Id { get; set; }

        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string ArrestRef { get; set; }

        [XmlElement("ActivityIdentification", Namespace = Namespaces.niemCore, Order = 1)]
        public ActivityIdentification ActivityId { get; set; }

        [XmlElement("ActivityDate", Namespace = Namespaces.niemCore, Order = 2)]
        public ActivityDate Date { get; set; }

        [XmlElement("ArrestCharge", Namespace = Namespaces.justice, Order = 3)]
        public ArrestCharge Charge { get; set; }

        [XmlElement("ArrestCategoryCode", Namespace = Namespaces.justice, Order = 4)]
        public string CategoryCode { get; set; }

        [XmlIgnore] public string SubjectCountCode { get; set; }

        [BsonIgnore]
        [XmlIgnore]
        [JsonIgnore]
        public Arrest Reference
        {
            get { return new Arrest(Id); }
        }

        [BsonIgnore] [XmlIgnore] [JsonIgnore] public string SequenceNumber { get; set; }
    }
}