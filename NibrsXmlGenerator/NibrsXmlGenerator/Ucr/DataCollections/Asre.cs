using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NibrsXml.Constants;
using NibrsXml.Constants.Ucr;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.NibrsReport.Substance;
using NibrsXml.Utility;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataCollections
{
    public class Asre : Data
    {
        public Asre()
        {
            OffenseToAsreCounts = new Dictionary<string, AsreCounts>
            {
                {"01A", new AsreCounts()},
                {"01B", new AsreCounts()},
                {"02b", new AsreCounts()},
                {"03b", new AsreCounts()},
                {"04b", new AsreCounts()},
                {"05b", new AsreCounts()},
                {"06b", new AsreCounts()},
                {"07b", new AsreCounts()},
                {"08b", new AsreCounts()},
                {"09b", new AsreCounts()},
                {"10b", new AsreCounts()},
                {"11b", new AsreCounts()},
                {"12b", new AsreCounts()},
                {"13b", new AsreCounts()},
                {"14b", new AsreCounts()},
                {"15b", new AsreCounts()},
                {"16b", new AsreCounts()},
                {"16A", new AsreCounts()},
                {"16B", new AsreCounts()},
                {"16C", new AsreCounts()},
                {"17b", new AsreCounts()},
                {"18b", new AsreCounts()},
                {"180", new AsreCounts()},
                {"18A", new AsreCounts()},
                {"18B", new AsreCounts()},
                {"18C", new AsreCounts()},
                {"18D", new AsreCounts()},
                {"185", new AsreCounts()},
                {"18E", new AsreCounts()},
                {"18F", new AsreCounts()},
                {"18G", new AsreCounts()},
                {"18H", new AsreCounts()},
                {"19b", new AsreCounts()},
                {"19A", new AsreCounts()},
                {"19B", new AsreCounts()},
                {"19C", new AsreCounts()},
                {"20b", new AsreCounts()},
                {"21b", new AsreCounts()},
                {"22b", new AsreCounts()},
                {"23b", new AsreCounts()},
                {"24b", new AsreCounts()},
                {"25b", new AsreCounts()},
                {"26b", new AsreCounts()},
                {"27b", new AsreCounts()},
                {"28b", new AsreCounts()},
                {"29b", new AsreCounts()},
                {"30b", new AsreCounts()},
                {"31b", new AsreCounts()}
            };

            JuvenileDispositionDepartment = 0;
            JuvenileDispositionCourt = 0;
            JuvenileDispositionWelfareAgency = 0;
            JuvenileDispositionOtherPoliceAgency = 0;
            JuvenileDispositionCriminalOrAdultCourt = 0;
            JuvenileDispositionTotal = 0;
        }

        private Dictionary<string, AsreCounts> OffenseToAsreCounts { get; set; }
        private int JuvenileDispositionDepartment { get; set; }
        private int JuvenileDispositionCourt { get; set; }
        private int JuvenileDispositionWelfareAgency { get; set; }
        private int JuvenileDispositionOtherPoliceAgency { get; set; }
        private int JuvenileDispositionCriminalOrAdultCourt { get; set; }
        private int JuvenileDispositionTotal { get; set; }

        /* Remove getters and aggregation functions?
        #region Getters
        // Get individual counts
        public int GetAgeSexCount(string offenseUcrCode, string age, string sex)
        {
            return OffenseToAsreCounts.ContainsKey(offenseUcrCode) ? OffenseToAsreCounts[offenseUcrCode].GetAgeSexCount(age, sex) : 0;
        }

        public int GetRaceCount(string offenseUcrCode, string race)
        {
            return OffenseToAsreCounts.ContainsKey(offenseUcrCode) ? OffenseToAsreCounts[offenseUcrCode].GetRaceCount(race) : 0;
        }

        public int GetEthnicityCount(string offenseUcrCode, string ethnicity)
        {
            return OffenseToAsreCounts.ContainsKey(offenseUcrCode) ? OffenseToAsreCounts[offenseUcrCode].GetEthnicityCount(ethnicity) : 0;
        }
        #endregion

        #region Aggregate Functions
        // Get subtotals
        public int GetAgeTotal(string age)
        {
            return OffenseToAsreCounts.Keys.Aggregate(0, (total, offense) => total + OffenseToAsreCounts[offense].GetAgeTotal(age));
        }

        public int GetOffenseSexTotal(string offenseUcrCode, string sex)
        {
            return OffenseToAsreCounts.ContainsKey(offenseUcrCode) ? OffenseToAsreCounts[offenseUcrCode].GetSexTotal(sex) : 0;
        }

        public int GetSexTotal(string sex)
        {
            return OffenseToAsreCounts.Keys.Aggregate(0, (total, offense) => total + OffenseToAsreCounts[offense].GetSexTotal(sex));
        }

        public int GetRaceTotal(string race)
        {
            return OffenseToAsreCounts.Keys.Aggregate(0, (total, offense) => total + (OffenseToAsreCounts[offense].GetRaceCount(race)));
        }

        public int GetEthnicityTotal(string ethnicity)
        {
            return OffenseToAsreCounts.Keys.Aggregate(0, (total, offense) => total + (OffenseToAsreCounts[offense].GetEthnicityCount(ethnicity)));
        }

        // Get grandtotal
        public int GetGrandTotal(string age)
        {
            return OffenseToAsreCounts.Keys.Aggregate(0, (total, offense) => total + OffenseToAsreCounts[offense].TotalAdultCount);
        }
        #endregion
        */

        public XDocument Serialize()
        {
            return new XDocument(
                new XProcessingInstruction(
                    "xml-stylesheet",
                    "type=\"text/xsl\" href=\"asre.xsl\""),
                new XElement(
                    "ASRSummary",
                    OffenseToAsreCounts.Select(offenseToCountsPair =>
                        new XElement(
                            "UCR",
                            new XAttribute("value", offenseToCountsPair.Key),
                            offenseToCountsPair.Value.Serialize())),
                    new XElement(
                        "JuvenileDispositions",
                        new XElement("Department", JuvenileDispositionDepartment),
                        new XElement("Court", JuvenileDispositionCourt),
                        new XElement("WelfareAgency", JuvenileDispositionWelfareAgency),
                        new XElement("OtherPoliceAgency", JuvenileDispositionOtherPoliceAgency),
                        new XElement("CriminalOrAdultCourt", JuvenileDispositionCriminalOrAdultCourt),
                        new XElement("Total", JuvenileDispositionTotal))));
        }

        private void AddCounts(string age, string sex, string race, string ethnicity, params string[] offenseClassificationKeys)
        {
            foreach (var key in offenseClassificationKeys)
                OffenseToAsreCounts.TryAdd(key).AddCounts(age, sex, race, ethnicity);
        }

        public void AddJuvenileDispositionCounts(string juvenileDispositionCode)
        {
            switch (juvenileDispositionCode)
            {
                case "C":
                    JuvenileDispositionCriminalOrAdultCourt++;
                    break;
                case "H":
                    JuvenileDispositionDepartment++;
                    break;
                case "J":
                    JuvenileDispositionCourt++;
                    break;
                case "R":
                    JuvenileDispositionOtherPoliceAgency++;
                    break;
                case "w":
                    JuvenileDispositionWelfareAgency++;
                    break;
                default:
                    return;
            }

            JuvenileDispositionTotal++;
        }

        public void AddDrugOffenseCounts(List<Substance> drugs, List<Offense> drugOffenses, string age, string sex, string race, string ethnicity)
        {
            //Always increment the totals. Since more specific data is not always provided, at least the scores for totals will be more accurate
            AddCounts(age, sex, race, ethnicity, "18b");

            if (drugs.IsNothingOrEmpty() || drugOffenses.IsNothingOrEmpty())
                return;

            //Criminal Activity Hierarchy is as follows
            //1: Sale/Manufacturing(NibrsCodeGroups.SaleOrManufacturingCriminalActivities)
            //2: Possession(NibrsCodeGroups.PossessionCriminalActivities)
            var criminalActivityTypes = drugOffenses.SelectMany(o => o.CriminalActivityCategoryCodes).Where(criminalActivityCode => criminalActivityCode != null).ToArray();
            var mostDangerousCriminalActivityType = criminalActivityTypes.Any(code => code.MatchOne(UcrCodeGroups.SaleOrManufacturingCriminalActivities))
                ? 1
                : criminalActivityTypes.Any(code => code.MatchOne(UcrCodeGroups.PossessionCriminalActivities))
                    ? 2
                    : 0;
            if (mostDangerousCriminalActivityType == 0)
                return;

            //Drug Category Hierarchy is as follows
            //1: NibrsCodeGroups.OpiumCocaineAndDerivedDrugs
            //2: NibrsCodeGroups.OtherDangerousNonnarcoticDrugs
            //3: NibrsCodeGroups.SyntheticNarcotics
            //4: NibrsCodeGroups.Marijuana
            var mostDangerousSuspectedDrugType = drugs.Any(d => d.DrugCategoryCode.MatchOne(UcrCodeGroups.OpiumCocaineAndDerivedDrugs))
                ? 1
                : drugs.Any(d => d.DrugCategoryCode.MatchOne(UcrCodeGroups.OtherDangerousNonnarcoticDrugs))
                    ? 2
                    : drugs.Any(d => d.DrugCategoryCode.MatchOne(UcrCodeGroups.SyntheticNarcotics))
                        ? 3
                        : drugs.Any(d => d.DrugCategoryCode.MatchOne(UcrCodeGroups.Marijuana))
                            ? 4
                            : 0;
            if (mostDangerousSuspectedDrugType == 0)
                return;

            //Determine what keys to add to
            var offenseClassificationKeys = new string[2];
            if (mostDangerousCriminalActivityType == 1)
            {
                offenseClassificationKeys[0] = "180";

                switch (mostDangerousSuspectedDrugType)
                {
                    case 1:
                        offenseClassificationKeys[1] = "18A";
                        break;
                    case 2:
                        offenseClassificationKeys[1] = "18D";
                        break;
                    case 3:
                        offenseClassificationKeys[1] = "18C";
                        break;
                    case 4:
                        offenseClassificationKeys[1] = "18B";
                        break;
                }
            }
            else
            {
                offenseClassificationKeys[0] = "185";

                switch (mostDangerousSuspectedDrugType)
                {
                    case 1:
                        offenseClassificationKeys[1] = "18E";
                        break;
                    case 2:
                        offenseClassificationKeys[1] = "18H";
                        break;
                    case 3:
                        offenseClassificationKeys[1] = "18G";
                        break;
                    case 4:
                        offenseClassificationKeys[1] = "18F";
                        break;
                }
            }

            //Add counts for the subtotal category (offenseClassificationKeys[0]) and the specific category based on criminal activity and drug type (offenseClassificationKeys[1])
            AddCounts(age, sex, race, ethnicity, offenseClassificationKeys);
        }

        public void AddNonDrugOffenseCounts(string offenseUcrCode, string age, string sex, string race, string ethnicity)
        {
            //Determine offense classification with offenseUcrCode
            var offenseClassificationKeys = new List<string>();
            var arresteeIsJuvenile = Convert.ToInt32(age) < 18;
            switch (offenseUcrCode)
            {
                case "09A":
                    offenseClassificationKeys.Add("01A");
                    break;
                case "09B":
                    offenseClassificationKeys.Add("01B");
                    break;
                case "11A":
                case "11B":
                case "11C":
                    offenseClassificationKeys.Add("02b");
                    break;
                case "120":
                    offenseClassificationKeys.Add("03b");
                    break;
                case "13A":
                    offenseClassificationKeys.Add("04b");
                    break;
                case "220":
                    offenseClassificationKeys.Add("05b");
                    break;
                case "23A":
                case "23B":
                case "23C":
                case "23D":
                case "23E":
                case "23F":
                case "23G":
                case "23H":
                    offenseClassificationKeys.Add("06b");
                    break;
                case "240":
                    offenseClassificationKeys.Add("07b");
                    break;
                case "13B":
                case "13C":
                    offenseClassificationKeys.Add("08b");
                    break;
                case "200":
                    offenseClassificationKeys.Add("09b");
                    break;
                case "250":
                    offenseClassificationKeys.Add("10b");
                    break;
                case "26A":
                case "26B":
                case "26C":
                case "26D":
                case "26E":
                case "90A":
                    offenseClassificationKeys.Add("11b");
                    break;
                case "270":
                    offenseClassificationKeys.Add("12b");
                    break;
                case "280":
                    offenseClassificationKeys.Add("13b");
                    break;
                case "290":
                    offenseClassificationKeys.Add("14b");
                    break;
                case "520":
                    offenseClassificationKeys.Add("15b");
                    break;
                case "40A":
                case "40B":
                case "40C":
                    offenseClassificationKeys.Add("16b");
                    offenseClassificationKeys.Add("16" + offenseUcrCode.ToUpper()[2]);
                    break;
                case "11D":
                case "36A":
                case "36B":
                    offenseClassificationKeys.Add("17b");
                    break;
                //Line 18. Drug Abuse Violations all counts handled in its own function Asre.AddDrugOffenseCounts
                case "39A":
                case "39B":
                case "39C":
                case "39D":
                    offenseClassificationKeys.Add("19b");
                    offenseClassificationKeys.Add("19C");
                    break;
                //Lines 19[abc] are not available in nibrs
                case "90F":
                    offenseClassificationKeys.Add("20b");
                    break;
                case "90D":
                    offenseClassificationKeys.Add("21b");
                    break;
                case "90G":
                    offenseClassificationKeys.Add("22b");
                    break;
                case "90E":
                    offenseClassificationKeys.Add("23b");
                    break;
                case "90C":
                    offenseClassificationKeys.Add("24b");
                    break;
                case "90B":
                    //This offense is classified as Vagrancy or Curfer/Loitering based on arrestee age
                    offenseClassificationKeys.Add(arresteeIsJuvenile ? "28b" : "25b");
                    break;
                case "35B":
                case "100":
                case "210":
                case "370":
                case "510":
                case "90H":
                case "90J":
                case "90Z":
                    offenseClassificationKeys.Add("26b");
                    break;
                //Line 27 Suspicion is not available in nibrs
                case "90I":
                    if (arresteeIsJuvenile)
                        offenseClassificationKeys.Add("29b");
                    break;
                case "64A":
                    offenseClassificationKeys.Add("30b");
                    break;
                case "64B":
                    offenseClassificationKeys.Add("31b");
                    break;
                default:
                    //No counts are added if the offense does not fall in any category
                    return;
            }
            AddCounts(age, sex, race, ethnicity, offenseClassificationKeys.ToArray());
        }

        private class AsreCounts
        {
            public AsreCounts()
            {
                AgeSexCounts = new Dictionary<string, Dictionary<string, int>>();
                AdultRaceCounts = new Dictionary<string, int>();
                AdultEthnicityCounts = new Dictionary<string, int>();
                JuvenileRaceCounts = new Dictionary<string, int>();
                JuvenileEthnicityCounts = new Dictionary<string, int>();
                TotalAdultCount = 0;
                TotalJuvenileCount = 0;
            }

            public void AddCounts(string age, string sex, string race, string ethnicity)
            {
                // Categorize ages by their ucr age groups. When serializing to xml, no further serialization will be required.
                var ageGroup = age.TrimStart('0');
                switch (ageGroup)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        ageGroup = AgeGroup.UnderTen;
                        break;
                    case "10":
                    case "11":
                    case "12":
                        ageGroup = AgeGroup.TenToTwelve;
                        break;
                    case "13":
                    case "14":
                        ageGroup = AgeGroup.ThirteenToFourteen;
                        break;
                    case "15":
                    case "16":
                    case "17":
                    case "18":
                    case "19":
                    case "20":
                    case "21":
                    case "22":
                    case "23":
                    case "24":
                        ageGroup = age;
                        break;
                    case "25":
                    case "26":
                    case "27":
                    case "28":
                    case "29":
                        ageGroup = AgeGroup.TwentyfiveToTwentynine;
                        break;
                    case "30":
                    case "31":
                    case "32":
                    case "33":
                    case "34":
                        ageGroup = AgeGroup.ThirtyToThirtyfour;
                        break;
                    case "35":
                    case "36":
                    case "37":
                    case "38":
                    case "39":
                        ageGroup = AgeGroup.ThirtyfiveToThirtynine;
                        break;
                    case "40":
                    case "41":
                    case "42":
                    case "43":
                    case "44":
                        ageGroup = AgeGroup.FortyToFortyfour;
                        break;
                    case "45":
                    case "46":
                    case "47":
                    case "48":
                    case "49":
                        ageGroup = AgeGroup.FortyfiveToFortynine;
                        break;
                    case "50":
                    case "51":
                    case "52":
                    case "53":
                    case "54":
                        ageGroup = AgeGroup.FiftyToFiftyfour;
                        break;
                    case "55":
                    case "56":
                    case "57":
                    case "58":
                    case "59":
                        ageGroup = AgeGroup.FiftyfiveToFiftynine;
                        break;
                    case "60":
                    case "61":
                    case "62":
                    case "63":
                    case "64":
                        ageGroup = AgeGroup.SixtyToSixtyfour;
                        break;
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
                    case "78":
                    case "79":
                    case "80":
                    case "81":
                    case "82":
                    case "83":
                    case "84":
                    case "85":
                    case "86":
                    case "87":
                    case "88":
                    case "89":
                    case "90":
                    case "91":
                    case "92":
                    case "93":
                    case "94":
                    case "95":
                    case "96":
                    case "97":
                    case "98":
                    case "99":
                        ageGroup = AgeGroup.SixtyfivePlus;
                        break;
                    default:
                        return;
                }
                AgeSexCounts.TryAdd(ageGroup).TryIncrement(sex);

                var arresteeIsJuvenile = Convert.ToInt32(age) < 18;
                if (arresteeIsJuvenile)
                {
                    // Translate nibrs code to specific keys before inserting into dictionary to accommodate ucr xml serialization.
                    if (race != null && NibrsRaceCodeToUcrElementName.ContainsKey(race))
                        JuvenileRaceCounts.TryIncrement(NibrsRaceCodeToUcrElementName[race]);

                    if (ethnicity != null && NibrsEthnicityCodeToUcrElementName.ContainsKey(ethnicity))
                        JuvenileEthnicityCounts.TryIncrement(NibrsEthnicityCodeToUcrElementName[ethnicity]);

                    // Increment total count for this particular offense
                    TotalJuvenileCount += 1;
                }
                else
                {
                    // Translate nibrs code to specific keys before inserting into dictionary to accommodate ucr xml serialization.
                    if (race != null && NibrsRaceCodeToUcrElementName.ContainsKey(race))
                        AdultRaceCounts.TryIncrement(NibrsRaceCodeToUcrElementName[race]);

                    if (ethnicity != null && NibrsEthnicityCodeToUcrElementName.ContainsKey(ethnicity))
                        AdultEthnicityCounts.TryIncrement(NibrsEthnicityCodeToUcrElementName[ethnicity]);

                    // Increment total count for this particular offense
                    TotalAdultCount += 1;
                }
            }

            /* Remove aggregation functions?
            #region Aggregate Functions
            // Get individual counts
            public int GetAgeSexCount(string age, string sex)
            {
                return AgeSexCounts.ContainsKey(age)
                    ? (AgeSexCounts[age].ContainsKey(sex) ? AgeSexCounts[age][sex] : 0)
                    : 0;
            }

            public int GetRaceCount(string race)
            {
                return AdultRaceCounts.ContainsKey(race) ? AdultRaceCounts[race] : 0;
            }

            public int GetEthnicityCount(string ethnicity)
            {
                return AdultEthnicityCounts.ContainsKey(ethnicity) ? AdultEthnicityCounts[ethnicity] : 0;
            }

            // Get totals
            public int GetAgeTotal(string age)
            {
                return AgeSexCounts.ContainsKey(age) ? AgeSexCounts[age].Keys.Aggregate(0, (total, sex) => total + AgeSexCounts[age][sex]) : 0;
            }

            public int GetSexTotal(string sex)
            {
                return AgeSexCounts.Keys.Aggregate(0, (total, age) => total + (AgeSexCounts[age].ContainsKey(sex) ? AgeSexCounts[age][sex] : 0));
            }
            #endregion
            */

            public XElement[] Serialize()
            {
                // Create XElements for ages, sexes, races, and ethnicities

                var ageElements = new XElement("Ages",
                    AgeSexCounts.Select(ageSexdictionaryPair => new XElement("Age",
                    new XAttribute("value", ageSexdictionaryPair.Key),
                    ageSexdictionaryPair.Value.Select(sexCountPair => new XElement(sexCountPair.Key, sexCountPair.Value)))));

                var adultElement = new XElement("Adult",
                    new XElement("Races", AdultRaceCounts.Select(raceCountPair => new XElement(raceCountPair.Key, raceCountPair.Value))),
                    new XElement("Ethnicities", AdultEthnicityCounts.Select(ethnicityCountPair => new XElement(ethnicityCountPair.Key, ethnicityCountPair.Value))));

                var juvenileElement = new XElement("Juvenile",
                    new XElement("Races", JuvenileRaceCounts.Select(raceCountPair => new XElement(raceCountPair.Key, raceCountPair.Value))),
                    new XElement("Ethnicities", JuvenileEthnicityCounts.Select(ethnicityCountPair => new XElement(ethnicityCountPair.Key, ethnicityCountPair.Value))));

                // Convert the list of XElements to an array and return
                return new[] { ageElements, adultElement, juvenileElement }; ;
            }

            #region Class Dictionaries for Serialization

            private static readonly Dictionary<string, string> NibrsRaceCodeToUcrElementName = new Dictionary<string, string>
            {
                {RACCode.AMERICAN_INDIAN_OR_ALASKAN_NATIVE.NibrsCode(), "AmericanIndian"},
                {RACCode.ASIAN.NibrsCode(), "AsianOrPacificIslander"},
                {RACCode.BLACK.NibrsCode(), "Black"},
                {RACCode.HAWAIIAN_OR_PACIFIC_ISLANDER.NibrsCode(), "AsianOrPacificIslander"},
                {RACCode.WHITE.NibrsCode(), "White"}
            };

            private static readonly Dictionary<string, string> NibrsEthnicityCodeToUcrElementName = new Dictionary<string, string>
            {
                {EthnicityCode.HISPANIC_OR_LATINO.NibrsCode(), "Hispanic"},
                {EthnicityCode.NOT_HISPANIC_OR_LATINO.NibrsCode(), "Non-Hispanic"}
            };

            #endregion

            #region Instance Variables and Properties

            private Dictionary<string, Dictionary<string, int>> AgeSexCounts { get; set; }
            private Dictionary<string, int> AdultRaceCounts { get; set; }
            private Dictionary<string, int> AdultEthnicityCounts { get; set; }
            private Dictionary<string, int> JuvenileRaceCounts { get; set; }
            private Dictionary<string, int> JuvenileEthnicityCounts { get; set; }
            public int TotalAdultCount { get; private set; }
            public int TotalJuvenileCount { get; private set; }

            #endregion
        }
    }
}