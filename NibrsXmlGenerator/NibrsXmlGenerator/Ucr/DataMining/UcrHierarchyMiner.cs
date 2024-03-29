﻿using System.Collections.Generic;
using System.Linq;
using NibrsModels.NibrsReport.Associations;
using NibrsModels.NibrsReport.Offense;

namespace NibrsXml.Ucr.DataMining
{
    public class UcrHierarchyMiner
    {
        public static List<string> UcrHierarchyOrderArray
        {
            get
            {
                //Reverse the list of offenses as found in spec to make sure the highest index belongs to the first (09A). 13B & 13C had to be added so that there is a priority for them as well even though
                //they are not part of the hierarchy rule.
                return new List<string> { "09A", "09B", "11A", "11B", "11C", "120", "13A", "220", "23A", "23B", "23C", "23D", "23E", "23F", "23G", "23H", "240", "200", "13B", "13C" };
            }
        }

        public Offense HighestRatedOffense { get; private set; }
        public List<OffenseVictimAssociation> VictimsRelatedToHighestRatedOffense { get; private set; }

        /// <summary>
        /// Returns the offense (or offenses if applicable) to count for this report
        /// </summary>
        /// <param name="offenses">List of Offenses for the Incident</param>
        public UcrHierarchyMiner(List<Offense> offenses, List<OffenseVictimAssociation> victimAssociations)
        {
            //Make sure there are offenses to mine.
            if (!offenses.Any()) return;

            //Select the highest ranking offense based on the reversed index of the array.
            HighestRatedOffense = offenses.OrderBy(o => UcrHierarchyOrderArray.IndexOf(o.UcrCode)).FirstOrDefault(o => UcrHierarchyOrderArray.IndexOf(o.UcrCode) >= 0);

            if (HighestRatedOffense == null) return;

            VictimsRelatedToHighestRatedOffense = victimAssociations.Where(ov => ov.RelatedOffense.UcrCode == HighestRatedOffense.UcrCode)
                .ToList();
        }
    }
}