using System;
using System.Collections.Generic;
using System.Linq;
using LoadBusinessLayer;
using LoadBusinessLayer.LibrsErrorConstants;
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
        private const string DrugNarcoticLibrsPropDesc = "10";

        public static Report Build(LIBRSIncident incident)
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

                // If incident is some kind of theft or larceny, we need to include cargo theft indicator
                // todo: for now we will always put "false" because WinLIBRS does not capture cargo theft
                if (rpt.Offenses.Any(o => o.UcrCode.MatchOne(NibrsCodeGroups.TheftOffenseFactorCodes)))
                {
                    rpt.Incident.CjisIncidentAugmentation.OffenseCargoTheftIndicator = false.ToString().ToLower();
                }

                BuildLocationsAndLocationAssociations(
                    offenses: rpt.Offenses,
                    locations: rpt.Locations,
                    locationAssociations: rpt.OffenseLocationAssocs,
                    uniqueReportPrefix: uniqueReportPrefix);
                
                BuildItemsAndSubstances(
                    nibrsItems: rpt.Items,
                    nibrsSubstances: rpt.Substances,
                    librsProperties: incident.PropDesc,
                    defaultItemRecoveryDate: rpt.Incident.ActivityDate.Date ?? rpt.Header.ReportDate.YearMonthDateTime.ToString("yyyy-MM-dd"));

                //Build Persons, EnforcementOfficials, Victims, Subjects, Arrestees
                new PersonBuilder().Build(
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
                    victims: rpt.Victims,
                    librsOffenses: incident.Offense);

                return rpt;
            }
            catch (Exception ex)
            {
                //todo: make log writing shared or something
                Console.WriteLine(string.Format("Ori:\t\t{0}\nIncid num:\t{1}\nMessage:\t{2}\nStackTrace:\t{3}", incident.Admin.ORINumber, incident.Admin.IncidentNumber, ex.Message, ex.StackTrace));
                return null;
            }
        }

        #region Helper Functions
        private static List<string> UniqueBiasMotivationCodes(List<LIBRSOffender> offenders)
        {
            var uniqueBiasMotivationCodes = new List<string>();
            foreach (var offender in offenders)
            {
                uniqueBiasMotivationCodes.UniqueAdd(offender.BiasMotivation);
            }
            return uniqueBiasMotivationCodes;
        }

        private static List<string> UniqueSuspectedOfUsingCodes(List<LIBRSOffenderUsing> offenderUsings)
        {
            var uniqueSuspectedofUsingCodes = new List<string>();
            foreach (var offenderUsing in offenderUsings)
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
        private static void BuildLocationsAndLocationAssociations(List<Offense> offenses, List<Location> locations, List<OffenseLocationAssociation> locationAssociations, string uniqueReportPrefix)
        {
            foreach (var offense in offenses)
            {
                //Modify and Use current offense Location Object
                //Add to location list only if category code does not already exist.
                if (locations.All(location => location.CategoryCode != offense.Location.CategoryCode))
                {
                    offense.Location.Id += "Location" + (locations.Count + 1);
                    locations.Add(offense.Location);
                }

                //Create Offense Location Object
                var offenseAssoc = new OffenseLocationAssociation(offense, locations.First(location => location.CategoryCode == offense.Location.CategoryCode));
                locationAssociations.Add(offenseAssoc);
            }
        }

        private static void BuildItemsAndSubstances(List<Item> nibrsItems, List<Substance> nibrsSubstances, List<LIBRSPropertyDescription> librsProperties, string defaultItemRecoveryDate)
        {
            var uniquePropLossTypeDesc = librsProperties.GroupBy(p => Tuple.Create(p.PropertyLossType, p.PropertyDescription));

            foreach (var prop in uniquePropLossTypeDesc)
            {
                if (prop.First().PropertyDescription == DrugNarcoticLibrsPropDesc && prop.First().PropertyLossType == LibrsErrorConstants.PLSeiz)
                {
                    // Translate LIBRS Suspected Drug Type to NIBRS Drug Category Code according to the LIBRS Spec
                    var drugCatCode = prop.First().SuspectedDrugType.Substring(0, 1) == "1" ? "C" : prop.First().SuspectedDrugType.Substring(0, 1);

                    // Translate LIBRS Property Description to NIBRS ItemStatusCode
                    var librsTypeOfPropertyLoss = prop.First().PropertyLossType.TrimStart('0');
                    var nibrsItemStatusCode = ((ItemStatusCode)Enum.Parse(typeof(ItemStatusCode), librsTypeOfPropertyLoss)).NibrsCode();

                    // If no recovery date available, use incident date -- if incident date not available, use report date
                    var recoveryDate = string.IsNullOrWhiteSpace(prop.First().DateRecovered) ? defaultItemRecoveryDate : prop.First().DateRecovered;

                    // Instantiate and add a new Substance object to the list of substances
                    nibrsSubstances.Add(new Substance(
                        drugCategoryCode: drugCatCode,
                        measureDecimalValue: prop.First().EstimatedDrugQty.Trim().TrimStart('0').TrimNullIfEmpty(),
                        substanceUnitCode: prop.First().TypeDrugMeas,
                        statusCode: nibrsItemStatusCode,
                        valueAmount: prop.First().PropertyValue,
                        valueDate: recoveryDate,
                        nibrsPropertyCategoryCode: prop.First().PropertyDescription, 
                        quantity: prop.Count().ToString()));
                }
                else
                {
                    // Translate LIBRS Property Description to NIBRS ItemStatusCode
                    var librsTypeOfPropertyLoss = prop.First().PropertyLossType.TrimStart('0');
                    var nibrsItemStatusCode = ((ItemStatusCode)Enum.Parse(typeof(ItemStatusCode), librsTypeOfPropertyLoss)).NibrsCode();

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

                    // Aggregate values of property
                    var totalValue = prop.Sum(p => Decimal.Parse(p.PropertyValue)).ToString();

                    // Total count
                    //string countOfProperties = prop.First().PropertyDescription.MatchOne(
                    //    PropertyCategoryCode.AUTOMOBILE.NibrsCode(), 
                    //    PropertyCategoryCode.TRUCKS.NibrsCode(), 
                    //    PropertyCategoryCode.BUSES.NibrsCode(), 
                    //    PropertyCategoryCode.RECREATIONAL_VEHICLES.NibrsCode(), 
                    //    PropertyCategoryCode.OTHER_MOTOR_VEHICLES.NibrsCode()) ? prop.Count().ToString(): null;

                    string countOfProperties= prop.Count().ToString();

                    // Instantiate and add a new Item object to the list of items
                    nibrsItems.Add(new Item(
                        statusCode: nibrsItemStatusCode,
                        valueAmount: totalValue,
                        valueDate: prop.First().DateRecovered.IsNullBlankOrEmpty() ? null : prop.First().DateRecovered.ConvertToNibrsYearMonthDay(),
                        nibrsPropCategCode: prop.First().PropertyDescription.TrimNullIfEmpty(),
                        quantity: countOfProperties)); // todo: ??? Data elements 18 and 19 (stolen and recovered vehicle counts) no longer seem to apply for the IEPD format
                }
            }
        }

        private static void BuildArrests(List<Arrest> arrests, LIBRSIncident incident, string uniquePrefix)
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
                                                        var arrest = arrList.First(arr => arr.Rank == minRank);
                                                        return
                                                        new Arrest(
                                                            uniquePrefix: uniquePrefix + incident.Admin.ActionType + "-",
                                                            arrestId: arrest.SeqNum,
                                                            activityId: new ActivityIdentification(arrest.TransactionNumber.Trim()),
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

        private static List<OffenseVictimAssociation> BuildOffenseVictimAssociations(List<Offense> offenses, List<Victim> victims, List<LoadBusinessLayer.LIBRSOffense.LIBRSOffense> librsOffenses)
        {
            return librsOffenses.Where(o => o.OffenseGroup.Equals("A", System.StringComparison.OrdinalIgnoreCase)).Join(
                inner: victims,
                outerKeySelector: offense => int.Parse(offense.OffConnecttoVic.Trim()),
                innerKeySelector: victim => int.Parse(victim.SeqNum.Trim()),
                resultSelector: (offense, victim) => new OffenseVictimAssociation(offenses.First(off => off.UcrCode == offense.AgencyAssignedNibrs), victim)).Distinct().ToList();
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