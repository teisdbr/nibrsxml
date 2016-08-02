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
        public Person.Person person { get; set; }

        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string subjectRef { get; set; }

        [XmlElement("RoleOfPerson", Namespace = Namespaces.niemCore, Order = 1)]
        public RoleOfPerson role { get; set; }

        [XmlElement("SubjectSequenceNumberText", Namespace = Namespaces.justice, Order = 2)]
        public string seqNum { get; set; }

        [XmlIgnore]
        public Subject reference { get { return new Subject(person.id); } }

        public Subject() { }

        public Subject(string subjectId)
        {
            this.subjectRef = subjectId;
        }

        public Subject(
            Person.Person person,
            int seqNum)
        {
            this.person = person;
            this.person.id = "PersonSubject" + seqNum.ToString();
            this.role = new RoleOfPerson(this.person.id);
            this.seqNum = seqNum.ToString();
        }
    }
}
