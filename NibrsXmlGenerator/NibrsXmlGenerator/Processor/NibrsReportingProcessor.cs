using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using LoadBusinessLayer;
using LoadBusinessLayer.Interfaces;
using MongoDB.Driver.Linq;
using NibrsInterface;
using NibrsModels.Constants;
using NibrsModels.NibrsReport;
using NibrsModels.Utility;
using NibrsXml.Builder;
using NibrsXml.Constants;
using TeUtil.Extensions;
using WebhookProcessor;
using WebhookProcessor.Constants;

namespace NibrsXml.Processor
{
    public class NibrsReportingProcessor
    {
        private readonly AppSettingsReader _appSettingsReader;
        private readonly Nibrs_Batch _nibrsBatchDal;
        private ConcurrentQueue<Tuple<Exception, string>> ExceptionsLogger { get; }

        private Logger Log { get; }

        public NibrsReportingProcessor()
        {
            ExceptionsLogger = new ConcurrentQueue<Tuple<Exception, string>>();
            Log = new Logger();
            _appSettingsReader = new AppSettingsReader();
            _nibrsBatchDal = new Nibrs_Batch();
        }

        /// <summary>
        /// Process the requests to report  NIBRS deletes for given LIBRS incidents.
        /// </summary>
        /// <param name="agencyIncidentsCollection"></param>
        /// <param name="batchFolderName"></param>
        /// <param name="forceDelete"></param>
        /// <returns></returns>
        public async Task<List<SubmissionBatchStatus>> ProcessDeletesBatchAsync(
            List<IncidentList> agencyIncidentsCollection,
            string batchFolderName)
        {
            var submissionBatchStatusLst = new List<SubmissionBatchStatus>();
            var anyFailedToSave = false;
            // Only process the agencyList with Environment C & P 
            agencyIncidentsCollection = agencyIncidentsCollection.Where(incList => incList.Environment != "T").ToList();

            foreach (var agencyGrp in agencyIncidentsCollection.GroupBy(collection => collection.OriNumber))
            {
                var ori = agencyGrp.Key;
               Log.WriteLog(ori, $"{DateTime.Now} : --------- BEGAN PROCESSING THE SUBMISSION DELETES--------------",
                    batchFolderName);
                submissionBatchStatusLst.AddRange((await RunBatchDeletesProcessAsync(agencyGrp.ToList(), batchFolderName, ori)));

                Log.WriteLog(ori,
                    $"{DateTime.Now} : --------- COMPLETED PROCESSING THE SUBMISSION DELETES--------------",
                    batchFolderName);
            }

            return submissionBatchStatusLst;
        }

        /// <summary>
        /// Starts the Nibrs Processing for the run-numbers of ORI
        /// </summary>
        /// <param name="ori"></param>
        /// <param name="batchFolderName"></param>
        /// <param name="agencyIncidentsCollection"></param>
        /// <param name="buildLibrsIncidentsListFunc"></param>
        /// <returns></returns>
        public async Task<List<SubmissionBatchStatus>> RunProcessForOriAsync(string ori, string batchFolderName,
            List<IncidentList> agencyIncidentsCollection,
            Func<string, string, Task<IncidentList>> buildLibrsIncidentsListFunc)
        {
            Log.WriteLog(ori,
                $"{DateTime.Now} : -------------------  RUNNING NIBRS BATCH PROCESS--------------------",
                batchFolderName);
            List<SubmissionBatchStatus> submissionBatchStatusLst = new List<SubmissionBatchStatus>();
            var runNumbers = new List<string>();

            // limit the IncidentList to the given ORI
            agencyIncidentsCollection = agencyIncidentsCollection?.Where(inc => inc.OriNumber == ori)?.ToList();

            //  Process any pending runnumber before taking up the new ones
            var isAnyFailedToReportFbi = await ProcessPendingTransactionsForOriAsync(ori, agencyIncidentsCollection, batchFolderName, buildLibrsIncidentsListFunc);

            // foreach run number loop through and update the NIBRS  Batch
            // Process the Batch in same order as it is received
            var dt = _nibrsBatchDal.Search(null, ori);
            // if no records present for the ORI in NIBRS batch, then this is first ever nibrs processing for the ORI,so no need to check the next runnumber in sequence from database.
            if (dt == null || dt?.Rows.Count == 0)
            {
                runNumbers = agencyIncidentsCollection?.OrderBy(inc => inc.Runnumber)
                    .Select(incList => incList.Runnumber).Distinct().ToList() ?? new List<string>();
            }
            else
            {
                // the stored procedure gives the run-numbers that are to be NIBRS processed in sequence
                var runNumbersdt = _nibrsBatchDal.GetNextRunNumbersInSequence(ori);
                if (runNumbersdt?.Rows != null)
                    foreach (DataRow runNumber in runNumbersdt?.Rows)
                    {
                        runNumbers.UniqueAdd(runNumber["RUNNUMBER"].ToString());
                    }
            }
            var incListCollection = BuildMissingRunNumbers(agencyIncidentsCollection, runNumbers, buildLibrsIncidentsListFunc);
            submissionBatchStatusLst.AddRange(await RunBatchProcessAsync(incListCollection,ori, batchFolderName, isAnyFailedToReportFbi));
            Log.WriteLog(ori,
                $"{DateTime.Now} : -------------------  PROCESSING NIBRS BATCH COMPLETED--------------------",
                batchFolderName);
            return submissionBatchStatusLst;
        }

