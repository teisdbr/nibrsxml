using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.NibrsReport.Person;

namespace NibrsXml.NibrsReport.Subject
{
    [XmlRoot("Subject", Namespace = Namespaces.justice)]
    public class Subject
    {
        [XmlIgnore]
        public Person.Person Person { get; set; }

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
        public Subject Reference { get { return new Subject(Person.Id); } }

        public Subject() { }

        public Subject(string subjectId)
        {
            this.SubjectRef = subjectId;
        }

        public Subject(
            Person.Person person,
            String seqNum)
        {
            this.Person = person;
            this.Person.Id += "PersonSubject" + seqNum;
            this.Role = new RoleOfPerson(this.Person.Id);
            this.SeqNum = seqNum;
        }
    }
}
