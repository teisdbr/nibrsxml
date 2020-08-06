using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NibrsModels.Constants;
using NibrsXml.Constants;
using NibrsModels.NibrsReport;
using NibrsModels.Utility;
using NibrsXml.Ucr.DataCollections;
using NibrsXml.Utility;
using Util.Extensions;

namespace NibrsXml.Ucr.DataMining
{
    internal class ReportMiner
    {
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public static ConcurrentDictionary<string, ReportData> Mine(Submission submission)
        {
            //Only process incidents with action type I (insert)
            var nibrsIncidentReports = submission.Reports.Where(r => r.Header.ReportActionCategoryCode == ReportActionCategoryCode.I.NibrsCode()).ToList();

            var monthlyOriReportData = new ConcurrentDictionary<string, ReportData>();

            foreach (var report in submission.RejectedReports)            
            {

                //Make sure there is at least an empty ReportData structure for this report
                monthlyOriReportData.TryAdd(report.UcrKey(), new ReportData());

                //Add this report to the list of accepted incidents
                monthlyOriReportData[report.UcrKey()].RejectedIncidents.Add(Tuple.Create(report.Incident.ActivityId.Id,report.HasFailedToBuildProperly));
            }

            //todo: make parallel
            foreach (var report in nibrsIncidentReports)
            {
                //Make sure there is at least an empty ReportData structure for this report
                monthlyOriReportData.TryAdd(report.UcrKey(), new ReportData());

                //Add this report to the list of accepted incidents
                monthlyOriReportData[report.UcrKey()].AcceptedIncidents.Add(report.Incident.ActivityId.Id);

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