using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;

namespace NibrsXml.NibrsReport.Subject
{
    [XmlRoot("Subject", Namespace = Namespaces.justice)]
    public class Subject
    {
        [XmlIgnore]
        public Person.Person Person { get; set; }

        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string Id { get; set; }

        /// <summary>
        /// This property is public only For serialization.
        /// It should only be set by using the Subject(string) constructor and accessed using the reference property.
        /// </summary>
        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string SubjectRef { get; set; }

        [XmlElement("RoleOfPerson", Namespace = Namespaces.niemCore, Order = 1)]
        public RoleOfPerson Role { get; set; }

        [XmlElement("SubjectSequenceNumberText", Namespace = Namespaces.justice, Order = 2)]
        public string SeqNum { get; set; }

        [XmlIgnore]
        public Subject Reference
        {
            get
            {
                return new Subject(this.Id);
            }
        }
        public Subject() { }

        public Subject(string subjectId)
        {
            this.SubjectRef = subjectId;
        }

        public Subject(
            Person.Person person,
            string seqNum,
            string uniquePrefix)
        {
            this.Person = person;
            //this.Person.Id += "PersonSubject" + seqNum.TrimStart('0');
            this.Role = new RoleOfPerson(this.Person.Id);
            this.SeqNum = seqNum.TrimStart('0');
            this.Id = uniquePrefix + "Subject" + seqNum.TrimStart('0');
        }
    }
}
