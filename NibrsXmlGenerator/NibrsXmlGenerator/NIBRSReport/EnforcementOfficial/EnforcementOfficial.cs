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
        public Person.Person Person { get; set; }

        [XmlIgnore]
        public String VictimSeqNum { get; set; }

        [XmlElement("RoleOfPerson", Namespace = Namespaces.niemCore, Order = 1)]
        public RoleOfPerson Role { get; set; }

        [XmlElement("EnforcementOfficialActivityCategoryCode", Namespace = Namespaces.justice, Order = 2)]
        public string ActivityCategoryCode { get; set; }

        [XmlElement("EnforcementOfficialAssignmentCategoryCode", Namespace = Namespaces.justice, Order = 3)]
        public string AssignmentCategoryCode { get; set; }

        [XmlElement("EnforcementOfficialUnit", Namespace = Namespaces.justice, Order = 4)]
        public EnforcementOfficialUnit Unit { get; set; }

        public EnforcementOfficial() { }

        public EnforcementOfficial(
            Person.Person person,
            String victimSeqNum,
            string activityCategoryCode,
            string assignmentCategoryCode,
            String agencyOri)
        {
            this.Person = person;
            this.Person.Id += "PersonVictim" + victimSeqNum.ToString();
            this.VictimSeqNum = victimSeqNum;
            this.Role = new RoleOfPerson(this.Person.Id);
            this.ActivityCategoryCode = activityCategoryCode;
            this.AssignmentCategoryCode = assignmentCategoryCode;
            this.Unit = new EnforcementOfficialUnit(new OrganizationAugmentation(new OrganizationORIIdentification(agencyOri)));
        }
    }
}
