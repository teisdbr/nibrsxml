using System;
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
        public string OffenseFactorBiasMotivationCode { get; set; }

        [XmlElement("OffenseStructuresEnteredQuantity", Namespace = Namespaces.justice, Order = 4)]
        public string OffenseStructuresEnteredQuantity { get; set; }

        [XmlElement("OffenseFactor", Namespace = Namespaces.justice, Order = 5)]
        public OffenseFactor OffenseFactor { get; set; }

        [XmlElement("OffenseEntryPoint", Namespace = Namespaces.justice, Order = 6)]
        public OffenseEntryPoint OffenseEntryPoint { get; set; }

        [XmlElement("OffenseForce", Namespace = Namespaces.justice, Order = 7)]
        public List<OffenseForce> OffenseForces { get; set; }

        [XmlElement("OffenseAttemptedIndicator", Namespace = Namespaces.justice, Order = 8)]
        public string OffenseAttemptedIndicator { get; set; }

        [XmlIgnore]
        public Offense Reference { get { return new Offense(this.Id); } }

        public Offense() { }

        public Offense(string offenseId)
        {
            this.OffenseRef = offenseId;
        }

        public Offense(
            int id,
            string offenceUcrCode,
            string criminalActivityCategoryCode,
            string offenseFactorBiasMotivationCode,
            int offenseStructuresEnteredQuantity,
            OffenseFactor offenseFactor,
            OffenseEntryPoint offenseEntryPoint,
            OffenseForce offenseForce,
            bool offenseAttemptedIndicator)
        {
            this.OffenseForces = new List<OffenseForce>();
            this.CriminalActivityCategoryCodes = new List<String>();

            this.Id = "Offense" + id.ToString();
            this.UcrCode = offenceUcrCode;
            this.CriminalActivityCategoryCodes.Add(criminalActivityCategoryCode);
            this.OffenseFactorBiasMotivationCode = offenseFactorBiasMotivationCode;
            this.OffenseStructuresEnteredQuantity = offenseStructuresEnteredQuantity.ToString();
            this.OffenseFactor = offenseFactor;
            this.OffenseEntryPoint = offenseEntryPoint;
            this.OffenseForces.Add(offenseForce);
            this.OffenseAttemptedIndicator = offenseAttemptedIndicator.ToString().ToLower();
        }
    }
}
