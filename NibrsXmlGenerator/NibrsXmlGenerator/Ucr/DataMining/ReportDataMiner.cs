using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Constants;
using NibrsXml.NibrsReport;
using NibrsXml.Ucr.DataCollections;
using System.Collections.Concurrent;

namespace NibrsXml.Ucr.DataMining
{
    class ReportDataMiner
    {
        public static ConcurrentDictionary<string, ReportData> Mine(List<Report> nibrsIncidentReports)
        {
            ConcurrentDictionary<string, ReportData> monthlyOriReportData = new ConcurrentDictionary<string, ReportData>();
            foreach (Report report in nibrsIncidentReports)
            {
                //Make sure there is at least an empty ReportData structure for this report
                monthlyOriReportData.TryAdd(report.UcrKey, new ReportData());

                //Mine data
                AsreMiner.MineAdd(monthlyOriReportData, report);

                //Mine Human Trafficking Data
                HumanTraffickingMiner.Mine(monthlyOriReportData, report);

                //Arson Data
                ArsonMiner.Mine(monthlyOriReportData, report);
            }

            return monthlyOriReportData;
        }
    }
}
