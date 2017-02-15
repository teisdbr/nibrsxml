using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NibrsXml.Constants;
using NibrsXml.NibrsReport;
using NibrsXml.NibrsReport.Arrest;
using NibrsXml.NibrsReport.Item;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.Ucr.DataCollections;
using NibrsXml.Utility;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataMining
{
    internal struct ClearanceDetails
    {
        public string UcrReportKey;
        public string ClassificationKey;
        public int AllScoresIncrementStep;
        public int JuvenileScoresIncrementStep;
    }

    internal abstract class GeneralSummaryMiner
    {
        protected GeneralSummaryMiner(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report)
        {
            Mine(monthlyReportData, report);
            ScoreClearances(monthlyReportData, report);
        }

        #region Abstract Functions
        /// <summary>
        ///     Every derived class (ReturnA, Arson, HumanTrafficking) needs to define this dictionary of psuedo-keys
        ///     that map to real keys that are used in the xml output.
        ///
        ///     This is because the determination of the classifications to score clearances on are different across the three reports.
        /// </summary>
        protected abstract Dictionary<string, string> ClearanceClassificationDictionary { get; }

        /// <summary>
        ///     All derived classes must override IncrementClearances because it is only there that this function
        ///     will have the context of what report it needs to be scoring for.
        /// </summary>
        /// <param name="monthlyReportData"></param>
        /// <param name="clearanceDetailsList"></param>
        protected abstract void IncrementClearances(ConcurrentDictionary<string, ReportData> monthlyReportData, ClearanceDetails clearanceDetailsList);

        protected abstract void Mine(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report);
        #endregion

        #region Static Functions (Helpers)
        /// <summary>
        ///     Converts the YYYY-MM-DD date to a YYYYMM format
        /// </summary>
        private static string ConvertNibrsDateToDateKeyPrefix(string nibrsDate)
        {
            return nibrsDate.Replace("-", "").Remove(6);
        }

        private static string GetClearanceClassificationPsuedoKey(Arrest fromArrest, List<Item> burnedItems)
        {
            return burnedItems == null
                ? fromArrest.Charge.UcrCode
                : fromArrest.Charge.UcrCode + ArsonMiner.GetPropertyClassification(burnedItems);
        }

        private static string GetClearanceClassificationPsuedoKey(Offense fromOffense)
        {
            return fromOffense.UcrCode;
        }
        #endregion

        #region Clearance Scoring Functions
        private string GetClassificationKey(Arrest fromArrest, List<Item> burnedItems = null)
        {
            return ClearanceClassificationDictionary.ContainsKey(GetClearanceClassificationPsuedoKey(fromArrest, burnedItems))
                ? ClearanceClassificationDictionary[GetClearanceClassificationPsuedoKey(fromArrest, burnedItems)]
                : null;
        }

        private string GetClassificationKey(Offense fromOffense)
        {
            return ClearanceClassificationDictionary.ContainsKey(GetClearanceClassificationPsuedoKey(fromOffense))
                ? ClearanceClassificationDictionary[GetClearanceClassificationPsuedoKey(fromOffense)]
                : null;
        }

        private ClearanceDetails GetClearanceDetails(Arrest forArrest, bool allArresteesAreJuvenile, string ori, List<Item> burnedItems)
        {
            var arrestDate = ConvertNibrsDateToDateKeyPrefix(forArrest.Date.YearMonthDate);
            return new ClearanceDetails
            {
                UcrReportKey = arrestDate + ori,
                ClassificationKey = forArrest.Charge.UcrCode == OffenseCode.ARSON.NibrsCode()
                    ? GetClassificationKey(forArrest, burnedItems)
                    : GetClassificationKey(forArrest),
                AllScoresIncrementStep = 1,
                JuvenileScoresIncrementStep = allArresteesAreJuvenile ? 1 : 0
            };
        }

        private ClearanceDetails? GetClearanceDetails(Offense forOffense, string ucrReportKey)
        {
            var classificationKey = GetClassificationKey(forOffense);

            if (classificationKey == null)
                return null;

            return new ClearanceDetails
            {
                UcrReportKey = ucrReportKey,
                ClassificationKey = classificationKey,
                AllScoresIncrementStep = 1,
                JuvenileScoresIncrementStep = 0
            };
        }

        private void ScoreClearances(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report)
        {
            //GET CLEARANCE DETAILS

            var clearanceDetailsList = new List<ClearanceDetails>();
            var ori = report.Header.ReportingAgency.OrgAugmentation.OrgOriId.Id;

            if (!report.Arrests.Any())
            {
                var incidentDate = report.Incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceDate.YearMonthDate;
                //No arrests available, so check if incident is cleared
                if (incidentDate != null)
                {
                    //Incident is cleared so we must clear all offenses of this incident
                    //Because there are no arrests, we cannot tell if only juveniles were involved. Score only for column 5

                    //Construct the UcrReportKey since it will be shared amongst all offenses here
                    incidentDate = ConvertNibrsDateToDateKeyPrefix(incidentDate);
                    var ucrReportKey = incidentDate + ori;

                    foreach (var offense in report.Offenses)
                        clearanceDetailsList.TryAdd(GetClearanceDetails(offense, ucrReportKey));
                }
                else
                    //Incident is not cleared means no scoring
                    return;
            }
            else
            {
                var burnedItems = report.Items.Where(i => i.Status.Code == ItemStatusCode.BURNED.NibrsCode()).ToList();
                var allArresteesAreJuvenile = report.ArrestSubjectAssocs.All(assoc => assoc.RelatedArrestee.Person.AgeMeasure.IsJuvenile);
                var arrestsWithAssociations = report.ArrestSubjectAssocs.Select(assoc => assoc.RelatedArrest);
                clearanceDetailsList.AddRange(arrestsWithAssociations.Select(arrest => GetClearanceDetails(arrest, allArresteesAreJuvenile, ori, burnedItems)));
            }

            //BEGIN SCORING

            foreach (var clearance in clearanceDetailsList)
                IncrementClearances(monthlyReportData, clearance);
        }
        #endregion

    }

}