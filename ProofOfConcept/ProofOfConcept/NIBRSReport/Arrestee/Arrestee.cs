using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.NibrsReport.Person;

namespace NibrsXml.NibrsReport.Arrestee
{
    [XmlRoot("Arrestee", Namespace = Namespaces.justice)]
    public class Arrestee //: Person.Person
    {
        //[XmlIgnore]
        //public override string personId { get { return base.personId; } set { base.personId = value; } }

        //[XmlIgnore]
        //public override PersonAgeMeasure ageMeasure { get { return base.ageMeasure; } set { base.ageMeasure = value; } }

        //[XmlIgnore]
        //public override string ethnicityCode { get { return base.ethnicityCode; } set { base.ethnicityCode = value; } }

        //[XmlIgnore]
        //public override PersonInjury injury { get { return base.injury; } set { base.injury = value; } }

        //[XmlIgnore]
        //public override string raceCode { get { return base.raceCode; } set { base.raceCode = value; } }

        //[XmlIgnore]
        //public override string residentCode { get { return base.residentCode; } set { base.residentCode = value; } }

        //[XmlIgnore]
        //public override string sexCode { get { return base.sexCode; } set { base.sexCode = value; } }

        //[XmlIgnore]
        //public override PersonAugmentation augmentation { get { return base.augmentation; } set { base.augmentation = value; } }
        
        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        private string id { get; set; }
        
        [XmlElement("RoleOfPerson", Namespace = Namespaces.niemCore, Order = 1)]
        public RoleOfPerson role { get; set; }
        
        [XmlElement("ArrestSequenceID", Namespace = Namespaces.justice, Order = 2)]
        public string seqId { get; set; }

        [XmlElement("ArresteeClearanceIndicator", Namespace = Namespaces.justice, Order = 3)]
        public string clearanceIndicator { get; set; }

        [XmlElement("ArresteeArmedWithCode", Namespace = Namespaces.justice, Order = 4)]
        public string armedWithCode { get; set; }

        [XmlElement("ArresteeJuvenileDispositionCode", Namespace = Namespaces.justice, Order = 5)]
        public string juvenileDispositionCode { get; set; }

        public Arrestee() { }

        public Arrestee(
            PersonAgeMeasure ageMeasure,
            string ethnicityCode,
            PersonInjury injury,
            string raceCode,
            string residentCode,
            string sexCode,
            PersonAugmentation augmentation,
            string seqId,
            string clearanceIndicator,
            string armedWithCode,
            string juvenileDispositionCode)
        {
            //this.personId = "PersonArrestee" + seqId.ToString();
            //this.ageMeasure = ageMeasure;
            //this.ethnicityCode = ethnicityCode;
            //this.injury = injury;
            //this.raceCode = raceCode;
            //this.residentCode = residentCode;
            //this.augmentation = augmentation;
            this.id = "Arrestee" + seqId.ToString();
            //this.role = new RoleOfPerson(personId);
            this.seqId = seqId;
            this.clearanceIndicator = clearanceIndicator;
            this.armedWithCode = armedWithCode;
            this.juvenileDispositionCode = juvenileDispositionCode;
        }
    }
}
