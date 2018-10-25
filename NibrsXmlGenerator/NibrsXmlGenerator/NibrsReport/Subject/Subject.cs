using System.Xml.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;

namespace NibrsXml.NibrsReport.Subject
{
    [XmlRoot("Subject", Namespace = Namespaces.justice)]
    public class Subject
    {
        public Subject()
        {
        }

        public Subject(string subjectId)
        {
            SubjectRef = subjectId;
        }

        public Subject(
            Person.Person person,
            string seqNum,
            string uniquePrefix)
        {
            Person = person;
            Role = new RoleOfPerson(Person.Id);
            this.SeqNum = int.Parse(seqNum).ToString();
            this.Id = uniquePrefix + "Subject" + this.SeqNum;
        }

        [XmlIgnore] public Person.Person Person { get; set; }

        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string Id { get; set; }

        /// <summary>
        ///     This property is public only For serialization.
        ///     It should only be set by using the Subject(string) constructor and accessed using the reference property.
        /// </summary>
        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string SubjectRef { get; set; }

        [XmlElement("RoleOfPerson", Namespace = Namespaces.niemCore, Order = 1)]
        public RoleOfPerson Role { get; set; }

        [XmlElement("SubjectSequenceNumberText", Namespace = Namespaces.justice, Order = 2)]
        public string SeqNum { get; set; }

        [BsonIgnore] [XmlIgnore]
        public Subject Reference
        {
            get { return new Subject(Id); }
        }
    }
}