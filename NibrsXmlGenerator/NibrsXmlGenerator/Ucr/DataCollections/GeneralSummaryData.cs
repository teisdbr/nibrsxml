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

        public Dictionary<String, GeneralSummaryCounts> ClassificationCounts { get; set; }

        #endregion

        public GeneralSummaryData()
        {
            this.ClassificationCounts = new Dictionary<string, GeneralSummaryCounts>();
        }

        public XDocument Serialize()
        {
            return new XDocument(
                new XProcessingInstruction(
                    "xml-stylesheet",
                    "type=\"text/xsl\" href=\"" + XslFileName + "\""),
                new XElement(XmlRootName,
                    new XElement("UcrCodeDictionary", new XElement("UCRDescription", new XAttribute("value", "A"), "A. Commercial Sex Acts"), new XElement("UCRDescription", new XAttribute("value", "B"), "B. Involuntary Servitude")),
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