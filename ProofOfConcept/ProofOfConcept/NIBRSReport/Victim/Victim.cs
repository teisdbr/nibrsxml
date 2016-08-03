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

        /// <summary>
        /// This property is public only for serialization.
        /// It should only be set by using the Victim(string) constructor and accessed using the reference property.
        /// </summary>
        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string victimRef { get; set; }

        [XmlElement("RoleOfPerson", Namespace = Namespaces.niemCore, Order = 1)]
        public RoleOfPerson role { get; set; }

        [XmlElement("VictimSequenceNumberText", Namespace = Namespaces.justice, Order = 2)]
        public int seqNum { get; set; }

        [XmlElement("VictimCategoryCode", Namespace = Namespaces.justice, Order = 3)]
        public string categoryCode { get; set; }

        [XmlElement("VictimAggravatedAssaultHomicideFactorCode", Namespace = Namespaces.justice, Order = 4)]
        public string aggravatedAssaultHomicideFactorCode { get; set; }

        [XmlElement("VictimJustifiableHomicideFactorCode", Namespace = Namespaces.justice, Order = 5)]
        public string justifiableHomicideFactorCode { get; set; }

        public Victim reference { get { return new Victim(this.person.id); } }

        public Victim() { }

        public Victim(string victimId)
        {
            this.victimRef = victimId;
        }


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
            this.seqNum = seqNum;
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
