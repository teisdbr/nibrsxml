using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.NibrsReport.Person;

namespace NibrsXml.NibrsReport.Victim
{
    [XmlRoot("Victim", Namespace = Namespaces.justice)]
    public class Victim
    {
        [XmlIgnore]
        public Person.Person person { get; set; }
        
        [XmlElement("RoleOfPerson", Namespace = Namespaces.niemCore, Order = 1)]
        public RoleOfPerson role { get; set; }

        [XmlElement("VictimSequenceNumberText", Namespace = Namespaces.justice, Order = 2)]
        public string seqNum { get; set; }

        [XmlElement("VictimCategoryCode", Namespace = Namespaces.justice, Order = 3)]
        public string categoryCode { get; set; }

        [XmlElement("VictimAggravatedAssaultHomicideFactorCode", Namespace = Namespaces.justice, Order = 4)]
        public string aggravatedAssaultHomicideFactorCode { get; set; }

        [XmlElement("VictimJustifiableHomicideFactorCode", Namespace = Namespaces.justice, Order = 5)]
        public string justifiableHomicideFactorCode { get; set; }

        public Victim() { }

        public Victim(
            Person.Person person,
            int seqNum,
            string categoryCode,
            string aggravatedAssaultHomicideFactorCode,
            string justifiableHomicideFactorCode)
        {
            this.person = person;
            this.person.id = "PersonVictim" + seqNum.ToString();
            this.role = new RoleOfPerson(this.person.id);
            this.seqNum = seqNum.ToString();
            this.categoryCode = categoryCode;
            this.aggravatedAssaultHomicideFactorCode = aggravatedAssaultHomicideFactorCode;
            this.justifiableHomicideFactorCode = justifiableHomicideFactorCode;
        }

        public Victim(
            EnforcementOfficial.EnforcementOfficial officer,
            string categoryCode,
            string aggravatedAssaultHomicideFactorCode,
            string justifiableHomicideFactorCode)
        {
            this.person = officer.person;
            this.role = officer.role;
            this.seqNum = officer.victimSeqNum;
            this.categoryCode = categoryCode;
            this.aggravatedAssaultHomicideFactorCode = aggravatedAssaultHomicideFactorCode;
            this.justifiableHomicideFactorCode = justifiableHomicideFactorCode;
        }
    }
}
