using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Utility;
using NibrsXml.Constants;

namespace NibrsXml.Ucr.DataCollections
{
    class Asre
    {
        private Dictionary<string, Dictionary<string, int>> ageSexCounts { get; set; }
        private Dictionary<string, int> raceCounts { get; set; }
        private Dictionary<string, int> ethnicityCounts { get; set; }
        public int TotalCount { get; private set; }

        public Asre()
        {
            this.ageSexCounts = new Dictionary<string, Dictionary<string, int>>();
            this.raceCounts = new Dictionary<string, int>();
            this.ethnicityCounts = new Dictionary<string, int>();
            this.TotalCount = 0;
        }

        public void AddCounts(string age, string sex, string race, string ethnicity)
        {
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
                    ageGroup =  AgeGroup.UnderTen;
                    break;
                case "10":
                case "11":
                case "12":
                    ageGroup =  AgeGroup.TenToTwelve;
                    break;
                case "13":
                case "14":
                    ageGroup =  AgeGroup.ThirteenToFourteen;
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

            ageSexCounts.TryAdd(ageGroup).TryIncrement(sex);
            raceCounts.TryIncrement(race);
            ethnicityCounts.TryIncrement(ethnicity);
            TotalCount += 1;
        }

        // Get individual counts
        public int GetAgeSexCount(string age, string sex)
        {
            return ageSexCounts.ContainsKey(age)
                ? (ageSexCounts[age].ContainsKey(sex) ? ageSexCounts[age][sex] : 0)
                : 0;
        }

        public int GetRaceCount(string race)
        {
            return raceCounts.ContainsKey(race) ? raceCounts[race] : 0;
        }

        public int GetEthnicityCount(string ethnicity)
        {
            return ethnicityCounts.ContainsKey(ethnicity) ? ethnicityCounts[ethnicity] : 0;
        }

        // Get totals
        public int GetAgeTotal(string age)
        {
            return ageSexCounts.ContainsKey(age) ? ageSexCounts[age].Keys.Aggregate(0, (total, sex) => total + ageSexCounts[age][sex]) : 0;
        }

        public int GetSexTotal(string sex)
        {
            return ageSexCounts.Keys.Aggregate(0, (total, age) => total + (ageSexCounts[age].ContainsKey(sex) ? ageSexCounts[age][sex] : 0));
        }
    }
}
