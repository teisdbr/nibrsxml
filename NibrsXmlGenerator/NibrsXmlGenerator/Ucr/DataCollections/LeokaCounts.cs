using System.Collections.Generic;
using System.Xml.Serialization;
using NibrsModels.Constants;
using NibrsModels.Utility;
using NibrsXml.Constants;
using NibrsXml.Constants.Ucr;
using NibrsXml.Utility;
using Util.Extensions;

namespace NibrsXml.Ucr.DataCollections
{
    public class LeokaCounts : XmlSerializer
    {
        public Dictionary<string, int> CountsDictionary { get; set; }

        public LeokaCounts()
        {
            CountsDictionary = new Dictionary<string, int>();
        }

        public static string GetWeaponKey(string weapon)
        {
            if (weapon.MatchOne(UcrCodeGroups.Firearms))
                return "B";

            if (weapon == ForceCategoryCode.LETHAL_CUTTING_INSTRUMENT.NibrsCode())
                return "C";

            if (weapon.MatchOne(UcrCodeGroups.OtherDangerousWeapons))
                return "D";

            if (weapon.MatchOne(UcrCodeGroups.StrongArmForces))
                return "E";

            return "";
        }

        public static string GetAssignmentKey(string assignment)
        {
            if (assignment == LEOKAOfficerAssignmentCategoryCode.TWO_OFFICER_VEHICLE.NibrsCode())
                return "F";
            
            if (assignment == LEOKAOfficerAssignmentCategoryCode.ONE_OFFICER_VEHICLE_ALONE.NibrsCode())
                return "G";

            if (assignment == LEOKAOfficerAssignmentCategoryCode.ONE_OFFICER_VEHICLE_ASSISTED.NibrsCode())
                return "H";

            if (assignment == LEOKAOfficerAssignmentCategoryCode.DETECTIVE_OR_SPECIAL_ASSIGNMENT_ALONE.NibrsCode())
                return "I";

            if (assignment == LEOKAOfficerAssignmentCategoryCode.DETECTIVE_OR_SPECIAL_ASSIGNMENT_ASSISTED.NibrsCode())
                return "J";

            if (assignment == LEOKAOfficerAssignmentCategoryCode.OTHER_ALONE.NibrsCode())
                return "K";

            if (assignment == LEOKAOfficerAssignmentCategoryCode.OTHER_ASSISTED.NibrsCode())
                return "L";

            return "";
        }

        public void IncrementClassificationCounters(string classificationCounterKey, int byValue = 1)
        {
            CountsDictionary.TryIncrement(classificationCounterKey, byValue);
        }
    }
}