using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IdentityModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using LoadBusinessLayer;
using LoadBusinessLayer.Interfaces;
using MongoDB.Bson;
using NibrsInterface;
using NibrsModels.Constants;
using NibrsModels.NibrsReport;
using NibrsXml.Builder;
using NibrsXml.Constants;
using TeUtil.Extensions;

namespace NibrsXml.Processor
{
    public class NibrsReportingProcessor
    {

        public ConcurrentQueue<Tuple<Exception, ObjectId>> ExceptionsLogger { get; set; }

        public HttpClient HttpClient { get; set; }

        public Logger Log { get; set; }

        public NibrsReportingProcessor()
        {
            ExceptionsLogger = new ConcurrentQueue<Tuple<Exception, ObjectId>>();
            HttpClient = new HttpClient();
            Log = new Logger();
        }

        /// <summary>
        /// Process the requests to report  NIBRS deletes for given LIBRS incidents.
        /// </summary>
        /// <param name="agencyIncidentsCollection"></param>
        /// <param name="batchFolderName"></param>
        /// <param name="forceDelete"></param>
        /// <returns></returns>
        public async Task<List<SubmissionBatchStatus>> ProcessDeletesBatchAsync(List<IncidentList> agencyIncidentsCollection,
            string batchFolderName, Func<string, string, Task<IncidentList>> buildLibrsIncidentsListFunc)
        {
            var submissionBatchStatusLst = new List<SubmissionBatchStatus>();
            var anyFailedToSave = false;
            // Only process the agencyList with Environment C & P 
            agencyIncidentsCollection = agencyIncidentsCollection.Where(incList => incList.Environment != "T").ToList();

            foreach (var agencyGrp in agencyIncidentsCollection.GroupBy(collection => collection.OriNumber))
            {
                var ori = agencyGrp.Key;
                var nibrsBatchDal = new Nibrs_Batch();
           

                // Process the deletes in Last In First Out order 
                var agencyincidentList = agencyGrp.ToList().OrderByDescending(grp => grp.Runnumber);

                foreach (var incidentList in agencyincidentList)
                {
                    List<Submission> submissions = new List<Submission>();
                    var runNumber = incidentList.Runnumber;

                    try
                    {
                        
                        submissions = SubmissionBuilder.BuildMultipleSubmission(incidentList)?.ToList();

                        Log.WriteLog(ori, $"{DateTime.Now} : --------- RUNNING THE PROCESS IN THE FORCE DELETE MODE--------------",
                            batchFolderName);
                        submissions = DeleteTransformer.TransformIntoDeletes(submissions);
                        Log.WriteLog(ori, $"{DateTime.Now} :TRANSFORMED NIBRS DATA INTO DELETE'S  FOR RUN-NUMBER: {runNumber}",
                            batchFolderName);
                        anyFailedToSave = await SubmitSubmissionsForRunNumberAsync(submissions, ori, runNumber, batchFolderName);

                        if (!anyFailedToSave)
                            nibrsBatchDal.Delete(runNumber, null);
                        else
                            break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        ExceptionsLogger.Enqueue(Tuple.Create(e, ObjectId.Empty));
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
                            HasErrorOccured = anyFailedToSave
                        };
                        submissionBatchStatusLst.Add(submissionBatchStatus);
                        PrintExceptions(ExceptionsLogger, Log, ori, batchFolderName);
                    }
                }
            }

            return submissionBatchStatusLst;
        }

