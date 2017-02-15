using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NibrsXml.NibrsReport;
using NibrsXml.Ucr.DataCollections;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataMining
{
    internal class HumanTraffickingMiner : GeneralSummaryMiner
    {
        private static readonly Dictionary<string, string> HumanTraffickingClearanceClassificationDictionary = new Dictionary<string, string>
        {
            {"64A", "A"},
            {"64B", "B"}
        };

        public HumanTraffickingMiner(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report) : base(monthlyReportData, report)
        {
            //All derived classes of GeneralSummaryMiner must implement this constructor that calls the base constructor.
            //No additional calls need to be made because the base constructor is making the appropriate calls already.
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

            var humanTraffickingData = monthlyReportData[report.UcrKey].HumanTraffickingData;

            var actualOffenses =
                report.OffenseVictimAssocs.Where(ov => ov.RelatedOffense.UcrCode.Matches("64[AB]"))
                    .Select(ov => ov.RelatedOffense)
                    .ToList();

            //Gather counts for Column 4 for Line A or B.
            foreach (var offense in actualOffenses)
                humanTraffickingData.IncrementActualOffense(offense.UcrCode.Substring(2, 1));

            //Columns 5 and 6 (All/Juvenile Clearances) will be handled by GeneralSummaryMiner
        }
    }
}