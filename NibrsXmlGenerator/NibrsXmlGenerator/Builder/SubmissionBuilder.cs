using System.Collections.Generic;
using LoadBusinessLayer;
using NibrsXml.NibrsReport;

namespace NibrsXml.Builder
{
    public class SubmissionBuilder
    {
        //public MessageMetadata MessageMD = new MessageMetadata();

        public static Submission Build(List<IncidentList> agencySpecificIncidents)
        {
            var sub = new Submission();
            sub.MessageMetadata = MessageMetaDataBuilder.ExtractNibrsMessageDateTime();
        
            foreach (var agencyIncidentList in agencySpecificIncidents)
            foreach (LIBRSIncident incident in agencyIncidentList)
            {
                var report = ReportBuilder.Build(incident);
               
               

                if (report == null)
                    continue;

                if (incident.HasErrors)
                    sub.RejectedReports.Add(report);
                    
                else
                    sub.Reports.Add(report);
            }

            return sub;
        }
    }
}