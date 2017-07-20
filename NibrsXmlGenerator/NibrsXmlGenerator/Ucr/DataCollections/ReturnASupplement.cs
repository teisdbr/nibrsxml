using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Item;
using NibrsXml.Utility;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataCollections
{
    /// <summary>
    ///     Unlike Asre and GeneralSummaryData reports, the ReturnASupplement report defines both :
    ///     1) the structure to hold data,
    ///     2) the functions to manipulate that structure.
    ///     Note: Burglaries in our system will never be scored as unknown because the xml that is fed in will always have
    ///     incident date/time data
    /// </summary>
    public class ReturnASupplement
    {
        private const string PropertyByTypeAndValueTotalDataEntryKey = "00";

        public ReturnASupplement()
        {
            //Hard-coded entries of keys for each row of the Supplement report
            SupplementData = new Dictionary<string, Tuple<int, int>>
            {
                //Property by Type and Value
                {"01", Tuple.Create(0, 0)},
                {"02", Tuple.Create(0, 0)},
                {"03", Tuple.Create(0, 0)},
                {"04", Tuple.Create(0, 0)},
                {"05", Tuple.Create(0, 0)},
                {"06", Tuple.Create(0, 0)},
                {"07", Tuple.Create(0, 0)},
                {"08", Tuple.Create(0, 0)},
                {"09", Tuple.Create(0, 0)},
                {"10", Tuple.Create(0, 0)},
                {"11", Tuple.Create(0, 0)},
                {PropertyByTypeAndValueTotalDataEntryKey, Tuple.Create(0, 0)},

                //Property Stolen by Classification
                ////Murder and Nonnegligent Manslaughter
                {"12", Tuple.Create(0, 0)},

                ////Forcible Rape
                {"20", Tuple.Create(0, 0)},

                ////Robberies
                {"31", Tuple.Create(0, 0)},
                {"32", Tuple.Create(0, 0)},
                {"33", Tuple.Create(0, 0)},
                {"34", Tuple.Create(0, 0)},
                {"35", Tuple.Create(0, 0)},
                {"36", Tuple.Create(0, 0)},
                {"37", Tuple.Create(0, 0)},
                {"30", Tuple.Create(0, 0)},

                ////Burglaries
                {"51", Tuple.Create(0, 0)},
                {"52", Tuple.Create(0, 0)},
                {"53", Tuple.Create(0, 0)},
                {"54", Tuple.Create(0, 0)},
                {"55", Tuple.Create(0, 0)},
                {"56", Tuple.Create(0, 0)},
                {"50", Tuple.Create(0, 0)},

                ////Larcenies by Stolen Value
                {"61", Tuple.Create(0, 0)},
                {"62", Tuple.Create(0, 0)},
                {"63", Tuple.Create(0, 0)},
                {"60", Tuple.Create(0, 0)},

                ////Motor Vehicle Theft
                {"70", Tuple.Create(0, 0)},

                ////Grand Total
                {"77", Tuple.Create(0, 0)},

                ////Nature of Larcenies
                {"81", Tuple.Create(0, 0)},
                {"82", Tuple.Create(0, 0)},
                {"83", Tuple.Create(0, 0)},
                {"84", Tuple.Create(0, 0)},
                {"85", Tuple.Create(0, 0)},
                {"86", Tuple.Create(0, 0)},
                {"87", Tuple.Create(0, 0)},
                {"88", Tuple.Create(0, 0)},
                {"89", Tuple.Create(0, 0)},
                {"80", Tuple.Create(0, 0)},

                ////Motor Vehicles Recovered
                {"91", Tuple.Create(0, 0)},
                {"92", Tuple.Create(0, 0)},
                {"90", Tuple.Create(0, 0)},
                {"93", Tuple.Create(0, 0)}
            };
        }

        /// <summary>
        ///     This is the main dictionary that holds all supplementary data.
        ///     The properties PropertyByTypeAndValue and PropertyStolenByClassification simply extract the appropriate
        ///     data from this dictionary.
        /// </summary>
        private Dictionary<string, Tuple<int, int>> SupplementData { get; set; }

        public List<Tuple<string, int, int>> PropertyByTypeAndValue
        {
            get
            {
                return SupplementData
                    .Where(kvp => kvp.Key.MatchOne("01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", PropertyByTypeAndValueTotalDataEntryKey))
                    .Select(kvp => Tuple.Create(kvp.Key, kvp.Value.Item1, kvp.Value.Item2))
                    .ToList();
            }
        }

        public List<Tuple<string, int, int>> PropertyStolenByClassification
        {
            get
            {
                return SupplementData
                    .Where(
                        kvp =>
                            kvp.Key.MatchOne("12", "20", "31", "32", "33", "34", "35", "36", "37", "30", "51", "52", "53", "54", "55", "56", "50", "61", "62", "63", "60", "70", "77", "81", "82", "83",
                                "84", "85",
                                "86", "87", "88", "89", "80", "91", "92", "90", "93"))
                    .Select(kvp => Tuple.Create(kvp.Key, kvp.Value.Item1, kvp.Value.Item2))
                    .ToList();
            }
        }

        public XDocument Serialize()
        {
            return new XDocument(
                new XProcessingInstruction(
                    "xml-stylesheet",
                    "type=\"text/xsl\" href=\"returnasupp.xsl\""),
                new XElement("ReturnASupplement",
                    new XElement("StolenAndRecoveredProperties",
                        PropertyByTypeAndValue.Select(tuple => new XElement("Property",
                            new XAttribute("entry", tuple.Item1),
                            tuple.Item2 == 0 ? null : new XElement("Stolen", tuple.Item2),
                            tuple.Item3 == 0 ? null : new XElement("Recovered", tuple.Item3)))),
                    new XElement("StolenPropertiesByClassification",
                        PropertyStolenByClassification.Select(tuple => new XElement("OffenseClassification",
                            new XAttribute("entry", tuple.Item1),
                            tuple.Item2 == 0 ? null : new XElement("Actual", tuple.Item2),
                            tuple.Item3 == 0 ? null : new XElement("Stolen", tuple.Item3))))));
        }

        private void IncrementValues(string dataEntryKey, int a, int b)
        {
            SupplementData[dataEntryKey] = Tuple.Create(SupplementData[dataEntryKey].Item1 + a, SupplementData[dataEntryKey].Item2 + b);
        }

        #region Stolen and Recovered Property by Type and Value

        public void ScoreRecoveredPropertyByTypeAndValue(Item recoveredItem)
        {
            //Recovered vehicles has its own entry that totals the number of recovered vehicles
            //This entry is in the section for PropertyStolenByClassification, but is irrelevant to offenses (namely 240s) and property loss type 7 (stolen)
            if (recoveredItem.NibrsPropertyCategoryCode.MatchOne(NibrsCodeGroups.VehicleProperties))
                IncrementValues("90", 1, 0);


            //Score Recovered by Type and Value
            var propertyTypeDataEntryKey = GetPropertyTypeDataEntryKey(recoveredItem);

            if (propertyTypeDataEntryKey == null)
                return;

            var itemValue = Convert.ToInt32(recoveredItem.Value.ValueAmount.Amount);

            IncrementValues(propertyTypeDataEntryKey, 0, itemValue);
            IncrementValues(PropertyByTypeAndValueTotalDataEntryKey, 0, itemValue);
        }

        public void ScoreStolenByTypeAndValue(List<Item> stolenItems)
        {
            if (stolenItems == null)
                return;

            foreach (var item in stolenItems)
            {
                var propertyTypeDataEntryKey = GetPropertyTypeDataEntryKey(item);

                if (propertyTypeDataEntryKey == null)
                    continue;

                var itemValue = Convert.ToInt32(item.Value.ValueAmount.Amount);

                //Score Type and Value
                IncrementValues(propertyTypeDataEntryKey, itemValue, 0);
                IncrementValues(PropertyByTypeAndValueTotalDataEntryKey, itemValue, 0);
            }
        }

        private static string GetPropertyTypeDataEntryKey(Item item)
        {
            switch (item.NibrsPropertyCategoryCode)
            {
                //Current, Notes, Etc.
                case "20":
                case "21":
                    return "01";

                //Jewelry and Precious Metals
                case "17":
                    return "02";

                //Clothing and Furs
                case "06":
                case "25":
                    return "03";

                //Locally Stolen Motor Vehicles
                case "03":
                case "05":
                case "24":
                case "28":
                case "37":
                    return "04";

                //Office Equipment
                case "07":
                case "23":
                    return "05";

                //Televisions, Radios, Stereos, Etc.
                case "26":
                case "27":
                case "74":
                    return "06";

                //Firearms
                case "13":
                    return "07";

                //Household Goods
                case "16":
                    return "08";

                //Consumable Goods
                case "02":
                case "08":
                case "10":
                case "47":
                case "64":
                    return "09";

                //Livestock
                case "18":
                    return "10";

                //!! Not handled in current NIBRS conversion
                case "88":
                case "99":
                    return null;

                //Miscellaneous
                default:
                    return "11";
            }
        }

        #endregion

        #region Property Stolen By Classification

        public void IncrementHomicide(int murderCount, int totalStolenValue)
        {
            IncrementValues("12", murderCount, totalStolenValue);
        }

        public void IncrementRape(int rapeCount, int totalStolenValue)
        {
            IncrementValues("20", rapeCount, totalStolenValue);
        }

        public void IncrementRobbery(string locationCategoryCode, int totalStolenValue)
        {
            switch (locationCategoryCode)
            {
                //Robbery - Highway (Streets, Alleys, Etc.)
                case "13":
                    IncrementValues("31", 1, totalStolenValue);
                    break;

                //Robbery - Commercial House (Except 33 and 34)
                case "03":
                case "05":
                case "08":
                case "09":
                case "12":
                case "14":
                case "17":
                case "21":
                case "24":
                    IncrementValues("32", 1, totalStolenValue);
                    break;

                //Robbery - Gas or Service Station
                case "23":
                    IncrementValues("33", 1, totalStolenValue);
                    break;

                //Robbery - Convenience Store
                case "07":
                    IncrementValues("34", 1, totalStolenValue);
                    break;

                //Robbery - Residence (Anywhere on Premises)
                case "20":
                    IncrementValues("35", 1, totalStolenValue);
                    break;

                //Robbery - Bank
                case "02":
                    IncrementValues("36", 1, totalStolenValue);
                    break;

                //Robbery - Miscellaneous
                default:
                    IncrementValues("37", 1, totalStolenValue);
                    break;
            }

            IncrementValues("30", 1, totalStolenValue);
        }

        public void IncrementBurglary(string incidentTime, string locationType, int premisesEnteredOrDefault, int totalStolenValue)
        {
            //night == 6pm to 6am, day == 6am to 6pm
            //51 - residence, night
            //52 - residence, day
            //53 - residence, unknown
            //54 - non-residence, night
            //55 - non-residence, day
            //56 - non-residence, unknown
            //From the nibrs data that we collect, we always have the incident hour, therefore 53 and 56 will never be incremented.
           var incidentHour = Convert.ToInt32(incidentTime.Substring(0, 2));
            var incidentOccuredDuringNight = incidentHour < 6 || incidentHour >= 18;
            if (locationType == LocationCategoryCode.RESIDENCE_HOME.NibrsCode())
                IncrementValues(
                    incidentOccuredDuringNight ? "51" : "52",
                    premisesEnteredOrDefault,
                    totalStolenValue);
            else
                IncrementValues(
                    incidentOccuredDuringNight ? "54" : "55",
                    premisesEnteredOrDefault,
                    totalStolenValue);
            IncrementValues("50", premisesEnteredOrDefault, totalStolenValue);
        }

        public void IncrementLarceny(string offenseUcrCode, List<Item> stolenItems)
        {
            //Even when stolenItems.Count == 0, this function must continue to process because it still needs to increment the Actual column's value
            //since this function is called in conjunction with the ReturnA larceny incrementer.

            //There are two sections for larceny based off of different criteria.
            //Both sections also have their own subtotals which should always be equal to each other.

            var totalStolenValue = stolenItems.TotalItemValue();


            //Larceny Section
            if (totalStolenValue < 50)
                IncrementValues("63", 1, totalStolenValue);
            else if (totalStolenValue < 200)
                IncrementValues("62", 1, totalStolenValue);
            else
                IncrementValues("61", 1, totalStolenValue);
            IncrementValues("60", 1, totalStolenValue);


            //Nature of Larceny Section
            //The following provides the algorithm for applying the Larceny Hierarchy for scoring larceny data into the ReturnASupplement
            switch (offenseUcrCode)
            {
                case "23A":
                    IncrementValues("81", 1, totalStolenValue);
                    break;

                case "23B":
                    IncrementValues("82", 1, totalStolenValue);
                    break;

                case "23C":
                    IncrementValues("83", 1, totalStolenValue);
                    break;

                case "23D":
                    IncrementValues("87", 1, totalStolenValue);
                    break;

                case "23E":
                    IncrementValues("88", 1, totalStolenValue);
                    break;

                case "23F":
                    IncrementValues("84", 1, totalStolenValue);
                    break;

                case "23G":
                    IncrementValues("85", 1, totalStolenValue);
                    break;

                case "23H":
                    var highestValuedProperty = stolenItems.Max();
                    var highestValuedPropertyType = highestValuedProperty == null
                        ? null
                        : highestValuedProperty.NibrsPropertyCategoryCode;
                    switch (highestValuedPropertyType)
                    {
                        case "38":
                            IncrementValues("85", 1, totalStolenValue);
                            break;

                        case "04":
                            IncrementValues("86", 1, totalStolenValue);
                            break;

                        default:
                            IncrementValues("89", 1, totalStolenValue);
                            break;
                    }
                    break;

                default:
                    return;
            }

            IncrementValues("80", 1, totalStolenValue);
        }

        public void IncrementVehicleTheft(int vehiclesStolen, int totalStolenValue)
        {
            IncrementValues("70", vehiclesStolen, totalStolenValue);
        }

        #endregion
    }
}