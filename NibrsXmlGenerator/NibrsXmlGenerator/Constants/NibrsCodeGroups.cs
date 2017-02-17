using System;
using NibrsXml.Utility;

namespace NibrsXml.Constants
{
    public static class NibrsCodeGroups
    {
        public static readonly string[] VehicleProperties =
        {
            PropertyCategoryCode.AUTOMOBILE.NibrsCode(),
            PropertyCategoryCode.TRUCKS.NibrsCode(),
            PropertyCategoryCode.RECREATIONAL_VEHICLES.NibrsCode(),
            PropertyCategoryCode.BUSES.NibrsCode(),
            PropertyCategoryCode.OTHER_MOTOR_VEHICLES.NibrsCode()
        };

        public static String[] Firearms =
        {
            ForceCategoryCode.FIREARM.NibrsCode(),
            ForceCategoryCode.AUTOMATIC_FIREARM.NibrsCode(),
            ForceCategoryCode.HANDGUN.NibrsCode(),
            ForceCategoryCode.AUTOMATIC_HANDGUN.NibrsCode(),
            ForceCategoryCode.RIFLE.NibrsCode(),
            ForceCategoryCode.AUTOMATIC_RIFLE.NibrsCode(),
            ForceCategoryCode.SHOTGUN.NibrsCode(),
            ForceCategoryCode.AUTOMATIC_SHOTGUN.NibrsCode(),
            ForceCategoryCode.OTHER_FIREARM.NibrsCode(),
            ForceCategoryCode.OTHER_AUTOMATIC_FIREARM.NibrsCode()
        };

        public static String[] OtherDangerousWeapons =
        {
            ForceCategoryCode.BLUNT_OBJECT.NibrsCode(),
            ForceCategoryCode.MOTOR_VEHICLE.NibrsCode(),
            ForceCategoryCode.POISON.NibrsCode(),
            ForceCategoryCode.EXPLOSIVES.NibrsCode(),
            ForceCategoryCode.FIRE_INCENDIARY_DEVICE.NibrsCode(),
            ForceCategoryCode.DRUGS_NARCOTICS_SLEEPING_PILLS.NibrsCode(),
            ForceCategoryCode.ASPHYXIATION.NibrsCode(),
            ForceCategoryCode.OTHER.NibrsCode(),
            ForceCategoryCode.UNKNOWN.NibrsCode()
        };

        public static String[] StrongArmForces =
        {
            ForceCategoryCode.PERSONAL_WEAPONS.NibrsCode(),
            ForceCategoryCode.NONE.NibrsCode()
        };

        public static String[] SimpleAssaultForces =
        {
            ForceCategoryCode.PERSONAL_WEAPONS.NibrsCode(),
            ForceCategoryCode.OTHER.NibrsCode(),
            ForceCategoryCode.UNKNOWN.NibrsCode(),
            ForceCategoryCode.NONE.NibrsCode()
        };
    }
}