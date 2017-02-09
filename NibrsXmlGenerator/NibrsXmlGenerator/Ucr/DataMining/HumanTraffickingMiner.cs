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
        public static void Mine(ConcurrentDictionary<String, ReportData> monthlyReportData, NibrsReport.Report report)
        {
            // Return if no human trafficking data to query
            if (!report.OffenseVictimAssocs.Any(ov => ov.RelatedOffense.UcrCode.Matches("64[AB]")))
                return;

            var actualOffenses =
                report.OffenseVictimAssocs.Where(ov => ov.RelatedOffense.UcrCode.Matches("64[AB]"))
                    .Select(ov => ov.OffenseRef)
                    .ToList();

            //Gather counts for Column 4 for Line A or B.
            foreach (var offense in actualOffenses)
            {
                monthlyReportData[report.UcrKey].HumanTraffickingData.ClassificationCounts.TryAdd(
                    offense.UcrCode.Substring(2, 1)).IncrementActualOffense();
            }

            //Gather counts for Column 5 for Line A or B.
            if (report.Incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceDate != null)
            {
                monthlyReportData[report.UcrKey].HumanTraffickingData.ClassificationCounts.TryAdd("A")
                    .IncrementAllClearences(actualOffenses.Count(o => o.UcrCode == "64A"));
                monthlyReportData[report.UcrKey].HumanTraffickingData.ClassificationCounts.TryAdd("B")
                    .IncrementAllClearences(actualOffenses.Count(o => o.UcrCode == "64B"));
            }
            else
            {
                monthlyReportData[report.UcrKey].HumanTraffickingData.ClassificationCounts.TryAdd("A")
                    .IncrementAllClearences(report.Arrests.Count(a => a.Charge.UcrCode == "64A"));
                monthlyReportData[report.UcrKey].HumanTraffickingData.ClassificationCounts.TryAdd("B")
                    .IncrementAllClearences(report.Arrests.Count(a => a.Charge.UcrCode == "64B"));
            }

            //Gather counts for Column 6 for Line A or B.
            var juvenileLineAClearance =
                report.ArrestSubjectAssocs.Count(
                    a => a.SubjectRef.Person.AgeMeasure.IsJuvenile && a.ActivityRef.Charge.UcrCode == "64A");

            var juvenileLineBClearance =
                report.ArrestSubjectAssocs.Count(
                    a => a.SubjectRef.Person.AgeMeasure.IsJuvenile && a.ActivityRef.Charge.UcrCode == "64B");

            monthlyReportData[report.UcrKey].HumanTraffickingData.ClassificationCounts.TryAdd("A")
                .IncrementAllClearences(juvenileLineAClearance);
            monthlyReportData[report.UcrKey].HumanTraffickingData.ClassificationCounts.TryAdd("B")
                .IncrementAllClearences(juvenileLineBClearance);
        }
    }
}