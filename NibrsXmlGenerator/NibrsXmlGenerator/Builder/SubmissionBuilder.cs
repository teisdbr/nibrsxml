using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.NibrsReport;
using LoadBusinessLayer;
using NibrsXml.Utility;

namespace NibrsXml.Builder
{
    class SubmissionBuilder 
    {
        public static Submission Build(List<IncidentList> agencySpecificIncidents)
        {
            Submission sub = new Submission();
            foreach (IncidentList agencyIncidentList in agencySpecificIncidents)
            {
                foreach (LIBRSIncident incident in agencyIncidentList) {
                    if (!incident.HasErrors)
                    {
                        sub.Reports.TryAdd(Builder.ReportBuilder.Build(incident));
                    }
                }
            }

            return sub;
        }
    }
}