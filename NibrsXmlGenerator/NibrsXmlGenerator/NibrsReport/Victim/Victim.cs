using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;
using LoadBusinessLayer.LIBRSVictim;
using NibrsXml.Utility;

namespace NibrsXml.NibrsReport.Victim
{
    [XmlRoot("Victim", Namespace = Namespaces.justice)]
    public class Victim
    {
        [XmlIgnore]
        public Person.Person Person { get; set; }

        /// <summary>
        /// This property is public only For serialization.
        /// It should only be set by using the Victim(string) constructor and accessed using the reference property.
        /// </summary>
        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string VictimRef { get; set; }

        [XmlElement("RoleOfPerson", Namespace = Namespaces.niemCore, Order = 1)]
        public RoleOfPerson Role { get; set; }

        [XmlElement("VictimSequenceNumberText", Namespace = Namespaces.justice, Order = 2)]
        public string SeqNum { get; set; }

        [XmlElement("VictimInjury", Namespace = Namespaces.justice, Order = 3)]
        public List<VictimInjury> VictimInjuries { get; set; }

        [XmlElement("VictimCategoryCode", Namespace = Namespaces.justice, Order = 4)]
        public string CategoryCode { get; set; }

        [XmlElement("VictimAggravatedAssaultHomicideFactorCode", Namespace = Namespaces.justice, Order = 5)]
        public List<string> AggravatedAssaultHomicideFactorCodes { get; set; }

        [XmlElement("VictimJustifiableHomicideFactorCode", Namespace = Namespaces.justice, Order = 6)]
        public string JustifiableHomicideFactorCode { get; set; }

        [XmlIgnore]
        public List<LIBRSVictimOffenderRelation> RelatedOffenders { get; set; }

        public Victim Reference => new Victim(this.Person.Id);
        public Victim() {
            this.RelatedOffenders = new List<LIBRSVictimOffenderRelation>();
        }

        public Victim(string victimId) : this()
        {
            this.VictimRef = victimId;
        }

        public Victim(
            Person.Person person,
            string seqNum,
            List<VictimInjury> injuries,
            string categoryCode,
            List<string> aggravatedAssaultHomicideFactorCodes,
            string justifiableHomicideFactorCode) : this()
        {
            //Initialize required properties
            if (person != null)
            {
                this.Person = person;
                this.Person.Id += "PersonVictim" + seqNum.TrimStart('0');
                this.Role = new RoleOfPerson(this.Person.Id);
            }
            this.SeqNum = seqNum.TrimStart('0').ToString();
            this.VictimInjuries = injuries ?? new List<VictimInjury>();
            this.CategoryCode = categoryCode;
            this.AggravatedAssaultHomicideFactorCodes = aggravatedAssaultHomicideFactorCodes;
            this.JustifiableHomicideFactorCode = justifiableHomicideFactorCode;
        }

        public Victim(
            EnforcementOfficial.EnforcementOfficial officer,
            List<VictimInjury> injuries,
            List<string> aggravatedAssaultHomicideFactorCode,
            string justifiableHomicideFactorCode) : this()
        {
            this.Person = officer.Person;
            this.Role = officer.Role;
            this.SeqNum = officer.VictimSeqNum.TrimStart('0').ToString();
            this.VictimInjuries = injuries ?? new List<VictimInjury>();
            this.CategoryCode = VictimCategoryCode.LAW_ENFORCEMENT_OFFICER.NibrsCode();
            //Translate 40 to 09 if applicable.
            this.AggravatedAssaultHomicideFactorCodes = aggravatedAssaultHomicideFactorCode.Select(a => a == "40" ? "09" : a).ToList();
            this.JustifiableHomicideFactorCode = justifiableHomicideFactorCode;
        }
    }
}
