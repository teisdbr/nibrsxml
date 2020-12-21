using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LoadBusinessLayer;
using LoadBusinessLayer.LibrsErrorConstants;
using LoadBusinessLayer.LIBRSOffender;
using LoadBusinessLayer.LIBRSOffense;
using LoadBusinessLayer.LIBRSProperty;
using NibrsModels.Constants;
using NibrsModels.NibrsReport;
using NibrsModels.NibrsReport.Arrest;
using NibrsModels.NibrsReport.Arrestee;
using NibrsModels.NibrsReport.Associations;
using NibrsModels.NibrsReport.Item;
using NibrsModels.NibrsReport.Location;
using NibrsModels.NibrsReport.Misc;
using NibrsModels.NibrsReport.Offense;
using NibrsModels.NibrsReport.ReportHeader;
using NibrsModels.NibrsReport.Substance;
using NibrsModels.NibrsReport.Victim;
using NibrsModels.Utility;
using TeUtil.Extensions;


namespace NibrsXml.Builder
{
    public class ReportBuilder
    {
        private const string DrugNarcoticLibrsPropDesc = "10";

        public static Report Build(LIBRSIncident incident, string reportingMonth, string reportingYear)
        {
            //Initialize a new report
            var rpt = new Report();
          

            try
            {             

                //Determine the unique report prefix to be used for all items that are to be identified within the report
                incident.IncidentNumber = incident.IncidentNumber.Trim();
                var uniqueReportPrefix = incident.Admin.ORINumber + "-" + incident.IncidentNumber + "-";

                //Build the report

                //Depending on the Nibrs report category and the action type code,
                //this function may return early when the minimal amount of data required is met for delete action types

                rpt.Header = ReportHeaderBuilder.Build(
                    //*
                    incident.Offense,
                    incident.Admin.ActionType,
                    incident.Admin,
                    reportingYear + "-" + reportingMonth);

                if (rpt.Header.NibrsReportCategoryCode == NibrsReportCategoryCode.A.NibrsCode())
                {
                    rpt.Incident = IncidentBuilder.Build(incident.Admin);
                   

                    //Send only the incident for group A deletes
                    if (incident.Admin.ActionType == "D") 
                       return rpt;
                }

                BuildArrests(
                    rpt.Arrests,
                    incident,
                    uniqueReportPrefix);

                if (rpt.Header.NibrsReportCategoryCode == NibrsReportCategoryCode.B.NibrsCode())
                {
                    //Do not send any group B reports if there are no accompanying arrests
                    if (rpt.Arrests.Count == 0) {
                       
                        return null;
                    }
                       


                    //Send only incident and arrests for group B deletes
                    if ( incident.Admin.ActionType == "D")
                    {
                        //if (rpt.Arrests.Count == 0)
                        //    return null;
                        // construct Arrest and Arrestee segements to report Group B Arrest Delete
                        rpt.Arrests.ForEach(arr => rpt.Arrestees.Add(new Arrestee(arr.SequenceNumber, uniqueReportPrefix)));
                        rpt.ArrestSubjectAssocs = BuildArrestSubjectAssociation(
                             rpt.Arrests,
                            rpt.Arrestees);
                        return rpt;
                    }

                    rpt.Incident = IncidentBuilder.Build(incident.Admin);
                    rpt.Header.ReportActionCategoryCode = "A";
                }


                rpt.Offenses = OffenseBuilder.Build(
                    incident.Offense,
                    UniqueBiasMotivationCodes(incident.Offender),
                    UniqueSuspectedOfUsingCodes(incident.OffUsing),
                    uniqueReportPrefix);

                // If incident is some kind of theft or larceny, we need to include cargo theft indicator
                // todo: for now we will always put "false" because WinLIBRS does not capture cargo theft
                if (rpt.Offenses.Any(o => o.UcrCode.MatchOne(NibrsCodeGroups.TheftOffenseFactorCodes)))
                    rpt.Incident.CjisIncidentAugmentation.OffenseCargoTheftIndicator = false.ToString().ToLower();

                BuildLocationsAndLocationAssociations(
                    rpt.Offenses,
                    rpt.Locations,
                    rpt.OffenseLocationAssocs,
                    uniqueReportPrefix);

                BuildItemsAndSubstances(
                    rpt.Items,
                    rpt.Substances,
                    incident.PropDesc,
                    rpt.Incident.ActivityDate.Date ?? rpt.Header.ReportDate.YearMonthDateTime.ToString("yyyy-MM-dd"));

                //Build Persons, EnforcementOfficials, Victims, Subjects, Arrestees
                new PersonBuilder().Build(
                    rpt.Persons,
                    rpt.Header,
                    rpt.Victims,
                    officers: rpt.Officers,
                    subjects: rpt.Subjects,
                    arrestees: rpt.Arrestees,
                    subjectVictimAssocs: rpt.SubjectVictimAssocs,
                    incident: incident,
                    uniquePrefix: uniqueReportPrefix);

                rpt.ArrestSubjectAssocs = BuildArrestSubjectAssociation(
                    rpt.Arrests,
                    rpt.Arrestees);

                rpt.OffenseVictimAssocs = BuildOffenseVictimAssociations(
                    rpt.Offenses,
                    rpt.Victims,
                    incident.Offense);

                return rpt;
            }
            catch (Exception ex)
            {
                //todo: make log writing shared or something
                Console.WriteLine("Ori:\t\t{0}\nIncid num:\t{1}\nMessage:\t{2}\nStackTrace:\t{3}",
                    incident.Admin.ORINumber, incident.Admin.IncidentNumber, ex.Message, ex.StackTrace);
                throw new Exception($"Ori: {incident.Admin.ORINumber}, Incid num:{incident.Admin.IncidentNumber}, Message:{ex.Message}, Stack Trace: {ex.StackTrace}",ex.InnerException);

            }
        }