        /// <summary>
        /// Starts the Nibrs Processing for the next pending run-numbers for given ORI
        /// </summary>
        /// <param name="ori"></param>
        /// <param name="batchFolderName"></param>
        /// <param name="agencyIncidentsCollection"></param>
        /// <param name="buildLibrsIncidentsListFunc"></param>
        /// <returns></returns>
        public async Task<List<SubmissionBatchStatus>> RunProcessForOriAsync(string ori, string batchFolderName, List<IncidentList> agencyIncidentsCollection, Func<string, string, Task<IncidentList>> buildLibrsIncidentsListFunc)
        {

            var nibrsBatchDal = new Nibrs_Batch();
            var agencyXmlDirectoryInfo = new AgencyNibrsDirectoryInfo(ori);
            var submissionBatchStatusLst = new List<SubmissionBatchStatus>();
            var runNumbers = new List<string>();

            // limit the IncidentList to the given ORI
            agencyIncidentsCollection = agencyIncidentsCollection?.Where(inc => inc.OriNumber == ori)?.ToList();

            Log.WriteLog(ori, $"{DateTime.Now} :  ---------------------------------------PROCESSING NIBRS DATA------------------------------------- ",
                batchFolderName);

            //  Process any pending runnumber before taking up the new ones
            var isOutOfSequence = await ProcessPendingTransactionsForOriAsync(ori, batchFolderName, buildLibrsIncidentsListFunc);

            if (isOutOfSequence)
            {
                submissionBatchStatusLst.Add(new SubmissionBatchStatus() { Ori = ori, HasErrorOccured = isOutOfSequence });
                return submissionBatchStatusLst;
            }

         
            // foreach run number loop through and update the NIBRS  Batch
            // Process the Batch in same order as it is received

            var dt = nibrsBatchDal.Search(null, ori);
            // if no records present for the ORI in NIBRS batch, then this is first ever nibrs processing for the ORI,so no need to check the next runnumber in sequence from database.
            if (dt == null || dt?.Rows.Count == 0)
            {
                runNumbers = agencyIncidentsCollection?.OrderBy(inc => inc.Runnumber)
                    .Select(incList => incList.Runnumber).Distinct().ToList() ?? new List<string>() ;

            }
            else
            {
                // the stored procedure gives the run-numbers that are to be NIBRS processed in sequence
                var runNumbersdt = nibrsBatchDal.GetNextRunNumbersInSequence(ori);
                if (runNumbersdt?.Rows != null)
                    foreach (DataRow runNumber in runNumbersdt?.Rows)
                    {
                        runNumbers.UniqueAdd(runNumber["RUNNUMBER"].ToString());
                    }
            }

            foreach (var runNumber in runNumbers)
            {
                List<Submission> submissions = new List<Submission>();

                // if the incident list for the next run-number not provided, build incident list using the delegate.
                var incidentList = agencyIncidentsCollection?.FirstOrDefault(incList => incList.Runnumber == runNumber) ??
                                   await buildLibrsIncidentsListFunc(runNumber, "NORMAL");

                try
                {
                    if (isOutOfSequence)
                        break;

                    submissions = SubmissionBuilder.BuildMultipleSubmission(incidentList)?.ToList();

                    Log.WriteLog(ori,
                        $"{DateTime.Now} : Adding Batch details to the Database run number : {runNumber}",
                        batchFolderName);

                    // Update the Nibrs Batch table to have this run-number saying it is attempted to process. 
                    nibrsBatchDal.Add(runNumber, incidentList.Count(incList => !incList.HasErrors), submissions?.Count, DateTime.Now, DateTime.Now, false);

                    if (!submissions.Any())
                    {
                        Log.WriteLog(ori, $"{DateTime.Now} : NO NIBRS DATA TO PROCESS FOR RUN-NUMBER: {runNumber}",
                            batchFolderName);
                        //TODO should we report Zero Report for this runnumber?
                        // Update the Nibrs Batch to have the RunNumber saying the data is processed
                        nibrsBatchDal.Edit(runNumber, null, null, null, DateTime.Now, true);
                        continue;
                    }

                    var saveLocalPath = agencyXmlDirectoryInfo.GetArchiveLocation();
                    WriteSubmissions(submissions, saveLocalPath, ExceptionsLogger);
                    Log.WriteLog(ori,
                        $"{DateTime.Now} : SAVED All XML FILES FOR RUNNUMBER: {runNumber} AT {saveLocalPath}",
                        batchFolderName);

                    isOutOfSequence = await SubmitSubmissionsForRunNumberAsync(submissions, ori, runNumber, batchFolderName);

                    Log.WriteLog(ori,
                        $"{DateTime.Now} : OUTOFSEQUENCE:  {isOutOfSequence} for run number: {runNumber}",
                        batchFolderName);

                    // Update the Nibrs Batch to have the RunNumber saying the data is processed
                    nibrsBatchDal.Edit(runNumber, null, null, null, DateTime.Now, !isOutOfSequence);

                }
                finally
                {
                    var submissionBatchStatus = new SubmissionBatchStatus()
                    {
                        RunNumber = runNumber,
                        Ori = ori,
                        Environmennt = incidentList.Environment,
                        NoOfSubmissions = (submissions)?.Count() ?? 0,
                        HasErrorOccured = isOutOfSequence
                    };
                    submissionBatchStatusLst.Add(submissionBatchStatus);
                }
            }

            return submissionBatchStatusLst;
        }

