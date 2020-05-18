using System.Collections.Generic;
using System.Xml.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using NibrsXml.Constants;
using Newtonsoft.Json;
using TeUtil.Extensions;

namespace NibrsXml.NibrsReport.Offense
{
    [XmlRoot("Offense", Namespace = Namespaces.justice)]
    public class Offense
    {
        public Offense()
        {
        }

        public Offense(string offenseId)
        {
            OffenseRef = offenseId;
        }

        public Offense(
            string id,
            string offenseUcrCode,
            List<string> criminalActivityCategoryCodes,
            List<string> offenseFactorBiasMotivationCodes,
            string offenseStructuresEnteredQuantity,
            List<OffenseFactor> offenseFactors,
            OffenseEntryPoint offenseEntryPoint,
            List<OffenseForce> offenseForces,
            string offenseAttemptedIndicator)
        {
            Id = "Offense" + id;
            UcrCode = offenseUcrCode;
            CriminalActivityCategoryCodes = criminalActivityCategoryCodes;
            FactorBiasMotivationCodes = offenseFactorBiasMotivationCodes;
            StructuresEnteredQuantity = offenseStructuresEnteredQuantity;
            Factors = offenseFactors;
            EntryPoint = offenseEntryPoint;
            Forces = offenseForces;
            AttemptedIndicator = offenseAttemptedIndicator.ToLower();
        }

        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string Id { get; set; }

        /// <summary>
        ///     This property is public only For serialization.
        ///     It should only be set by using the Offense(string) constructor and accessed using the reference property.
        /// </summary>
        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string OffenseRef { get; set; }

        [XmlElement("OffenseUCRCode", Namespace = Namespaces.cjisNibrs, Order = 1)]
        public string UcrCode { get; set; }

    
        [XmlElement("CriminalActivityCategoryCode", Namespace = Namespaces.cjisNibrs, Order = 2)]
        public List<string> CriminalActivityCategoryCodes { get; set; }

        [XmlElement("OffenseFactorBiasMotivationCode", Namespace = Namespaces.justice, Order = 3)]
        public List<string> FactorBiasMotivationCodes { get; set; }

        [XmlElement("OffenseStructuresEnteredQuantity", Namespace = Namespaces.justice, Order = 4)]
        public string StructuresEnteredQuantity { get; set; }

        [XmlElement("OffenseFactor", Namespace = Namespaces.justice, Order = 5)]
        public List<OffenseFactor> Factors { get; set; }

        [XmlElement("OffenseEntryPoint", Namespace = Namespaces.justice, Order = 6)]
        public OffenseEntryPoint EntryPoint { get; set; }

        [XmlElement("OffenseForce", Namespace = Namespaces.justice, Order = 7)]
        public List<OffenseForce> Forces { get; set; }

        [XmlElement("OffenseAttemptedIndicator", Namespace = Namespaces.justice, Order = 8)]
        public string AttemptedIndicator { get; set; }

        [BsonIgnore]
        [XmlIgnore]
       [JsonIgnore]
        public Offense Reference
        {
            get { return new Offense(Id); }
        }

        [XmlIgnore] public Location.Location Location { get; set; }
    }
}