using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Ucr.DataCollections;

namespace NibrsXml.Ucr.DataMining
{
    class ReturnAMiner
    {

        internal static void Mine(System.Collections.Concurrent.ConcurrentDictionary<string, DataCollections.ReportData> monthlyOriReportData, NibrsReport.Report report)
        {
            //UCR Hierarchy Rules to return only the offense to score.
            

            //Score Homicide Offenses
            ReturnA.ScoreHomicides(homicideOffenses: report.Offenses);

            //Score Rape Offenses
            ReturnA.ScoreRapeFunctions(rapeOffenses: report.Offenses);

            //Score Assault Offenses
            ReturnA.ScoreAssaultOffenses(assaultOffenses: report.Offenses);

            //Score Vehicle Offenses
            ReturnA.ScoreVehicleOffenses(vehicularOffenses: report.Offenses);

        }
    }
}