        /// <summary>
        /// Process the Nibrs Batch for the given LIBRS Batch of Incidents
        /// </summary>
        /// <param name="agencyIncidentsCollection"></param>
        /// <param name="batchFolderName"></param>
        /// <param name="buildLibrsIncidentsListFunc"></param>
        /// <returns></returns>
        public async Task<List<SubmissionBatchStatus>> ProcessSubmissionsBatchAsync(
            List<IncidentList> agencyIncidentsCollection, string batchFolderName,
            Func<string, string, Task<IncidentList>> buildLibrsIncidentsListFunc)
        {
            AgencyCode agencyCode = new AgencyCode();
            var submissionBatchStatusLst = new List<SubmissionBatchStatus>();

            var nibrsBatchDal = new Nibrs_Batch();

            // Only process the agencyList with Environment C & P 
            agencyIncidentsCollection = agencyIncidentsCollection.Where(incList => incList.Environment != "T")?.ToList();

            List<string> oriList = new List<string>();

            // if agencyIncidentsCollection is provided stick to those ORIs
            if (agencyIncidentsCollection != null && agencyIncidentsCollection.Any())
                oriList = agencyIncidentsCollection.Select(incList => incList.OriNumber)?.Distinct().ToList();
            // Else get all failed to save ORIs process them one after the other.
            else
            {

                var nibrsBatchdt = nibrsBatchDal.GetORIsWithPendingIncidentsToProcess();
                foreach (DataRow row in nibrsBatchdt.Rows)
                {
                    
                        oriList.UniqueAdd(row["ori_number"]?.ToString()?.Trim());
                }
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

                    var submissionBatchList = await RunProcessForOriAsync(ori, batchFolderName, agencyIncidentsCollection,
                        buildLibrsIncidentsListFunc);

                    submissionBatchStatusLst.AddRange(submissionBatchList);


                    Log.WriteLog(ori, $"{DateTime.Now} :  PROCESSING NIBRS DATA COMPLETED",
                        batchFolderName);

                }
                catch (Exception ex)
                {
                    Log.WriteLog(ori, "Exception:" + ex.Message, batchFolderName);
                    ExceptionsLogger.Enqueue(Tuple.Create(ex, ObjectId.Empty));

                    Log.WriteLog(ori, DateTime.Now + " : " + "FAILED TO PROCESS NIBRS  DATA ",
                        batchFolderName);

                    SendErrorEmail($"Something went wrong while trying to process the submission batch for ORI:{ori}",
                        $"Please check the logs for more" +
                        $" details.{Environment.NewLine} Batch Folder Name {batchFolderName}  {Environment.NewLine} Exception {ex.Message} {ex.InnerException}");

                }
                finally
                {
                    ReleaseLockOnAgency(agencyCode, lockKey, ori);
                    PrintExceptions(ExceptionsLogger, Log, ori, batchFolderName);
                }
            }

