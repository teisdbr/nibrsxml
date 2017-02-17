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
        private static readonly string[] ApplicableReturnAUcrCodes =
        {
            OffenseCode.MURDER_NONNEGLIGENT.NibrsCode(),
            OffenseCode.NEGLIGENT_MANSLAUGHTER.NibrsCode(),
            OffenseCode.RAPE.NibrsCode(),
            OffenseCode.ROBBERY.NibrsCode(),
            OffenseCode.AGGRAVATED_ASSAULT.NibrsCode(),
            OffenseCode.SIMPLE_ASSAULT.NibrsCode(),
            OffenseCode.INTIMIDATION.NibrsCode(),
            OffenseCode.BURGLARY_BREAKING_AND_ENTERING.NibrsCode(),
            OffenseCode.PICKPOCKETING.NibrsCode(),
            OffenseCode.PURSE_SNATCHING.NibrsCode(),
            OffenseCode.SHOPLIFTING.NibrsCode(),
            OffenseCode.THEFT_FROM_BUILDING.NibrsCode(),
            OffenseCode.THEFT_FROM_COIN_OPERATED_MACHINE.NibrsCode(),
            OffenseCode.THEFT_FROM_MOTOR_VEHICLE.NibrsCode(),
            OffenseCode.THEFT_OF_MOTOR_VEHICLE_PARTS_OR_ACCESSORIES.NibrsCode(),
            OffenseCode.OTHER_LARCENY.NibrsCode(),
            OffenseCode.MOTOR_VEHICLE_THEFT.NibrsCode()
        };

        private static readonly string[] ApplicableHumanTraffickingUcrCodes =
        {
            OffenseCode.HUMAN_TRAFFICKING_COMMERCIAL_SEX_ACTS.NibrsCode(),
            OffenseCode.HUMAN_TRAFFICKING_INVOLUNTARY_SERVITUDE.NibrsCode()
        };

        private static readonly string[] ApplicableArsonUcrCodes = {OffenseCode.ARSON.NibrsCode()};

        public static ConcurrentDictionary<string, ReportData> Mine(List<Report> nibrsIncidentReports)
        {
            var monthlyOriReportData = new ConcurrentDictionary<string, ReportData>();
            foreach (var report in nibrsIncidentReports)
            {
                //Make sure there is at least an empty ReportData structure for this report
                monthlyOriReportData.TryAdd(report.UcrKey, new ReportData());

                //Mine data
                //AsreMiner.MineAdd(monthlyOriReportData, report);

                //Mine Human Trafficking Data
                HumanTraffickingMiner.Mine(monthlyOriReportData, report);

                //Arson Data
                ArsonMiner.Mine(monthlyOriReportData, report);

                //Return A Data
                if (report.Offenses.Count(o => o.UcrCode.MatchOne(ApplicableReturnAUcrCodes)) > 0) ReturnAMiner.Mine(monthlyOriReportData, report);
            }
            return monthlyOriReportData;
        }
    }
}