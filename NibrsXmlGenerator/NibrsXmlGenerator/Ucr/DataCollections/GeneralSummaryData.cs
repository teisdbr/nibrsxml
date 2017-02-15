using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Arrestee;
using NibrsXml.NibrsReport.Incident;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.Utility;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataCollections
{
    using ExceptionalClearances = Int32;
    using ExceptionalClearanceOfJuveniles = Int32;

    public abstract class GeneralSummaryData
    {
        #region Constants

        public const String GrandTotal = "Grand Total";

        #endregion

        #region Configuration

        public abstract String XmlRootName { get; }
        public abstract String XslFileName { get; }

        #endregion

        #region Properties

        protected Dictionary<String, GeneralSummaryCounts> ClassificationCounts { get; set; }

        #endregion

        protected GeneralSummaryData()
        {
            this.ClassificationCounts = new Dictionary<string, GeneralSummaryCounts>();

            ClassificationCountEntryInstantiations();

            //Define the basic shared "Grand Total" row of all reports.
            this.ClassificationCounts.Add(GrandTotal, new GeneralSummaryCounts());
        }

        protected virtual void ClassificationCountEntryInstantiations() { }

        public XDocument Serialize()
        {
            return new XDocument(
                new XProcessingInstruction(
                    "xml-stylesheet",
                    "type=\"text/xsl\" href=\"" + XslFileName + "\""),
                new XElement(XmlRootName,
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
                            : null,
                        classif.Value.EstimatedValueOfPropertyDamage.HasValue
                            ? new XElement("EstimatedValueOfDamage", classif.Value.EstimatedValueOfPropertyDamage)
                            : null))));
        }

        public virtual void IncrementActualOffense(String key, int byValue = 1)
        {
            ClassificationCounts.TryAdd(key).IncrementActualOffense(byValue);
            ClassificationCounts[GrandTotal].IncrementActualOffense(byValue);
        }

        public virtual void IncrementAllClearences(String key, int byValue = 1, Boolean allArresteesAreJuvenile = false)
        {
            ClassificationCounts.TryAdd(key).IncrementAllClearences(byValue);
            ClassificationCounts[GrandTotal].IncrementAllClearences(byValue);

            //Only increment whenever the caller 
            if (allArresteesAreJuvenile) IncrementJuvenileClearences(key, byValue);
        }

        protected virtual void IncrementJuvenileClearences(String key, int byValue = 1)
        {
            ClassificationCounts.TryAdd(key).IncrementJuvenileClearences(byValue);
            ClassificationCounts[GrandTotal].IncrementJuvenileClearences(byValue);
        }

        public virtual void IncrementEstimatedValueOfPropertyDamage(String key, long byValue = 1)
        {
            ClassificationCounts.TryAdd(key).IncrementEstimatedValueOfPropertyDamage(byValue);
            ClassificationCounts[GrandTotal].IncrementEstimatedValueOfPropertyDamage(byValue);
        }

        public virtual Tuple<ExceptionalClearances, ExceptionalClearanceOfJuveniles> DetermineClearanceCounts(Incident incident, List<Arrestee> arrestees)
        {
            //Determine if incident is exceptionally cleared or not.
            if (incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceDate != null)
            {
                //Determine if any arrests occurred that cleared this incident.

                //Determine if any juvenile were arrested.
            }
            else
            {
                
            }

            return Tuple.Create(1, 2);
        }
    }
}