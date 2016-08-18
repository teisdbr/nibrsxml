﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoadBusinessLayer;
using LoadBusinessLayer.LIBRSErrorConstants;
using LoadBusinessLayer.LIBRSOffender;
using LoadBusinessLayer.LIBRSOffense;
using LoadBusinessLayer.LIBRSProperty;
using NibrsXml.NibrsReport;
using NibrsXml.Constants;
using NibrsXml.Utility;
using NibrsXml.NibrsReport.Location;
using NibrsXml.NibrsReport.Victim;
using NibrsXml.NibrsReport.Associations;
using NibrsXml.NibrsReport.Item;
using NibrsXml.NibrsReport.Substance;
using LoadBusinessLayer.LIBRS;
using NibrsXml.NibrsReport.Offense;

namespace NibrsXml.Builder
{
    public class ReportBuilder
    {
        private static string drugNarcoticLibrsPropDesc = "10";
        
        public static NibrsReport.Report Build(LIBRSIncident incident)
        {
            //Initialize a new report
            Report rpt = new Report();

            //Determine the unique report prefix to be used for all items that are to be identified within the report
            var uniqueReportPrefix = incident.Admin.ORINumber + "-" + incident.IncidentNumber + "-";

            //Build the report

            rpt.Header = ReportHeaderBuilder.Build(
                offenses: incident.Offense,
                actionType: incident.ActionType,
                admin: incident.Admin);
            rpt.Incident = IncidentBuilder.Build(admin: incident.Admin);
            rpt.Offenses = OffenseBuilder.Build(
                offenses: incident.Offense,
                uniqueBiasMotivationCodes: UniqueBiasMotivationCodes(incident.Offender),
                uniqueSuspectedOfUsingCodes: UniqueSuspectedOfUsingCodes(incident.OffUsing),
                uniqueReportPrefix: uniqueReportPrefix);
            
            //Build list of locations and location associations
            LocationListBuilder(
                offenses: rpt.Offenses,
                locations: rpt.Locations,
                locationAssociations: rpt.OffenseLocationAssocs,
                uniqueReportPrefix: uniqueReportPrefix);
            
            BuildItemsAndSubstances(
                nibrsItems: rpt.Items,
                nibrsSubstances: rpt.Substances,
                librsProperties: incident.PropDesc);
            
            //Build Persons, EnforcementOfficials, Victims, Subjects, Arrestees
            PersonBuilder.Build(
                persons: rpt.Persons, 
                victims: rpt.Victims,
                officers: rpt.Officers,
                subjects: rpt.Subjects,
                arrestees: rpt.Arrestees,
                arrests: rpt.Arrests,
                subjectVictimAssocs: rpt.SubjectVictimAssocs,
                incident: incident, 
                uniquePrefix: uniqueReportPrefix);
            //todo: ArresteeBuilder
            //todo: ArrestBuilder

            //todo: ArrestSubjectAssociationBuilder
            rpt.OffenseVictimAssocs = BuildOffenseVictimAssociations(
                offenses: rpt.Offenses,
                victims: rpt.Victims);
            //todo: SubjectVictimAssociationBuilder
            return rpt;
        }

        #region Helper Functions
        private static List<String> UniqueBiasMotivationCodes(List<LIBRSOffender> offenders)
        {
            List<string> uniqueBiasMotivationCodes = new List<string>();
            foreach (LIBRSOffender offender in offenders)
            {
                uniqueBiasMotivationCodes.UniqueAdd(offender.BiasMotivation);
            }
            return uniqueBiasMotivationCodes;
        }

        private static List<String> UniqueSuspectedOfUsingCodes(List<LIBRSOffenderUsing> offenderUsings)
        {
            List<string> uniqueSuspectedofUsingCodes = new List<string>();
            foreach (LIBRSOffenderUsing offenderUsing in offenderUsings)
            {
                uniqueSuspectedofUsingCodes.UniqueAdd(offenderUsing.OffUsingGamingMot);
            }
            return uniqueSuspectedofUsingCodes;
        } 
        #endregion

