﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NibrsXml.Ucr.DataCollections;
using NibrsXml.Utility;
using TeUtil.Extensions;
using NibrsXml.Constants;
using NibrsXml.NibrsReport;
using NibrsXml.NibrsReport.Item;

namespace NibrsXml.Ucr.DataMining
{
    class ArsonMiner
    {
        private static readonly String[] VehicleProperties = {
            PropertyCategoryCode.AUTOMOBILE.NibrsCode(),
            PropertyCategoryCode.BUSES.NibrsCode(),
            PropertyCategoryCode.OTHER_MOTOR_VEHICLES.NibrsCode(),
            PropertyCategoryCode.TRUCKS.NibrsCode()
        };

        private static readonly String[] StructureProperties = {
            PropertyCategoryCode.SINGLE_OCCUPANCY_DWELLING_STRUCTURE.NibrsCode(),
            PropertyCategoryCode.OTHER_DWELLING_STRUCTURE.NibrsCode(),
            PropertyCategoryCode.OTHER_COMMERCIAL_BUSINESS_STRUCTURE.NibrsCode(),
            PropertyCategoryCode.INDUSTRIAL_MANUFACTURING_STRUCTURE.NibrsCode(),
            PropertyCategoryCode.PUBLIC_COMMUNITY_STRUCTURE.NibrsCode(),
            PropertyCategoryCode.STORAGE_STRUCTURE.NibrsCode(),
            PropertyCategoryCode.OTHER_STRUCTURE.NibrsCode(),
        };

        private static readonly String[] OtherMobileProperties = {
            PropertyCategoryCode.AIRCRAFT.NibrsCode(),
            PropertyCategoryCode.FARM_EQUIPMENT.NibrsCode(),
            PropertyCategoryCode.CONSTRUCTION_INDUSTRIAL_EQUIPMENT.NibrsCode(),
            PropertyCategoryCode.RECREATIONAL_VEHICLES.NibrsCode(),
            PropertyCategoryCode.WATERCRAFT.NibrsCode(),
            PropertyCategoryCode.TRAILERS.NibrsCode(),
        };

        private static readonly String[] TotalOtherProperties =
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
            PropertyCategoryCode.OTHER_WEAPONS.NibrsCode(),
        };

        public static void Mine(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report)
        {
            // Return if no arson data to query
            if (!report.Offenses.Any(offense => offense.UcrCode.Matches(Arson.ArsonUcrCode)) || !report.Items.Any(i => i.Status.Code == ItemStatusCode.BURNED.NibrsCode()))
                return;

            //Get Classification Counts to operate on for this report
            var arsonData = monthlyReportData[report.UcrKey].ArsonData;

            //Get all arson offenses in report.
            var offenses =
                report.Offenses.Where(offense => offense.UcrCode.Matches(Arson.ArsonUcrCode) && offense.AttemptedIndicator == "false").ToList();

            //Determine property to use based on ucr hierarchy: structure -> mobile -> other
            var selectedProperty = GetPropertyToUse(report.Items.Where(i => i.Status.Code == ItemStatusCode.BURNED.NibrsCode()).ToList());
            if (selectedProperty == null) return;

            //Identify key to use based on identified property and score actual counts
            var keyForScoring = GetPropertyClassification(selectedProperty.NibrsPropertyCategoryCode);

            #region Column 4
            
            //Use the corresponding key to increment the count of actual offenses.
            arsonData.IncrementActualOffense(keyForScoring);
            #endregion

            #region Column 5

            //Clearances - Count one if exceptional clearance or arrest exists.
            if (report.Incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceDate != null || report.Arrests.Any())
            {
                arsonData.IncrementAllClearences(keyForScoring);
            }

            #endregion

            #region Column 6

            //Clearances Involving Juveniles - Count only one if arrest exists.
            if (report.Arrestees.Any(a => a.Person.AgeMeasure.IsJuvenile))
            {
                arsonData.IncrementJuvenileClearences(keyForScoring);
            }

            #endregion

            //Column 7 is not available in NIBRS per the documentation. This needs to be verified given that other data in a similar fashion has since been 
            //been incorporated into NIBRS.

            #region Column 8

            //Add value of all properties stolen regardless of description, priorities, etc. It may not be necessary to filter only by properties that apply.
            var totalValueOfBurnedProperties = report.Items.Sum(item => Convert.ToInt64(item.Value.ValueAmount.Amount));
            arsonData.IncrementEstimatedValueOfPropertyDamage(keyForScoring, totalValueOfBurnedProperties);

            #endregion
        }

        /// <summary>
        /// Translates the property classification from a property description.
        /// Property classification is synonymous to the row header of the arson report.
        /// </summary>
        /// <param name="propertyDescription"></param>
        /// <returns></returns>
        private static String GetPropertyClassification(String propertyDescription)
        {
            switch (propertyDescription)
            {
                //Structure Properties
                case "29":
                    return "A";
                case "30":
                    return "B";
                case "34":
                    return "C";
                case "32":
                    return "D";
                case "31":
                    return "E";
                case "33":
                    return "F";
                case "35":
                    return "G";
                //Motor Vehicle Properties
                case "03":
                case "05":
                case "24":
                case "37":
                    return "H";
                //Other Mobile Properties
                case "01":
                case "12":
                case "15":
                case "28":
                case "39":
                case "78":
                    return "I";
                //Total Other Properties
                case "02":
                case "04":
                case "06":
                case "07":
                case "08":
                case "09":
                case "10":
                case "11":
                case "13":
                case "14":
                case "16":
                case "17":
                case "18":
                case "19":
                case "20":
                case "21":
                case "22":
                case "23":
                case "25":
                case "26":
                case "27":
                case "36":
                case "38":
                case "41":
                case "42":
                case "43":
                case "44":
                case "45":
                case "46":
                case "47":
                case "48":
                case "49":
                case "59":
                case "64":
                case "65":
                case "66":
                case "67":
                case "68":
                case "69":
                case "70":
                case "71":
                case "72":
                case "73":
                case "74":
                case "75":
                case "76":
                case "77":
                case "79":
                case "80":
                    return "J";
                default:
                    return "J";
            }
        }

        /// <summary>
        /// It returns a property based on the highest value and hierarchical requirements.
        /// </summary>
        public static Item GetPropertyToUse(List<Item> burnedItems)
        {
            //Get at most one property per offense.
            //----Gather only one structure property with the highest value. Given OrderBy sorts in ascending, Last() provides desired property.
            var structureProperty =
                burnedItems.Where(i => i.NibrsPropertyCategoryCode.MatchOne(StructureProperties)).Max();
            //--------If structureProperty is not null, return it
            if (structureProperty != null) return structureProperty;

            //----Gather only one vehicle as above.
            var vehicleProperty =
                burnedItems.Where(i => i.NibrsPropertyCategoryCode.MatchOne(VehicleProperties)).Max();

            //--------If structureProperty is not null, return it
            if (vehicleProperty != null) return vehicleProperty;

            //----Gather all other mobile properties
            var otherMobileProperty =
                burnedItems.Where(i => i.NibrsPropertyCategoryCode.MatchOne(OtherMobileProperties)).Max();

            //--------If structureProperty is not null, return it
            if (otherMobileProperty != null) return otherMobileProperty;

            //----Gather Total Other Property
            var totalOtherProperty = burnedItems.FirstOrDefault(i => i.NibrsPropertyCategoryCode.MatchOne(TotalOtherProperties));

            //--------Return Total other property or null if code reached this point.
            return totalOtherProperty;
        }
    }
}