using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NibrsXml.Constants;
using NibrsXml.NibrsReport;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.Ucr.DataCollections;
using NibrsXml.Utility;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataMining
{
    internal class LeokaMiner : GeneralSummaryMiner
    {
        protected override string[] ApplicableUcrCodes
        {
            get { return new[] {"09A", "09B", "13A", "13B"}; }
        }

        private string ExtractLeokaWeapons(List<OffenseForce> offenseForces)
        {
            return Convert.ToChar(Encoding.ASCII.GetBytes(offenseForces.ExtractWeaponGroup()).First() + 1).ToString().ToUpper();
        }

        public LeokaMiner(ConcurrentDictionary<string, ReportData> monthlyOriReportData, Report report) : base(monthlyOriReportData, report)
        {
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
                var leokaWithEnforcement = leokaVictims.Join(report.Officers, l => l.RelatedVictim.Person.Id, o => o.Person.Id,
                    (v, o) => Tuple.Create(v.RelatedVictim, v.RelatedOffense, o)).ToList();

                //--Get Officers Killed Feloneously
                leoka.OfficersKilled.IncrementCount("09A", leokaWithEnforcement.Count(v => v.Item2.UcrCode == "09A"));
                //--Get Officers Killed By Accident
                leoka.OfficersKilled.IncrementCount("09B", leokaWithEnforcement.Count(v => v.Item2.UcrCode == "09B"));

                //********************************************************************************************Get Officers Assaulted Information
                foreach (var tuple in leokaWithEnforcement.Where(l => l.Item2.UcrCode.MatchOne("13A", "13B")).ToList())
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

        protected override void ScoreClearances(ConcurrentDictionary<string, ReportData> monthlyReportData, string ucrReportKey, Report fauxReport, bool doScoreColumn6)
        {
            throw new NotImplementedException();
        }

        protected override Dictionary<string, string> ClearanceClassificationDictionary
        {
            get { throw new NotImplementedException(); }
        }

        protected override void IncrementClearances(ConcurrentDictionary<string, ReportData> monthlyReportData, GeneralSummaryMiner.ClearanceDetails clearanceDetailsList)
        {
            throw new NotImplementedException();
        }
    }
}