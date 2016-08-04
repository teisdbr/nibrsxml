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
        public string Id { get; set; }

        [XmlElement("Subject", Namespace = Namespaces.justice, Order = 1)]
        public Subject.Subject SubjectRef { get; set; }

        [XmlElement("Victim", Namespace = Namespaces.justice, Order = 2)]
        public Victim.Victim VictimRef { get; set; }

        [XmlElement("VictimToSubjectRelationshipCode", Namespace = Namespaces.justice, Order = 3)]
        public string RelationshipCode { get; set; }

        public SubjectVictimAssociation() { }

        public SubjectVictimAssociation(int id, Subject.Subject subject, Victim.Victim victim, string relationshipCode)
        {
            this.Id = "SubjectVictimAssocSP" + id.ToString();
            this.SubjectRef = subject.Reference;
            this.VictimRef = victim.Reference;
            this.RelationshipCode = RelationshipCode;
        }

        public SubjectVictimAssociation(int id, string subjectRef, string victimRef, string relationshipCode)
        {
            this.Id = "SubjectVictimAssocSP" + id.ToString();
            this.SubjectRef = new Subject.Subject(subjectRef);
            this.VictimRef = new Victim.Victim(victimRef);
            this.RelationshipCode = relationshipCode;
        }
    }
}
