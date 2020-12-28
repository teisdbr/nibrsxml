using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NibrsModels.Constants;
using NibrsXml.Constants;
using NibrsModels.NibrsReport;
using NibrsModels.NibrsReport.Offense;
using NibrsModels.Utility;
using NibrsModels.NibrsReport.Offense;
using NibrsXml.Ucr.DataCollections;
using NibrsXml.Utility;
using TeUtil.Extensions;


namespace NibrsXml.Ucr.DataMining
{
    internal class LeokaMiner : ClearanceMiner
    {
        public LeokaMiner(ConcurrentDictionary<string, ReportData> monthlyOriReportData, Report report) : base(monthlyOriReportData, report) { }

        private static readonly string[] ApplicableLeokaUcrCodes = { "09A", "09B", "13A", "13B" };

        protected override string[] ApplicableUcrCodes
        {
            get
            {
                return ApplicableLeokaUcrCodes;
            }
        }

        private static string ExtractLeokaWeapons(List<OffenseForce> offenseForces)
        {
            return Convert.ToChar(Encoding.ASCII.GetBytes(offenseForces.ExtractWeaponGroup()).First() + 1).ToString().ToUpper();
        }

        protected override void Mine(ConcurrentDictionary<string, ReportData> monthlyOriReportData, Report report)
        {
            try
            {
                //Make sure the UCR Report to which this report would belong exists.
                monthlyOriReportData.TryAdd(report.UcrKey(), new ReportData());

                //Instance of the Leoka report to modify
                var leoka = monthlyOriReportData[report.UcrKey()].LeokaData;

                //********************************************************************************************Get Officers Killed or Assaulted Information
                var leokaVictims =
                    report.OffenseVictimAssocs.Where(
                        ov => ov.RelatedVictim.CategoryCode == VictimCategoryCode.LAW_ENFORCEMENT_OFFICER.NibrsCode() && ov.RelatedOffense.UcrCode.MatchOne(ApplicableUcrCodes)).ToList();
                var officersOfReportingAgency = report.Officers.Where(leo => leo.Unit == null);
                var leokaWithEnforcement = leokaVictims.Join(officersOfReportingAgency, l => l.RelatedVictim.Person.Id, o => o.Person.Id,
                    (v, o) => Tuple.Create(v.RelatedVictim, v.RelatedOffense, o)).ToList();

                //Ensure matched/found leoka victims do not have a null assignment or activity type
                //todo: this is a temporary fix given bad data in database and using bad data to test.
                var leokaWithEnforcementWithValidData = leokaWithEnforcement.Where(l => l.Item3.ActivityCategoryCode != null && l.Item3.AssignmentCategoryCode != null).ToList();

                //--Get Officers Killed Feloneously
                leoka.OfficersKilled.IncrementCount("09A", leokaWithEnforcement.Count(v => v.Item2.UcrCode == "09A"));
                //--Get Officers Killed By Accident
                leoka.OfficersKilled.IncrementCount("09B", leokaWithEnforcement.Count(v => v.Item2.UcrCode == "09B"));

                //********************************************************************************************Get Officers Assaulted Information
                foreach (var tuple in leokaWithEnforcementWithValidData.Where(l => l.Item2.UcrCode.MatchOne("13A", "13B")).ToList())
                {
                    //Score Weapons and Assignments for the first 11 classification lines (Activities)
                    leoka.ScoreActivityCounts(Leoka.ActivityTranslatorDictionary[tuple.Item3.ActivityCategoryCode], ExtractLeokaWeapons(tuple.Item2.Forces), tuple.Item3.AssignmentCategoryCode);

                    //Score Injuries by Weapon
                    leoka.ScoreActivityCounts("13", ExtractLeokaWeapons(tuple.Item2.Forces), null, tuple.Item1.VictimInjuries.Count(i => i.CategoryCode.MatchOne("B", "I", "L", "M", "O", "T", "U")));
                    leoka.ScoreActivityCounts("14", ExtractLeokaWeapons(tuple.Item2.Forces), null, tuple.Item1.VictimInjuries.Count(i => i.CategoryCode == "N"));

                    //Score Timing of Assaults
                    leoka.ScoreAssaultTime(report.Incident.ActivityDate.DateTime);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        ///     Leoka scores clearances differently from ReturnA, Arson, and HT because Leoka data is exceptional to the offense hierarchy rule.
        ///     All homicides and assaults on officers must be scored, not the highest ranking one.
        ///     This function will score one clearance per victim, assuming all clearance requirements are met.
        /// </summary>
        /// <param name="monthlyReportData"></param>
        /// <param name="report"></param>
        protected override void ScoreClearances(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report)
        {
            string[] clearanceUcrCodes;
            string clearanceYearMonth;

            if (report.ArrestSubjectAssocs.Any())
            {
                //Filter the available arrests to contain only leoka-qualified charges
                var leokaArrests = report.ArrestSubjectAssocs
                    .Where(assoc => assoc.RelatedArrest.Charge.UcrCode.MatchOne(ApplicableUcrCodes))
                    .Select(assoc => assoc.RelatedArrest)
                    .ToList();

                if (!leokaArrests.Any())
                    //Cannot score clearances if there are arrests, but none relate to killings or assaults
                    return;

                clearanceUcrCodes = leokaArrests
                    .Select(a => a.Charge.UcrCode)
                    .Distinct()
                    .ToArray(); //ToArray here in order for this variable to be used as a variadic parameter

                clearanceYearMonth = ConvertNibrsDateToDateKeyPrefix(leokaArrests.Min(a => a.Date.DateTime ?? a.Date.Date));
            }
            else
            {
                //No arrests means check if the incident is cleared
                if (report.Incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceDate == null || report.Incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceDate.Date == null)
                    //Incident is not cleared, no clearances to score
                    return;

                //Incident is cleared, use incident clearance date
                clearanceYearMonth = ConvertNibrsDateToDateKeyPrefix(report.Incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceDate.Date);

                //All offenses are cleared, so all ucr codes applicable for leoka data are fair game
                clearanceUcrCodes = ApplicableUcrCodes;
            }
            
            //Get leoka data
            var leokaVictims = report.OffenseVictimAssocs
                .Where(ov => ov.RelatedVictim.CategoryCode == VictimCategoryCode.LAW_ENFORCEMENT_OFFICER.NibrsCode() && ov.RelatedOffense.UcrCode.MatchOne(clearanceUcrCodes))
                .ToList();
            var officersOfReportingAgency = report.Officers
                .Where(leo => leo.Unit == null);
            var leokaWithEnforcement = leokaVictims
                .Join(officersOfReportingAgency, l => l.RelatedVictim.Person.Id, o => o.Person.Id, (v, o) => Tuple.Create(v.RelatedVictim, v.RelatedOffense, o))
                .ToList();

            //Create the ucr report key based off of the earliest arrest date
            var ucrReportKey = clearanceYearMonth + report.Header.ReportingAgency.OrgAugmentation.OrgOriId.Id;
            monthlyReportData.TryAdd(ucrReportKey, new ReportData());
            var leokaData = monthlyReportData[ucrReportKey].LeokaData;
            
            //Begin scoring clearances
            foreach (var leoka in leokaWithEnforcement)
                leokaData.ScoreClearanceCounts(leoka.Item3.ActivityCategoryCode);
        }
    }
}