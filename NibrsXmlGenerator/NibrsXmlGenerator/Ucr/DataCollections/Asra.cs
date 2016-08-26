using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NibrsXml.Ucr.DataCollections
{
    public class Asra : Data
    {
        // Properties
        private Dictionary<string, Asre> offenseAsre { get; set; }
        public string AgencyIdentifier { get; set; }
        public string AgencyName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string MonthYear { get; set; }
        public string PreparedBy { get; set; }
        public string PreparedByTitle { get; set; }
        public string PreparedByPhone { get; set; }
        public string DatePrepared { get; set; }
        /// <summary>
        /// Chief, Sheriff, Comissioner or Superintendent
        /// </summary>
        public string AdministrativeOfficial { get; set; }

        public Asra()
        {
            this.offenseAsre = new Dictionary<string, Asre>();
        }

        // Increment counts
        public void AddCounts(string offenseUcrCode, string age, string sex, string race, string ethnicity)
        {
            TryAdd(offenseUcrCode).AddCounts(age, sex, race, ethnicity);
        }

        private Asre TryAdd(string key)
        {
            if (!offenseAsre.ContainsKey(key))
            {
                offenseAsre.Add(key, new Asre());
            }
            return offenseAsre[key];
        }

        // Get individual counts
        public int GetAgeSexCount(string offenseUcrCode, string age, string sex)
        {
            return offenseAsre.ContainsKey(offenseUcrCode) ? offenseAsre[offenseUcrCode].GetAgeSexCount(age, sex) : 0;
        }

        public int GetRaceCount(string offenseUcrCode, string race)
        {
            return offenseAsre.ContainsKey(offenseUcrCode) ? offenseAsre[offenseUcrCode].GetRaceCount(race) : 0;
        }

        public int GetEthnicityCount(string offenseUcrCode, string ethnicity)
        {
            return offenseAsre.ContainsKey(offenseUcrCode) ? offenseAsre[offenseUcrCode].GetEthnicityCount(ethnicity) : 0;
        }

        // Get subtotals
        public int GetAgeTotal(string age)
        {
            return offenseAsre.Keys.Aggregate(0, (total, offense) => total + offenseAsre[offense].GetAgeTotal(age));
        }

        public int GetOffenseSexTotal(string offenseUcrCode, string sex)
        {
            return offenseAsre.ContainsKey(offenseUcrCode) ? offenseAsre[offenseUcrCode].GetSexTotal(sex) : 0;
        }

        public int GetSexTotal(string sex)
        {
            return offenseAsre.Keys.Aggregate(0, (total, offense) => total + offenseAsre[offense].GetSexTotal(sex));
        }

        public int GetRaceTotal(string race)
        {
            return offenseAsre.Keys.Aggregate(0, (total, offense) => total + (offenseAsre[offense].GetRaceCount(race)));
        }

        public int GetEthnicityTotal(string ethnicity)
        {
            return offenseAsre.Keys.Aggregate(0, (total, offense) => total + (offenseAsre[offense].GetEthnicityCount(ethnicity)));
        }

        // Get grandtotal
        public int GetGrandTotal(string age)
        {
            return offenseAsre.Keys.Aggregate(0, (total, offense) => total + offenseAsre[offense].TotalCount);
        }

        public XDocument Serialize() 
        {
            return new XDocument(
                new XProcessingInstruction(
                    "xml-stylesheet",
                    "type=\"text/xsl\" href=\"asre.xsl\""),
                new XElement(
                    "ASRSummary",
                    // todo: translate all offense codes to their actual description representation (this may not be the appropriate place to do so)
                    offenseAsre.Select(offenseToCountsPair => new XElement("UCR", new XAttribute("value", offenseToCountsPair.Key), offenseToCountsPair.Value.Serialize()))));
        }
    }
}
