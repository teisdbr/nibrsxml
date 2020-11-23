using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoadBusinessLayer;
using NibrsModels.Constants;
using NibrsModels.NibrsReport;
using NibrsModels.Utility;

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
                    var report = ReportBuilder.Build(incident, agencyIncidentList.ReportMonth, agencyIncidentList.ReportYear);


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
            ConcurrentDictionary<string, List<Submission>> trackIncidentsDic = new ConcurrentDictionary<string, List<Submission>>();

            Parallel.ForEach(agencySpecificIncidents, agencyIncidentList =>
            {

                if (agencyIncidentList.IsZeroReport)
                {
                   
                    var sub = new Submission(agencyIncidentList.Runnumber, agencyIncidentList.Environment);
                    sub.MessageMetadata = MessageMetaDataBuilder.Build(sub.Id, agencyIncidentList.OriNumber);

                    sub.Reports.Add(ReportBuilder.BuildZeroReport(agencyIncidentList));
                    TryAddSubToDictionary(trackIncidentsDic, sub, "");
                }


                foreach (LIBRSIncident incident in agencyIncidentList)
                {
                    if (incident.HasErrors) continue;
                    var report = ReportBuilder.Build(incident,agencyIncidentList.ReportMonth,agencyIncidentList.ReportYear);

                    if (report == null || report.HasFailedToBuildProperly)
                        continue;
                   


                    var sub = new Submission(agencyIncidentList.Runnumber, agencyIncidentList.Environment);
                    sub.MessageMetadata = MessageMetaDataBuilder.Build(sub.Id, agencyIncidentList.OriNumber);          

                    sub.Reports.Add(report);

                    TryAddSubToDictionary(trackIncidentsDic, sub, incident.Admin.IncidentNumber);
                }
            });

            // Using the track incidents Dictionary to keep track of Insert and Delete action type incidents, so that they can be merged into Replace action type
            
            var KeyValuePairs = trackIncidentsDic.AsEnumerable().ToList();

            Parallel.ForEach(KeyValuePairs, KeyValuePair =>
            {
             var delsub = KeyValuePair.Value.Find(sub => sub.Reports[0].Header.ReportActionCategoryCode == "D");
             var insertOrAddSub = KeyValuePair.Value.Find(sub => sub.Reports[0].Header.ReportActionCategoryCode != "D");

             // replace action type  condition 
             if (delsub != null && insertOrAddSub != null)
             {
                 // For the Group B Arrest Report Incident has to be null for the Replace Action Type.
                 if (KeyValuePair.Key.EndsWith(NibrsReportCategoryCode.B.NibrsCode()))
                 {
                     insertOrAddSub.Reports[0].Incident = null;
                 }

                 // send the insert or add incident with R action type and leave Delete Incident.
                 insertOrAddSub.Reports[0].Header.ReportActionCategoryCode = "R";

                 lock (submissions)
                 {
                     submissions.Add(insertOrAddSub);
                 }

             }
             else
             {
                 if(delsub != null)
                 {
                     //TODO:?? If the GroupB Arrest Report Delete cannnot find the matching insert the total arrests will be zero, as we dont submit Arrestee Delete's along with the incident delete's in Librs flatfile.
                     if (KeyValuePair.Key.EndsWith(NibrsReportCategoryCode.B.NibrsCode())  && ! delsub.Reports[0].Arrests.Any())
                     {
                         return;                         
                     }
                     lock (submissions)
                     {
                         submissions.Add(delsub);
                     }
                 }

                 if (insertOrAddSub != null)
                 {
                     lock (submissions)
                     {
                         submissions.Add(insertOrAddSub);
                     }
                 }
             }           
         });


            return submissions.ToArray();
        }


        private static void TryAddSubToDictionary(ConcurrentDictionary<string, List<Submission>> trackIncidentsDic, Submission sub,string incidentNum)
        {
            var key = sub.Ori + "_" + incidentNum + "_" + sub.Runnumber + "_" + sub.Reports[0].Header.NibrsReportCategoryCode;

            lock (trackIncidentsDic)
            {
                if (trackIncidentsDic.ContainsKey(key))
                    trackIncidentsDic[key].Add(sub);
                else
                    trackIncidentsDic.TryAdd(key, new List<Submission> { sub });
            }

        }
    }
}