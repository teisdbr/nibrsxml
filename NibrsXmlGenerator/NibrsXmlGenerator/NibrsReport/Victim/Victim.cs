using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using LoadBusinessLayer.LIBRSVictim;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.Utility;

namespace NibrsXml.NibrsReport.Victim
{
    [XmlRoot("Victim", Namespace = Namespaces.justice)]
    public class Victim
    {
        public Victim()
        {
            RelatedOffenders = new List<LIBRSVictimOffenderRelation>();
        }

        public Victim(string victimId) : this()
        {
            VictimRef = victimId;
        }

        public Victim(
            Person.Person person,
            string seqNum,
            List<VictimInjury> injuries,
            string categoryCode,
            List<string> aggravatedAssaultHomicideFactorCodes,
            string justifiableHomicideFactorCode,
            string uniquePrefix) : this()

        {
            //Initialize required properties
            if (person != null)
            {
                Person = person;
                //this.Person.Id += "PersonVictim" + seqNum.TrimStart('0');
                Role = new RoleOfPerson(Person.Id);
            }

            Id = uniquePrefix + "Victim" + seqNum.TrimStart('0');
            SeqNum = seqNum.TrimStart('0');
            VictimInjuries = injuries ?? new List<VictimInjury>();
            CategoryCode = categoryCode;
            AggravatedAssaultHomicideFactorCodes = aggravatedAssaultHomicideFactorCodes;
            JustifiableHomicideFactorCode = justifiableHomicideFactorCode;
        }

        public Victim(
            EnforcementOfficial.EnforcementOfficial officer,
            List<VictimInjury> injuries,
            List<string> aggravatedAssaultHomicideFactorCode,
            string justifiableHomicideFactorCode,
            string uniquePrefix ) : this()
        {
            if (officer.Person != null)
            {
                Person = officer.Person;
                Role = new RoleOfPerson(Person.Id); 
            }
            Id = uniquePrefix + "Victim" + officer.VictimSeqNum.TrimStart('0');
            SeqNum = officer.VictimSeqNum.TrimStart('0').ToString();
            VictimInjuries = injuries ?? new List<VictimInjury>();
            CategoryCode = VictimCategoryCode.LAW_ENFORCEMENT_OFFICER.NibrsCode();
            // Translate 40 to 09 if applicable.
            this.AggravatedAssaultHomicideFactorCodes = aggravatedAssaultHomicideFactorCode;
            this.JustifiableHomicideFactorCode = justifiableHomicideFactorCode;
        }

        [XmlIgnore] public Person.Person Person { get; set; }

        /// <summary>
        ///     This property is public only For serialization.
        ///     It should only be set by using the Victim(string) constructor and accessed using the reference property.
        /// </summary>
        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string Id { get; set; }

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

        [XmlIgnore] public List<LIBRSVictimOffenderRelation> RelatedOffenders { get; set; }

        [BsonIgnore] [JsonIgnore]
        public Victim Reference
        {
            get { return new Victim(Id); }
        }
    }
}