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

        [XmlElement("OffenseUCRCode", Namespace = Namespaces.cjisNibrs)]
        public string offenseUcrCode { get; set; }

        [XmlElement("CriminalActivityCategoryCode", Namespace = Namespaces.cjisNibrs)]
        public string criminalActivityCategoryCode { get; set; }

        [XmlElement("OffenseFactorBiasMotivationCode", Namespace = Namespaces.justice)]
        public string offenseFactorBiasMotivationCode { get; set; }

        [XmlElement("OffenseStructuresEnteredQuantity", Namespace = Namespaces.justice)]
        public string offenseStructuresEnteredQuantity { get; set; }

        public OffenseFactor offenseFactor { get; set; }
        public OffenseEntryPoint offenseEntryPoint { get; set; }
        public OffenseForce offenseForce { get; set; }

        [XmlElement("OffenseAttemptedIndicator", Namespace = Namespaces.justice)]
        public string offenseAttemptedIndicator { get; set; }

        public Offense() { }

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
            this.offenseId = id.ToString();
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
