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

        protected override void ClassificationCountEntryInstantiations()
        {
            ClassificationCounts.Add("A", new GeneralSummaryCounts());
            ClassificationCounts.Add("B", new GeneralSummaryCounts());
            ClassificationCounts.Add("C", new GeneralSummaryCounts());
            ClassificationCounts.Add("D", new GeneralSummaryCounts());
            ClassificationCounts.Add("E", new GeneralSummaryCounts());
            ClassificationCounts.Add("F", new GeneralSummaryCounts());
            ClassificationCounts.Add("G", new GeneralSummaryCounts());
            ClassificationCounts.Add(TotalStructure, new GeneralSummaryCounts());
            ClassificationCounts.Add("H", new GeneralSummaryCounts());
            ClassificationCounts.Add("I", new GeneralSummaryCounts());
            ClassificationCounts.Add(TotalMobile, new GeneralSummaryCounts());
            ClassificationCounts.Add("J", new GeneralSummaryCounts());
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
                ClassificationCounts[ClassificationToSubtotalDictionary[key]].IncrementActualOffense(byValue);
        }

        public override void IncrementAllClearences(String key, int byValue = 1)
        {
            base.IncrementAllClearences(key, byValue);

            if (ClassificationToSubtotalDictionary.ContainsKey(key))
                ClassificationCounts[ClassificationToSubtotalDictionary[key]].IncrementAllClearences(byValue);
        }

        public override void IncrementJuvenileClearences(String key, int byValue = 1)
        {
            base.IncrementJuvenileClearences(key, byValue);

            if (ClassificationToSubtotalDictionary.ContainsKey(key))
                ClassificationCounts[ClassificationToSubtotalDictionary[key]].IncrementJuvenileClearences(byValue);
        }
    }
}