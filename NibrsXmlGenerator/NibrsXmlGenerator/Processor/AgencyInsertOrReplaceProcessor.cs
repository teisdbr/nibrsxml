﻿using LoadBusinessLayer;
using NibrsModels.NibrsReport;
using NibrsXml.Builder;
using NibrsXml.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeUtil.Extensions;

namespace NibrsXml.Processor
{
    public class AgencyInsertOrReplaceProcessor : BaseProcessor
    {
        private Func<string, string, Task<IncidentList>> BuildLibrsIncidentsListFunc { get; set; }

        public AgencyInsertOrReplaceProcessor(LogManager logManager, List<IncidentList> agencyIncidentsCollection,
            Func<string, string, Task<IncidentList>> buildLibrsIncidentsListFunc) : base(logManager, agencyIncidentsCollection)
        {
            BuildLibrsIncidentsListFunc = buildLibrsIncidentsListFunc;
        }

        public override async Task ProcessAsync()
        {
            LogManager.PrintStartOfProcess();
            var runNumbers = new List<string>();

            //  Process any pending runnumber before taking up the new ones
            var isAnyFailedToReportFbi = await ProcessPendingTransactionsForOriAsync();

            // foreach run number loop through and update the NIBRS  Batch
            // Process the Batch in same order as it is received
            var dt = _nibrsBatchDal.Search(null, Ori);
            // if no records present for the ORI in NIBRS batch, then this is first ever nibrs processing for the ORI,so no need to check the next runnumber in sequence from database.
            if (dt == null || dt?.Rows.Count == 0)
            {
                runNumbers = AgencyBatchCollection?.OrderBy(inc => inc.Runnumber)
                    .Select(incList => incList.Runnumber).Distinct().ToList() ?? new List<string>();
            }
            else
            {
                // the stored procedure gives the run-numbers that are to be NIBRS processed in sequence
                var runNumbersdt = _nibrsBatchDal.GetNextRunNumbersInSequence(Ori);
                if (runNumbersdt?.Rows != null)
                    foreach (DataRow runNumber in runNumbersdt?.Rows)
                    {
                        runNumbers.UniqueAdd(runNumber["RUNNUMBER"].ToString());
                    }
            }
            await RunBatchProcessAsync(runNumbers, isAnyFailedToReportFbi);
            LogManager.PrintEndOfProcess();
        }

        private async Task<bool> ProcessPendingTransactionsForOriAsync()
        {

            List<string> runNumbers = new List<string>();

            // get the pending runnumbers of the ORI
            var nibrsBatchdt = _nibrsBatchDal.GetORIsWithPendingIncidentsToProcess(Ori);
            foreach (DataRow row in nibrsBatchdt.Rows)
            {
                if (row["ori_number"].ToString() == Ori)
                {
                    // get the pending runnumber
                    runNumbers.UniqueAdd(row["runnumber"].ToString());
                }
            }
            var reportToFbi = Convert.ToBoolean(_appSettingsReader.GetValue("ReportToFBI", typeof(Boolean)));

            if (!runNumbers.Any())
                return false;
            else if (!reportToFbi)
            {
                // If Report to FBI is false, there is no use to reprocess just skip reprocess.
                return true;
            }
            var statusList =
                await RunBatchProcessAsync(runNumbers, false, reProcess: true);
            return statusList.Any(status => status.HasErrorOccured);
        }

        /// <summary>
        /// Builds the submissions and sends them to FBI based on 'isAnyPendingToUpload' boolean and save the documents to the database. Reprocess boolean will report all documents with R action type when set to true.
        /// </summary>
        /// <param name="runNumbers"></param>
        /// <param name="isAnyPendingToUpload"></param>
        /// <param name="reProcess"></param>
        /// <returns></returns>
        private async Task<List<SubmissionBatchStatus>> RunBatchProcessAsync(List<string> runNumbers,
          bool isAnyPendingToUpload, bool reProcess = false)
        {

            var incListCollection =
               BuildMissingRunNumbers(runNumbers);
            if (incListCollection == null)
            {
                return new List<SubmissionBatchStatus>();
            }
            var submissionBatchStatusLst = new List<SubmissionBatchStatus>();
            // sort them in First In First Out order
            incListCollection = incListCollection.OrderBy(list => list.Runnumber).ToList();
            foreach (var incidentList in incListCollection)
            {
                List<Submission> submissions = new List<Submission>();

                var runNumber = incidentList.Runnumber;
                LogManager.PrintStartedProcessForRunNumber(runNumber);
                bool completedSuccessFully = false;
                try
                {
                    //Build the submissions
                    submissions = SubmissionBuilder.BuildMultipleSubmission(incidentList)?.ToList() ??
                                  new List<Submission>();
                    LogManager.PrintSubmissionSummary(submissions.Count, runNumber);

                    if (!submissions.Any())
                    {
                        //Generate Zero Report when there are no submissions.
                        submissions.Add(SubmissionBuilder.BuildZeroReportSubmission(incidentList));

                        LogManager.PrintGeneratedZeroReport(runNumber);
                    }
                    else if (reProcess)
                    {
                        LogManager.PrintTransformIntoReplace(runNumber);
                        submissions.ForEach(Translator.TranslateAsReplaceSub);
                    }

                    LogManager.PrintAddingBatchDetailsToDatabase(runNumber);

                    // Update the Nibrs Batch table to have this run-number saying it is attempted to process. 
                    var dt = _nibrsBatchDal.Search(runNumber, Ori);
                    if (dt.Rows.Count == 0)
                    {
                        _nibrsBatchDal.Add(runNumber, incidentList.Count(incList => !incList.HasErrors),
                            submissions.Count,
                            DateTime.Now, DateTime.Now, false);
                    }                  

                    completedSuccessFully =  await AttemptToReportDocumentsAsync(runNumber, submissions, reportDocuments:  !isAnyPendingToUpload);
                    _nibrsBatchDal.Edit(runNumber, incidentList.Count(incList => !incList.HasErrors),
                           submissions.Count, null, DateTime.Now, completedSuccessFully);
                    // Update the Nibrs Batch to have the RunNumber saying the data is processed
                    if (submissions != null)
                    {

                        var submissionBatchStatus = new SubmissionBatchStatus()
                        {
                            RunNumber = runNumber,
                            Ori = Ori,
                            Environmennt = incidentList.Environment,
                            NoOfSubmissions = (submissions)?.Count() ?? 0,
                            HasErrorOccured = ! completedSuccessFully
                        };
                        submissionBatchStatusLst.Add(submissionBatchStatus);
                    }

                    // if all documents uploaded successFully from current batch then set any upload fail to false and vice versa.
                    isAnyPendingToUpload = !completedSuccessFully;

                }               
                finally
                {
                    LogManager.PrintProcessCompletedForRunNumber(runNumber);
                    LogManager.PrintStatusAfterProcessForRunNumber(runNumber, isAnyPendingToUpload);                    
                }
            }

            return submissionBatchStatusLst;
        }


        private  List<IncidentList> BuildMissingRunNumbers(
          List<string> runNumbers)
        {
            var buildIncListFunc = new Func<string, IncidentList>((runNumber) =>
            {
                var task = BuildLibrsIncidentsListFunc(runNumber, "NORMAL");
                task.Wait();
                return task.Result;
            });

            var incidentList = runNumbers.ConvertAll(runNumber =>
                AgencyBatchCollection?.FirstOrDefault(incList => incList.Runnumber == runNumber) ??
                buildIncListFunc(runNumber));
            return incidentList;
        }

    }
}
