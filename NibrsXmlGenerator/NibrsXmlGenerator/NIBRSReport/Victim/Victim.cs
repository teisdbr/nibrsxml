using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.NibrsReport.Person;
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

        [XmlElement("VictimCategoryCode", Namespace = Namespaces.justice, Order = 3)]
        public string CategoryCode { get; set; }

        [XmlElement("VictimAggravatedAssaultHomicideFactorCode", Namespace = Namespaces.justice, Order = 4)]
        public List<String> AggravatedAssaultHomicideFactorCode { get; set; }

        [XmlElement("VictimJustifiableHomicideFactorCode", Namespace = Namespaces.justice, Order = 5)]
        public string JustifiableHomicideFactorCode { get; set; }

        [XmlIgnore]
        public List<LIBRSVictimOffenderRelation> RelatedOffenders { get; set; }

        public Victim Reference { get { return new Victim(this.Person.Id); } }

        public Victim() { }

        public Victim(string victimId)
        {
            this.VictimRef = victimId;
        }


        public Victim(
            Person.Person person,
            String seqNum,
            string categoryCode,
            List<String> aggravatedAssaultHomicideFactorCode,
            string justifiableHomicideFactorCode)
        {
            //Initialize required properties
            if (person != null)
            {
                this.Person = person;
                this.Person.Id += "PersonVictim" + seqNum;
                this.Role = new RoleOfPerson(this.Person.Id);
            }
            this.SeqNum = seqNum.ToString();
            this.CategoryCode = categoryCode;
            this.AggravatedAssaultHomicideFactorCode = aggravatedAssaultHomicideFactorCode;
            this.JustifiableHomicideFactorCode = justifiableHomicideFactorCode;
        }

        public Victim(
            EnforcementOfficial.EnforcementOfficial officer,
            List<String> aggravatedAssaultHomicideFactorCode,
            string justifiableHomicideFactorCode)
        {
            this.Person = officer.Person;
            this.Role = officer.Role;
            this.SeqNum = officer.VictimSeqNum.ToString();
            this.CategoryCode = VictimCategoryCode.LAW_ENFORCEMENT_OFFICER.NibrsCode();
            this.AggravatedAssaultHomicideFactorCode = aggravatedAssaultHomicideFactorCode;
            this.JustifiableHomicideFactorCode = justifiableHomicideFactorCode;
        }
    }
}