        /// <summary>
        /// Process the Nibrs Batch for the given LIBRS Batch of Incidents
        /// </summary>
        /// <param name="agencyIncidentsCollection"></param>
        /// <param name="batchFolderName"></param>
        /// <param name="buildLibrsIncidentsListFunc"></param>
        /// <returns></returns>
        public async Task<List<SubmissionBatchStatus>> ProcessAgenciesBatchAsync(
            List<IncidentList> agencyIncidentsCollection, string batchFolderName,
            Func<string, string, Task<IncidentList>> buildLibrsIncidentsListFunc)
        {
            AgencyCode agencyCode = new AgencyCode();
            var submissionBatchStatusLst = new List<SubmissionBatchStatus>();

            // Only process the agencyList with Environment C & P 
            agencyIncidentsCollection =
                agencyIncidentsCollection?.Where(incList => incList.Environment != "T")?.ToList() ??
                new List<IncidentList>();

            List<string> oriList = new List<string>();

            // if agencyIncidentsCollection is provided stick to those ORIs
            if (agencyIncidentsCollection.Any())
                oriList = agencyIncidentsCollection.Select(incList => incList.OriNumber)?.Distinct().ToList();
            // Else get all failed to save ORIs process them one after the other.
            else
            {
                // commented out because the submission failed to build properly from LIBRS incidents will keep on failing unless fixed,
                // so it will overwhelm the error notification emails.
                // var nibrsBatchdt = nibrsBatchDal.GetORIsWithPendingIncidentsToProcess();
                // foreach (DataRow row in nibrsBatchdt.Rows)
                // {
                //     oriList.UniqueAdd(row["ori_number"]?.ToString()?.Trim());
                // }
                return new List<SubmissionBatchStatus>();
            }

            foreach (var ori in oriList)
            {
                var lockKey = Guid.NewGuid().ToByteArray();
                try
                {
                    if (!PlaceLockOnAgency(agencyCode, lockKey, ori))
                    {
                        Log.WriteLog(ori, $"{DateTime.Now} : COULDN'T PLACE THE LOCK ON THE ORI: {ori}",
                            batchFolderName);
                        continue;
                    }

                    var submissionBatchList = await RunProcessForOriAsync(ori, batchFolderName,
                        agencyIncidentsCollection,
                        buildLibrsIncidentsListFunc);

                    submissionBatchStatusLst.AddRange(submissionBatchList);
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ori, "Exception:" + ex.Message, batchFolderName);
                    ExceptionsLogger.Enqueue(Tuple.Create(ex, ""));

                    Log.WriteLog(ori, DateTime.Now + " : " + "FAILED TO PROCESS NIBRS  DATA ",
                        batchFolderName);

                    SendErrorEmail($"Something went wrong while trying to process the submission batch for ORI:{ori}",
                        $"Please check the logs for more" +
                        $" details.{Environment.NewLine} Batch Folder Name {batchFolderName} {Environment.NewLine} Exception {ex.Message} {ex.InnerException}");
                }
                finally
                {
                    ReleaseLockOnAgency(agencyCode, lockKey, ori);
                    PrintExceptions(ExceptionsLogger, Log, ori, batchFolderName);
                }
            }