            return submissionBatchStatusLst;
        }


        /// <summary>
        /// This Method calls the LCRx API.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="transaction"></param>
        /// <param name="endpointURL"></param>
        private static async Task<bool> PostAPIRequestAsync(NibrsXmlTransaction transaction, string endpointURL, HttpClient client)
        {


            var buffer = Encoding.UTF8.GetBytes(transaction.JsonString);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var responseMessage = await client.PostAsync(endpointURL, byteContent);
            if (!responseMessage.IsSuccessStatusCode)
            {
                var content = await responseMessage.Content.ReadAsStringAsync();
                Console.Write(content);
                var code = (int)responseMessage.StatusCode;

                throw new HttpException(code, content);
            }
            return responseMessage.IsSuccessStatusCode;
        }


        private static async Task<String> GetAPIRequestAsync(string endpointURL, HttpClient client)
        {
            var responseMessage = await client.GetAsync(endpointURL);
            var content = await responseMessage.Content.ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                Console.Write(content);
                var code = (int)responseMessage.StatusCode;

                throw new HttpException(code, content);
            }
            return content;
        }


        private async Task<bool> ProcessPendingTransactionsForOriAsync(string ori, string batchFolderName, Func<string, string, Task<IncidentList>> buildLibrsIncidentsListFunc)
        {
            bool isAnyFailedToSave = false;
            AgencyNibrsDirectoryInfo agencyNibrsDirectoryInfo = new AgencyNibrsDirectoryInfo(ori.Trim());
            string runNumber = String.Empty;
            var dal = new Nibrs_Batch();
            var errorDirectory = agencyNibrsDirectoryInfo.GetErroredDirectory();
            // get the pending runnumber of the ORI
            var nibrsBatchdt = dal.GetORIsWithPendingIncidentsToProcess();
            foreach (DataRow row in nibrsBatchdt.Rows)
            {
                if (row["ori_number"].ToString() == ori)
                {
                    // get the pending runnumber
                    runNumber = row["runnumber"].ToString();
                }
            }

            // no pending to be processed
            if (runNumber.IsNullBlankOrEmpty())
                return false;

            Log.WriteLog(ori, DateTime.Now + ": RE-PROCESSING PENDING RUN NUMBER:" + runNumber, batchFolderName);

            // As batch failed, we will re-process the whole batch
            var incidentList = await buildLibrsIncidentsListFunc(runNumber, "NORMAL");
            var submissions = SubmissionBuilder.BuildMultipleSubmission(incidentList);
            var deleteSubs = DeleteTransformer.TransformIntoDeletes(submissions);
            // Report deletes first
            Log.WriteLog(ori, DateTime.Now + ":REPORTING SUBMISSION DELETE'S FOR RUN NUMBER:" + runNumber, batchFolderName);
            isAnyFailedToSave =
                await AttemptToReportDocumentsAsync(ori, errorDirectory.FullName, deleteSubs);
            if (isAnyFailedToSave)
            {
                Log.WriteLog(ori, DateTime.Now + ":DOCUMENTS FAILED TO SAVE IN MONGODB FOR RUN NUMBER:" + runNumber, batchFolderName);
                return true;
            }
            Log.WriteLog(ori, $"{DateTime.Now}: REPORTING SUBMISSION FOR RUN NUMBER:{runNumber}", batchFolderName);
            isAnyFailedToSave =
                    await AttemptToReportDocumentsAsync(ori, errorDirectory.FullName, submissions);
            dal.Edit(runNumber,null,null,null,DateTime.Now, !isAnyFailedToSave);
            Log.WriteLog(ori, DateTime.Now + ":COMPLETED REPROCESSING FOR RUN NUMBER:" + runNumber, batchFolderName);

            return isAnyFailedToSave;
        }


      


        /// <summary>
        ///  This method will submit the NibrsXml to FBI and attempt to save the NibrsXmlTransaction in MongoDb using LCRX API, returns the NibrsXmlTransaction failed to save in MongoDb or failed to send FBI.
        /// Warning: This process does-not guarantee that all the requests made in the order as provided. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pathToSaveErrorTransactions"></param>
        /// <param name="documentsBatch"></param>
        /// <returns></returns>
        private async Task<bool> AttemptToReportDocumentsAsync<T>(string ori,
            string pathToSaveErrorTransactions, IEnumerable<T> documentsBatch)
        {

            var appSettingsReader = new AppSettingsReader();

            var baseURL = Convert.ToString(appSettingsReader.GetValue("LcrxAPIURL", typeof(string)));
            var endpoint = appSettingsReader.GetValue("SaveNibrsXmlEndpoint", typeof(String)).ToString();

            var maxDegreeOfParallelism = Convert.ToInt32(appSettingsReader.GetValue("MaxDegreeOfParallelism", typeof(int)));

            var reportToFbi = Convert.ToBoolean(appSettingsReader.GetValue("ReportToFBI", typeof(Boolean)));
            // TODO check with the LCRX API to see, if they are any failed to Upload for this ORI
            var checkPendingToUploadEndPoint = appSettingsReader.GetValue("CheckAnyPendingToUploadEndpoint", typeof(String)).ToString();
            checkPendingToUploadEndPoint =  $"{checkPendingToUploadEndPoint}?ori={ori}";
             var anyPending =
                    await GetAPIRequestAsync(baseURL + checkPendingToUploadEndPoint, HttpClient);
            ConcurrentBag<NibrsXmlTransaction> failedToSave = new ConcurrentBag<NibrsXmlTransaction>();
            ConcurrentBag<NibrsXmlTransaction> errorTransactions = new ConcurrentBag<NibrsXmlTransaction>();

            var requestTasks = new List<Task>();

            // The purpose of the semaphoreSlim to control the max number of concurrent tasks that can be ran in the requestTasks
            // MaxDegreeOfParallelism defines max number of tasks that can be at a time.
            SemaphoreSlim semaphoreSlim = new SemaphoreSlim(maxDegreeOfParallelism, maxDegreeOfParallelism);

            foreach (var doc in documentsBatch)
            {
                requestTasks.Add(Task.Run(async () =>
                {
                    var isSaved = false;

                    await semaphoreSlim.WaitAsync();
                    try
                    {
                        NibrsXmlTransaction nibrsTrans;

                        if (doc.GetType() == typeof(Submission))
                        {
                            var sub = doc as Submission;
                           
                            var response = sub.IsNibrsReportable  && reportToFbi && !Convert.ToBoolean(anyPending) ? NibrsSubmitter.SendReport(sub.Xml) : null;
                            //Wrap both response and submission and then save to database
                            nibrsTrans = new NibrsXmlTransaction(sub, response);
                        }
                        else
                        {
                            nibrsTrans = doc as NibrsXmlTransaction;

                            //// Check if any failed to upload and reattempt to report FBI.
                            //if (nibrsTrans.Status == NibrsSubmissionStatusCodes.UploadFailed)
                            //{
                            //    var response = NibrsSubmitter.SendReport(nibrsTrans.Submission.Xml);

                            //    nibrsTrans.SetNibrsXmlSubmissionResponse(response);
                            //}
                        }

                        // If upload failed don't attempt to save in mongoDB.
                        //if (nibrsTrans.Status == NibrsSubmissionStatusCodes.UploadFailed)
                        //{
                        //    failedToSave.Add(nibrsTrans);
                        //    return;
                        //}

                        try
                        {
                            isSaved = await PostAPIRequestAsync(nibrsTrans, baseURL + endpoint, HttpClient);
                        }
                        catch (Exception ex)
                        {
                            ExceptionsLogger.Enqueue(Tuple.Create(ex, nibrsTrans.Id));
                            if (ex is HttpException)
                            {
                                var httpException = (HttpException)ex;
                                if (httpException.GetHttpCode() == (int)HttpStatusCode.InternalServerError)
                                {
                                    errorTransactions.Add(nibrsTrans);
                                    return;
                                }
                            }

                        }
                        if (!isSaved)
                        {
                            failedToSave.Add(nibrsTrans);
                        }
                    }

                    catch (Exception ex)
                    {
                        ExceptionsLogger.Enqueue(Tuple.Create(ex, ObjectId.Empty));
                    }
                    finally
                    {
                        //When the task is ready, release the semaphore. It is vital to ALWAYS release the semaphore when we are ready, or else we will end up with a Semaphore that is forever locked.
                        //This is why it is important to do the Release within a try...finally clause; program execution may crash or take a different path, this way you are guaranteed execution
                        lock (semaphoreSlim)
                        {
                            semaphoreSlim.Release();
                        }
                    }
                }));

            };
            await Task.WhenAll(requestTasks.ToArray());

            // save in failed folder
            if (errorTransactions.Any())
            {
                WriteTransactions(errorTransactions, pathToSaveErrorTransactions, ExceptionsLogger);
                SendErrorEmail($"Found Some files failed to save in MongoDb Clusters, that needs your attention", $"Please check the logs and Directory:{pathToSaveErrorTransactions} for more details.{Environment.NewLine}");
            }

            return failedToSave.Any();

        }

        private async Task<bool> SubmitSubmissionsForRunNumberAsync(List<Submission> subs, string ori, string runNumber,
            string batchFolderName)
        {

            try
            {
                var agencyDir = new AgencyNibrsDirectoryInfo(ori);
                var errorPath = agencyDir.GetErroredDirectory().FullName;

                Log.WriteLog(ori,
                    DateTime.Now + " : " + "STARTED FILES PROCESSING FOR RUN-NUMBER : " +
                    runNumber,
                    batchFolderName);


                var anyFailedToSave =
                    await AttemptToReportDocumentsAsync(ori,errorPath, subs );
                return anyFailedToSave;

            }
            finally
            {

                PrintExceptions(ExceptionsLogger, Log, ori, batchFolderName);
                Log.WriteLog(ori,
                    DateTime.Now + " : " +
                    "COMPLETED FILES PROCESSING FOR RUN-NUMBER : " + runNumber,
                    batchFolderName);
            }

            
        }

        #region Helpers
        private static void SendErrorEmail(string subject, string body)
        {

            try
            {
                var appSettingsReader = new AppSettingsReader();
                var emails =
                    Convert.ToString(appSettingsReader.GetValue("CriticalErrorToEmails", typeof(string)));

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
        private static void WriteSubmissions(IEnumerable<Submission> submissions, string path, ConcurrentQueue<Tuple<Exception, ObjectId>> exceptions)
        {
            try
            {
                // save xml file locally 
                Parallel.ForEach(submissions, new ParallelOptions { MaxDegreeOfParallelism = 5 }, submission =>
                {
                    var fileName = path + "\\" + submission.Runnumber;
                    if (!Directory.Exists(fileName))
                    {
                        Directory.CreateDirectory(fileName);
                    }
                    var docName = submission.Id + ".json";
                    string[] fullpath = { fileName, docName };
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
                    exceptions.Enqueue(Tuple.Create(innerexception, ObjectId.Empty));
                }
            }
        }


        private static void WriteTransactions(IEnumerable<NibrsXmlTransaction> transactions, string path, ConcurrentQueue<Tuple<Exception, ObjectId>> exceptions)
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
                    string[] filePath = { fileName, docName };
                    string errorPath = Path.Combine(filePath);
                    File.WriteAllText(errorPath, trans.JsonString);
                });
            }
            catch (AggregateException exception)
            {
                foreach (var innerexception in exception.InnerExceptions)
                {
                    exceptions.Enqueue(Tuple.Create(innerexception, ObjectId.Empty));
                }
            }
        }

        private static void PrintExceptions(ConcurrentQueue<Tuple<Exception, ObjectId>> exceptionsLogger, Logger log,
            string ori, string batchFolderName)
        {
            if (exceptionsLogger.Any())
            {

                while (exceptionsLogger.TryDequeue(out Tuple<Exception, ObjectId> tuple))
                {
                    log.WriteLog(ori,
                        "Message :" + tuple.Item1.Message + "<br/>" + Environment.NewLine + "StackTrace :" +
                        tuple.Item1.StackTrace +
                        "" + Environment.NewLine + " File:" + tuple.Item2 + ".xml" + "Date :" +
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
        #endregion

    }
}
