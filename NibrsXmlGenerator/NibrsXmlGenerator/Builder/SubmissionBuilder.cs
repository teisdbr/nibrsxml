using System.Collections.Generic;
using LoadBusinessLayer;
using NibrsXml.NibrsReport;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

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

        public static Submission[] BuildMultipleSubmission(List<IncidentList> agencySpecificIncidents)
        {
            var submissions = new ConcurrentBag<Submission>();

            Parallel.ForEach(agencySpecificIncidents, (agencyIncidentList) =>
            {
                foreach (LIBRSIncident incident in agencyIncidentList)
                {
                    var report = ReportBuilder.Build(incident);

                    if (report == null)
                        continue;

                    if (!incident.HasErrors)
                    {
                        var sub = new Submission();
                        sub.MessageMetadata = MessageMetaDataBuilder.ExtractNibrsMessageDateTime();
                        sub.Reports.Add(report);
                        submissions.Add(sub);
                    }
                }   
            });

            // This line will run when all tasks are complete
            return submissions.ToArray();

            //foreach (var agencyIncidentList in agencySpecificIncidents)
            //    foreach (LIBRSIncident incident in agencyIncidentList)
            //    {
            //        var report = ReportBuilder.Build(incident);

            //        if (report == null)
            //            continue;

            //        if (!incident.HasErrors)
            //        {
            //            var sub = new Submission();
            //            sub.MessageMetadata = MessageMetaDataBuilder.ExtractNibrsMessageDateTime();
            //            sub.Reports.Add(report);
            //            submissions.Add(sub);
            //        }
            //    }



            //return submissions.ToArray();
        }
    }
}