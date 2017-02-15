using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoadBusinessLayer;
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

            var returnA = new ReturnA();

            //Assign the ReturnA to the ORI month and year key of the monthlyOriReportData dictionary.

            //Score Homicide Offense
            returnA.ScoreHomicide(mostImportantOffenseForReport);

            //Score Rape Offense
            returnA.ScoreRape(mostImportantOffenseForReport);

            //Score Assault Offense
            returnA.ScoreAssault(mostImportantOffenseForReport);

            //Score Vehicle Offense
            returnA.ScoreVehicleTheft(mostImportantOffenseForReport);

            //Score Burglary
            returnA.ScoreBurglary(robberyOffense: mostImportantOffenseForReport);
            
            returnA.ScoreClearances(report.Arrests,report.Header.ReportingAgency.OrgAugmentation.OrgOriId.Id);
        }
    }
}
