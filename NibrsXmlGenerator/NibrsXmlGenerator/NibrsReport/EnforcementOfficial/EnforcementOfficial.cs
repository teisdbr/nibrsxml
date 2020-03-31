using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;
using TeUtil.Extensions;

namespace NibrsXml.NibrsReport.EnforcementOfficial
{
    [XmlRoot("EnforcementOfficial", Namespace = Namespaces.justice)]
    public class EnforcementOfficial
    {
        public EnforcementOfficial()
        {
        }

        public EnforcementOfficial(
            Person.Person person,
            string victimSeqNum,
            string activityCategoryCode,
            string assignmentCategoryCode,
            string agencyOri)
        {
            Person = person;
            VictimSeqNum = victimSeqNum;
            Role = new RoleOfPerson(Person.Id);
            ActivityCategoryCode = activityCategoryCode;
            AssignmentCategoryCode = assignmentCategoryCode;
            if (agencyOri != null && agencyOri.HasValue(true))
                Unit = new EnforcementOfficialUnit(
                    new OrganizationAugmentation(new OrganizationORIIdentification(agencyOri)));
        }

        [XmlIgnore] public Person.Person Person { get; set; }

        [XmlIgnore] public string VictimSeqNum { get; set; }

        [XmlElement("RoleOfPerson", Namespace = Namespaces.niemCore, Order = 1)]
        public RoleOfPerson Role { get; set; }

        [XmlElement("EnforcementOfficialActivityCategoryCode", Namespace = Namespaces.justice, Order = 2)]
        public string ActivityCategoryCode { get; set; }

        [XmlElement("EnforcementOfficialAssignmentCategoryCode", Namespace = Namespaces.justice, Order = 3)]
        public string AssignmentCategoryCode { get; set; }

        [XmlElement("EnforcementOfficialUnit", Namespace = Namespaces.justice, Order = 4)]
        public EnforcementOfficialUnit Unit { get; set; }
    }
}