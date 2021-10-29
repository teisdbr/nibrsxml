using LoadBusinessLayer;
using NibrsModels.NibrsReport;
using NibrsXml.Builder;
using NibrsXml.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace NibrsXml.Processor
{
    public class AgencyDeleteProcessor : BaseProcessor
    {      

        public AgencyDeleteProcessor(LogManager logManager, List<IncidentList> agencyIncidentsCollection) : base(logManager, agencyIncidentsCollection)
        {
            // sort the agencyBatch 
            // Process the deletes in Last In First Out order 
            AgencyBatchCollection = AgencyBatchCollection.OrderByDescending(grp => grp.Runnumber).ToList();
        }      

        public override async Task ProcessAsync()
        {
            
            var resultTuple =
             CheckConditionToReportFbi(AgencyBatchCollection.ConvertAll(item => item.Runnumber), Ori.Trim());
            var isAnyPendingToUpload = !resultTuple.Item1; // if conditon met to report to fbi, set the isAnyPendingToUpload false, so that current runnumbers can be reported to the fbi


            LogManager.PrintRunningInForceDeleteMode();
            LogManager.PrintWhetherDeletesReportToFBI(resultTuple.Item1);

            foreach (var incidentList in AgencyBatchCollection)
            {
                List<Submission> submissions = new List<Submission>();
                var runNumber = incidentList.Runnumber;
                bool completedSuccessFully = false;
                try
                {
                    submissions = SubmissionBuilder.BuildMultipleSubmission(incidentList)?.ToList();
                    LogManager.PrintSubmissionSummary(submissions.Count, runNumber);
                    // if (!submissions.Any())
                    //     submissions.Add(SubmissionBuilder.BuildZeroReportSubmission(incidentList));
                    submissions = Translator.TransformIntoDeletes(submissions);

                    LogManager.PrintTransformIntoDelete(runNumber);
                    LogManager.PrintStartedProcessForRunNumber(runNumber);
                    completedSuccessFully =  await AttemptToReportDocumentsAsync(runNumber,  submissions, reportDocuments: !isAnyPendingToUpload);
                    if (!completedSuccessFully &&
                       !resultTuple.Item2.Any(pendingRunNumber =>
                           pendingRunNumber == runNumber))
                        throw new DeleteRequestAbortException(runNumber);
                    _nibrsBatchDal.Delete(runNumber, null);

                    // if all documents uploaded successFully from current batch then set any upload fail to false and vice versa.
                    isAnyPendingToUpload = !completedSuccessFully;
                }
                finally
                {  
                    LogManager.PrintProcessCompletedForRunNumber(runNumber);
                }
            }
        }      

        private (bool, List<string>) CheckConditionToReportFbi(List<string> runNumbersToProcess, string Ori)
        {
            var nibrsBatchdt = _nibrsBatchDal.GetORIsWithPendingIncidentsToProcess(Ori);
            List<string> runNumbersPendingToUpload = new List<string>();
            foreach (DataRow row in nibrsBatchdt.Rows)
            {
                if (row["Ori_number"].ToString() == Ori)
                {
                    var tuple = (row["runnumber"].ToString());
                    // get the pending runnumbers
                    runNumbersPendingToUpload.Add(tuple);
                }
            }

            // HERE we are deciding whether the current batch reported to FBI or not, To process the runnumbers in the sequence we are doing below condition checks
            // 1) check if any pending runnumbers to upload? if none return true
            // 2) If any pending then check if the runNumbers to process in the provided 'runNumbersToProcess' includes all the runNumbers that are pending according to database
            // 3) based on above conditions initialize the reportToFbi 
            // you have to process deletes for all pending runnumbers in the database to report the runnumbers to FBI.
            bool reportToFbi = !runNumbersPendingToUpload.Any() ||
                               runNumbersPendingToUpload.All(runNum => runNumbersToProcess.Contains(runNum));  
            return (reportToFbi, runNumbersPendingToUpload);
        }

    }
}
