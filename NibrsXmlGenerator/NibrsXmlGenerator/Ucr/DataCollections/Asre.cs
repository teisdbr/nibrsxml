using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Utility;

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
            // todo: parse counts into ranges for ages 25+ before adding it to ageSexCounts
            ageSexCounts.TryAdd(age).TryIncrement(sex);
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
