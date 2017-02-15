using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.Constants;
using NibrsXml.Utility;
using NibrsXml.Ucr.DataMining;

namespace NibrsXml.Ucr.DataCollections
{
    public class ReturnA : GeneralSummaryData
    {
        public override string XmlRootName
        {
            get { return "ReturnASummary"; }
        }

        public override string XslFileName
        {
            get { return "returna.xsl"; }
        }

        protected override void ClassificationCountEntryInstantiations()
        {
            ClassificationCounts.Add("1", new GeneralSummaryCounts());
            ClassificationCounts.Add("1a", new GeneralSummaryCounts());
            ClassificationCounts.Add("1b", new GeneralSummaryCounts());
            ClassificationCounts.Add("2", new GeneralSummaryCounts());
            ClassificationCounts.Add("2a", new GeneralSummaryCounts());
            ClassificationCounts.Add("2b", new GeneralSummaryCounts());
            ClassificationCounts.Add("3", new GeneralSummaryCounts());
            ClassificationCounts.Add("3a", new GeneralSummaryCounts());
            ClassificationCounts.Add("3b", new GeneralSummaryCounts());
            ClassificationCounts.Add("3c", new GeneralSummaryCounts());
            ClassificationCounts.Add("3d", new GeneralSummaryCounts());
            ClassificationCounts.Add("4", new GeneralSummaryCounts());
            ClassificationCounts.Add("4a", new GeneralSummaryCounts());
            ClassificationCounts.Add("4b", new GeneralSummaryCounts());
            ClassificationCounts.Add("4c", new GeneralSummaryCounts());
            ClassificationCounts.Add("4d", new GeneralSummaryCounts());
            ClassificationCounts.Add("4e", new GeneralSummaryCounts());
            ClassificationCounts.Add("5", new GeneralSummaryCounts());
            ClassificationCounts.Add("5a", new GeneralSummaryCounts());
            ClassificationCounts.Add("5b", new GeneralSummaryCounts());
            ClassificationCounts.Add("5c", new GeneralSummaryCounts());
            ClassificationCounts.Add("6", new GeneralSummaryCounts());
            ClassificationCounts.Add("7", new GeneralSummaryCounts());
            ClassificationCounts.Add("7a", new GeneralSummaryCounts());
            ClassificationCounts.Add("7b", new GeneralSummaryCounts());
            ClassificationCounts.Add("7c", new GeneralSummaryCounts());
        }

        public override void IncrementActualOffense(string key, int byValue = 1)
        {
            base.IncrementActualOffense(key, byValue);


            if (key.Length == 2)
                //Increment subtotals
                IncrementActualOffense(key.Substring(0, 1), byValue);
        }

        public override void IncrementAllClearences(string key, int byValue = 1)
        {
            base.IncrementAllClearences(key, byValue);

            if (key.Length == 2)
                //Increment subtotals
                IncrementAllClearences(key.Substring(0, 1), byValue);
        }

        public override void IncrementJuvenileClearences(string key, int byValue = 1)
        {
            base.IncrementJuvenileClearences(key, byValue);

            if (key.Length == 2)
                //Increment subtotals
                IncrementJuvenileClearences(key.Substring(0, 1), byValue);
        }

        /// <summary>
        /// Scores counts for 1, 1a, and 1b offense classifications
        /// </summary>
        /// <param name="homicideOffense">A 09A or 09B offense</param>
        internal void ScoreHomicide(Offense homicideOffense)
        {
            //Column 4
            //This line extracts the 3rd char and appends it to a "1" string to create the classification (not reusable outside this function)
            var offenseClassification = "1" + homicideOffense.UcrCode.Substring(2, 1).ToLower();
            IncrementActualOffense(offenseClassification);

            //Column 5
            //Column 6
        }

        /// <summary>
        /// Scores counts for 2, 2a, and 2b offense classifications
        /// If rapes are to be scored based on victim sex as well, that will have to be handled at a higher level
        /// </summary>
        /// <param name="rapeOffense">An 11A offense</param>
        internal void ScoreRape(Offense rapeOffense)
        {
            //Column 4
            //Determine the classification based on the attempted/completed status
            var offenseClassification = Convert.ToBoolean(rapeOffense.AttemptedIndicator) ? "2b" : "2a";
            IncrementActualOffense(offenseClassification);

            //Column 5


            //Column 6

        }

