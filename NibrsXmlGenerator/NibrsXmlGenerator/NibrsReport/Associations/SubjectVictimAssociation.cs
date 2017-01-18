using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;
using TeUtil.Extensions;

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

        public SubjectVictimAssociation(String uniquePrefix,String id, Subject.Subject subject, Victim.Victim victim, string relationshipCode)
        {
            this.Id = uniquePrefix + "SubjectVictimAssocSP" + id;
            this.SubjectRef = subject.Reference;
            this.VictimRef = victim.Reference;
            this.RelationshipCode = TranslateRelationship(victim,relationshipCode);
        }

        public SubjectVictimAssociation(String uniquePrefix, String id, string subjectRef, string victimRef, string relationshipCode)
        {
            this.Id = "SubjectVictimAssocSP" + id;
            this.SubjectRef = new Subject.Subject(subjectRef);
            this.VictimRef = new Victim.Victim(victimRef);
            this.RelationshipCode = relationshipCode;
        }

        private string TranslateRelationship(Victim.Victim victim,string relationship)
        {
            //Derived relationship
            String derivedVicOffRelationship = relationship;

            //If boyfriend or girlfriend related, add gender for specific translation
            if (relationship.MatchOne("XB", "BG"))
            {
                //Append the sex for dictionary lookup.
                derivedVicOffRelationship = derivedVicOffRelationship + victim.Person.SexCode;
            }

            return VictimOffenderRelationshipLibrsNibrsTranslation[derivedVicOffRelationship];
        }

        private static Dictionary<String, String> VictimOffenderRelationshipLibrsNibrsTranslation = new Dictionary
            <string, string>()
            {
                {"SE", "Family Member_Spouse"},
                {
                    "CS",
                    "Family Member_Spouse_Common Law"
                },

                {
                    "PA",
                    "Family Member_Parent"
                },
                {
                    "SB",
                    "Family Member_Sibling"
                },
                {
                    "CH",
                    "Family Member_Child"
                },
                {
                    "GP",
                    "Family Member_Grandparent"
                },
                {
                    "GC",
                    "Family Member_Grandchild"
                },
                {
                    "IL",
                    "Family Member_In-Law"
                },
                {
                    "SP",
                    "Family Member_Stepparent"
                },
                {
                    "SC",
                    "Family Member_Stepchild"
                },
                {
                    "SS",
                    "Family Member_Stepsibling"
                },
                {
                    "OF",
                    "Family Member"
                },
                {
                    "NM",
                    "Family Member_Spouse_Common Law"
                },
                {
                    "VO",
                    "Victim Was Offender"
                },
                {
                    "AQ",
                    "Acquaintance"
                },
                {
                    "FR",
                    "Friend"
                },
                {
                    "NE",
                    "Neighbor"
                },
                {
                    "BE",
                    "Babysittee"
                },
                {
                    "BGM",
                    "Boyfriend"
                },
                {
                    "BGF",
                    "Girlfriend"
                },
                {
                    "XBM",
                    "Boyfriend"
                },
                {
                    "XBF",
                    "Girlfriend"
                },
                {
                    "CF",
                    "Child of Boyfriend_Girlfriend"
                },
                {
                    "HR",
                    "Homosexual relationship"
                },
                {
                    "XS",
                    "Ex_Spouse"
                },
                {
                    "EE",
                    "Employee"
                },
                {
                    "ER",
                    "Employer"
                },
                {
                    "OK",
                    "NonFamily_Otherwise Known"
                },
                {
                    "ES",
                    "Family Member_Spouse"
                },
                {
                    "RU",
                    "Relationship Unknown"
                },
                {
                    "ST",
                    "Stranger"
                }
            };
    }
}
