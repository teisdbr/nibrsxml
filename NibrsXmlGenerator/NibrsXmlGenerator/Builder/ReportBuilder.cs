using System;
using System.Collections.Generic;
using System.Linq;
using LoadBusinessLayer;
using LoadBusinessLayer.LIBRSErrorConstants;
using LoadBusinessLayer.LIBRSOffender;
using LoadBusinessLayer.LIBRSProperty;
using NibrsXml.NibrsReport;
using NibrsXml.Constants;
using NibrsXml.Utility;
using NibrsXml.NibrsReport.Location;
using NibrsXml.NibrsReport.Victim;
using NibrsXml.NibrsReport.Associations;
using NibrsXml.NibrsReport.Item;
using NibrsXml.NibrsReport.Substance;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.NibrsReport.Arrest;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.NibrsReport.Arrestee;
using TeUtil.Extensions;

namespace NibrsXml.Builder
{
    public class ReportBuilder
    {
        private static string drugNarcoticLibrsPropDesc = "10";
        
        public static NibrsReport.Report Build(LIBRSIncident incident)
        {
            try
            {
                //Initialize a new report
                var rpt = new Report();

                //Determine the unique report prefix to be used for all items that are to be identified within the report
                incident.IncidentNumber = incident.IncidentNumber.Trim();
                var uniqueReportPrefix = incident.Admin.ORINumber + "-" + incident.IncidentNumber + "-";

                //Build the report

                //Depending on the Nibrs report category and the action type code,
                //this function may return early when the minimal amount of data required is met for delete action types

                rpt.Header = ReportHeaderBuilder.Build(
                    offenses: incident.Offense,
                    actionType: incident.Admin.ActionType,
                    admin: incident.Admin);

                if (rpt.Header.NibrsReportCategoryCode == NibrsReportCategoryCode.A.NibrsCode())
                {
                    rpt.Incident = IncidentBuilder.Build(admin: incident.Admin);

                    //Send only the incident for group A deletes
                    if (incident.Admin.ActionType == "D") return rpt;
                }

                BuildArrests(
                    arrests: rpt.Arrests,
                    incident: incident,
                    uniquePrefix: uniqueReportPrefix);

                if (rpt.Header.NibrsReportCategoryCode == NibrsReportCategoryCode.B.NibrsCode())
                {
                    //Do not send any group B reports if there are no accompanying arrests
                    if (rpt.Arrests.Count == 0) return null;

                    //Send only the arrests for group B deletes
                    if (incident.Admin.ActionType == "D") return rpt;

                    rpt.Incident = IncidentBuilder.Build(admin: incident.Admin);
                }

                rpt.Offenses = OffenseBuilder.Build(
                    offenses: incident.Offense,
                    uniqueBiasMotivationCodes: UniqueBiasMotivationCodes(incident.Offender),
                    uniqueSuspectedOfUsingCodes: UniqueSuspectedOfUsingCodes(incident.OffUsing),
                    uniqueReportPrefix: uniqueReportPrefix);

                BuildLocationsAndLocationAssociations(
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
                    subjectVictimAssocs: rpt.SubjectVictimAssocs,
                    incident: incident,
                    uniquePrefix: uniqueReportPrefix);

                rpt.ArrestSubjectAssocs = BuildArrestSubjectAssociation(
                    arrests: rpt.Arrests,
                    arrestees: rpt.Arrestees);

                rpt.OffenseVictimAssocs = BuildOffenseVictimAssociations(
                    offenses: rpt.Offenses,
                    victims: rpt.Victims);

                return rpt;
            }
            catch (Exception ex)
            {
                //todo: make log writing shared or something
                Console.WriteLine(String.Format("Ori:\t\t{0}\nIncid num:\t{1}\nMessage:\t{2}\nStackTrace:\t{3}", incident.Admin.ORINumber, incident.Admin.IncidentNumber, ex.Message, ex.StackTrace));
                return null;
            }
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
        private static void BuildLocationsAndLocationAssociations(List<Offense> offenses, List<Location> locations, List<OffenseLocationAssociation> locationAssociations, String uniqueReportPrefix)
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
                        measureDecimalValue: prop.EstimatedDrugQty.Trim().TrimStart('0').TrimNullIfEmpty(),
                        substanceUnitCode: prop.TypeDrugMeas));
                }
                else
                {
                    // Translate LIBRS Property Description to NIBRS ItemStatusCode
                    string librsTypeOfPropertyLoss = prop.PropertyLossType.TrimStart('0');
                    string nibrsItemStatusCode = ((ItemStatusCode)Enum.Parse(typeof(ItemStatusCode), librsTypeOfPropertyLoss)).NibrsCode();

                    // todo: ??? May need to also create the minimal Item within condition for when the Property Loss Type (ItemStatusCode) is Unknown (8) 
                    if (nibrsItemStatusCode == ItemStatusCode.NONE.NibrsCode()) 
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
                        valueAmount: prop.PropertyValue.Trim().TrimStart('0').TrimNullIfEmpty(),
                        valueDate: prop.DateRecovered.IsNullBlankOrEmpty() ? null : prop.DateRecovered.ConvertToNibrsYearMonthDay(),
                        nibrsPropCategCode: prop.PropertyDescription.TrimNullIfEmpty(),
                        quantity: null)); // todo: ??? Data elements 18 and 19 (stolen and recovered vehicle counts) no longer seem to apply for the IEPD format
                }
            }
        }

        private static void BuildArrests(List<Arrest> arrests, LIBRSIncident incident, String uniquePrefix)
        {
            var arrestList =
                incident.Arrestee.Join(
                    inner: incident.ArrStatute,
                    outerKeySelector: arrest => arrest.ArrestSeqNum,
                    innerKeySelector: arrlrs => arrlrs.ArrestSeqNum,
                    resultSelector: (arrest, lrs) => new
                    {
                        TransactionNumber = arrest.ArrestNumber,
                        ActivityDate = arrest.ArrestDate.ConvertToNibrsYearMonthDay(),
                        Charge = lrs.AgencyAssignedNibrs.HasValue(trim:true) ? lrs.AgencyAssignedNibrs : LarsList.LarsDictionary[lrs.LrsNumber.Trim()].Nibr,
                        CategoryCode = arrest.ArrestType,
                        ArrestCount = arrest.MultipleArresteeIndicator,
                        SeqNum = arrest.ArrestSeqNum,
                        //TODO: MAKE SURE TO VERIFY WHETHER THE FOLLOWING CODE SHOULD BE MODIFIED TO TAKE INTO CONSIDERATION AGENCYASSIGNEDNIBRS
                        Rank = Convert.ToDouble(LarsList.LarsDictionary[lrs.LrsNumber.Trim()].Lrank)
                    }
                ).ToList();

            var groupedArrests = arrestList.GroupBy(arr => arr.SeqNum,
                                                    arr => arr,
                                                    (seq, arrList) => { 
                                                        var minRank = arrList.Min(arr => arr.Rank);
                                                        var arrest = arrList.Where(arr => arr.Rank == minRank).First();
                                                        return
                                                        new Arrest(
                                                            uniquePrefix: uniquePrefix + incident.Admin.ActionType + "-",
                                                            arrestId: arrest.SeqNum,
                                                            activityId: new NibrsReport.Misc.ActivityIdentification(arrest.TransactionNumber.Trim()),
                                                            date: new ActivityDate(arrest.ActivityDate),
                                                            charge: new ArrestCharge(arrest.Charge),
                                                            categoryCode: arrest.CategoryCode,
                                                            subjectCountCode: arrest.ArrestCount
                                                            );
                                                    }).ToList();

            foreach (var arr in groupedArrests)
            {
                arrests.Add(arr);
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

        private static List<ArrestSubjectAssociation> BuildArrestSubjectAssociation(List<Arrest> arrests, List<Arrestee> arrestees)
        {
            return arrests.Join(
                inner: arrestees,
                outerKeySelector: (Arrest arrest) => arrest.SequenceNumber,
                innerKeySelector: (Arrestee arrestee) => arrestee.SeqId,
                resultSelector: (arrest, arrestee) => new ArrestSubjectAssociation(arrest,arrestee)).ToList();
        }
        #endregion
    }
}