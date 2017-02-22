using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Arrest;
using NibrsXml.NibrsReport;
using NibrsXml.Ucr.DataMining;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.Utility;

namespace NibrsXml.Ucr
{
    public static class Extensions
    {
        #region Nibrs Extensions

        public static String ArrestUcrKey(this List<Arrest> arrests, String ori)
        {
            var earliestArrest = arrests.OrderBy(a => a.Date.Date).FirstOrDefault();
            return earliestArrest == null ? null : earliestArrest.Date.Date.Replace("-", "").Substring(0,6) + ori;
        }

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
        public static string ExtractWeaponGroup(this List<OffenseForce> offenseForces)
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

        #region Ucr Extensions
        /// <summary>
        /// Returns the suggested offense to be used for scoring of columns 5 and 6 of the ReturnA, Arson, and HumanTrafficking reports.
        /// Returns null if no clearances are to be scored for this report.
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public static string ClearanceUcrCode(this Report report)
        {
            if (report.ArrestSubjectAssocs.Any(assoc => assoc.RelatedArrest.Date.Date != null))
            {
                var suggestedClearanceUcrCode = report.ArrestSubjectAssocs.Select(assoc => assoc.RelatedArrest).OrderBy(a => UcrHierarchyMiner.UcrHierarchyOrderArray.IndexOf(a.Charge.UcrCode)).First().Charge.UcrCode;
                return  suggestedClearanceUcrCode;
            }

            var highestRatedOffense = new UcrHierarchyMiner(report.Offenses, report.OffenseVictimAssocs).HighestRatedOffense;

            //If incident is cleared, use highest ranking offense in incident
            return report.Incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceDate == null ||
                report.Incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceDate.Date == null ||
                highestRatedOffense == null
                ? null
                : highestRatedOffense.UcrCode;
        }

        public static string ClearanceDate(this Report report)
        {
            string nibrsDate = null;

            if (report.ArrestSubjectAssocs.Any(assoc => assoc.RelatedArrest.Date.Date != null))
            {
                var clearedArrests = report.ArrestSubjectAssocs.Where(assoc => assoc.RelatedArrest.Date.Date != null);
                var arrestDates = clearedArrests.Select(assoc => assoc.RelatedArrest.Date.Date);
                nibrsDate = arrestDates.Min();
            }
            else if (report.Incident.JxdmIncidentAugmentation != null &&
                report.Incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceDate != null &&
                report.Incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceDate.Date != null)
            {
                nibrsDate = report.Incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceDate.Date;
            }

            return nibrsDate;
        }

        public static bool DoScoreColumn6ForGeneralSummaryData(this Report report)
        {
            bool allAreJuvenile;
            bool hasAtLeastOneJuvenile;
            bool allAreJuvenileOrUnknownAge;

            if (report.ArrestSubjectAssocs.Any())
            {
                var arrestees = report.ArrestSubjectAssocs.Select(assoc => assoc.RelatedArrestee).ToArray();
                allAreJuvenile = arrestees.All(a => a.Person.IsJuvenile);
                hasAtLeastOneJuvenile = arrestees.Any(a => a.Person.IsJuvenile);
                allAreJuvenileOrUnknownAge = arrestees.All(a => a.Person.IsJuvenile || a.Person.AgeIsUnknown);
                return allAreJuvenile || (hasAtLeastOneJuvenile && allAreJuvenileOrUnknownAge);
            }

            if (report.Incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceDate.YearMonthDate == null)
                return false;

            if (!report.Subjects.Any())
                return false;

            allAreJuvenile = report.Subjects.All(s => s.Person.IsJuvenile);
            hasAtLeastOneJuvenile = report.Subjects.Any(s => s.Person.IsJuvenile);
            allAreJuvenileOrUnknownAge = report.Subjects.All(s => s.Person.IsJuvenile || s.Person.AgeIsUnknown);
            return allAreJuvenile || (hasAtLeastOneJuvenile && allAreJuvenileOrUnknownAge);
        }

        /// <summary>
        /// A composite key comprised of the report date and the agency ori
        /// </summary>
        public static string UcrKey(this Report report)
        {
            {
                return report.Incident.ActivityDate.DateTime.Replace("-", "").Substring(0, 6) + report.Header.ReportingAgency.OrgAugmentation.OrgOriId.Id;
            }
        }

        #endregion
    }
}