            return submissionBatchStatusLst;
        }

        private async Task<bool> ProcessPendingTransactionsForOriAsync(string ori, List<IncidentList> agencyIncidentsCollection,string batchFolderName, 
            Func<string, string, Task<IncidentList>> buildLibrsIncidentsListFunc)
        {
            ori = ori.Trim();
            List<string> runNumbers = new List<string>();
            

            // get the pending runnumbers of the ORI
            var nibrsBatchdt = _nibrsBatchDal.GetORIsWithPendingIncidentsToProcess(ori);
            foreach (DataRow row in nibrsBatchdt.Rows)
            {
                if (row["ori_number"].ToString() == ori)
                {
                    // get the pending runnumber
                    runNumbers.UniqueAdd(row["runnumber"].ToString());
                }
            }

            if (!runNumbers.Any())
                return false;
   
            var incListCollection = BuildMissingRunNumbers(agencyIncidentsCollection, runNumbers, buildLibrsIncidentsListFunc);
            var statusList =   await RunBatchProcessAsync(incListCollection,ori, batchFolderName, false, reProcess: true);
           return statusList.Any(status => status.HasErrorOccured);
        }


        /// <summary>
        ///  This method will submit the NibrsXml to FBI and attempt to save the NibrsXmlTransaction in MongoDb using LCRX API, returns the boolean that represents if any failed to  to send FBI.
        /// Warning: This process does-not guarantee that all the requests made in the order as provided. 
        /// </summary>
        /// <param name="ori"></param>
        /// <param name="documentsBatch"></param>
        /// <param name="isAnyPendingToUpload"></param>
        /// <returns></returns>
        private async Task<bool> AttemptToReportDocumentsAsync(string ori,
            IEnumerable<Submission> documentsBatch, bool isAnyPendingToUpload = false)
        {
            var agencyDir = new AgencyNibrsDirectoryInfo(ori);
            var pathToSaveErrorTransactions = agencyDir.GetErroredDirectory().FullName;
            var baseURL = Convert.ToString(_appSettingsReader.GetValue("LcrxAPIURL", typeof(string)));
            var endpoint = _appSettingsReader.GetValue("SaveNibrsXmlEndpoint", typeof(String)).ToString();

            var maxDegreeOfParallelism =
                Convert.ToInt32(_appSettingsReader.GetValue("MaxDegreeOfParallelism", typeof(int)));

            var reportToFbi = Convert.ToBoolean(_appSettingsReader.GetValue("ReportToFBI", typeof(Boolean)));
            var authInfo = new AuthorizationInfo() {AuthType = AuthorizationType.NoAuth};

            // // TODO: check with the LCRX API to see, if they are any failed to Upload for this ORI
            // var checkPendingToUploadEndPoint =
            //     appSettingsReader.GetValue("CheckAnyPendingToUploadEndpoint", typeof(String)).ToString();
            // checkPendingToUploadEndPoint = $"{checkPendingToUploadEndPoint}?ori={ori}";
            // var anyPending =
            //     await HttpActions.Get<AuthorizationInfo, bool>(String.Empty, baseURL + checkPendingToUploadEndPoint,
            //         authInfo);

            ConcurrentBag<NibrsXmlTransaction> errorTransactions = new ConcurrentBag<NibrsXmlTransaction>();

            var requestTasks = new List<Task<bool>>();

            // The purpose of the semaphoreSlim to control the max number of concurrent tasks that can be ran in the requestTasks
            // MaxDegreeOfParallelism defines max number of tasks that can be at a time.
            SemaphoreSlim semaphoreSlim = new SemaphoreSlim(maxDegreeOfParallelism, maxDegreeOfParallelism);

            foreach (var sub in documentsBatch)
            {
                requestTasks.Add(Task.Run(async () =>
                {
                    var reportedToFbi = true;

                    await semaphoreSlim.WaitAsync();
                    NibrsXmlTransaction nibrsTrans = null;
                    try
                    {
                        var response = sub.IsNibrsReportable && reportToFbi && !isAnyPendingToUpload
                            ? NibrsSubmitter.SendReport(sub.Xml)
                            : null;
                        //Wrap both response and submission and then save to database
                        nibrsTrans = new NibrsXmlTransaction(sub, response);

                        // Mark as upload failed.
                        if (nibrsTrans.Status == NibrsSubmissionStatusCodes.UploadFailed)
                        {
                            reportedToFbi = false;
                        }

                        try
                        {
                            await HttpActions.Post<NibrsXmlTransaction, AuthorizationInfo, object>(nibrsTrans,
                                baseURL + endpoint, authInfo);
                        }
                        catch (Exception ex)
                        {
                            ExceptionsLogger.Enqueue(Tuple.Create(ex,
                                $"Incident Number: {nibrsTrans?.Submission?.IncidentNumber ?? String.Empty} , Arrest ID: {nibrsTrans?.Submission?.Reports[0]?.Arrests?.FirstOrDefault()?.ActivityId?.Id ?? String.Empty}"));
                            if (ex is HttpException)
                            {
                                var httpException = (HttpException) ex;
                                if (httpException.GetHttpCode() == (int) HttpStatusCode.InternalServerError)
                                {
                                    // set exception message
                                    nibrsTrans.NibrsSubmissionResponse.LastException = ex.Message;
                                    errorTransactions.Add(nibrsTrans);
                                    return reportedToFbi;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionsLogger.Enqueue(Tuple.Create(ex,
                            nibrsTrans?.Submission?.IncidentNumber ??
                            nibrsTrans?.Submission?.Reports[0]?.Arrests?.FirstOrDefault()?.ActivityId?.Id ??
                            String.Empty));
                    }
                    finally
                    {
                        //Release the semaphore. It is vital to ALWAYS release the semaphore so that tasks waiting to enter can takeover or else we will end up with a Semaphore that is forever locked.
                        //final block is recommended to release, program execution may crash or take a different path, this way you are guaranteed execution
                        lock (semaphoreSlim)
                        {
                            semaphoreSlim.Release();
                        }
                    }

                    return reportedToFbi;
                }));
            }

            var attemptResults = await Task.WhenAll(requestTasks.ToArray());

            // save in failed folder
            if (errorTransactions.Any())
            {
                WriteTransactions(errorTransactions, pathToSaveErrorTransactions);
                SendErrorEmail($"Found Some files failed to save in MongoDb Clusters, that needs your attention",
                    $"Please check the logs and Directory:{pathToSaveErrorTransactions} for more details.{Environment.NewLine}");
            }

            return attemptResults.Any(isSaved => !isSaved);
        }

        public async Task<List<SubmissionBatchStatus>> RunBatchProcessAsync(List<IncidentList> agencyBatchCollection, string ori,
            string batchFolderName, bool isAnyPendingToUpload, bool reProcess = false)
        {
            if (agencyBatchCollection == null)
            {
                return new List<SubmissionBatchStatus>();
            }
          
            var agencyXmlDirectoryInfo = new AgencyNibrsDirectoryInfo(ori);
            var submissionBatchStatusLst = new List<SubmissionBatchStatus>();

            // sort them in First In First Out order
            agencyBatchCollection = agencyBatchCollection.OrderBy(list => list.Runnumber).ToList();
            foreach (var incidentList in agencyBatchCollection)
            {
                List<Submission> submissions = new List<Submission>();
                var runNumber = incidentList.Runnumber;

                try
                {
                   //Build the submissions
                    submissions =  SubmissionBuilder.BuildMultipleSubmission(incidentList)?.ToList() ??
                                  new List<Submission>();
                    Log.WriteLog(ori,
                        $"{DateTime.Now} : NO OF SUBMISSIONS BUILT TO PROCESS FOR RUN-NUMBER: {runNumber} ARE {submissions.Count}",
                        batchFolderName);

                    if (!submissions.Any())
                    {
                        //Generate Zero Report when there are no submissions.
                        submissions.Add(SubmissionBuilder.BuildZeroReportSubmission(incidentList));

                        Log.WriteLog(ori,
                            $"{DateTime.Now} : GENERATING ZERO REPORT AS TOTAL SUBMISSIONS TO PROCESS FOR RUN-NUMBER: {runNumber} IS ZERO",
                            batchFolderName);
                    }
                    else if (reProcess)
                    {
                        Log.WriteLog(ori,
                            $"{DateTime.Now} : TRANSFORMING THE INSERT AS REPLACE BECAUSE REPROCESS MODE IS ENABLED FOR RUNNUMBER : {runNumber}",
                            batchFolderName);
                        submissions.ForEach(Translator.TranslateAsReplaceSub);
                    }

                    Log.WriteLog(ori,
                        $"{DateTime.Now} : ADDING BATCH DETAILS TO THE DATABASE RUN-NUMBER : {runNumber}",
                        batchFolderName);

                    // Update the Nibrs Batch table to have this run-number saying it is attempted to process. 
                   var dt = _nibrsBatchDal.Search(runNumber, ori);
                   if (dt.Rows.Count == 0)
                   {
                       _nibrsBatchDal.Add(runNumber, incidentList.Count(incList => !incList.HasErrors), submissions.Count,
                           DateTime.Now, DateTime.Now, false);
                   }
                   var saveLocalPath = agencyXmlDirectoryInfo.GetArchiveLocation();
                    saveLocalPath = saveLocalPath + "\\" + runNumber;
                    WriteSubmissions(submissions, saveLocalPath);
                    Log.WriteLog(ori,
                        $"{DateTime.Now} : SAVED All XML FILES FOR RUN-NUMBER: {runNumber} AT {saveLocalPath}",
                        batchFolderName);

                    Log.WriteLog(ori,
                        DateTime.Now + " : " + "STARTED FILES PROCESSING FOR RUN-NUMBER : " +
                        runNumber,
                        batchFolderName);

                    isAnyPendingToUpload = await AttemptToReportDocumentsAsync(ori, submissions,isAnyPendingToUpload);
                }
                finally
                {
                    PrintExceptions(ExceptionsLogger, Log, ori, batchFolderName);
                    Log.WriteLog(ori,
                        DateTime.Now + " : " +
                        "COMPLETED FILES PROCESSING FOR RUN-NUMBER : " + runNumber,
                        batchFolderName);
                    Log.WriteLog(ori,
                        $"{DateTime.Now} : Failed To Report FBI Or Process?:  {isAnyPendingToUpload} FOR RUN-NUMBER: {runNumber}",
                        batchFolderName);

                    // Update the Nibrs Batch to have the RunNumber saying the data is processed
                    if (submissions != null)
                    {
                        _nibrsBatchDal.Edit(runNumber, incidentList.Count(incList => !incList.HasErrors),
                            submissions.Count, null, DateTime.Now, !isAnyPendingToUpload);
                        var submissionBatchStatus = new SubmissionBatchStatus()
                        {
                            RunNumber = runNumber,
                            Ori = ori,
                            Environmennt = incidentList.Environment,
                            NoOfSubmissions = (submissions)?.Count() ?? 0,
                            HasErrorOccured = isAnyPendingToUpload
                        };
                        submissionBatchStatusLst.Add(submissionBatchStatus);
                    }
                }
            }
           
            return submissionBatchStatusLst;
        }

        private async Task<List<SubmissionBatchStatus>> RunBatchDeletesProcessAsync(
            List<IncidentList> agencyBatchCollection, string batchFolderName, string ori)
        {
            var submissionBatchStatusLst = new List<SubmissionBatchStatus>();
            // sort the agencyBatch 
           // Process the deletes in Last In First Out order 
            agencyBatchCollection = agencyBatchCollection.OrderByDescending(grp => grp.Runnumber).ToList();
            var resultTuple =
                CheckConditionToReportFbi(agencyBatchCollection.ConvertAll(item => item.Runnumber), ori.Trim());
            bool isAnyPendingToUpload = !resultTuple.Item1; // if report to FBI true set the isAnyPendingToUpload false
             Log.WriteLog(ori,
                 $"{DateTime.Now} : --------- RUNNING THE PROCESS IN THE FORCE DELETE MODE--------------",
                 batchFolderName);
             Log.WriteLog(ori,
                 $"{DateTime.Now} : --------- Report To FBI is initialized to ${resultTuple.Item1} --------------",
                 batchFolderName);
            foreach (var incidentList in agencyBatchCollection)
            {
                List<Submission> submissions = new List<Submission>();
                var runNumber = incidentList.Runnumber;

                try
                {
                    submissions = SubmissionBuilder.BuildMultipleSubmission(incidentList)?.ToList();
                    Log.WriteLog(ori,
                        $"{DateTime.Now} : TOTAL {submissions?.Count} SUBMISSIONS BUILT TO PROCESS FOR RUN-NUMBER: {runNumber}",
                        batchFolderName);
                    // if (!submissions.Any())
                    //     submissions.Add(SubmissionBuilder.BuildZeroReportSubmission(incidentList));
                    submissions = Translator.TransformIntoDeletes(submissions);
                    Log.WriteLog(ori,
                        $"{DateTime.Now} : TRANSFORMED NIBRS DATA INTO DELETE  FOR RUN-NUMBER: {runNumber}",
                        batchFolderName);
                    
                    Log.WriteLog(ori,
                        DateTime.Now + " : " + "STARTED FILES PROCESSING FOR RUN-NUMBER : " +
                        runNumber,
                        batchFolderName);

                    isAnyPendingToUpload = await AttemptToReportDocumentsAsync(ori, submissions,isAnyPendingToUpload);

                    if (isAnyPendingToUpload &&
                        resultTuple.Item2.First(pendingRunNumberInfoTuple => pendingRunNumberInfoTuple.Item1 == runNumber).Item2)
                        throw new Exception(
                            $"Exception Occured while processing the Deletes, Cannot Report Deletes to incidents that are to reported to the FBI previously, So deleting the current batch with runnumber {runNumber} can cause duplicate incidents errors in future reporting", null);
                    _nibrsBatchDal.Delete(runNumber, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ExceptionsLogger.Enqueue(Tuple.Create(e, ""));
                    throw;
                }
                finally
                {
                    var submissionBatchStatus = new SubmissionBatchStatus()
                    {
                        RunNumber = runNumber,
                        Ori = ori,
                        Environmennt = incidentList.Environment,
                        NoOfSubmissions = (submissions)?.Count() ?? 0,
                        HasErrorOccured = isAnyPendingToUpload
                    };
                    Log.WriteLog(ori,
                        DateTime.Now + " : " + "COMPLETED FILES PROCESSING FOR RUN-NUMBER : " +
                        runNumber,
                        batchFolderName);
                    submissionBatchStatusLst.Add(submissionBatchStatus);
                    PrintExceptions(ExceptionsLogger, Log, ori, batchFolderName);
                }
            }
            return submissionBatchStatusLst;
        }
        
        public  (bool,List<(string,bool)>) CheckConditionToReportFbi(List<string> runNumbersToProcess, string ori)
        {
           
            var nibrsBatchdt = _nibrsBatchDal.GetORIsWithPendingIncidentsToProcess(ori);
            List<(string,bool)> runNumbersPendingToUpload = new List<(string,bool)>();
            foreach (DataRow row in nibrsBatchdt.Rows)
            {
                if (row["ori_number"].ToString() == ori)
                {
                    var tuple = (row["runnumber"].ToString(), (bool) row["Is_Processed"]);
                    // get the pending runnumbers
                    runNumbersPendingToUpload.Add(tuple);
                }
            }
             
            // HERE we are deciding whether the current batch reported to FBI or not, To process the runnumbers in the sequence we are doing below conditionc checks
            // 1) check if any pending runnumbers to upload? if none return true
            // 2) If any pending then check if the runNumbers to process in the provided 'runNumbersToProcess' includes all the runNumbers that are pending according to database
            // 3) based on above conditions initialize the reportToFbi 
            // you have to process deletes for all pending runnumbers in the database to make isAnyPendingToUpload false, isAnyPendingToUpload decides whether to report the runnumbers to FBI or not.
            bool reportToFbi = !runNumbersPendingToUpload.Any() || runNumbersPendingToUpload.All( tuple => runNumbersToProcess.Contains(tuple.Item1));
            return (reportToFbi,runNumbersPendingToUpload);
        }
        

        #region Helpers

            public  List<IncidentList> BuildMissingRunNumbers(List<IncidentList> agencyIncidentsCollection, List<string> runNumbers,  Func<string, string, Task<IncidentList>> buildLibrsIncidentsListFunc)
            {
                var buildIncListFunc = new Func<string, IncidentList>((runNumber) =>
                {
                    var task = buildLibrsIncidentsListFunc(runNumber, "NORMAL");
                    task.Wait();
                    return task.Result;
                });
         
                var incidentList = runNumbers.ConvertAll( runNumber =>
                    agencyIncidentsCollection?.FirstOrDefault(incList => incList.Runnumber == runNumber) ??
                    buildIncListFunc(runNumber));
                return incidentList;
            }

            private  void SendErrorEmail(string subject, string body)
            {
                try
                {
                    
                    var emails =
                        Convert.ToString(_appSettingsReader.GetValue("CriticalErrorToEmails", typeof(string)));

                    EmailSender emailSender = new EmailSender();

                    emailSender.SendCriticalErrorEmail(emails,
                        subject,
                        body, false,
                        "donotreply@lcrx.librs.org", "");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            private void WriteSubmissions(IEnumerable<Submission> submissions, string path,
                bool clearPathBeforeSave = true)
            {
                try
                {
                    // check if path exists and clear the files
                    if (Directory.Exists(path) && clearPathBeforeSave)
                    {
                        Directory.Delete(path, clearPathBeforeSave);
                    }

                    // save xml file locally 
                    Parallel.ForEach(submissions, new ParallelOptions {MaxDegreeOfParallelism = 5}, submission =>
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        var docName = submission.Id + ".json";
                        string[] fullpath = {path, docName};
                        string nibrsSchemaLocation = Misc.schemaLocation;
                        //Save locally
                        submission.XsiSchemaLocation = nibrsSchemaLocation;
                        string fullPath = Path.Combine(fullpath);
                        File.WriteAllText(fullPath, submission.JsonString);
                    });
                }
                catch (AggregateException exception)
                {
                    foreach (var innerexception in exception.InnerExceptions)
                    {
                        ExceptionsLogger.Enqueue(Tuple.Create(innerexception, ""));
                    }
                }
            }

            private void WriteTransactions(IEnumerable<NibrsXmlTransaction> transactions, string path)
            {
                try
                {
                    // failed to save in MongoDb
                    Parallel.ForEach(transactions, trans =>
                    {
                        // save failed files.
                        string fileName = path + "\\" + trans.Submission.Runnumber;
                        if (!Directory.Exists(fileName))
                        {
                            Directory.CreateDirectory(fileName);
                        }

                        var docName = trans.Submission.Id + ".json";
                        string[] filePath = {fileName, docName};
                        string errorPath = Path.Combine(filePath);
                        File.WriteAllText(errorPath, trans.JsonString);
                    });
                }
                catch (AggregateException exception)
                {
                    foreach (var innerexception in exception.InnerExceptions)
                    {
                        ExceptionsLogger.Enqueue(Tuple.Create(innerexception, ""));
                    }
                }
            }

            

            private static void PrintExceptions(ConcurrentQueue<Tuple<Exception, string>> exceptionsLogger, Logger log,
                string ori, string batchFolderName)
            {
                if (exceptionsLogger.Any())
                {
                    while (exceptionsLogger.TryDequeue(out Tuple<Exception, string> tuple))
                    {
                        log.WriteLog(ori,
                            "Message :" + tuple.Item1.Message + "<br/>" + Environment.NewLine + "StackTrace :" +
                            tuple.Item1.StackTrace +
                            "" + Environment.NewLine + " File:" + tuple.Item2 + "Date :" +
                            DateTime.Now, batchFolderName);
                        log.WriteLog(ori,
                            Environment.NewLine +
                            "-----------------------------------------------------------------------------" +
                            Environment.NewLine, batchFolderName);
                    }
                }
            }

            private static bool PlaceLockOnAgency(AgencyCode agencyCode, byte[] lockKey, string ori)
            {
                // lock processing agency
                agencyCode.LockAgency(ori, lockKey);
                var dtLockedAgency = agencyCode.GetLockedAgency(ori, lockKey);
                if (dtLockedAgency.Rows.Count == 0)
                {
                    return false;
                }

                return true;
            }

            private static void ReleaseLockOnAgency(AgencyCode agencyCode, byte[] lockKey, string ori)
            {
                var dtLockedAgency = agencyCode.GetLockedAgency(ori, lockKey);
                if (dtLockedAgency.Rows.Count > 0)
                {
                    agencyCode.UnLockAgency(ori);
                }
            }
            public void MarkRunNumbersAsPending(List<string> runNumbers)
            {
                runNumbers.ForEach( runNumber => _nibrsBatchDal.Edit(runNumber,null,null,null,null,false));
            }
            #endregion

           
    }
    }