        internal void ScoreRobbery(Offense robberyOffense)
        {
            //Column 4
            //Determine the classification based on the weapon/force code after hierarchy rules are applied
            var offenseClassification = "3" + ExtractWeaponGroup(robberyOffense.Forces);
            IncrementActualOffense(offenseClassification);

            //Column 5


            //Column 6
        }

        internal void ScoreAssault(Offense assaultOffense)
        {
            //Column 4
            String offenseClassification = null;
            if (assaultOffense.UcrCode.Substring(2, 1) == "A")
                offenseClassification = "4" + ExtractWeaponGroup(assaultOffense.Forces);
            else if(assaultOffense.Forces.All(o => NibrsCodeGroups.SimpleAssaultForces.Contains(o.CategoryCode)))
            {
                offenseClassification = "4e";
            }

            if (offenseClassification != null)
                IncrementActualOffense(offenseClassification);

            //Column 5


            //Column 6
        }

        internal void ScoreVehicleTheft(Offense vehicularOffense)
        {
            //Column 4


            //Column 5


            //Column 6
        }

        /// <summary>
        /// Applies a weapon hierarchy over all offense forces applicable
        /// Weapon classes are as follows:
        /// "a" - Firearms
        /// "b" - Knife or Cutting Instrument
        /// "c" - Other Dangerous Weapon
        /// "d" - Strong-Arm (Hands, Fists, Feet, Etc.)
        /// </summary>
        /// <param name="offenseForces"></param>
        /// <returns>The weapon class of the most dangerous weapon in this list</returns>
        private string ExtractWeaponGroup(List<OffenseForce> offenseForces)
        {
            //Extract force codes and sort because they codes are generally already hierarchically ordered,
            //with the exception of personal weapons (40)
            var offenseForceCodes = offenseForces.Select(o => o.CategoryCode).OrderBy(f => f);
            var mostDangerousWeapon = offenseForceCodes.First();
            
            //Handle the personal weapons (40) exception
            //Because of the sorting, it is implied there are no "a" or "b" class weapons
            //Therefore, if there are any "c" class weapons, "c" is returned regardless if personal weapons is first in the list
            //because personal weapons are in the "d" class
            if (mostDangerousWeapon == ForceCategoryCode.PERSONAL_WEAPONS.NibrsCode() && offenseForceCodes.Any(f => NibrsCodeGroups.OtherDangerousWeapons.Contains(f)))
                return "c";

            //Return based on hierarchy
            return NibrsCodeGroups.Firearms.Contains(mostDangerousWeapon)
                ? "a"
                : mostDangerousWeapon == ForceCategoryCode.LETHAL_CUTTING_INSTRUMENT.NibrsCode()
                    ? "b"
                    : NibrsCodeGroups.OtherDangerousWeapons.Contains(mostDangerousWeapon)
                        ? "c"
                        : "d";
        }

        #region Burglaries

        internal void ScoreBurglary(NibrsReport.Offense.Offense robberyOffense)
        {
            //Column 4 - Increment Actual Offense
            IncrementBurglaryActualOffenses(robberyOffense);

            //Column 5 - Increment Total Offenses Cleared By Arrest of Exceptional Means
            IncremenetBurglaryExceptionalClearance();

            //Column 6 - Increment Number of Clearances involving Juveniles
            IncrementBurglaryExceptionalClearanceWithJuveniles();



        }

        private void IncremenetBurglaryExceptionalClearance()
        {
            throw new NotImplementedException();
        }

        private void IncrementBurglaryExceptionalClearanceWithJuveniles()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function determines which line to increment
        /// of 5[abc]
        /// </summary>
        private void IncrementBurglaryActualOffenses(NibrsReport.Offense.Offense robberyOffense)
        {
            switch (robberyOffense.AttemptedIndicator)
            {
                //Offense is attempted
                case "true":
                    IncrementActualOffense("5c");
                    break;
                //Offense is completed
                case "false":
                    switch (robberyOffense.EntryPoint.PassagePointMethodCode)
                    {
                        //Force was used
                        case "F":
                            IncrementActualOffense("5a");
                            break;
                        //No Force was used
                        case "N":
                            IncrementActualOffense("5b");
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion


        #region GenericClearanceHelper
        //public Boolean 
        #endregion

        internal ReturnAMiner.ClearanceDetails ScoreClearances(List<NibrsReport.Arrest.Arrest> list, string p)
        {
            throw new NotImplementedException();
        }
    }
}