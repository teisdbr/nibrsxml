using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LoadBusinessLayer.Rs;
using NibrsXml.Utility;

namespace NibrsXml.Ucr.DataCollections
{
    public abstract class GeneralSummaryData
    {
        #region Constants

        public const String GrandTotal = "Grand Total";

        #endregion

        #region Configuration

        public abstract String XmlRootName { get; }
        public abstract String XslFileName { get; }
        public abstract XElement MappingDictionary { get; }

        #endregion

        #region Properties

        public Dictionary<String, GeneralSummaryCounts> ClassificationCounts { get; set; }

        #endregion

        public GeneralSummaryData()
        {
            this.ClassificationCounts = new Dictionary<string, GeneralSummaryCounts>();

            //Define the basic shared "Grand Total" row of all reports.
            this.ClassificationCounts.Add(GrandTotal, new GeneralSummaryCounts());
        }

        public XDocument Serialize()
        {
            return new XDocument(
                new XProcessingInstruction(
                    "xml-stylesheet",
                    "type=\"text/xsl\" href=\"" + XslFileName + "\""),
                new XElement(XmlRootName,
                    MappingDictionary,
                    this.ClassificationCounts.Select(classif => new XElement("Classification",
                        new XAttribute("name", classif.Key),
                        classif.Value.ActualOffenses.HasValue
                            ? new XElement("Actual", classif.Value.ActualOffenses)
                            : null,
                        classif.Value.ClearedByArrestOrExcepMeans.HasValue
                            ? new XElement("ClearedByArrest", classif.Value.ClearedByArrestOrExcepMeans)
                            : null,
                        classif.Value.ClearencesInvolvingJuveniles.HasValue
                            ? new XElement("ClearedByJuvArrest", classif.Value.ClearencesInvolvingJuveniles)
                            : null))));
        }

    }
}