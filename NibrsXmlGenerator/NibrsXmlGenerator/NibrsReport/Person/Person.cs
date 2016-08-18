using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Constants;
using System.Xml.Serialization;
using NibrsXml.Utility;

namespace NibrsXml.NibrsReport.Person
{
    [XmlRoot("Person", Namespace = Namespaces.niemCore)]
    public class Person
    {
        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string Id { get; set; }

        [XmlElement("PersonAgeMeasure", Namespace = Namespaces.niemCore, Order = 1)]
        public PersonAgeMeasure AgeMeasure { get; set; }

        [XmlElement("PersonEthnicityCode", Namespace = Namespaces.justice, Order = 2)]
        public string EthnicityCode { get; set; }

        [XmlElement("PersonInjury", Namespace = Namespaces.niemCore, Order = 3)]
        public PersonInjury Injury { get; set; }

        [XmlElement("PersonRaceNDExCode", Namespace = Namespaces.justice, Order = 4)]
        public string RaceCode { get; set; }

        [XmlElement("PersonResidentCode", Namespace = Namespaces.justice, Order = 5)]
        public string ResidentCode { get; set; }

        [XmlElement("PersonSexCode", Namespace = Namespaces.justice, Order = 6)]
        public string SexCode { get; set; }

        [XmlElement("PersonAugmentation", Namespace = Namespaces.justice, Order = 7)]
        public PersonAugmentation Augmentation { get; set; }

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
            this.Id = id;
            this.AgeMeasure = ageMeasure;
            this.EthnicityCode = ethnicityCode == null ? null : ethnicityCode.TrimNullIfEmpty();
            this.Injury = injury;
            this.RaceCode = raceCode == null ? null : raceCode.TrimNullIfEmpty();
            this.ResidentCode = residentCode == null ? null : residentCode.TrimNullIfEmpty();
            this.SexCode = sexCode == null ? null : sexCode.TrimNullIfEmpty();
            this.Augmentation = augmentation;
        }
    }
}
