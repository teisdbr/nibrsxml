using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NibrsXml.Constants;
using NibrsXml.Constants.Ucr;
using NibrsXml.NibrsReport;
using NibrsXml.NibrsReport.Item;
using NibrsXml.Ucr.DataCollections;
using NibrsXml.Utility;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataMining
{
    internal class ArsonMiner : GeneralSummaryMiner
    {
        private static readonly string[] ApplicableArsonUcrCodes = {OffenseCode.ARSON.NibrsCode()};

        private static readonly Dictionary<string, string> ArsonClearanceClassificationDictionary = new Dictionary<string, string>
        {
            {"200A", "A"},
            {"200B", "B"},
            {"200C", "C"},
            {"200D", "D"},
            {"200E", "E"},
            {"200F", "F"},
            {"200G", "G"},
            {"200H", "H"},
            {"200I", "I"},
            {"200J", "J"}
        };

        public ArsonMiner(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report) : base(monthlyReportData, report)
        {
            //All derived classes of GeneralSummaryMiner must implement this constructor that calls the base constructor.
            //No additional calls need to be made because the base constructor is making the appropriate calls already.
        }

        protected override string[] ApplicableUcrCodes
        {
            get { return ApplicableArsonUcrCodes; }
        }

        protected override Dictionary<string, string> ClearanceClassificationDictionary
        {
            get
            {
                return ArsonClearanceClassificationDictionary;
            }
        }

        protected override void IncrementClearances(ConcurrentDictionary<string, ReportData> monthlyReportData, ClearanceDetails clearanceDetailsList)
        {
            monthlyReportData.TryAdd(clearanceDetailsList.UcrReportKey, new ReportData());
            monthlyReportData[clearanceDetailsList.UcrReportKey].ArsonData.IncrementAllClearences(clearanceDetailsList.ClassificationKey, clearanceDetailsList.AllScoresIncrementStep);
        }

        protected override void Mine(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report)
        {
            // Return if no arson data to query
            if (!report.Offenses.Any(offense => offense.UcrCode.Matches(OffenseCode.ARSON.NibrsCode())) || report.Items.All(i => i.Status.Code != ItemStatusCode.BURNED.NibrsCode()))
                return;

            //Get Classification Counts to operate on for this report
            var arsonData = monthlyReportData[report.UcrKey()].ArsonData;

            //Determine property to use based on ucr hierarchy: structure -> mobile -> other
            var selectedProperty = GetPropertyToUse(report.Items.Where(i => i.Status.Code == ItemStatusCode.BURNED.NibrsCode()).ToList());
            if (selectedProperty == null) return;

            //Identify key to use based on identified property and score actual counts
            var keyForScoring = GetPropertyClassification(selectedProperty.NibrsPropertyCategoryCode);

            #region Column 4

            //Use the corresponding key to increment the count of actual offenses.
            arsonData.IncrementActualOffense(keyForScoring);

            #endregion

            //Columns 5 and 6 (All/Juvenile Clearances) will be handled by GeneralSummaryMiner

            //Column 7 is not available in NIBRS per the documentation. This needs to be verified given that other data in a similar fashion has since been 
            //been incorporated into NIBRS.

            #region Column 8

            //Add value of all properties stolen regardless of description, priorities, etc. It may not be necessary to filter only by properties that apply.
            var totalValueOfBurnedProperties = report.Items.Sum(item => Convert.ToInt64(item.Value.ValueAmount.Amount));
            arsonData.IncrementEstimatedValueOfPropertyDamage(keyForScoring, totalValueOfBurnedProperties);

            #endregion
        }

        /// <summary>
        ///     Takes in a list of items, determines which of the items to use, then extracts the classification from that property
        ///     Property classification is synonymous to the row header of the arson report.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string GetPropertyClassification(List<Item> items)
        {
            var selectedProperty = GetPropertyToUse(items.Where(i => i.Status.Code == ItemStatusCode.BURNED.NibrsCode()).ToList());
            return selectedProperty == null ? null : GetPropertyClassification(selectedProperty.NibrsPropertyCategoryCode);
        }

        /// <summary>
        ///     Translates the property classification from a property description.
        ///     Property classification is synonymous to the row header of the arson report.
        /// </summary>
        /// <param name="propertyDescription"></param>
        /// <returns></returns>
        private static string GetPropertyClassification(string propertyDescription)
        {
            switch (propertyDescription)
            {
                //Structure Properties
                case "29":
                    return "A";
                case "30":
                    return "B";
                case "34":
                    return "C";
                case "32":
                    return "D";
                case "31":
                    return "E";
                case "33":
                    return "F";
                case "35":
                    return "G";
                //Motor Vehicle Properties
                case "03":
                case "05":
                case "24":
                case "37":
                    return "H";
                //Other Mobile Properties
                case "01":
                case "12":
                case "15":
                case "28":
                case "39":
                case "78":
                    return "I";
                //Total Other Properties
                case "02":
                case "04":
                case "06":
                case "07":
                case "08":
                case "09":
                case "10":
                case "11":
                case "13":
                case "14":
                case "16":
                case "17":
                case "18":
                case "19":
                case "20":
                case "21":
                case "22":
                case "23":
                case "25":
                case "26":
                case "27":
                case "36":
                case "38":
                case "41":
                case "42":
                case "43":
                case "44":
                case "45":
                case "46":
                case "47":
                case "48":
                case "49":
                case "59":
                case "64":
                case "65":
                case "66":
                case "67":
                case "68":
                case "69":
                case "70":
                case "71":
                case "72":
                case "73":
                case "74":
                case "75":
                case "76":
                case "77":
                case "79":
                case "80":
                    return "J";
                default:
                    return "J";
            }
        }

        /// <summary>
        ///     It returns a property based on the highest value and hierarchical requirements.
        /// </summary>
        private static Item GetPropertyToUse(List<Item> burnedItems)
        {
            //Get at most one property per offense.
            //----Gather only one structure property with the highest value. Given OrderBy sorts in ascending, Last() provides desired property.
            var structureProperty =
                burnedItems.Where(i => i.NibrsPropertyCategoryCode.MatchOne(UcrCodeGroups.StructureProperties)).Max();
            //--------If structureProperty is not null, return it
            if (structureProperty != null) return structureProperty;

            //----Gather only one vehicle as above.
            var vehicleProperty =
                burnedItems.Where(i => i.NibrsPropertyCategoryCode.MatchOne(NibrsCodeGroups.VehicleProperties)).Max();

            //--------If structureProperty is not null, return it
            if (vehicleProperty != null) return vehicleProperty;

            //----Gather all other mobile properties
            var otherMobileProperty =
                burnedItems.Where(i => i.NibrsPropertyCategoryCode.MatchOne(UcrCodeGroups.OtherMobileProperties)).Max();

            //--------If structureProperty is not null, return it
            if (otherMobileProperty != null) return otherMobileProperty;

            //----Gather Total Other Property
            var totalOtherProperty = burnedItems.FirstOrDefault(i => i.NibrsPropertyCategoryCode.MatchOne(UcrCodeGroups.TotalOtherProperties));

            //--------Return Total other property or null if code reached this point.
            return totalOtherProperty;
        }

        protected override void ScoreClearances(ConcurrentDictionary<string, ReportData> monthlyReportData, string ucrReportKey, Report fauxReport, bool doScoreColumn6)
        {
            //This is the data that pertains to the report of the clearance date of the arrest/incident
            monthlyReportData.TryAdd(ucrReportKey, new ReportData());
            var arsonData = monthlyReportData[ucrReportKey].ArsonData;
            var keyForScoring = GetPropertyClassification(fauxReport.Items);
            arsonData.IncrementAllClearences(keyForScoring, 1, doScoreColumn6);
        }

        protected override List<Item> CreateFauxItems(Report report, string ucrClearanceCode)
        {
            return report.Items.Where(i => i.Status.Code == ItemStatusCode.BURNED.NibrsCode()).ToList();
        }
    }
}