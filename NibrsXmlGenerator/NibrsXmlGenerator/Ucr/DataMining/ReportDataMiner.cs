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
            ConcurrentDictionary<string, ReportData> monthlyReportData = new ConcurrentDictionary<string, ReportData>();
            foreach (Report report in nibrsIncidentReports)
            {
                monthlyReportData.TryAdd(report.Header.ReportDate.YearMonthDate, new ReportData());
                AsreMiner.MineAdd(monthlyReportData, report);
            }

            return monthlyReportData;
        }
    }
}
