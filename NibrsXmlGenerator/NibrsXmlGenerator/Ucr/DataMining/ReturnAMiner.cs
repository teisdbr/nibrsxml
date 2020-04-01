using System;
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
        public static readonly string[] ApplicableReturnAUcrCodes =
        {
            OffenseCode.MURDER_NONNEGLIGENT.NibrsCode(),
            OffenseCode.NEGLIGENT_MANSLAUGHTER.NibrsCode(),
            OffenseCode.RAPE.NibrsCode(),
            OffenseCode.SODOMY.NibrsCode(),
            OffenseCode.SEXUAL_ASSAULT_WITH_OBJECT.NibrsCode(),
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

        private static readonly Dictionary<string, string> ReturnAClearanceClassificationDictionary =
            new Dictionary<string, string>
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
                {"240", "7a"}
            };

        public ReturnAMiner(ConcurrentDictionary<string, ReportData> monthlyOriReportData, Report report) : base(
            monthlyOriReportData, report)
        {
            //All derived classes of GeneralSummaryMiner must implement this constructor that calls the base constructor.
            //No additional calls need to be made because the base constructor is making the appropriate calls already.
        }

        protected override string[] ApplicableUcrCodes
        {
            get { return ApplicableReturnAUcrCodes; }
        }

        protected override Dictionary<string, string> ClearanceClassificationDictionary
        {
            get { return ReturnAClearanceClassificationDictionary; }
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

                //--Get all weapons/forces of highest rated offense
                //--used for scoring robberies
                var highestRatedOffenseForces = applicableOffenses
                    .Where(o => o.UcrCode == highestRatedOffense.UcrCode)
                    .SelectMany(o => o.Forces.Select(f => f.CategoryCode));

                //--Selected victims related to highest rated offense
                var victimsOfMostImportantOffense = ucrHierarchyData.VictimsRelatedToHighestRatedOffense;

                //--Collect all stolen items except pending inventory and blank (88 and 99) because those should not be used for scoring
                // If the Report has multiple offense (eg- 13A and 220) and one is choosen as per the UCR hierarchy (13A) and that offense does not require items
                // todo: What to do with the items? I dont know, I am just recording all the items and let the furthur logic handle. 

                
                var stolenItems = report.Items.Where(
                    i => i.Status.Code == ItemStatusCode.STOLEN.NibrsCode() &&
                         i.NibrsPropertyCategoryCode != PropertyCategoryCode.PENDING_INVENTORY.NibrsCode() &&
                         i.NibrsPropertyCategoryCode != PropertyCategoryCode.BLANK.NibrsCode()
                ).ToList();
                var recoveredItems = report.Items.Where(
                    i => i.Status.Code == ItemStatusCode.RECOVERED.NibrsCode() &&
                         i.NibrsPropertyCategoryCode != PropertyCategoryCode.PENDING_INVENTORY.NibrsCode() &&
                         i.NibrsPropertyCategoryCode != PropertyCategoryCode.BLANK.NibrsCode()
                ).ToList();

                // Assault doesnot want to score the incidental items.
                if (highestRatedOffense.UcrCode.Matches("13[ABC]"))
                {
                    stolenItems.Clear();
                    recoveredItems.Clear();

                }

                //Report Offenses and Victims not considered for UCR Report.
                // ReSharper disable once UnusedVariable
                var ignoredOffenses = report.Offenses.Where(o => o.UcrCode != highestRatedOffense.UcrCode).ToList();
                // ReSharper disable once UnusedVariable
                var ignoredVictimAssociations =
                    report.OffenseVictimAssocs.Where(ov => ov.RelatedOffense.UcrCode != highestRatedOffense.UcrCode)
                        .ToList();

                #endregion

                monthlyOriReportData.TryAdd(report.UcrKey(), new ReportData());
                var returnA = monthlyOriReportData[report.UcrKey()].ReturnAData;

                //Assign the ReturnA to the ORI month and year key of the monthlyOriReportData dictionary.
                //Call one of the scoring functions per report.
                ScoreHighestRatedOffenseGroup(returnA, highestRatedOffense, victimsOfMostImportantOffense,
                    report.StolenVehicles, stolenItems, report.Incident.ActivityDate.Time, highestRatedOffenseForces);

                //Score recovered items for ReturnASupplement
                //These items cannot be scored the same as stolen items because these have a date associated with them.
                //They are scored to the appropriate report based on the recovery date.
                ScoreRecoveredItemsForSupplement(monthlyOriReportData, recoveredItems,
                    report.Header.ReportingAgency.OrgAugmentation.OrgOriId.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private static void ScoreHighestRatedOffenseGroup(
            ReturnA returnA,
            Offense highestRatedOffense,
            List<OffenseVictimAssociation> victimsOfMostImportantOffense,
            List<Item> stolenVehicles,
            List<Item> stolenItems,
            string incidentTime,
            IEnumerable<string> highestRatedOffenseForces,
            bool? doScoreColumn6 = null)
        {
            var totalStolenValue = stolenItems.TotalItemValue();
      

            switch (highestRatedOffense.UcrCode)
            {
                //Score Homicide Offense by counting victims via Data element 24 - Could be more than one per report.
                case "09A":
                case "09B":
                    returnA.ScoreHomicide(victimsOfMostImportantOffense, totalStolenValue, doScoreColumn6);
                    break;

                //Score Rape Offense by counting victims via Data element 24 - Could be more than one per report.
                case "11A":
                case "11B":
                case "11C":
                    returnA.ScoreRape(victimsOfMostImportantOffense, totalStolenValue, doScoreColumn6);
                    break;

                //Score Robberies via Data Element 6
                case "120":
                    returnA.ScoreRobbery(highestRatedOffense, totalStolenValue, highestRatedOffenseForces,
                        doScoreColumn6);
                    break;

                //Score Assault Offense by counting victims via Data element 24 - Could be more than one per report.
                case "13A":
                case "13B":
                case "13C":
                    returnA.ScoreAssault(victimsOfMostImportantOffense, doScoreColumn6);
                
                    break;

                //Score Burglary by counting Data Element 6 - At most one per report.
                case "220":
                    returnA.ScoreBurglary(highestRatedOffense, incidentTime, totalStolenValue, doScoreColumn6);
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
                    returnA.ScoreLarcenyThefts(highestRatedOffense, stolenItems, doScoreColumn6);
                    break;

                //Score Vehicle Offense by counting vehicle properties for 240 offense - Could be more than one per report.
                case "240":
                    returnA.ScoreVehicleTheft(highestRatedOffense, stolenVehicles, totalStolenValue, doScoreColumn6);
                    break;
                default:
                    return;
            }

          
                returnA.Supplement.ScoreStolenByTypeAndValue(stolenItems);
               
            }

        private static void ScoreRecoveredItemsForSupplement(
            ConcurrentDictionary<string, ReportData> monthlyOriReportData, List<Item> recoveredItems, string ori)
        {
            foreach (var item in recoveredItems)
            {
                var recoveryDate = ConvertNibrsDateToDateKeyPrefix(item.Value.ValueDate.Date);
                var ucrReportKey = recoveryDate + ori;
                monthlyOriReportData.TryAdd(ucrReportKey, new ReportData());
                monthlyOriReportData[ucrReportKey].ReturnASupplementData.ScoreRecoveredPropertyByTypeAndValue(item);
            }
        }

        #region Clearance Functions

        protected override void IncrementClearances(ConcurrentDictionary<string, ReportData> monthlyReportData,
            ClearanceDetails clearanceDetailsList)
        {
            monthlyReportData.TryAdd(clearanceDetailsList.UcrReportKey, new ReportData());
            monthlyReportData[clearanceDetailsList.UcrReportKey].ReturnAData
                .IncrementAllClearences(clearanceDetailsList.ClassificationKey,
                    clearanceDetailsList.AllScoresIncrementStep);
        }

        protected override void ScoreClearances(ConcurrentDictionary<string, ReportData> monthlyReportData,
            string ucrReportKey, Report fauxReport, bool doScoreColumn6)
        {
            monthlyReportData.TryAdd(ucrReportKey, new ReportData());
            ScoreHighestRatedOffenseGroup(
                monthlyReportData[ucrReportKey].ReturnAData,
                fauxReport.Offenses[0],
                fauxReport.OffenseVictimAssocs,
                fauxReport.Items, //ReturnAMiner.CreateFauxItems returns a list of stolen vehicles
                null,
                null,
                fauxReport.Offenses[0].Forces?.Select(f => f.CategoryCode),
                doScoreColumn6);
        }

        /// <summary>
        ///     If the ucrClearanceCode is of one of the offenses where victim data is not required
        /// </summary>
        /// <param name="report"></param>
        /// <param name="ucrClearanceCode"></param>
        /// <returns></returns>
        protected override Offense CreateFauxOffense(Report report, string ucrClearanceCode)
        {
            var clearedOffenses = report.Offenses.Where(o => o.UcrCode == ucrClearanceCode).ToArray();
            if (clearedOffenses.Any())
            {
                var clearanceOffense = clearedOffenses.First();

                if (ucrClearanceCode == OffenseCode.BURGLARY_BREAKING_AND_ENTERING.NibrsCode())
                {
                    //Additional logic to affix the returned offense to contain numbers for number of premises entered if any of burglaries were of rental storage facilities
                    //This is required because 1) location type & 2) number of prem. entered may affect the number of scores an offense can count towards
                    var rentalStorageFacilityBurglaries = clearedOffenses
                        .Where(o => o.Location.CategoryCode == LocationCategoryCode.RENTAL_STORAGE_FACILITY.NibrsCode())
                        .ToArray();

                    if (rentalStorageFacilityBurglaries.Any())
                    {
                        var totalPremisesEntered = rentalStorageFacilityBurglaries.Aggregate(0,
                            (total, offense) => Convert.ToInt32(offense.StructuresEnteredQuantity));
                        clearanceOffense.Location.CategoryCode =
                            LocationCategoryCode.RENTAL_STORAGE_FACILITY.NibrsCode();
                        clearanceOffense.StructuresEnteredQuantity = totalPremisesEntered.ToString();
                    }
                }

                if (ucrClearanceCode.MatchOne(OffenseCode.AGGRAVATED_ASSAULT.NibrsCode(),
                    OffenseCode.ROBBERY.NibrsCode()))
                    clearanceOffense.Forces = clearedOffenses
                        .Where(o => o.UcrCode == ucrClearanceCode)
                        .SelectMany(o => o.Forces)
                        .ToList();

                return clearanceOffense;
            }

            return null;
        }

        /// <summary>
        ///     If the ucrClearanceCode requires victim data, this returns the context of all offenses and victim data of that ucr
        ///     code
        /// </summary>
        /// <param name="report"></param>
        /// <param name="ucrClearanceCode"></param>
        /// <returns></returns>
        protected override List<OffenseVictimAssociation> CreateFauxOffenseVictimAssociations(Report report,
            string ucrClearanceCode)
        {
            if (!ucrClearanceCode.MatchOne(
                OffenseCode.MURDER_NONNEGLIGENT.NibrsCode(),
                OffenseCode.NEGLIGENT_MANSLAUGHTER.NibrsCode(),
                OffenseCode.RAPE.NibrsCode(),
                OffenseCode.SODOMY.NibrsCode(),
                OffenseCode.SEXUAL_ASSAULT_WITH_OBJECT.NibrsCode(),
                OffenseCode.AGGRAVATED_ASSAULT.NibrsCode(),
                OffenseCode.SIMPLE_ASSAULT.NibrsCode(),
                OffenseCode.INTIMIDATION.NibrsCode())) return null;

            var clearedOffVicAssocs = report.OffenseVictimAssocs
                .Where(ov => ov.RelatedOffense.UcrCode == ucrClearanceCode).ToList();

            return clearedOffVicAssocs.Any() ? clearedOffVicAssocs : null;
        }

        protected override List<Item> CreateFauxItems(Report report, string ucrClearanceCode)
        {
            return ucrClearanceCode == OffenseCode.MOTOR_VEHICLE_THEFT.NibrsCode()
                ? report.StolenVehicles
                : null;
        }

        #endregion
    }
}