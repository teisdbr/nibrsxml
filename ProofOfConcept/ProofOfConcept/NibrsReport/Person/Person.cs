using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Person
{
    [XmlInclude(typeof(EnforcementOfficial.EnforcementOfficial))]
    [XmlInclude(typeof(Victim.Victim))]
    [XmlInclude(typeof(Subject.Subject))]
    [XmlInclude(typeof(Arrestee.Arrestee))]
    [XmlRoot("Person", Namespace = Namespaces.niemCore)]
    public class Person
    {
        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public virtual string id { get; set; }

        [XmlElement("PersonAgeMeasure", Namespace = Namespaces.niemCore, Order = 1)]
        public virtual PersonAgeMeasure ageMeasure { get; set; }

        [XmlElement("PersonEthnicityCode", Namespace = Namespaces.justice, Order = 2)]
        public virtual string ethnicityCode { get; set; }

        [XmlElement("PersonInjury", Namespace = Namespaces.niemCore, Order = 3)]
        public virtual PersonInjury injury { get; set; }

        [XmlElement("PersonRaceNDExCode", Namespace = Namespaces.justice, Order = 4)]
        public virtual string raceCode { get; set; }

        [XmlElement("PersonResidentCode", Namespace = Namespaces.justice, Order = 5)]
        public virtual string residentCode { get; set; }

        [XmlElement("PersonSexCode", Namespace = Namespaces.justice, Order = 6)]
        public virtual string sexCode { get; set; }

        [XmlElement("PersonAugmentation", Namespace = Namespaces.justice, Order = 7)]
        public virtual PersonAugmentation augmentation { get; set; }

        public Person() { }

        public Person(
            PersonAgeMeasure ageMeasure,
            string ethnicityCode,
            PersonInjury injury,
            string raceCode,
            string residentCode,
            string sexCode,
            PersonAugmentation augmentation)
            : this(
                "",
                ageMeasure,
                ethnicityCode,
                injury,
                raceCode,
                residentCode,
                sexCode,
                augmentation) { }

        public Person(
            string id,
            PersonAgeMeasure ageMeasure,
            string ethnicityCode,
            PersonInjury injury,
            string raceCode,
            string residentCode,
            string sexCode,
            PersonAugmentation augmentation)
        {
            this.id = id;
            this.ageMeasure = ageMeasure;
            this.ethnicityCode = ethnicityCode;
            this.injury = injury;
            this.raceCode = raceCode;
            this.residentCode = residentCode;
            this.sexCode = sexCode;
            this.augmentation = augmentation;
        }
    }
}
