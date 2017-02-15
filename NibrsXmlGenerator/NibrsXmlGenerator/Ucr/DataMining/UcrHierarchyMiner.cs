﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.NibrsReport.Offense;


namespace NibrsXml.Ucr.DataMining
{
    public class UcrHierarchyMiner
    {
        public static List<String> UcrHierarchyOrderArray
        {
            get
            {
                //Reverse the list of offenses as found in spec to make sure the highest index belongs to the first (09A).
                return new List<string>() {"09A", "09B", "11A", "120", "13A", "220", "23A", "23B", "23C", "23D", "23E", "23F", "23G", "23H","240","200"};
            }
        }

        public Offense HighestRatedOffense { get; private set; }

        /// <summary>
        /// Returns the offense (or offenses if applicable) to count for this report
        /// </summary>
        /// <param name="offenses">List of Offenses for the Incident</param>
        public UcrHierarchyMiner(List<Offense> offenses)
        {
            //Make sure there are offenses to mine.
            if (!offenses.Any()) return;

            //Select the highest ranking offense based on the reversed index of the array.
            this.HighestRatedOffense = offenses.OrderBy(o => UcrHierarchyOrderArray.IndexOf(o.UcrCode)).FirstOrDefault();
        }
    }
}
