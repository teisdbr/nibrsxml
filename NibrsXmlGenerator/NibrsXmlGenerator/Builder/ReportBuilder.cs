using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoadBusinessLayer;
using LoadBusinessLayer.LIBRSOffender;
using LoadBusinessLayer.LIBRSOffense;
using NibrsXml.NibrsReport;
using NibrsXml.Utility;
using NibrsXml.NibrsReport;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.NibrsReport.Location;
using NibrsXml.NibrsReport.Associations;

namespace NibrsXml.Builder
{
    public class ReportBuilder
    {
        public static NibrsReport.Report Build(LIBRSIncident incident)
        {
            Report rpt = new Report();

            var uniqueReportPrefix = incident.Admin.ORINumber + "-" + incident.IncidentNumber + "-";

            rpt.Header = ReportHeaderBuilder.Build(incident.Offense, incident.ActionType, incident.Admin);
            rpt.Incident = IncidentBuilder.Build(incident.Admin);
            rpt.Offenses = OffenseBuilder.Build(incident.Offense, UniqueBiasMotivationCodes(incident.Offender), UniqueSuspectedOfUsingCodes(incident.OffUsing), uniqueReportPrefix: uniqueReportPrefix);
            
            //Build list of locations and location associations
            LocationListBuilder(offenses: rpt.Offenses, locations: rpt.Locations, locationAssociations: rpt.OffenseLocationAssocs, uniqueReportPrefix: uniqueReportPrefix);
            //todo: ItemBuilder
            //todo: SubstanceBuilder
            //todo: EnforcementOfficialBuilder
            //todo: VictimBuilder
            //todo: SubjectBuilder
            //todo: ArresteeBuilder
            //todo: ArrestBuilder
            //todo: ArrestSubjectAssociationBuilder
            //todo: OffenseLocationAssociationBuilder
            //todo: OffenseVictimAssociationBuilder
            //todo: SubjectVictimAssociationBuilder
            return rpt;
        }
        
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

        /// <summary>
        /// This method initializes the list of Locations and the list of OffenseLocationAssociations
        /// </summary>
        /// <param name="offenses">List of offenses to build locations from</param>
        /// <param name="locations">List of locations to match with offenses (This is empty, but initialized)</param>
        /// <param name="locationAssociations">List of location associations to match to locations</param>
        private static void LocationListBuilder(List<Offense> offenses, List<Location> locations, List<OffenseLocationAssociation> locationAssociations, String uniqueReportPrefix)
        {
            foreach (var offense in offenses) {
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
    }
}
