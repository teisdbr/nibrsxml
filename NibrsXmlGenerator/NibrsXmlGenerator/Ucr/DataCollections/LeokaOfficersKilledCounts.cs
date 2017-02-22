using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Utility;

namespace NibrsXml.Ucr.DataCollections
{
    public class LeokaOfficersKilledCounts
    {
        [Description("09A")]
        public int? Feloniously { get; set; }

        [Description("09B")]
        public int? ByAccident { get; set; }

        public void IncrementCount(string key, int byValue = 1)
        {
            //Do nothing if value is zero (0)
            if (byValue == 0) return;
            
            //Score each key respectively.
            if (key == "09A")
            {
                Feloniously = Feloniously.GetValueOrDefault(0) + byValue;
            }
            else if (key == "09B")
            {
                ByAccident = ByAccident.GetValueOrDefault(0) + byValue;
            }
        }
    }
}
