using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NibrsXml.Constants;
using NibrsXml.NibrsReport;
using NibrsXml.NibrsReport.Associations;
using NibrsXml.Ucr.DataCollections;
using NibrsXml.Utility;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataMining
{
    internal class HumanTraffickingMiner : GeneralSummaryMiner
    {
        private static readonly string[] ApplicableHumanTraffickingUcrCodes =
        {
            OffenseCode.HUMAN_TRAFFICKING_COMMERCIAL_SEX_ACTS.NibrsCode(),
            OffenseCode.HUMAN_TRAFFICKING_INVOLUNTARY_SERVITUDE.NibrsCode()
        };

        private static readonly Dictionary<string, string> HumanTraffickingClearanceClassificationDictionary = new Dictionary<string, string>
        {
            {"64A", "A"},
            {"64B", "B"}
        };

        public HumanTraffickingMiner(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report) : base(monthlyReportData, report)
        {
            // Return if no human trafficking data to query
            //No additional calls need to be made because the base constructor is making the appropriate calls already.
        }

        protected override string[] ApplicableUcrCodes
        {
            get { return ApplicableHumanTraffickingUcrCodes; }
        }

        protected override Dictionary<string, string> ClearanceClassificationDictionary
        {
            get { return HumanTraffickingClearanceClassificationDictionary; }
        }

        protected override void IncrementClearances(ConcurrentDictionary<string, ReportData> monthlyReportData, ClearanceDetails clearanceDetailsList)
        {
            monthlyReportData.TryAdd(clearanceDetailsList.UcrReportKey, new ReportData());
            monthlyReportData[clearanceDetailsList.UcrReportKey].HumanTraffickingData.IncrementAllClearences(clearanceDetailsList.ClassificationKey, clearanceDetailsList.AllScoresIncrementStep);
        }

        protected override void Mine(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report)
        {
            // Return if no human trafficking data to query
            if (!report.OffenseVictimAssocs.Any(ov => ov.RelatedOffense.UcrCode.Matches("64[AB]")))
                return;

            var humanTraffickingData = monthlyReportData[report.UcrKey()].HumanTraffickingData;

            var actualOffenses =
                report.OffenseVictimAssocs.Where(ov => ov.RelatedOffense.UcrCode.Matches("64[AB]"))
                    .Select(ov => ov.RelatedOffense)
                    .ToList();

            //Gather counts for Column 4 for Line A or B.
            foreach (var offense in actualOffenses)
                humanTraffickingData.IncrementActualOffense(offense.UcrCode.Substring(2, 1));
        }

        protected override void ScoreClearances(ConcurrentDictionary<string, ReportData> monthlyReportData, string ucrReportKey, Report fauxReport, bool doScoreColumn6)
        {
            //Gather counts for Column 5 for Line A or B.
            monthlyReportData.TryAdd(ucrReportKey, new ReportData());
            var humanTraffickingData = monthlyReportData[ucrReportKey].HumanTraffickingData;

            var clearedOffenses = fauxReport.OffenseVictimAssocs.Select(ov => ov.RelatedOffense);

            foreach (var offense in clearedOffenses)
                humanTraffickingData.IncrementAllClearences(offense.UcrCode.Substring(2, 1), 1, doScoreColumn6);
        }

        protected override List<OffenseVictimAssociation> CreateFauxOffenseVictimAssociations(Report report, string ucrClearanceCode)
        {
            return report.OffenseVictimAssocs.Where(ov => ov.RelatedOffense.UcrCode.MatchOne(ApplicableUcrCodes)).ToList();
        }
    }
}