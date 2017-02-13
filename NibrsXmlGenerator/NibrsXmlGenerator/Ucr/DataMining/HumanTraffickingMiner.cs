using System;
using System.Collections.Concurrent;
using System.Linq;
using NibrsXml.Ucr.DataCollections;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataMining
{
    internal class HumanTraffickingMiner
    {
        public static void Mine(ConcurrentDictionary<String, ReportData> monthlyReportData, NibrsReport.Report report)
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
            {
                humanTraffickingData.IncrementActualOffense(offense.UcrCode.Substring(2, 1));
            }

            //Gather counts for Column 5 for Line A or B.
            if (report.Incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceDate != null)
            {
                humanTraffickingData.IncrementAllClearences("A", actualOffenses.Count(o => o.UcrCode == "64A"));
                humanTraffickingData.IncrementAllClearences("B", actualOffenses.Count(o => o.UcrCode == "64B"));
            }
            else
            {
                humanTraffickingData.IncrementAllClearences("A", report.Arrests.Count(a => a.Charge.UcrCode == "64A"));
                humanTraffickingData.IncrementAllClearences("B", report.Arrests.Count(a => a.Charge.UcrCode == "64B"));
            }

            //Gather counts for Column 6 for Line A or B.
            var juvenileLineAClearanceCount =
                report.ArrestSubjectAssocs.Count(
                    a => a.RelatedArrestee.Person.AgeMeasure.IsJuvenile && a.RelatedArrest.Charge.UcrCode == "64A");

            var juvenileLineBClearanceCount =
                report.ArrestSubjectAssocs.Count(
                    a => a.RelatedArrestee.Person.AgeMeasure.IsJuvenile && a.RelatedArrest.Charge.UcrCode == "64B");

            humanTraffickingData.IncrementJuvenileClearences("A", juvenileLineAClearanceCount);
            humanTraffickingData.IncrementJuvenileClearences("B", juvenileLineBClearanceCount);
        }
    }
}