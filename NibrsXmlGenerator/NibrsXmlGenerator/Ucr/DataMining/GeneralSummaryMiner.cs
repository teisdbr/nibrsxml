using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NibrsModels.Constants;
using NibrsXml.Constants;
using NibrsModels.NibrsReport;
using NibrsModels.NibrsReport.Arrest;
using NibrsModels.NibrsReport.Associations;
using NibrsModels.NibrsReport.Item;
using NibrsModels.NibrsReport.Offense;
using NibrsModels.Utility;
using NibrsXml.Ucr.DataCollections;
using NibrsXml.Utility;
using TeUtil.Extensions;


namespace NibrsXml.Ucr.DataMining
{
    /** !!IMPORTANT!! ** CLEARANCE SCORING PROCESS (shared among ReturnA, Arson, and HumanTrafficking)
     * 1. Incident contains arrests
     *      a. Check arrestees' ages to see if incident involves only juveniles
     *      b. Use only the arrest with the highest ranked ucr charge
     *      c. Based on ucr charge, collect the following data element(s)
     *          i.   Corresponding offense
     *          ii.  Corresponding offense-victim associations
     *          iii. Corresponding offense, corresponding items
     *      d. Pass data elements to lower level for scoring.
     * 2. Incident has no arrests
     *      a. Incident has exceptional clearance date
     *          i.   Use offenders to check if incident involves only juveniles (reuse allArresteesAreJuvenile boolean)
     *          ii.  Score column 5 as you would for column 4 with the additional allArresteesAreJuvenile boolean
     * 3. No clearance scores applicable
    */

    internal abstract class GeneralSummaryMiner : ClearanceMiner
    {
        /// <summary>
        ///     Calling the constructor of any general summary report will automatically increment the appropriate reports' columns
        ///     4, 5, 6, and 8.
        /// </summary>
        /// <param name="monthlyReportData"></param>
        /// <param name="report"></param>
        protected GeneralSummaryMiner(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report) : base(monthlyReportData, report) { }

        /// <summary>
        ///     This function handles incrementing the counters for columns 5 and 6 of general summary reports
        /// </summary>
        /// <param name="monthlyReportData"></param>
        /// <param name="report"></param>
        protected override void ScoreClearances(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report)
        {
            var clearanceUcrCode = report.ClearanceUcrCode();
            if (clearanceUcrCode == null || !clearanceUcrCode.MatchOne(ApplicableUcrCodes))
                //Do not score clearances if there's no applicable clearance code avaiable
                return;

            //Move forward with assumption that the report contains a valid clearance

            if (report.Offenses.All(o => o.UcrCode != clearanceUcrCode))
            {
                ScoreClearancesWithAssumptions(monthlyReportData, report);
                return;
            }

            //Create ucrReportKey
            var clearanceDate = ConvertNibrsDateToDateKeyPrefix(report.ClearanceDate());
            var ucrReportKey = clearanceDate + report.Header.ReportingAgency.OrgAugmentation.OrgOriId.Id;

            //A faux report will have to be created, but at the discretion of the derived classes because the faux
            //report may be created differently depending on the ucr report type
            var fauxReport = CreateFauxClearanceReport(report, clearanceUcrCode);
            ScoreClearances(monthlyReportData, ucrReportKey, fauxReport, report.DoScoreColumn6ForGeneralSummaryData());
        }

        protected abstract void ScoreClearances(ConcurrentDictionary<string, ReportData> monthlyReportData, string ucrReportKey, Report fauxReport, bool doScoreColumn6);

        private Report CreateFauxClearanceReport(Report report, string ucrClearanceCode)
        {
            //Faux report objects should only contain data we need to score the clearance for the original report
            var fauxOffense = CreateFauxOffense(report, ucrClearanceCode);
            var fauxItems = CreateFauxItems(report, ucrClearanceCode);
            var fauxOffenseVictimAssocs = CreateFauxOffenseVictimAssociations(report, ucrClearanceCode);

            return new Report
            {
                Offenses = fauxOffense == null ? null : new List<Offense> {fauxOffense},
                Items = fauxItems,
                OffenseVictimAssocs = fauxOffenseVictimAssocs
            };
        }

