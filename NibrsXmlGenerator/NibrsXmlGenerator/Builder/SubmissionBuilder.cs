using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoadBusinessLayer;
using NibrsXml.NibrsReport;

namespace NibrsXml.Builder
{
    public class SubmissionBuilder
    {
        public static Submission Build(List<IncidentList> agencySpecificIncidents, string oriNumber)
        {
            var sub = new Submission();

            sub.MessageMetadata = MessageMetaDataBuilder.Build(sub.Id, oriNumber);

            foreach (var agencyIncidentList in agencySpecificIncidents)
            foreach (LIBRSIncident incident in agencyIncidentList)
            {
                var report = ReportBuilder.Build(incident);


                if (report == null)
                    continue;

                if (incident.HasErrors || (report.HasFailedToBuildProperly))
                    sub.RejectedReports.Add(report);

                else
                    sub.Reports.Add(report);
            }

            return sub;
        }

        public static Submission[] BuildMultipleSubmission(List<IncidentList> agencySpecificIncidents)
        {
            var submissions = new ConcurrentBag<Submission>();

            Parallel.ForEach(agencySpecificIncidents, agencyIncidentList =>
            {
                foreach (LIBRSIncident incident in agencyIncidentList)
                {
                    if (incident.HasErrors) continue;
                    var report = ReportBuilder.Build(incident);

                    if (report == null || report.HasFailedToBuildProperly)
                        continue;
                    

                    var sub = new Submission {Runnumber = agencyIncidentList.Runnumber};
                    sub.MessageMetadata = MessageMetaDataBuilder.Build(sub.Id, agencyIncidentList.OriNumber);
                    sub.Reports.Add(report);

                    lock (submissions)
                    {
                        submissions.Add(sub);
                    }
                }
            });

            return submissions.ToArray();
        }
    }
}