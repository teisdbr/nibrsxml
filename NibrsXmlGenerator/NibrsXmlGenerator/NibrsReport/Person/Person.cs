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
    public class Person
    {
        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string id { get; set; }

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

        /// <summary>
        /// Creates a Person object without an id
        /// </summary>
        /// <param name="ageMeasure"></param>
        /// <param name="ethnicityCode"></param>
        /// <param name="injury"></param>
        /// <param name="raceCode"></param>
        /// <param name="residentCode"></param>
        /// <param name="sexCode"></param>
        /// <param name="augmentation"></param>
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
