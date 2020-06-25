using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NibrsModels.NibrsReport.Arrestee;
using NibrsModels.NibrsReport.Incident;
using Util.Extensions;

namespace NibrsXml.Ucr.DataCollections
{
    using ExceptionalClearances = Int32;
    using ExceptionalClearanceOfJuveniles = Int32;

    public abstract class GeneralSummaryData
    {
        #region Constants

        public const string GrandTotal = "Grand Total";

        #endregion

        protected GeneralSummaryData()
        {
            ClassificationCounts = new Dictionary<string, GeneralSummaryCounts>();

            ClassificationCountEntryInstantiations();

            //Define the basic shared "Grand Total" row of all reports.
            ClassificationCounts.Add(GrandTotal, new GeneralSummaryCounts());
        }

        #region Properties

        protected Dictionary<string, GeneralSummaryCounts> ClassificationCounts { get; set; }

        #endregion

        protected virtual void ClassificationCountEntryInstantiations()
        {
        }

        public XDocument Serialize()
        {
            return new XDocument(
                new XProcessingInstruction(
                    "xml-stylesheet",
                    "type=\"text/xsl\" href=\"" + XslFileName + "\""),
                new XElement(XmlRootName,
                    ClassificationCounts.Select(classif => new XElement("Classification",
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

        public virtual void IncrementActualOffense(string key, int byValue = 1)
        {
            ClassificationCounts.TryAdd(key).IncrementActualOffense(byValue);
            ClassificationCounts[GrandTotal].IncrementActualOffense(byValue);
        }

        public virtual void IncrementAllClearences(string key, int byValue = 1, bool allArresteesAreJuvenile = false)
        {
            ClassificationCounts.TryAdd(key).IncrementAllClearences(byValue);
            ClassificationCounts[GrandTotal].IncrementAllClearences(byValue);

            //Only increment whenever the caller 
            if (allArresteesAreJuvenile) IncrementJuvenileClearences(key, byValue);
        }

        protected virtual void IncrementJuvenileClearences(string key, int byValue = 1)
        {
            ClassificationCounts.TryAdd(key).IncrementJuvenileClearences(byValue);
            ClassificationCounts[GrandTotal].IncrementJuvenileClearences(byValue);
        }

        public virtual void IncrementEstimatedValueOfPropertyDamage(string key, long byValue = 1)
        {
            ClassificationCounts.TryAdd(key).IncrementEstimatedValueOfPropertyDamage(byValue);
            ClassificationCounts[GrandTotal].IncrementEstimatedValueOfPropertyDamage(byValue);
        }

        public virtual Tuple<int, int> DetermineClearanceCounts(Incident incident, List<Arrestee> arrestees)
        {
            //Determine if incident is exceptionally cleared or not.
            if (incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceDate != null)
            {
                //Determine if any arrests occurred that cleared this incident.

                //Determine if any juvenile were arrested.
            }

            return Tuple.Create(1, 2);
        }

        #region Configuration

        public abstract string XmlRootName { get; }
        public abstract string XslFileName { get; }

        #endregion
    }
}