        #region Helper Functions


      

        private static List<string> UniqueBiasMotivationCodes(List<LIBRSOffender> offenders)
        {
            var uniqueBiasMotivationCodes = new List<string>();
            foreach (var offender in offenders) uniqueBiasMotivationCodes.UniqueAdd(offender.BiasMotivation);
            return uniqueBiasMotivationCodes;
        }

        private static List<string> UniqueSuspectedOfUsingCodes(List<LIBRSOffenderUsing> offenderUsings)
        {
            var uniqueSuspectedofUsingCodes = new List<string>();
            foreach (var offenderUsing in offenderUsings)
                uniqueSuspectedofUsingCodes.UniqueAdd(offenderUsing.OffUsingGamingMot);
            return uniqueSuspectedofUsingCodes;
        }

        #endregion

        #region Builder Functions




        /// <summary>
        ///     This method initializes the list of Locations and the list of OffenseLocationAssociations
        /// </summary>
        /// <param name="offenses">List of offenses to build locations from</param>
        /// <param name="locations">List of locations to match with offenses (This is empty, but initialized)</param>
        /// <param name="locationAssociations">List of location associations to match to locations</param>
        private static void BuildLocationsAndLocationAssociations(List<Offense> offenses, List<Location> locations,
            List<OffenseLocationAssociation> locationAssociations, string uniqueReportPrefix)
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
                var offenseAssoc = new OffenseLocationAssociation(offense,
                    locations.First(location => location.CategoryCode == offense.Location.CategoryCode));
                locationAssociations.Add(offenseAssoc);
            }
        }

        private static void BuildItemsAndSubstances(List<Item> nibrsItems, List<Substance> nibrsSubstances,
            List<LIBRSPropertyDescription> librsProperties, string defaultItemRecoveryDate)
        {
            var uniquePropLossTypeDesc =
                librsProperties.GroupBy(p => Tuple.Create(p.PropertyLossType, p.PropertyDescription));

            foreach (var prop in uniquePropLossTypeDesc)
                if (prop.First().PropertyDescription == DrugNarcoticLibrsPropDesc &&  (prop.First().PropertyLossType == LibrsErrorConstants.PLNone ||
                    prop.First().PropertyLossType == LibrsErrorConstants.PLSeiz))
                {
                    // Translate LIBRS Suspected Drug Type to NIBRS Drug Category Code according to the LIBRS Spec
                    var drugCatCode = prop.First().SuspectedDrugType.Substring(0, 1) == "1"
                        ? "C"
                        : prop.First().SuspectedDrugType.Substring(0, 1);

                    // Translate LIBRS Property Description to NIBRS ItemStatusCode
                    var librsTypeOfPropertyLoss = prop.First().PropertyLossType.TrimStart('0');
                    var nibrsItemStatusCode =
                        ((ItemStatusCode) Enum.Parse(typeof(ItemStatusCode), librsTypeOfPropertyLoss)).NibrsCode();

                    // If no recovery date available, use incident date -- if incident date not available, use report date
                    var recoveryDate = string.IsNullOrWhiteSpace(prop.First().DateRecovered)
                        ? defaultItemRecoveryDate
                        : prop.First().DateRecovered;

                    // Instantiate and add a new Substance object to the list of substances
                    nibrsSubstances.Add(new Substance(
                        drugCatCode,
                        prop.First().EstimatedDrugQty.TrimNullIfEmpty(),
                        prop.First().TypeDrugMeas,
                        nibrsItemStatusCode,
                        prop.First().PropertyValue,
                        recoveryDate,
                        prop.First().PropertyDescription,
                        prop.Count().ToString()));
                }
                else
                {
                    // Translate LIBRS Property Description to NIBRS ItemStatusCode
                    var librsTypeOfPropertyLoss = prop.First().PropertyLossType.TrimStart('0');
                    var nibrsItemStatusCode =
                        ((ItemStatusCode) Enum.Parse(typeof(ItemStatusCode), librsTypeOfPropertyLoss)).NibrsCode();

                    // todo: ??? May need to also create the minimal Item within condition for when the Property Loss Type (ItemStatusCode) is Unknown (8) 
                    if (nibrsItemStatusCode == ItemStatusCode.NONE.NibrsCode() || nibrsItemStatusCode == ItemStatusCode.UNKNOWN.NibrsCode())
                    {
                        nibrsItems.Add(new Item(
                            nibrsItemStatusCode,
                            null,
                            null,
                            null,
                            null));
                        continue;
                    }

                    // Aggregate values of property
                    var totalValue = prop.Sum(p =>
                    {
                        try
                        {
                            return decimal.Parse(p.PropertyValue);
                        }
                        catch
                        {
                            return 0;
                        }
                        
                    }).ToString(CultureInfo.InvariantCulture);

                    var propDes = prop.First().PropertyDescription.TrimNullIfEmpty();
                    var countOfProperties = NibrsCodeGroups.VehicleProperties.Contains(propDes) ? prop.Count().ToString() : null;

                    // Instantiate and add a new Item object to the list of items
                    nibrsItems.Add(new Item(
                        nibrsItemStatusCode,
                        totalValue,
                        prop.First().DateRecovered.IsNullBlankOrEmpty()
                            ? null
                            : prop.First().DateRecovered.ConvertToNibrsYearMonthDay(),
                        propDes,
                        countOfProperties));
                }
        }

        private static void BuildArrests(List<Arrest> arrests, LIBRSIncident incident, string uniquePrefix)
        {
            var arrestList =
                incident.Arrestee.Join(
                    incident.ArrStatute,
                    arrest => arrest.ArrestSeqNum,
                    arrlrs => arrlrs.ArrestSeqNum,
                    (arrest, lrs) => new
                    {
                        TransactionNumber = arrest.ArrestNumber,
                        ActivityDate = arrest.ArrestDate.ConvertToNibrsYearMonthDay(),
                        Charge = lrs.AgencyAssignedNibrs.HasValue(true)
                            ? lrs.AgencyAssignedNibrs
                            : LarsList.LarsDictionaryBuildNibrsXmlForUcrExtract[lrs.LrsNumber.Trim()].Nibr,
                        CategoryCode = arrest.ArrestType,
                        ArrestCount = arrest.MultipleArresteeIndicator,
                        SeqNum = arrest.ArrestSeqNum,
                        //TODO: MAKE SURE TO VERIFY WHETHER THE FOLLOWING CODE SHOULD BE MODIFIED TO TAKE INTO CONSIDERATION AGENCYASSIGNEDNIBRS
                        Rank = Convert.ToDouble(LarsList.LarsDictionaryBuildNibrsXmlForUcrExtract[lrs.LrsNumber.Trim()].Lrank)
                    }
                ).ToList();

            var groupedArrests = arrestList.GroupBy(arr => arr.SeqNum,                arr => arr,
                (seq, arrList) =>
                {
                    var minRank = arrList.Min(arr => arr.Rank);
                    var arrest = arrList.First(arr => arr.Rank == minRank);
                    return
                        new Arrest(
                            uniquePrefix + incident.Admin.ActionType + "-",
                            arrest.SeqNum,
                            new ActivityIdentification(arrest.TransactionNumber.Trim()),
                            new ActivityDate(arrest.ActivityDate, "23:30:00"),
                            new ArrestCharge(arrest.Charge),
                            arrest.CategoryCode,
                            arrest.ArrestCount
                        );
                }).ToList();

            foreach (var arr in groupedArrests) arrests.Add(arr);
        }

        private static List<OffenseVictimAssociation> BuildOffenseVictimAssociations(List<Offense> offenses,
            List<Victim> victims, List<LIBRSOffense> librsOffenses)
        {
            return librsOffenses
                .Where(offense => offense.OffenseGroup.Equals("A", StringComparison.OrdinalIgnoreCase))
                .GroupBy(offense => offense.AgencyAssignedNibrs + offense.OffConnecttoVic)
                .Select(group => new {group.First().AgencyAssignedNibrs, group.First().OffConnecttoVic})
                .Join(
                    victims,
                    offense => int.Parse(offense.OffConnecttoVic.Trim()),
                    victim => int.Parse(victim.SeqNum.Trim()),
                    (offense, victim) =>
                        new OffenseVictimAssociation(offenses.First(off => off.UcrCode == offense.AgencyAssignedNibrs),
                            victim)).Distinct().ToList();
        }

        private static List<ArrestSubjectAssociation> BuildArrestSubjectAssociation(List<Arrest> arrests,
            List<Arrestee> arrestees)
        {
            return arrests.Join(
                arrestees,
                arrest => arrest.SequenceNumber,
                arrestee => arrestee.SeqId,
                (arrest, arrestee) => new ArrestSubjectAssociation(arrest, arrestee)).ToList();
        }

        #endregion

        public static Report BuildZeroReport(IncidentList agencyIncidentList)
        {
            //Initialize a new report
            var rpt = new Report();
            rpt.Header =  new ReportHeader(NibrsReportCategoryCode.ZERO.NibrsCode(), "A", new ReportDate(agencyIncidentList.ReportYear + "-"+agencyIncidentList.ReportMonth), new ReportingAgency(new OrganizationAugmentation(new OrganizationORIIdentification(agencyIncidentList.OriNumber))));
            return rpt;
        }
    }
}