using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Ucr.DataCollections;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataMining
{
    class ReturnAMiner : GeneralSummaryMiner
    {
        internal struct ClearanceDetails
        {
            private String UcrReportKey;
            private int AllScores;
            private int JuvenileScores;
        }

        internal static void Mine(System.Collections.Concurrent.ConcurrentDictionary<string, DataCollections.ReportData> monthlyOriReportData, NibrsReport.Report report)
        {
            //UCR Hierarchy Rules to return only the offense to score.
            var applicableOffenses = report.Offenses.Where(o => o.UcrCode.MatchOne(UcrHierarchyMiner.UcrHierarchyOrderArray.ToArray())).ToList();

            var mostImportantOffenseForReport = new UcrHierarchyMiner(applicableOffenses).HighestRatedOffense;

            //Score Homicide Offenses
            var returnA = new ReturnA();

            //Assign the ReturnA to the ORI month and year key of the monthlyOriReportData dictionary.


            returnA.ScoreHomicides(homicideOffenses: report.Offenses);

            //Score Rape Offenses
            returnA.ScoreRapeFunctions(rapeOffenses: report.Offenses);

            //Score Assault Offenses
            returnA.ScoreAssaultOffenses(assaultOffenses: report.Offenses);

            //Score Vehicle Offenses
            returnA.ScoreVehicleOffenses(vehicularOffenses: report.Offenses);

            //Score Burglaries
            returnA.ScoreBurglaries(robberyOffense: mostImportantOffenseForReport);
            
            returnA.ScoreClearances(report.Arrests,report.Header.ReportingAgency.OrgAugmentation.OrgOriId.Id);
        }
    }
}
