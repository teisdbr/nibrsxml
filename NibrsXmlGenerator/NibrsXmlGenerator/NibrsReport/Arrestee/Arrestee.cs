using System.Collections.Generic;
using System.Xml.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.Utility;

namespace NibrsXml.NibrsReport.Arrestee
{
    [XmlRoot("Arrestee", Namespace = Namespaces.justice)]
    public class Arrestee
    {
        public Arrestee()
        {
        }

        public Arrestee(string arresteeId)
        {
            PersonRef = arresteeId;
        }
        /// <summary>
        /// Use this constructor  to build the empty arrestee to report Group B Arrest Delete's.
        /// </summary>
        /// <param name="seqId"></param>
        /// <param name="uniquePerfix"></param>
        public Arrestee(string seqId, string uniquePerfix)
        {
            Id = uniquePerfix + "Arrestee" + seqId.TrimStart('0');
            SeqId = seqId.TrimStart('0');
        }




        public Arrestee(
            Person.Person person,
            string seqId,
            string clearanceIndicator,
            List<string> armedWithCode,
            string juvenileDispositionCode,
            string subjectCountCode,
            string uniquePerfix)
        {
            Person = person;
            Id = uniquePerfix + "Arrestee" + seqId.TrimStart('0');
            //this.Person.Id += "PersonArrestee" + seqId.TrimStart('0');
            Role = new RoleOfPerson(Person.Id);
            SeqId = seqId.TrimStart('0');
            ClearanceIndicator = clearanceIndicator.ToLower().TrimNullIfEmpty();
            ArmedWithCodes = armedWithCode;
            JuvenileDispositionCode = juvenileDispositionCode;
            SubjectCountCode = subjectCountCode;
        }

        [XmlIgnore] public Person.Person Person { get; set; }

        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string Id { get; set; }

        /// <summary>
        ///     This property is public only For serialization.
        ///     It should only be set by using the Arrestee(string) constructor and accessed using the reference property.
        /// </summary>
        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string PersonRef { get; set; }

        [XmlElement("RoleOfPerson", Namespace = Namespaces.niemCore, Order = 1)]
        public RoleOfPerson Role { get; set; }

        [XmlElement("ArrestSequenceID", Namespace = Namespaces.justice, Order = 2)]
        public string SeqId { get; set; }

        [XmlElement("ArresteeClearanceIndicator", Namespace = Namespaces.justice, Order = 3)]
        public string ClearanceIndicator { get; set; }

        [XmlElement("ArresteeArmedWithCode", Namespace = Namespaces.justice, Order = 4)]
        public List<string> ArmedWithCodes { get; set; }

        [XmlElement("ArresteeJuvenileDispositionCode", Namespace = Namespaces.justice, Order = 5)]
        public string JuvenileDispositionCode { get; set; }

        [XmlElement("ArrestSubjectCountCode", Namespace = Namespaces.justice, Order = 6)]
        public string SubjectCountCode { get; set; }

        [BsonIgnore]
        [JsonIgnore]
        public Arrestee Reference
        {
            get { return new Arrestee(Id); }
        }
    }
}