using System;
using System.Collections.Generic;
using System.Linq;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Associations;
using NibrsXml.NibrsReport.Item;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.Utility;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataCollections
{
    public class ReturnA : GeneralSummaryData
    {
        public ReturnA()
        {
            Supplement = new ReturnASupplement();
        }

        public ReturnASupplement Supplement { get; set; }

        #region Instantiations

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

        #endregion

        #region Miscelaneous Functions

        /// <summary>
        ///     Applies a weapon hierarchy over all offense forces applicable
        ///     Weapon classes are as follows:
        ///     "a" - Firearms
        ///     "b" - Knife or Cutting Instrument
        ///     "c" - Other Dangerous Weapon
        ///     "d" - Strong-Arm (Hands, Fists, Feet, Etc.)
        /// </summary>
        /// <param name="offenseForces"></param>
        /// <returns>The weapon class of the most dangerous weapon in this list</returns>
        private string ExtractWeaponGroup(List<OffenseForce> offenseForces)
        {
            //Extract force codes and sort because they codes are generally already hierarchically ordered,
            //with the exception of personal weapons (40)
            var offenseForceCodes = offenseForces.Select(o => o.CategoryCode).OrderBy(f => f);
            var mostDangerousWeapon = offenseForceCodes.FirstOrDefault();

            //Make sure a weapon exists, otherwise classify it as e.
            if (mostDangerousWeapon == null) return "e";

            //Handle the personal weapons (40) exception
            //Because of the sorting, it is implied there are no "a" or "b" class weapons
            //Therefore, if there are any "c" class weapons, "c" is returned regardless if personal weapons is first in the list
            //because personal weapons are in the "d" class
            if (mostDangerousWeapon == ForceCategoryCode.PERSONAL_WEAPONS.NibrsCode() &&
                offenseForceCodes.Any(f => NibrsCodeGroups.OtherDangerousWeapons.Contains(f)))
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

        #endregion

        #region Serialization Properties

        public override string XmlRootName
        {
            get { return "ReturnASummary"; }
        }

        public override string XslFileName
        {
            get { return "returna.xsl"; }
        }

        #endregion

        #region Base Overriden Incrementers

        public void IncrementActualOffense(string key, int byValue = 1, bool doNotCareAboutThis = false)
        {
            base.IncrementActualOffense(key, byValue);

            if (key.Length == 2)
            {
                //Increment subtotals
                var subtotalKey = key.Substring(0, 1);
                ClassificationCounts.TryAdd(subtotalKey).IncrementActualOffense(byValue);
            }
                
        }

        public override void IncrementAllClearences(string key, int byValue = 1, bool allArresteesAreJuvenile = false)
        {
            base.IncrementAllClearences(key, byValue, allArresteesAreJuvenile);

            if (key.Length == 2)
            {
                //Increment subtotals
                var subtotalKey = key.Substring(0, 1);
                ClassificationCounts.TryAdd(subtotalKey).IncrementAllClearences(byValue);
            }
        }

        protected override void IncrementJuvenileClearences(string key, int byValue = 1)
        {
            base.IncrementJuvenileClearences(key, byValue);

            if (key.Length == 2)
            {
                //Increment subtotals
                var subtotalKey = key.Substring(0, 1);
                ClassificationCounts.TryAdd(subtotalKey).IncrementJuvenileClearences(byValue);
            }
        }

        #endregion

        #region Homicides

        /// <summary>
        ///     Scores counts for 1, 1a, and 1b offense classifications
        /// </summary>
        /// <param name="homicideVictimAssociations"></param>
        /// <param name="allArresteesAreJuvenile"></param>
        /// <param name="totalStolenValue"></param>
        internal void ScoreHomicide(List<OffenseVictimAssociation> homicideVictimAssociations, int totalStolenValue, bool? allArresteesAreJuvenile = null)
        {
            if (allArresteesAreJuvenile.HasValue)
            {
                IncrementHomicideScore(homicideVictimAssociations, IncrementAllClearences, allArresteesAreJuvenile.Value);
            }
            else
            {
                IncrementHomicideScore(homicideVictimAssociations, IncrementActualOffense);
                Supplement.IncrementHomicide(homicideVictimAssociations.Count(ov => ov.RelatedOffense.UcrCode.MatchOne("09A")), totalStolenValue);
            }
        }

        internal void IncrementHomicideScore(List<OffenseVictimAssociation> homicideVictimAssociations, Action<string, int, bool> incrementer, bool allArresteesAreJuvenile = false)
        {
            //Column 4
            //--Line 1a Non-negligent Murders - 09A
            incrementer("1a", homicideVictimAssociations.Count(ov => ov.RelatedOffense.UcrCode.MatchOne("09A")), allArresteesAreJuvenile);
            //--Line 1b Negligent Murders - 09B
            incrementer("1b", homicideVictimAssociations.Count(ov => ov.RelatedOffense.UcrCode.MatchOne("09B")), allArresteesAreJuvenile);
        }

        #endregion

        #region Rapes

        /// <summary>
        ///     Scores counts for 2, 2a, and 2b offense classifications
        ///     If rapes are to be scored based on victim sex as well, that will have to be handled at a higher level
        /// </summary>
        /// <param name="rapeVictimOffenseAssoc"></param>
        /// <param name="allArresteeAreJuvenile"></param>
        /// <param name="totalStolenValue"></param>
        internal void ScoreRape(List<OffenseVictimAssociation> rapeVictimOffenseAssoc, int totalStolenValue, bool? allArresteeAreJuvenile = null)
        {
            if (allArresteeAreJuvenile.HasValue)
            {
                IncrementRapeScore(rapeVictimOffenseAssoc, IncrementAllClearences, allArresteeAreJuvenile.Value);
            }
            else
            {
                var rapeCount = rapeVictimOffenseAssoc.Count(ov => ov.RelatedOffense.UcrCode.MatchOne("11A", "11B", "11C"));
                IncrementRapeScore(rapeVictimOffenseAssoc, IncrementActualOffense);
                Supplement.IncrementRape(rapeCount, totalStolenValue);
            }
        }

        internal void IncrementRapeScore(List<OffenseVictimAssociation> rapeVictimOffenseAssoc, Action<string, int, bool> incrementer, bool allArresteesAreJuvenile = false)
        {
            //Column 4
            //--Line 2a for Completed Offense Rapes
            incrementer("2a", rapeVictimOffenseAssoc.Count(ov => ov.RelatedOffense.UcrCode.MatchOne("11A", "11B", "11C") && !Convert.ToBoolean(ov.RelatedOffense.AttemptedIndicator)),
                allArresteesAreJuvenile);
            //--Line 2a for Attempted Offense Rapes
            incrementer("2b", rapeVictimOffenseAssoc.Count(ov => ov.RelatedOffense.UcrCode.MatchOne("11A", "11B", "11C") && Convert.ToBoolean(ov.RelatedOffense.AttemptedIndicator)),
                allArresteesAreJuvenile);
        }

        #endregion

        #region Robberies

        internal void ScoreRobbery(Offense robberyOffense, int totalStolenValue, bool? allArresteesAreJuvenile = null)
        {
            if (allArresteesAreJuvenile.HasValue)
            {
                IncrementRobberyScore(robberyOffense, IncrementAllClearences, allArresteesAreJuvenile.Value);
            }
            else
            {
                IncrementRobberyScore(robberyOffense, IncrementActualOffense);
                Supplement.IncrementRobbery(robberyOffense.Location.CategoryCode, totalStolenValue);
            }
        }

        internal void IncrementRobberyScore(Offense robberyOffense, Action<string, int, bool> incrementer, bool allArresteesAreJuvenile = false)
        {
            //Column 4
            //Determine the classification based on the weapon/force code after hierarchy rules are applied
            var offenseClassification = "3" + ExtractWeaponGroup(robberyOffense.Forces);
            incrementer(offenseClassification, 1, allArresteesAreJuvenile);
        }

        #endregion

        #region Assaults

        internal void ScoreAssault(List<OffenseVictimAssociation> assaultVictimOffenseAssoc, bool? allArresteesAreJuvenile = null)
        {
            if (allArresteesAreJuvenile.HasValue)
                IncrementAssaultScore(assaultVictimOffenseAssoc, IncrementAllClearences, allArresteesAreJuvenile.Value);
            else
                IncrementAssaultScore(assaultVictimOffenseAssoc, IncrementActualOffense);
        }

        private void IncrementAssaultScore(List<OffenseVictimAssociation> assaultVictimOffenseAssoc, Action<string, int, bool> incrementer, bool allArresteesAreJuvenile = false)
        {
            //Column 4
            //--Collect a list of keyvaluepairs of Victims and Most Serious Weapons.
            var victimsAndWeapons =
                assaultVictimOffenseAssoc.Select(vo => new KeyValuePair<OffenseVictimAssociation, string>(vo, ExtractWeaponGroup(vo.RelatedOffense.Forces))).ToList();

            //--Line 4a - Firearms
            incrementer("4a", victimsAndWeapons.Count(vw => vw.Key.RelatedOffense.UcrCode == "13A" && vw.Value == "a"), allArresteesAreJuvenile);
            //--Line 4b - Knife or Cutting Instrument
            incrementer("4b", victimsAndWeapons.Count(vw => vw.Key.RelatedOffense.UcrCode == "13A" && vw.Value == "b"), allArresteesAreJuvenile);
            //--Line 4c - Other Dangerous Weapons
            incrementer("4c", victimsAndWeapons.Count(vw => vw.Key.RelatedOffense.UcrCode == "13A" && vw.Value == "c"), allArresteesAreJuvenile);
            //--Line 4d - Hands, Fists, Feet, etc
            incrementer("4d", victimsAndWeapons.Count(vw => vw.Key.RelatedOffense.UcrCode == "13A" && vw.Value == "d"), allArresteesAreJuvenile);
            //--Line 4e - Simple, Not Aggravated
            incrementer("4e", victimsAndWeapons.Count(vw => vw.Key.RelatedOffense.UcrCode.MatchOne("13[BC]") && vw.Value == "e"), allArresteesAreJuvenile);
        }

        #endregion

        #region Burglaries

        internal void ScoreBurglary(Offense robberyOffense, string incidentTime, int totalStolenValue, bool? allArresteesAreJuvenile = null)
        {
            //This will always contain the count of 1 if there are no premises and if the location is not that of a storage facility.
            var numberOfPremisesOrDefault = robberyOffense.StructuresEnteredQuantity != null && Convert.ToInt32(robberyOffense.StructuresEnteredQuantity) > 0 &&
                                            robberyOffense.Location.CategoryCode == LocationCategoryCode.RENTAL_STORAGE_FACILITY.NibrsCode()
                ? Convert.ToInt32(robberyOffense.StructuresEnteredQuantity)
                : 1;

            //Column 4 - Increment Actual Offense
            if (allArresteesAreJuvenile.HasValue)
            {
                IncrementBurglaryOffenses(robberyOffense, IncrementAllClearences, numberOfPremisesOrDefault, allArresteesAreJuvenile.Value);
            }
            else
            {
                IncrementBurglaryOffenses(robberyOffense, IncrementActualOffense, numberOfPremisesOrDefault);
                Supplement.IncrementBurglary(incidentTime, robberyOffense.Location.CategoryCode, totalStolenValue, numberOfPremisesOrDefault);
            }
        }

        private static void IncrementBurglaryOffenses(Offense robberyOffense, Action<string, int, bool> incrementingFunction, int numberOfPremisesOrDefault, bool allArresteesAreJuvenile = false)
        {
            switch (robberyOffense.AttemptedIndicator)
            {
                //Offense is attempted
                case "true":
                    incrementingFunction("5c", numberOfPremisesOrDefault, allArresteesAreJuvenile);
                    break;
                //Offense is completed
                case "false":
                    switch (robberyOffense.EntryPoint.PassagePointMethodCode)
                    {
                        //Force was used
                        case "F":
                            incrementingFunction("5a", numberOfPremisesOrDefault, allArresteesAreJuvenile);
                            break;
                        //No Force was used
                        case "N":
                            incrementingFunction("5b", numberOfPremisesOrDefault, allArresteesAreJuvenile);
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Larceny-Theft

        internal void ScoreLarcenyThefts(Offense larcenyOffense, List<Item> stolenItems, bool? allArresteesAreJuvenile = null)
        {
            if (allArresteesAreJuvenile.HasValue)
            {
                IncrementLarcenyTheft(IncrementAllClearences, allArresteesAreJuvenile.Value);
            }
            else
            {
                IncrementLarcenyTheft(IncrementActualOffense);
                Supplement.IncrementLarceny(larcenyOffense.UcrCode, stolenItems);
            }
        }

        private static void IncrementLarcenyTheft(Action<string, int, bool> incrementer, bool allArresteesAreJuvenile = false)
        {
            incrementer("6", 1, allArresteesAreJuvenile);
        }

        #endregion

        #region Motor Vehicle Theft

        internal void ScoreVehicleTheft(Offense motorVehicleOffense, List<Item> vehicleProperties, int totalStolenValue, bool? allArresteesAreJuvenile = null)
        {
            if (allArresteesAreJuvenile.HasValue)
            {
                IncrementVehicleScore(motorVehicleOffense, vehicleProperties, IncrementAllClearences, allArresteesAreJuvenile.Value);
            }
            else
            {
                var stolenVehicleCount = vehicleProperties.Count(v => v.NibrsPropertyCategoryCode.MatchOne(NibrsCodeGroups.VehicleProperties));
                IncrementVehicleScore(motorVehicleOffense, vehicleProperties, IncrementActualOffense);
                Supplement.IncrementVehicleTheft(stolenVehicleCount, totalStolenValue);
            }
        }

        private static void IncrementVehicleScore(Offense motorVehicleOffense, List<Item> vehicleProperties, Action<string, int, bool> incrementer, bool allArresteesAreJuvenile = false)
        {
            var isAttemptedOffense = Convert.ToBoolean(motorVehicleOffense.AttemptedIndicator);

            //Attempted Offense Calculation
            if (isAttemptedOffense)
            {
                incrementer("7a",
                    vehicleProperties.Count(v => v.NibrsPropertyCategoryCode.MatchOne(NibrsCodeGroups.VehicleProperties)), allArresteesAreJuvenile);
            }
            //Completed Offense Calculation
            else
            {
                //---7c - Motor Vehicle Theft - Other Vehicles
                incrementer("7c", vehicleProperties.Count(v => v.NibrsPropertyCategoryCode == PropertyCategoryCode.OTHER_MOTOR_VEHICLES.NibrsCode()), allArresteesAreJuvenile);
                //---7b - Motor Vehicle Theft - Trucks & Buses
                incrementer("7b",
                    vehicleProperties.Count(
                        v =>
                            v.NibrsPropertyCategoryCode.MatchOne(PropertyCategoryCode.BUSES.NibrsCode(), PropertyCategoryCode.RECREATIONAL_VEHICLES.NibrsCode(),
                                PropertyCategoryCode.RECREATIONAL_VEHICLES.NibrsCode())), allArresteesAreJuvenile);
                //---7a - Motor Vehicle Theft - Autos
                incrementer("7a", vehicleProperties.Count(v => v.NibrsPropertyCategoryCode == PropertyCategoryCode.AUTOMOBILE.NibrsCode()), allArresteesAreJuvenile);
            }
        }

        #endregion
    }
}