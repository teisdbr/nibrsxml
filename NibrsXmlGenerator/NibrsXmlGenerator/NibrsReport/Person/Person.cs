using System.Xml.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using NibrsXml.Constants;
using NibrsXml.Utility;

namespace NibrsXml.NibrsReport.Person
{
    [XmlRoot("Person", Namespace = Namespaces.niemCore)]
    public class Person
    {
        public Person()
        {
        }


        public Person(
            string id,
            PersonAgeMeasure ageMeasure,
            string ethnicityCode,
            string raceCode,
            string residentCode,
            string sexCode,
            PersonAugmentation augmentation)
        {
            Id = id;
            AgeMeasure = ageMeasure;
            EthnicityCode = ethnicityCode == null ? null : ethnicityCode.TrimNullIfEmpty();
            RaceCode = raceCode == null ? null : raceCode.TrimNullIfEmpty();
            ResidentCode = residentCode == null ? null : residentCode.TrimNullIfEmpty();
            SexCode = sexCode == null ? null : sexCode.TrimNullIfEmpty();
            Augmentation = augmentation;
        }

        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string Id { get; set; }

        [XmlElement("PersonAgeMeasure", Namespace = Namespaces.niemCore, Order = 1)]
        public PersonAgeMeasure AgeMeasure { get; set; }

        [XmlElement("PersonEthnicityCode", Namespace = Namespaces.justice, Order = 2)]
        public string EthnicityCode { get; set; }

        [XmlElement("PersonRaceNDExCode", Namespace = Namespaces.justice, Order = 4)]
        public string RaceCode { get; set; }

        [XmlElement("PersonResidentCode", Namespace = Namespaces.justice, Order = 5)]
        public string ResidentCode { get; set; }

        [XmlElement("PersonSexCode", Namespace = Namespaces.justice, Order = 6)]
        public string SexCode { get; set; }

        [XmlElement("PersonAugmentation", Namespace = Namespaces.justice, Order = 7)]
        public PersonAugmentation Augmentation { get; set; }

        [BsonIgnore]
        [XmlIgnore]
        [JsonIgnore]
        public bool AgeIsUnknown
        {
            get { return Augmentation != null && Augmentation.AgeCode == PersonAgeCode.UNKNOWN.NibrsCode(); }
        }

        [BsonIgnore]
        [XmlIgnore]
        [JsonIgnore]
        public bool IsJuvenile
        {
            get { return AgeMeasure != null && AgeMeasure.IsJuvenile; }
        }
    }
}