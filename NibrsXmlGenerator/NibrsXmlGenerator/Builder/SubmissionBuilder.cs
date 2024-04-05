using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoadBusinessLayer;
using NibrsModels.Constants;
using NibrsModels.NibrsReport;
using NibrsModels.Utility;
using NibrsXml.Processor;

namespace NibrsXml.Builder
{
    public class SubmissionBuilder
    {
        /// <summary>
        /// To build single Submission having multiple Reports, capable to generate NIBRS XML for the UCR reports (FBI doesn't accept XMLs generated in this format ).
        /// </summary>
        /// <param name="agencySpecificIncidents"></param>
        /// <param name="oriNumber"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Build multiple submissions, in a format capable to report NIBRS XMLs to FBI.
        /// </summary>
        /// <param name="agencyIncidentList"></param>
        /// <returns></returns>
        public static Submission[] BuildMultipleSubmission(IncidentList agencyIncidentList)
        {
            var submissions = new ConcurrentBag<Submission>();
            ConcurrentDictionary<string, List<Submission>> trackIncidentsDic = new ConcurrentDictionary<string, List<Submission>>();


            if (agencyIncidentList.IsZeroReport)
            {
                TryAddSubToDictionary(trackIncidentsDic, BuildZeroReportSubmission(agencyIncidentList), "");
            }
            else
            {
                foreach (LIBRSIncident incident in agencyIncidentList)
                {

                    try
                    {
                        if (incident.HasErrors)
                            continue;
                        FileLogger.WriteInfo("Started ReportBuilder.Build().");
                        var report = ReportBuilder.Build(incident, agencyIncidentList.ReportMonth, agencyIncidentList.ReportYear);
                        FileLogger.WriteInfo("Completed ReportBuilder.Build().");

                        if (report == null)
                            continue;

                        var sub = new Submission(agencyIncidentList.Runnumber, agencyIncidentList.Environment, agencyIncidentList.SubmissionDate);
                        sub.MessageMetadata = MessageMetaDataBuilder.Build(sub.Id, agencyIncidentList.OriNumber);

                        sub.Reports.Add(report);

                        TryAddSubToDictionary(trackIncidentsDic, sub, incident.Admin.IncidentNumber);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
            // Using the track incidents Dictionary to keep track of Insert and Delete action type incidents, so that they can be merged into Replace action type

            var KeyValuePairs = trackIncidentsDic.AsEnumerable().ToList();

            Parallel.ForEach(KeyValuePairs, KeyValuePair =>
            {
                var delsub = KeyValuePair.Value.Find(sub => sub.Reports[0].Header.ReportActionCategoryCode == "D");
                var insertOrAddSub = KeyValuePair.Value.Find(sub => sub.Reports[0].Header.ReportActionCategoryCode != "D");

                // "replace" action type  condition 
                if (delsub != null && insertOrAddSub != null)
                {
                  Translator.TranslateAsReplaceSub(insertOrAddSub);
                  lock (submissions)
                    {
                        submissions.Add(insertOrAddSub);
                    }
                }
                else
                {
                    if (delsub != null)
                    {
                        //TODO:?? If the GroupB Arrest Report Delete cannnot find the matching insert the total arrests will be zero, as we don't submit Arrestee deletes along with the incident deletes in Librs flatfile.
                        if (KeyValuePair.Key.EndsWith(NibrsReportCategoryCode.B.NibrsCode()) && !delsub.Reports[0].Arrests.Any())
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

        public static Submission[] BuildMultipleSubmission(List<IncidentList> agencySpecificIncidents)
        {
            var submissions = new ConcurrentBag<Submission[]>();

            Parallel.ForEach(agencySpecificIncidents, agencyIncidentList =>
            {
              
                    submissions.Add(BuildMultipleSubmission(agencyIncidentList));

            });

            return submissions.SelectMany(subs => subs).ToArray();
        }


        public static Submission BuildZeroReportSubmission(IncidentList agencyIncidentList)
        {
            var sub = new Submission(agencyIncidentList.Runnumber, agencyIncidentList.Environment,agencyIncidentList.SubmissionDate);
            sub.MessageMetadata = MessageMetaDataBuilder.Build(sub.Id, agencyIncidentList.OriNumber);
            sub.Reports.Add(ReportBuilder.BuildZeroReport(agencyIncidentList));
            return sub;
        }


        private static void TryAddSubToDictionary(ConcurrentDictionary<string, List<Submission>> trackIncidentsDic, Submission sub, string incidentNum)
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