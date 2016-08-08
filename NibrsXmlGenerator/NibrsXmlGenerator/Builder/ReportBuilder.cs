using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoadBusinessLayer;
using LoadBusinessLayer.LIBRSOffender;
using NibrsXml.NibrsReport;
using NibrsXml.Utility;

namespace NibrsXml.Builder
{
    public class ReportBuilder
    {
        public static NibrsReport.Report Build(LIBRSIncident incident)
        {
            Report rpt = new Report();
            rpt.Header = ReportHeaderBuilder.Build(incident.Offense, incident.ActionType, incident.Admin);
            rpt.Incident = IncidentBuilder.Build(incident.Admin);
            rpt.Offenses = OffenseBuilder.Build(incident.Offense, UniqueBiasMotivationCodes(incident.Offender), UniqueSuspectedOfUsingCodes(incident.OffUsing));
            //todo: LocationBuilder
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
    }
}
