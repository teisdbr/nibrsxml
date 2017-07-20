using System.Collections.Generic;
using NibrsXml.NibrsReport;
using LoadBusinessLayer;
using TeUtil.Extensions;

namespace NibrsXml.Builder
{
    public class SubmissionBuilder 
    {   
        public static Submission Build(List<IncidentList> agencySpecificIncidents)
        {
            var sub = new Submission();
            //todo: Turn this into a queue and remove an incident from the queue whenever it is used by a report to release the memory that is held by that incident
            //todo: Implement this using threads so that each thread returns a built report
            foreach (var agencyIncidentList in agencySpecificIncidents)
            {
                foreach (LIBRSIncident incident in agencyIncidentList) {
                    if (!incident.HasErrors)
                    {
                        sub.Reports.TryAdd(ReportBuilder.Build(incident));
                    }
                }
            }
            return sub;
        }
    }
}