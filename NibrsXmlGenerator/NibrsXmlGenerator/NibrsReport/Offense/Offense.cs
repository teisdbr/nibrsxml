﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Offense
{
    [XmlRoot("Offense", Namespace = Namespaces.justice)]
    public class Offense
    {
        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string Id { get; set; }

        /// <summary>
        /// This property is public only For serialization.
        /// It should only be set by using the Offense(string) constructor and accessed using the reference property.
        /// </summary>
        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string OffenseRef { get; set; }

        [XmlElement("OffenseUCRCode", Namespace = Namespaces.cjisNibrs, Order = 1)]
        public string UcrCode { get; set; }

        [XmlElement("CriminalActivityCategoryCode", Namespace = Namespaces.cjisNibrs, Order = 2)]
        public List<String> CriminalActivityCategoryCodes { get; set; }

        [XmlElement("OffenseFactorBiasMotivationCode", Namespace = Namespaces.justice, Order = 3)]
        public List<String> FactorBiasMotivationCodes { get; set; }

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

        [XmlIgnore]
        public Offense Reference { get { return new Offense(this.Id); } }

        [XmlIgnore]
        public Location.Location Location { get; set; }

        [XmlIgnore]
        public String librsVictimSequenceNumber { get; set; }

        public Offense() { }

        public Offense(string offenseId)
        {
            this.OffenseRef = offenseId;
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
            this.Id = "Offense" + id;
            this.UcrCode = offenseUcrCode;
            this.CriminalActivityCategoryCodes = criminalActivityCategoryCodes;
            this.FactorBiasMotivationCodes = offenseFactorBiasMotivationCodes;
            this.StructuresEnteredQuantity = offenseStructuresEnteredQuantity.ToString();
            this.Factors = offenseFactors;
            this.EntryPoint = offenseEntryPoint;
            this.Forces = offenseForces;
            this.AttemptedIndicator = offenseAttemptedIndicator.ToString().ToLower();
        }
    }
}
