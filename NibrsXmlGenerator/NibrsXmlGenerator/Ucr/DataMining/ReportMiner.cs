using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NibrsXml.Constants;
using NibrsXml.NibrsReport;
using NibrsXml.Ucr.DataCollections;
using NibrsXml.Utility;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataMining
{
    internal class ReportMiner
    {
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public static ConcurrentDictionary<string, ReportData> Mine(List<Report> nibrsIncidentReports)
        {
            var monthlyOriReportData = new ConcurrentDictionary<string, ReportData>();

            //todo: make parallel
            foreach (var report in nibrsIncidentReports)
            {
                //Make sure there is at least an empty ReportData structure for this report
                monthlyOriReportData.TryAdd(report.UcrKey(), new ReportData());

                //Asre Data
                AsreMiner.MineAdd(monthlyOriReportData, report);

                //Human Trafficking Data
                new HumanTraffickingMiner(monthlyOriReportData, report);

                //Arson Data
                new ArsonMiner(monthlyOriReportData, report);

                //Return A Data (also handles supplement data)
                if (report.Offenses.Count(o => o.UcrCode.MatchOne(ReturnAMiner.ApplicableReturnAUcrCodes)) > 0)
                {
                    new ReturnAMiner(monthlyOriReportData, report);
                }
                    
                //Leoka Data
                if (report.Victims.Any(v => v.CategoryCode == VictimCategoryCode.LAW_ENFORCEMENT_OFFICER.NibrsCode()))
                {
                    new LeokaMiner(monthlyOriReportData, report);
                }

                //Supplementary Homicide Data
                SupplementaryHomicideMiner.Mine(monthlyOriReportData, report);

                //Hate Crime Data
                HateCrimeMiner.Mine(monthlyOriReportData, report);
            }

            return monthlyOriReportData;
        }
    }
}