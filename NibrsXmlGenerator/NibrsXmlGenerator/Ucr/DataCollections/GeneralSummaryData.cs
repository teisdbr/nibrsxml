using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LoadBusinessLayer.Rs;

namespace NibrsXml.Ucr.DataCollections
{
    public abstract class GeneralSummaryData
    {
        #region Configuration
        public abstract String XmlRootName { get; }
        public abstract String XslFileName { get; }
        #endregion

        #region Properties
        public Dictionary<String, Counts> ClassificationCounts { get; set; }
        #endregion

        public XDocument Serialize()
        {
            return new XDocument(
                    new XProcessingInstruction(
                        "xml-stylesheet",
                        "type=\"text/xsl\" href=\"" + XslFileName + "\""),
                        new XElement(XmlRootName,
                            this.ClassificationCounts.Select(classif => new XElement("Classification",
                                                                                new XAttribute("name", classif.Key),
                                                                                new XElement("Actual", classif.Value.ActualOffenses),
                                                                                new XElement("ClearedByArrest", classif.Value.ClearedByArrestOrExcepMeans),
                                                                                new XElement("ClearedByJuvArrest", classif.Value.ClearencesInvolvingJuveniles)))));
        }
    }

    #region Public Structs
    public struct Counts
    {
        public int? ActualOffenses { get; private set; }
        public int? ClearedByArrestOrExcepMeans { get; private set; }
        public int? ClearencesInvolvingJuveniles { get; private set; }

        public void IncrementActualOffense(int byValue = 1)
        {
            //Verify not null before adding
            if (this.ActualOffenses.HasValue == false) { this.ActualOffenses = 0; }

            this.ActualOffenses += byValue;
        }

        public void IncrementAllClearences(int byValue = 1)
        {
            //Verify not null before adding
            if (this.ActualOffenses.HasValue == false) { this.ActualOffenses = 0; }

            this.ClearedByArrestOrExcepMeans += byValue;
        }

        public void IncrementJuvenileClearences(int byValue = 1)
        {
            //Verify not null before adding
            if (this.ActualOffenses.HasValue == false) { this.ActualOffenses = 0; }

            this.ClearencesInvolvingJuveniles += byValue;
        }
    }
    #endregion
}
