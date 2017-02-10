using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Ucr.DataCollections;
using NibrsXml.Utility;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataMining
{
    class HumanTraffickingMiner
    {
        private const String GrandTotal = "Grand Total";

        /// <summary>
        /// Function that returns a report specific Grand Total Classification Counts
        /// </summary>
        private static Func<ConcurrentDictionary<String, ReportData>, NibrsReport.Report, GeneralSummaryCounts>
            grandTotalIncrementer =
                (monthlyReportData, report) =>
                {
                    return monthlyReportData[report.UcrKey].HumanTraffickingData.ClassificationCounts.TryAdd(GrandTotal);
                };

        public static void Mine(ConcurrentDictionary<String, ReportData> monthlyReportData, NibrsReport.Report report)
        {
            // Return if no human trafficking data to query
            if (!report.OffenseVictimAssocs.Any(ov => ov.RelatedOffense.UcrCode.Matches("64[AB]")))
                return;

            var actualOffenses =
                report.OffenseVictimAssocs.Where(ov => ov.RelatedOffense.UcrCode.Matches("64[AB]"))
                    .Select(ov => ov.RelatedOffense)
                    .ToList();

            //Gather counts for Column 4 for Line A or B.
            foreach (var offense in actualOffenses)
            {
                monthlyReportData[report.UcrKey].HumanTraffickingData.ClassificationCounts.TryAdd(
                        offense.UcrCode.Substring(2, 1))
                    .IncrementActualOffense(
                        incrementHandler:
                        i => grandTotalIncrementer(monthlyReportData, report).IncrementActualOffense(i));
            }

            //Gather counts for Column 5 for Line A or B.
            if (report.Incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceDate != null)
            {
                monthlyReportData[report.UcrKey].HumanTraffickingData.ClassificationCounts.TryAdd("A")
                    .IncrementAllClearences(actualOffenses.Count(o => o.UcrCode == "64A"), i => grandTotalIncrementer(monthlyReportData, report).IncrementAllClearences(i));
                monthlyReportData[report.UcrKey].HumanTraffickingData.ClassificationCounts.TryAdd("B")
                    .IncrementAllClearences(actualOffenses.Count(o => o.UcrCode == "64B"), i => grandTotalIncrementer(monthlyReportData, report).IncrementAllClearences(i));
            }
            else
            {
                monthlyReportData[report.UcrKey].HumanTraffickingData.ClassificationCounts.TryAdd("A")
                    .IncrementAllClearences(report.Arrests.Count(a => a.Charge.UcrCode == "64A"), i => grandTotalIncrementer(monthlyReportData, report).IncrementAllClearences(i));
                monthlyReportData[report.UcrKey].HumanTraffickingData.ClassificationCounts.TryAdd("B")
                    .IncrementAllClearences(report.Arrests.Count(a => a.Charge.UcrCode == "64B"), i => grandTotalIncrementer(monthlyReportData, report).IncrementAllClearences(i));
            }

            //Gather counts for Column 6 for Line A or B.
            var juvenileLineAClearance =
                report.ArrestSubjectAssocs.Count(
                    a => a.RelatedArrestee.Person.AgeMeasure.IsJuvenile && a.RelatedArrest.Charge.UcrCode == "64A");

            var juvenileLineBClearance =
                report.ArrestSubjectAssocs.Count(
                    a => a.RelatedArrestee.Person.AgeMeasure.IsJuvenile && a.RelatedArrest.Charge.UcrCode == "64B");

            monthlyReportData[report.UcrKey].HumanTraffickingData.ClassificationCounts.TryAdd("A")
                .IncrementJuvenileClearences(juvenileLineAClearance, i => grandTotalIncrementer(monthlyReportData, report).IncrementJuvenileClearences(i));
            monthlyReportData[report.UcrKey].HumanTraffickingData.ClassificationCounts.TryAdd("B")
                .IncrementJuvenileClearences(juvenileLineBClearance, i => grandTotalIncrementer(monthlyReportData, report).IncrementJuvenileClearences(i));
        }
    }
}