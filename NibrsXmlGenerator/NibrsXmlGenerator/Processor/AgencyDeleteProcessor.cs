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

        public AgencyDeleteProcessor(LogManager logManager, List<IncidentList> agencyIncidentsCollection, string environment) : base(logManager, agencyIncidentsCollection, environment)
        {
            // sort the agencyBatch 
            // Process the deletes in Last In First Out order 
            AgencyBatchCollection = AgencyBatchCollection.OrderByDescending(grp => grp.Runnumber).ToList();
        }      

        public override async Task ProcessAsync()
        {
            if (!AgencyBatchCollection.Any())
                return;

            var resultTuple =
             CheckConditionToReportFbi(AgencyBatchCollection.ConvertAll(item => item.Runnumber), Ori.Trim());
            var isAnyPendingToUpload = !resultTuple.Item1; // if condition met to report to fbi, set the isAnyPendingToUpload false, so that current runnumbers can be reported to the fbi


            LogManager.PrintRunningInForceDeleteMode();
            LogManager.PrintWhetherDeletesReportToFBI(resultTuple.Item1);

            foreach (var incidentList in AgencyBatchCollection)
            {
                List<Submission> submissions = new List<Submission>();
                var runNumber = incidentList.Runnumber;
                try
                {
                    submissions = SubmissionBuilder.BuildMultipleSubmission(incidentList).ToList();
                    LogManager.PrintSubmissionSummary(submissions.Count, runNumber);
                    // if (!submissions.Any())
                    //     submissions.Add(SubmissionBuilder.BuildZeroReportSubmission(incidentList));
                    submissions = Translator.TransformIntoDeletes(submissions);
                    LogManager.PrintTransformIntoDelete(runNumber);
                    LogManager.PrintStartedProcessForRunNumber(runNumber);
                   var batchResponseStatus =  await AttemptToReportDocumentsAsync(runNumber,  submissions, incidentList, reportDocuments: !isAnyPendingToUpload);
                    if (!batchResponseStatus.UploadedToFbi && resultTuple.Item2.All(pendingRunNumber => pendingRunNumber != runNumber))
                    {
                        _nibrsBatchDal.Edit(runNumber, null, null, null, DateTime.Now, null, null,true);

                        throw new DeleteRequestAbortException(runNumber);
                    }
                       
                    _nibrsBatchDal.Delete(runNumber, null);

                    // if all documents uploaded successFully from current batch then set any upload fail to false and vice versa.
                    isAnyPendingToUpload = !batchResponseStatus.UploadedToFbi;
                }
                finally
                {  
                    LogManager.PrintProcessCompletedForRunNumber(runNumber);
                }
            }
        }      


        //TODO: Get Back to Implement this method when we have developement hours.
        private void ApplyOptimalStrategy(string runNumber,
            IEnumerable<Submission> documentsBatch, bool reportDocuments)
        {
            // search NibrsBatch and get details
            // if pending  is false and uploaded is false we can apply local delete eg: Delete By Runnumber since we are only modifying local database (Not Caring about FBI)
            // if partially is true  or uploaded is true reported then attempt to FBI by standard process

        }

        private (bool, List<string>) CheckConditionToReportFbi(List<string> runNumbersToProcess, string Ori)
        {
            List<PendingRunNumbers> runNumbersPendingToUpload = GetPendingRunNumbers();
            var  runNumbers = runNumbersPendingToUpload.Where(pd => pd.IsUploadedToFBI == false).Select(pd => pd.RunNumber).ToList();
            // HERE we are deciding whether the current batch should be reported to FBI or not, To process the runnumbers in the sequence we are doing below condition checks
            // 1) check if any pending runnumbers to upload? if none return true
            // 2) If any pending then check if the runNumbers to process in the provided 'runNumbersToProcess' includes all the runNumbers that are pending according to database
            // 3) based on above conditions initialize the reportToFbi 
            // you have to process deletes for all pending runnumbers in the database to report the runnumbers to FBI.
            bool reportToFbi = !runNumbers.Any() ||
                               runNumbers.All(runNumbersToProcess.Contains);  
            return (reportToFbi, runNumbers);
        }

    }
}