        #region Builder Functions
        /// <summary>
        /// This method initializes the list of Locations and the list of OffenseLocationAssociations
        /// </summary>
        /// <param name="offenses">List of offenses to build locations from</param>
        /// <param name="locations">List of locations to match with offenses (This is empty, but initialized)</param>
        /// <param name="locationAssociations">List of location associations to match to locations</param>
        private static void LocationListBuilder(List<Offense> offenses, List<Location> locations, List<OffenseLocationAssociation> locationAssociations, String uniqueReportPrefix)
        {
            foreach (var offense in offenses)
            {
                //Modify and Use current offense Location Object
                //Add to location list only if category code does not already exist.
                if ((locations.Where(location => location.CategoryCode == offense.Location.CategoryCode)).Count() == 0)
                {
                    offense.Location.Id += "Location" + (locations.Count + 1).ToString();
                    locations.Add(offense.Location);
                }

                //Create Offense Location Object
                OffenseLocationAssociation offenseAssoc = new OffenseLocationAssociation();
                offenseAssoc.OffenseRef = offense.Reference;
                offenseAssoc.LocationRef = locations.Where(location => location.CategoryCode == offense.Location.CategoryCode).First().Reference;
                locationAssociations.Add(offenseAssoc);
            }
        }

        private static void BuildItemsAndSubstances(List<Item> nibrsItems, List<Substance> nibrsSubstances, List<LIBRSPropertyDescription> librsProperties)
        {
            foreach (LIBRSPropertyDescription prop in librsProperties)
            {
                if (prop.PropertyDescription == drugNarcoticLibrsPropDesc && prop.PropertyLossType == LIBRSErrorConstants.PLSeiz)
                {
                    // Translate LIBRS Suspected Drug Type to NIBRS Drug Category Code according to the LIBRS Spec
                    String drugCatCode = prop.SuspectedDrugType.Substring(0, 1) == "1" ? "C" : prop.SuspectedDrugType.Substring(0, 1);

                    // Instantiate and add a new Substance object to the list of substances
                    nibrsSubstances.Add(new Substance(
                        drugCategoryCode: drugCatCode,
                        measureDecimalValue: prop.EstimatedDrugQty,
                        substanceUnitCode: prop.TypeDrugMeas));
                }
                else
                {
                    // Translate LIBRS Property Description to NIBRS ItemStatusCode
                    string librsTypeOfPropertyLoss = prop.PropertyLossType.TrimStart('0');
                    string nibrsItemStatusCode = ((ItemStatusCode)Enum.Parse(typeof(ItemStatusCode), librsTypeOfPropertyLoss)).NibrsCode();

                    if (nibrsItemStatusCode == ItemStatusCode.NONE.NibrsCode()) // todo: maybe UNKNOWN will need to ignore extra elements
                    {
                        nibrsItems.Add(new Item(
                            statusCode: nibrsItemStatusCode,
                            valueAmount: null,
                            valueDate: null,
                            nibrsPropCategCode: null,
                            quantity: null));
                        continue;
                    }

                    // Instantiate and add a new Item object to the list of items
                    nibrsItems.Add(new Item(
                        statusCode: nibrsItemStatusCode,
                        valueAmount: prop.PropertyValue.Trim() == String.Empty ? null : prop.PropertyValue.Trim(),
                        valueDate: prop.DateRecovered.Trim() == String.Empty ? null : prop.DateRecovered.ConvertToNibrsYearMonthDay(),
                        nibrsPropCategCode: prop.PropertyDescription,
                        quantity: null)); // todo: ??? Data elements 18 and 19 (stolen and recovered vehicle counts) no longer seem to apply for the IEPD format
                }
            }
        }

        private static List<OffenseVictimAssociation> BuildOffenseVictimAssociations(List<Offense> offenses, List<Victim> victims)
        {
            return offenses.Join(
                inner: victims.Where(victim => victim.CategoryCode == LIBRSErrorConstants.VTIndividual || victim.CategoryCode == LIBRSErrorConstants.VTLawEnfOfficer),
                outerKeySelector: offense => offense.librsVictimSequenceNumber,
                innerKeySelector: victim => victim.SeqNum,
                resultSelector: (offense, victim) => new OffenseVictimAssociation(offense, victim)).ToList();
        }
        #endregion
    }
}
