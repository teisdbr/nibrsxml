using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace NibrsXml.Ucr.DataCollections
{
    public class Arson : GeneralSummaryData
    {
        public override string XmlRootName
        {
            get { return "ArsonSummary"; }
        }

        public override string XslFileName
        {
            get { return "arson.xsl"; }
        }

        public override XElement MappingDictionary
        {
            get
            {
                return new XElement("UcrCodeDictionary",
                    new XElement("UCRDescription", new XAttribute("value", "A"), "A. Commercial Sex Acts"),
                    new XElement("UCRDescription", new XAttribute("value", "B"), "B. Involuntary Servitude"));
            }
        }

        #region Constants

        public const String ArsonUcrCode = "200";

        private const string TotalStructure = "Total Structure";
        private const string TotalMobile = "Total Mobile";

        private static readonly Dictionary<string, string> ClassificationToSubtotalDictionary = new Dictionary<string, string>
        {
            {"A", TotalStructure},
            {"B", TotalStructure},
            {"C", TotalStructure},
            {"D", TotalStructure},
            {"E", TotalStructure},
            {"F", TotalStructure},
            {"G", TotalStructure},
            {"H", TotalMobile},
            {"I", TotalMobile}
        };

        #endregion

        public override void IncrementActualOffense(String key, int byValue = 1)
        {
            base.IncrementActualOffense(key, byValue);

            if (ClassificationToSubtotalDictionary.ContainsKey(key))
                ClassificationCounts[key].IncrementActualOffense(byValue);
        }

        public override void IncrementAllClearences(String key, int byValue = 1)
        {
            base.IncrementAllClearences(key, byValue);

            if (ClassificationToSubtotalDictionary.ContainsKey(key))
                ClassificationCounts[key].IncrementAllClearences(byValue);
        }

        public override void IncrementJuvenileClearences(String key, int byValue = 1)
        {
            base.IncrementJuvenileClearences(key, byValue);

            if (ClassificationToSubtotalDictionary.ContainsKey(key))
                ClassificationCounts[key].IncrementJuvenileClearences(byValue);
        }
    }
}