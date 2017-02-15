using System;
using System.Collections.Concurrent;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NibrsXml.Constants;
using NibrsXml.NibrsReport;
using NibrsXml.NibrsReport.Associations;
using NibrsXml.NibrsReport.Item;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.Ucr.DataCollections;
using NibrsXml.Utility;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataMining
{
    internal class ReturnAMiner : GeneralSummaryMiner
    {
        private static readonly Dictionary<string, string> ReturnAClearanceClassificationDictionary = new Dictionary<string, string>
        {
            {"09A", "1a"},
            {"09B", "1b"},
            {"11A", "2a"},
            {"120", "3d"},
            {"13A", "4d"},
            {"13B", "4e"},
            {"13C", "4e"},
            {"220", "5a"},
            {"23A", "6"},
            {"23B", "6"},
            {"23C", "6"},
            {"23D", "6"},
            {"23E", "6"},
            {"23F", "6"},
            {"23G", "6"},
            {"23H", "6"},
            {"240", "7a"},
            {"240", "7a"},
            {"240", "7a"}
        };

        public ReturnAMiner(ConcurrentDictionary<string, ReportData> monthlyOriReportData, Report report) : base(monthlyOriReportData, report)
        {
            //All derived classes of GeneralSummaryMiner must implement this constructor that calls the base constructor.
            //No additional calls need to be made because the base constructor is making the appropriate calls already.
        }

        protected override void Mine(ConcurrentDictionary<string, ReportData> monthlyOriReportData, Report report)
        {
            try
            {
                #region Hierarchy Data Determination

                //UCR Hierarchy Rules to return only the offense to score.
                //--Select all offenses that match any of the UCR reportable offenses
                var applicableOffenses =
                    report.Offenses.Where(o => o.UcrCode.MatchOne(UcrHierarchyMiner.UcrHierarchyOrderArray.ToArray()))
                        .ToList();
                //--Gather data based on highest rated offense
                var ucrHierarchyData = new UcrHierarchyMiner(applicableOffenses, report.OffenseVictimAssocs);
                //--Select the highest rated offense
                var highestRatedOffense = ucrHierarchyData.HighestRatedOffense;
                //--Selected victims related to highest rated offense
                var victimsOfMostImportantOffense = ucrHierarchyData.VictimsRelatedToHighestRatedOffense;

                //Report Offenses and Victims not considered for UCR Report.
                var ignoredOffenses = report.Offenses.Where(o => o.UcrCode != highestRatedOffense.UcrCode).ToList();
                var ignoredVictimAssociations =
                    report.OffenseVictimAssocs.Where(ov => ov.RelatedOffense.UcrCode != highestRatedOffense.UcrCode).ToList();

                #endregion

                var returnA = monthlyOriReportData[report.UcrKey].ReturnAData ?? new ReturnA();

                //Assign the ReturnA to the ORI month and year key of the monthlyOriReportData dictionary.
                //Call one of the scoring functions per report.
                ScoreHighestRatedOffenseGroup(returnA, highestRatedOffense, victimsOfMostImportantOffense, report.Items);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected override Dictionary<string, string> ClearanceClassificationDictionary
        {
            get { return ReturnAClearanceClassificationDictionary; }
        }
        private static void ScoreHighestRatedOffenseGroup(ReturnA returnA, Offense highestRatedOffense, List<OffenseVictimAssociation> victimsOfMostImportantOffense, List<Item> items)
        {
            
            switch (highestRatedOffense.UcrCode)
            {
                //Score Homicide Offense by counting victims via Data element 24 - Could be more than one per report.
                case "09A":
                case "09B":
                    returnA.ScoreHomicide(victimsOfMostImportantOffense);
                    break;

                //Score Rape Offense by counting victims via Data element 24 - Could be more than one per report.
                case "11A":
                    returnA.ScoreRape(victimsOfMostImportantOffense);
                    break;

                //Score Robberies via Data Element 6
                case "120":
                    returnA.ScoreRobbery(highestRatedOffense);
                    break;

                //Score Assault Offense by counting victims via Data element 24 - Could be more than one per report.
                case "13A":
                case "13B":
                case "13C":
                    returnA.ScoreAssault(victimsOfMostImportantOffense);
                    break;

                //Score Burglary by counting Data Element 6 - At most one per report.
                case "220":
                    returnA.ScoreBurglary(highestRatedOffense);
                    break;

                //Score Larceny by counting Data Element 6
                case "23A":
                case "23B":
                case "23C":
                case "23D":
                case "23E":
                case "23F":
                case "23G":
                case "23H":
                    returnA.ScoreLarcenyThefts(highestRatedOffense);
                    break;

                //Score Vehicle Offense by counting vehicle properties for 240 offense - Could be more than one per report.
                case "240":
                    returnA.ScoreVehicleTheft(highestRatedOffense,
                        items.Where(i => i.NibrsPropertyCategoryCode.MatchOne(NibrsCodeGroups.VehicleProperties) && i.Status.Code == ItemStatusCode.STOLEN.NibrsCode()).ToList());
                    break;
                default:
                    break;
            }
        }

        protected override void IncrementClearances(ConcurrentDictionary<string, ReportData> monthlyReportData, ClearanceDetails clearanceDetailsList)
        {
            monthlyReportData.TryAdd(clearanceDetailsList.UcrReportKey, new ReportData());
            monthlyReportData[clearanceDetailsList.UcrReportKey].ReturnAData.IncrementAllClearences(clearanceDetailsList.ClassificationKey, clearanceDetailsList.AllScoresIncrementStep);
        }
    }
}