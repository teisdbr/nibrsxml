using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Associations
{
    [XmlRoot("SubjectVictimAssociation", Namespace = Namespaces.justice)]
    public class SubjectVictimAssociation
    {
        public SubjectVictimAssociation()
        {
        }

        public SubjectVictimAssociation(string uniquePrefix, string id, Subject.Subject subject, Victim.Victim victim,
            string relationshipCode)
        {
            Id = uniquePrefix + "SubjectVictimAssocSP" + id;
            SubjectRef = subject.Reference;
            VictimRef = victim.Reference;
            RelationshipCode = relationshipCode;
            RelatedVictim = victim;
            RelatedSubject = subject;
        }

        public SubjectVictimAssociation(string uniquePrefix, string id, string subjectRef, string victimRef,
            string relationshipCode)
        {
            Id = "SubjectVictimAssocSP" + id;
            SubjectRef = new Subject.Subject(subjectRef);
            VictimRef = new Victim.Victim(victimRef);
            RelationshipCode = relationshipCode;
        }

        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string Id { get; set; }

        [XmlElement("Subject", Namespace = Namespaces.justice, Order = 1)]
        public Subject.Subject SubjectRef { get; set; }

        [XmlElement("Victim", Namespace = Namespaces.justice, Order = 2)]
        public Victim.Victim VictimRef { get; set; }

        [XmlElement("VictimToSubjectRelationshipCode", Namespace = Namespaces.cjisNibrs, Order = 3)]
        public string RelationshipCode { get; set; }

        [XmlIgnore] public Subject.Subject RelatedSubject { get; set; }

        [XmlIgnore] public Victim.Victim RelatedVictim { get; set; }
    }
}