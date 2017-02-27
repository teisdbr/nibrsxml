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

        public static readonly string[] OtherDangerousWeapons =
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

        public static readonly string[] StrongArmForces =
        {
            ForceCategoryCode.PERSONAL_WEAPONS.NibrsCode(),
            ForceCategoryCode.NONE.NibrsCode()
        };

        public static readonly string[] SimpleAssaultForces =
        {
            ForceCategoryCode.PERSONAL_WEAPONS.NibrsCode(),
            ForceCategoryCode.OTHER.NibrsCode(),
            ForceCategoryCode.UNKNOWN.NibrsCode(),
            ForceCategoryCode.NONE.NibrsCode()
        };

        public static readonly string[] StructureProperties =
        {
            PropertyCategoryCode.SINGLE_OCCUPANCY_DWELLING_STRUCTURE.NibrsCode(),
            PropertyCategoryCode.OTHER_DWELLING_STRUCTURE.NibrsCode(),
            PropertyCategoryCode.OTHER_COMMERCIAL_BUSINESS_STRUCTURE.NibrsCode(),
            PropertyCategoryCode.INDUSTRIAL_MANUFACTURING_STRUCTURE.NibrsCode(),
            PropertyCategoryCode.PUBLIC_COMMUNITY_STRUCTURE.NibrsCode(),
            PropertyCategoryCode.STORAGE_STRUCTURE.NibrsCode(),
            PropertyCategoryCode.OTHER_STRUCTURE.NibrsCode()
        };

        public static readonly string[] OtherMobileProperties =
        {
            PropertyCategoryCode.AIRCRAFT.NibrsCode(),
            PropertyCategoryCode.FARM_EQUIPMENT.NibrsCode(),
            PropertyCategoryCode.CONSTRUCTION_INDUSTRIAL_EQUIPMENT.NibrsCode(),
            PropertyCategoryCode.RECREATIONAL_VEHICLES.NibrsCode(),
            PropertyCategoryCode.WATERCRAFT.NibrsCode(),
            PropertyCategoryCode.TRAILERS.NibrsCode()
        };

        public static readonly string[] TotalOtherProperties =
        {
            PropertyCategoryCode.ALCOHOL.NibrsCode(),
            PropertyCategoryCode.BICYCLES.NibrsCode(),
            PropertyCategoryCode.CLOTHING.NibrsCode(),
            PropertyCategoryCode.COMPUTER_HARDWARE_SOFTWARE.NibrsCode(),
            PropertyCategoryCode.CONSUMABLES.NibrsCode(),
            PropertyCategoryCode.CREDIT_DEBIT_CARDS.NibrsCode(),
            PropertyCategoryCode.DRUGS_NARCOTICS.NibrsCode(),
            PropertyCategoryCode.DRUG_NARCOTIC_EQUIPMENT.NibrsCode(),
            PropertyCategoryCode.FIREARMS.NibrsCode(),
            PropertyCategoryCode.GAMBLING_EQUIPMENT.NibrsCode(),
            PropertyCategoryCode.HOUSEHOLD_GOODS.NibrsCode(),
            PropertyCategoryCode.JEWELRY_PRECIOUS_METALS_GEMS.NibrsCode(),
            PropertyCategoryCode.LIVESTOCK.NibrsCode(),
            PropertyCategoryCode.MERCHANDISE.NibrsCode(),
            PropertyCategoryCode.MONEY.NibrsCode(),
            PropertyCategoryCode.NEGOTIABLE_INSTRUMENTS.NibrsCode(),
            PropertyCategoryCode.NONNEGOTIABLE_INSTRUMENTS.NibrsCode(),
            PropertyCategoryCode.OFFICE_TYPE_EQUIPMENT.NibrsCode(),
            PropertyCategoryCode.PURSES_HANDBAGS_WALLETS.NibrsCode(),
            PropertyCategoryCode.RADIOS_TVS_VCRS_DVD_PLAYERS.NibrsCode(),
            PropertyCategoryCode.AUDIO_VISUAL_RECORDINGS.NibrsCode(),
            PropertyCategoryCode.TOOLS.NibrsCode(),
            PropertyCategoryCode.VEHICLE_PARTS_ACCESSORIES.NibrsCode(),
            PropertyCategoryCode.AIRCRAFT_PARTS_ACCESSORIES.NibrsCode(),
            PropertyCategoryCode.ARTISTIC_SUPPLIES_ACCESSORIES.NibrsCode(),
            PropertyCategoryCode.BUILDING_MATERIALS.NibrsCode(),
            PropertyCategoryCode.CAMPING_HUNTING_FISHING_EQUIPMENT_SUPPLIES.NibrsCode(),
            PropertyCategoryCode.CHEMICALS.NibrsCode(),
            PropertyCategoryCode.COLLECTIONS_COLLECTIBLES.NibrsCode(),
            PropertyCategoryCode.CROPS.NibrsCode(),
            PropertyCategoryCode.PERSONAL_BUSINESS_DOCUMENTS.NibrsCode(),
            PropertyCategoryCode.EXPLOSIVES.NibrsCode(),
            PropertyCategoryCode.FIREARM_ACCESSORIES.NibrsCode(),
            PropertyCategoryCode.FUEL.NibrsCode(),
            PropertyCategoryCode.IDENTITY_DOCUMENTS.NibrsCode(),
            PropertyCategoryCode.IDENTITY.NibrsCode(),
            PropertyCategoryCode.LAW_ENFORCEMENT_EQUIPMENT.NibrsCode(),
            PropertyCategoryCode.LAWN_YARD_GARDEN_EQUIPMENT.NibrsCode(),
            PropertyCategoryCode.LOGGING_EQUIPMENT.NibrsCode(),
            PropertyCategoryCode.MEDICAL_OR_MEDICAL_LAB_EQUIPMENT.NibrsCode(),
            PropertyCategoryCode.METALS.NibrsCode(),
            PropertyCategoryCode.MUSICAL_INSTRUMENTS.NibrsCode(),
            PropertyCategoryCode.PETS.NibrsCode(),
            PropertyCategoryCode.PHOTOGRAPHIC_OPTICAL_EQUIPMENT.NibrsCode(),
            PropertyCategoryCode.PORTABLE_ELECTRONIC_COMMUNICATIONS.NibrsCode(),
            PropertyCategoryCode.RECREATIONAL_SPORTS_EQUIPMENT.NibrsCode(),
            PropertyCategoryCode.OTHER.NibrsCode(),
            PropertyCategoryCode.WATERCRAFT_EQUIPMENT_PARTS_ACCESSORIES.NibrsCode(),
            PropertyCategoryCode.OTHER_WEAPONS.NibrsCode()
        };

        public static readonly string[] OpiumCocaineAndDerivedDrugs =
        {

            DrugCategoryCode.CRACK_COCAINE.NibrsCode(),
            DrugCategoryCode.COCAINE.NibrsCode(),
            DrugCategoryCode.HEROIN.NibrsCode(),
            DrugCategoryCode.MORPHINE.NibrsCode(),
            DrugCategoryCode.OPIUM.NibrsCode()
        };

        public static readonly string[] OtherDangerousNonnarcoticDrugs =
        {
            DrugCategoryCode.LSD.NibrsCode(),
            DrugCategoryCode.PCP.NibrsCode(),
            DrugCategoryCode.OTHER_HALLUCINOGEN.NibrsCode(),
            DrugCategoryCode.AMPHETAMINES_METHAPHETAMINES.NibrsCode(),
            DrugCategoryCode.OTHER_STIMULANT.NibrsCode(),
            DrugCategoryCode.BARBITURATES.NibrsCode(),
            DrugCategoryCode.OTHER_DEPRESSANT.NibrsCode(),
            DrugCategoryCode.OTHER_DRUG.NibrsCode(),
            DrugCategoryCode.UNKNOWN_DRUG_TYPE.NibrsCode(),
            DrugCategoryCode.OVER_THREE_DRUG_TYPES.NibrsCode()
        };

        public static readonly string[] SyntheticNarcotics =
        {
            DrugCategoryCode.OTHER_NARCOTIC.NibrsCode()
        };

        public static readonly string[] Marijuana =
        {
            DrugCategoryCode.HASHISH.NibrsCode(),
            DrugCategoryCode.MARIJUANA.NibrsCode()
        };

        public static readonly string[] SaleOrManufacturingCriminalActivities =
        {
            CriminalActivityCategoryCode.C.NibrsCode(),
            CriminalActivityCategoryCode.D.NibrsCode(),
            CriminalActivityCategoryCode.E.NibrsCode(),
            CriminalActivityCategoryCode.O.NibrsCode(),
            CriminalActivityCategoryCode.T.NibrsCode()
        };

        public static readonly string[] PossessionCriminalActivities =
        {
            CriminalActivityCategoryCode.B.NibrsCode(),
            CriminalActivityCategoryCode.P.NibrsCode(),
            CriminalActivityCategoryCode.U.NibrsCode()
        };

        public static readonly string[] KnownSexCodes =
        {
            SexCode.MALE.NibrsCode(),
            SexCode.FEMALE.NibrsCode()
        };

        public static readonly string[] KnownRaceCodes =
        {
            RACCode.AMERICAN_INDIAN_OR_ALASKAN_NATIVE.NibrsCode(),
            RACCode.ASIAN.NibrsCode(),
            RACCode.BLACK.NibrsCode(),
            RACCode.HAWAIIAN_OR_PACIFIC_ISLANDER.NibrsCode(),
            RACCode.WHITE.NibrsCode()
        };

        public static readonly string[] KnownEthnicityCodes =
        {
            EthnicityCode.HISPANIC_OR_LATINO.NibrsCode(),
            EthnicityCode.NOT_HISPANIC_OR_LATINO.NibrsCode()
        };
    }
}