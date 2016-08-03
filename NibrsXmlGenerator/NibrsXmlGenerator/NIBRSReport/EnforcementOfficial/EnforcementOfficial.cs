using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.NibrsReport.Person;

namespace NibrsXml.NibrsReport.EnforcementOfficial
{
    [XmlRoot("EnforcementOfficial", Namespace = Namespaces.justice)]
    public class EnforcementOfficial
    {
        [XmlIgnore]
        public Person.Person person { get; set; }

        [XmlIgnore]
        public int victimSeqNum { get; set; }
        
        [XmlElement("RoleOfPerson", Namespace = Namespaces.niemCore, Order = 1)]
        public RoleOfPerson role { get; set; }

        [XmlElement("EnforcementOfficialActivityCategoryCode", Namespace = Namespaces.justice, Order = 2)]
        public string activityCategoryCode { get; set; }

        [XmlElement("EnforcementOfficialAssignmentCategoryCode", Namespace = Namespaces.justice, Order = 3)]
        public string assignmentCategoryCode { get; set; }

        [XmlElement("EnforcementOfficialUnit", Namespace = Namespaces.justice, Order = 4)]
        public EnforcementOfficialUnit unit { get; set; }

        public EnforcementOfficial() { }

        public EnforcementOfficial(
            Person.Person person,
            int victimSeqNum,
            string activityCategoryCode,
            string assignmentCategoryCode,
            EnforcementOfficialUnit unit)
        {
            this.person = person;
            this.person.id = "PersonVictim" + victimSeqNum.ToString();
            this.victimSeqNum = victimSeqNum;
            this.role = new RoleOfPerson(this.person.id);
            this.activityCategoryCode = activityCategoryCode;
            this.assignmentCategoryCode = assignmentCategoryCode;
            this.unit = unit;
        }
    }
}
