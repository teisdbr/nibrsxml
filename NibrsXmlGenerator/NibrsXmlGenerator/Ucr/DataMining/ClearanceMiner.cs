using System.Collections.Concurrent;
using NibrsModels.NibrsReport;
using NibrsXml.Ucr.DataCollections;

namespace NibrsXml.Ucr.DataMining
{
    internal abstract class ClearanceMiner
    {
        protected ClearanceMiner(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report)
        {
            Mine(monthlyReportData, report);
            ScoreClearances(monthlyReportData, report);
        }

        protected abstract string[] ApplicableUcrCodes { get; }
        protected abstract void Mine(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report);
        protected abstract void ScoreClearances(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report);

        /// <summary>
        ///     Converts the YYYY-MM-DD date to a YYYYMM format
        /// </summary>
        public static string ConvertNibrsDateToDateKeyPrefix(string nibrsDate)
        {
            return nibrsDate.Replace("-", "").Remove(6);
        }

    }
}
