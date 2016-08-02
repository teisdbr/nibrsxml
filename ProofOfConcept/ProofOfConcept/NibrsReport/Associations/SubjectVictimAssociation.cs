using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Associations
{
    [XmlRoot("SubjectVictimAssociation", Namespace = Namespaces.justice)]
    public class SubjectVictimAssociation// : Association
    {
        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string id { get; set; }

        [XmlElement("Subject", Namespace = Namespaces.justice, Order = 1)]
        public Subject.Subject subjectRef { get; set; }

        [XmlElement("Victim", Namespace = Namespaces.justice, Order = 2)]
        public Victim.Victim victimRef { get; set; }

        [XmlElement("VictimToSubjectRelationshipCode", Namespace = Namespaces.justice, Order = 3)]
        public string relationshipCode { get; set; }

        public SubjectVictimAssociation() { }

        public SubjectVictimAssociation(int id, Subject.Subject subject, Victim.Victim victim, string relationshipCode)
        {
            this.id = "SubjectVictimAssocSP" + id.ToString();
            this.subjectRef = subject.reference;
            this.victimRef = victim.reference;
            this.relationshipCode = relationshipCode;
        }
    }
}
