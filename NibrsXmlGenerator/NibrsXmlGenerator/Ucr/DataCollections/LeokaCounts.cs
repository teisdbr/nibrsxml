using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.Utility;
using TeUtil.Extensions;


namespace NibrsXml.Ucr.DataCollections
{
    public class LeokaCounts : XmlSerializer
    {
        public Dictionary<string, int> CountsDictionary = new Dictionary<string, int>();

        public static string GetWeaponKey(string weapon)
        {
            if (weapon.MatchOne(NibrsCodeGroups.Firearms))
            {
                return "B";
            }
            else if (weapon == ForceCategoryCode.LETHAL_CUTTING_INSTRUMENT.NibrsCode())
            {
                return "C";
            }
            else if (weapon.MatchOne(NibrsCodeGroups.OtherDangerousWeapons))
            {
                return "D";
            }
            else if (weapon.MatchOne(NibrsCodeGroups.StrongArmForces))
            {
                return "E";
            }
            else
            {
                return "";
            }
        }

        public static string GetAssignmentKey(string assignment)
        {
            if (assignment == LEOKAOfficerAssignmentCategoryCode.TWO_OFFICER_VEHICLE.NibrsCode())
            {
                return "F";
            }
            else if (assignment == LEOKAOfficerAssignmentCategoryCode.ONE_OFFICER_VEHICLE_ALONE.NibrsCode())
            {
                return "G";
            }
            else if (assignment == LEOKAOfficerAssignmentCategoryCode.ONE_OFFICER_VEHICLE_ASSISTED.NibrsCode())
            {
                return "H";
            }
            else if (assignment == LEOKAOfficerAssignmentCategoryCode.DETECTIVE_OR_SPECIAL_ASSIGNMENT_ALONE.NibrsCode())
            {
                return "I";
            }
            else if (assignment == LEOKAOfficerAssignmentCategoryCode.DETECTIVE_OR_SPECIAL_ASSIGNMENT_ASSISTED.NibrsCode())
            {
                return "J";
            }
            else if (assignment == LEOKAOfficerAssignmentCategoryCode.OTHER_ALONE.NibrsCode())
            {
                return "K";
            }
            else if (assignment == LEOKAOfficerAssignmentCategoryCode.OTHER_ASSISTED.NibrsCode())
            {
                return "L";
            }
            else
            {
                return "";
            }
        }

        public void IncrementClassificationCounters(string classificationCounterKey, int byValue = 1)
        {
            CountsDictionary.TryIncrement(classificationCounterKey, byValue);
        }
    }
}