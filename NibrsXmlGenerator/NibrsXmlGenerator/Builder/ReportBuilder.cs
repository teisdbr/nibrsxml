using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoadBusinessLayer;
using NibrsXml.NibrsReport;

namespace NibrsXml.Builder
{
    public class ReportBuilder
    {
        public static NibrsReport.Report Build(LIBRSIncident incident)
        {
            Report rpt = new Report();
            rpt.Header = ReportHeaderBuilder.Build(incident.Offense, incident.ActionType, incident.Admin);
            rpt.Incident = IncidentBuilder.Build(incident.Admin);
            rpt.Offenses = OffenseBuilder.Build(incident.Offense);
            return rpt;
        }
    }
}
