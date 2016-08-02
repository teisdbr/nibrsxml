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
        public string offenseId { get; set; }

        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string offenseRef { get; set; }

        [XmlElement("OffenseUCRCode", Namespace = Namespaces.cjisNibrs, Order = 1)]
        public string offenseUcrCode { get; set; }

        [XmlElement("CriminalActivityCategoryCode", Namespace = Namespaces.cjisNibrs, Order = 2)]
        public string criminalActivityCategoryCode { get; set; }

        [XmlElement("OffenseFactorBiasMotivationCode", Namespace = Namespaces.justice, Order = 3)]
        public string offenseFactorBiasMotivationCode { get; set; }

        [XmlElement("OffenseStructuresEnteredQuantity", Namespace = Namespaces.justice, Order = 4)]
        public string offenseStructuresEnteredQuantity { get; set; }

        [XmlElement("OffenseFactor", Namespace = Namespaces.justice, Order = 5)]
        public OffenseFactor offenseFactor { get; set; }
        
        [XmlElement("OffenseEntryPoint", Namespace = Namespaces.justice, Order = 6)]
        public OffenseEntryPoint offenseEntryPoint { get; set; }
        
        [XmlElement("OffenseForce", Namespace = Namespaces.justice, Order = 7)]
        public OffenseForce offenseForce { get; set; }

        [XmlElement("OffenseAttemptedIndicator", Namespace = Namespaces.justice, Order = 8)]
        public string offenseAttemptedIndicator { get; set; }

        [XmlIgnore]
        public Offense reference { get { return new Offense(this.offenseId); } }

        public Offense() { }

        private Offense(string offenseId)
        {
            this.offenseRef = offenseId;
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
            this.offenseId = "Offense" + id.ToString();
            this.offenseUcrCode = offenceUcrCode;
            this.criminalActivityCategoryCode = criminalActivityCategoryCode;
            this.offenseFactorBiasMotivationCode = offenseFactorBiasMotivationCode;
            this.offenseStructuresEnteredQuantity = offenseStructuresEnteredQuantity.ToString();
            this.offenseFactor = offenseFactor;
            this.offenseEntryPoint = offenseEntryPoint;
            this.offenseForce = offenseForce;
            this.offenseAttemptedIndicator = offenseAttemptedIndicator.ToString().ToLower();
        }
    }
}
