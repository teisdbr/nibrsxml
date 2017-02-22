using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NibrsXml.Constants;
using NibrsXml.NibrsReport;
using NibrsXml.Ucr.DataCollections;
using NibrsXml.Utility;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataMining
{
    internal class ReportDataMiner
    {
        public static ConcurrentDictionary<string, ReportData> Mine(List<Report> nibrsIncidentReports)
        {
            var monthlyOriReportData = new ConcurrentDictionary<string, ReportData>();
            foreach (var report in nibrsIncidentReports)
            {
                //Make sure there is at least an empty ReportData structure for this report
                monthlyOriReportData.TryAdd(report.UcrKey(), new ReportData());

                //Mine data
                //AsreMiner.MineAdd(monthlyOriReportData, report);

                //Mine Human Trafficking Data
                new HumanTraffickingMiner(monthlyOriReportData, report);

                //Arson Data
                new ArsonMiner(monthlyOriReportData, report);

                //Return A Data
                if (report.Offenses.Count(o => o.UcrCode.MatchOne(ReturnAMiner.ApplicableReturnAUcrCodes)) > 0)
                    new ReturnAMiner(monthlyOriReportData, report);

                //Leoka Data
                LeokaMiner leokaMiner = new LeokaMiner();
                if (report.Victims.Any(v => v.CategoryCode == VictimCategoryCode.LAW_ENFORCEMENT_OFFICER.NibrsCode())) leokaMiner.Mine(monthlyOriReportData, report);
            }
            return monthlyOriReportData;
        }
    }
}