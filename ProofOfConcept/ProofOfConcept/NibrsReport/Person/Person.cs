using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Person
{
    [XmlRoot("Person", Namespace = Namespaces.niemCore)]
    public abstract class Person
    {
        [XmlElement("PersonAgeMeasure", Namespace = Namespaces.niemCore, Order = 1)]
        public PersonAgeMeasure ageMeasure { get; set; }

        [XmlElement("PersonEthnicityCode", Namespace = Namespaces.justice, Order = 2)]
        public string ethnicityCode { get; set; }

        [XmlElement("PersonInjury", Namespace = Namespaces.niemCore, Order = 3)]
        public PersonInjury injury { get; set; }

        [XmlElement("PersonRaceNDExCode", Namespace = Namespaces.justice, Order = 4)]
        public string raceCode { get; set; }

        [XmlElement("PersonResidentCode", Namespace = Namespaces.justice, Order = 5)]
        public string residentCode { get; set; }

        [XmlElement("PersonSexCode", Namespace = Namespaces.justice, Order = 6)]
        public string sexCode { get; set; }

        [XmlElement("PersonAugmentation", Namespace = Namespaces.justice, Order = 7)]
        public PersonAugmentation augmentation { get; set; }

        public Person() { }

        public Person(
            PersonAgeMeasure ageMeasure,
            string ethnicityCode,
            PersonInjury injury,
            string raceCode,
            string residentCode,
            string sexCode,
            PersonAugmentation augmentation)
        {
            this.ageMeasure = ageMeasure;
            this.ethnicityCode = ethnicityCode;
            this.injury = injury;
            this.raceCode = raceCode;
            this.residentCode = residentCode;
            this.augmentation = augmentation;
        }
    }
}