        protected virtual Offense CreateFauxOffense(Report report, string ucrClearanceCode)
        {
            return null;
        }

        protected virtual List<OffenseVictimAssociation> CreateFauxOffenseVictimAssociations(Report report, string ucrClearanceCode)
        {
            return null;
        }

        protected virtual List<Item> CreateFauxItems(Report report, string ucrClearanceCode)
        {
            return null;
        }

        #region Clearance Scoring Implementation With Assumptions

        internal struct ClearanceDetails
        {
            public string UcrReportKey;
            public string ClassificationKey;
            public int AllScoresIncrementStep;
            public int JuvenileScoresIncrementStep;
        }

        /// <summary>
        ///     Every derived class (ReturnA, Arson, HumanTrafficking) needs to define this dictionary of psuedo-keys
        ///     that map to real keys that are used in the xml output.
        ///     This is because the determination of the classifications to score clearances on are different across the three
        ///     reports.
        /// </summary>
        protected abstract Dictionary<string, string> ClearanceClassificationDictionary { get; }

        /// <summary>
        ///     All derived classes must override IncrementClearances because it is only there that this function
        ///     will have the context of what report it needs to be scoring for.
        /// </summary>
        /// <param name="monthlyReportData"></param>
        /// <param name="clearanceDetailsList"></param>
        protected abstract void IncrementClearances(ConcurrentDictionary<string, ReportData> monthlyReportData, ClearanceDetails clearanceDetailsList);

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

        private ClearanceDetails? GetClearanceDetails(Arrest forArrest, bool allArresteesAreJuvenile, string ori, List<Item> burnedItems)
        {
            var classificationKey = forArrest.Charge.UcrCode == OffenseCode.ARSON.NibrsCode()
                ? GetClassificationKey(forArrest, burnedItems)
                : GetClassificationKey(forArrest);

            if (classificationKey == null)
                return null;

            var arrestDate = ConvertNibrsDateToDateKeyPrefix(forArrest.Date.DateTime ?? forArrest.Date.Date);

            return new ClearanceDetails
            {
                UcrReportKey = arrestDate + ori,
                ClassificationKey = classificationKey,
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

        private void ScoreClearancesWithAssumptions(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report)
        {
            //GET CLEARANCE DETAILS

            var clearanceDetailsList = new List<ClearanceDetails>();
            var ori = report.Header.ReportingAgency.OrgAugmentation.OrgOriId.Id;

            if (!report.Arrests.Any())
            {
                var incidentClearanceDate = report.Incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceDate.YearMonthDate;
                //No arrests available, so check if incident is cleared
                if (incidentClearanceDate != null)
                {
                    //Incident is cleared so we must clear all offenses of this incident
                    //Because there are no arrests, we cannot tell if only juveniles were involved. Score only for column 5

                    //Construct the UcrReportKey since it will be shared amongst all offenses here
                    incidentClearanceDate = ConvertNibrsDateToDateKeyPrefix(incidentClearanceDate);
                    var ucrReportKey = incidentClearanceDate + ori;

                    foreach (var offense in report.Offenses)
                        clearanceDetailsList.TryAdd(GetClearanceDetails(offense, ucrReportKey));
                }
                else
                    //Incident is not cleared means no scoring
                {
                    return;
                }
            }
            else
            {
                var burnedItems = report.Items.Where(i => i.Status.Code == ItemStatusCode.BURNED.NibrsCode()).ToList();
                var allArresteesAreJuvenile = report.ArrestSubjectAssocs.All(assoc => assoc.RelatedArrestee.Person.AgeMeasure.IsJuvenile);
                var arrestsWithAssociations = report.ArrestSubjectAssocs.Select(assoc => assoc.RelatedArrest);
                foreach (var arrest in arrestsWithAssociations)
                    clearanceDetailsList.TryAdd(GetClearanceDetails(arrest, allArresteesAreJuvenile, ori, burnedItems));
            }

            //BEGIN SCORING

            foreach (var clearance in clearanceDetailsList)
                IncrementClearances(monthlyReportData, clearance);
        }

        #endregion
    }
}