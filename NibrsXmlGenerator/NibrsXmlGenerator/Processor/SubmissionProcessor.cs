using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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

namespace NibrsXml.Processor
{
    public class SubmissionProcessor
    {

        public ConcurrentQueue<Tuple<Exception, ObjectId>> ExceptionsLogger { get; set; }

        public HttpClient HttpClient { get; set; }

        public Logger Log { get; set; }

        public SubmissionProcessor()
        {
            ExceptionsLogger = new ConcurrentQueue<Tuple<Exception, ObjectId>>();
            HttpClient = new HttpClient();
            Log = new Logger();
        }

        /// <summary>
        /// Process the requests to report  NIBRS deletes or refresh the NIBRS data, for given LIBRS incidents.
        /// </summary>
        /// <param name="agencyIncidentsCollection"></param>
        /// <param name="batchFolderName"></param>
        /// <param name="forceDelete"></param>
        /// <returns></returns>
        public async Task<List<SubmissionBatchStatus>> ProcessReAttemptSubmissionsBatchAsync(List<IncidentList> agencyIncidentsCollection,
            string batchFolderName, bool forceDelete)
        {
            var submissionBatchStatusLst = new List<SubmissionBatchStatus>();

            // Only process the agencyList with Environment C & P 
            agencyIncidentsCollection = agencyIncidentsCollection.Where(incList => incList.Environment != "T").ToList();

            foreach (var agencyGrp in agencyIncidentsCollection.GroupBy(collection => collection.OriNumber))
            {
                var ori = agencyGrp.Key;

                var isOutOfSequence = await ReProcessPendingTransactionsAsync(ori, batchFolderName);

                var agencyincidentList = forceDelete
                         ? agencyIncidentsCollection.ToList().OrderByDescending(grp => grp.Runnumber)
                         : agencyIncidentsCollection.ToList().OrderBy(grp => grp.Runnumber);

                foreach (var incidentList in agencyincidentList)
                {
                    List<Submission> submissions = new List<Submission>();
                    var runNumber = incidentList.Runnumber;

                    try
                    {
                        submissions = SubmissionBuilder.BuildMultipleSubmission(incidentList)?.ToList();
                        if (isOutOfSequence)
                            break;
                       
                        if (forceDelete)
                        {
                            Log.WriteLog(ori, $"{DateTime.Now} : --------- RUNNING THE PROCESS IN THE FORCE DELETE MODE--------------",
                                batchFolderName);
                            submissions = DeleteTransformer.TransformIntoDeletes(submissions);
                            Log.WriteLog(ori, $"{DateTime.Now} :TRANSFORMED NIBRS DATA INTO DELETE'S  FOR RUN-NUMBER: {runNumber}",
                                batchFolderName);
                        }
                        isOutOfSequence = await SubmitSubmissionsAsync(submissions, ori, runNumber, batchFolderName);
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
                            NoOfSubmissions = (submissions ?? new List<Submission>()).Count(),
                            HasErrorOccured = isOutOfSequence
                        };
                        submissionBatchStatusLst.Add(submissionBatchStatus);
                        PrintExceptions(ExceptionsLogger, Log, ori, batchFolderName);
                    }
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
        /// <param name="saveLocally"></param>
        /// <param name="placeLock"></param>
        /// <returns></returns>
        public async Task<List<SubmissionBatchStatus>> ProcessSubmissionsBatchAsync(
            List<IncidentList> agencyIncidentsCollection, string batchFolderName,
            Func<string, string, Task<IncidentList>> buildLibrsIncidentsListFunc,
            bool saveLocally = true, bool placeLock = true)
        {
            AgencyCode agencyCode = new AgencyCode();
            var submissionBatchStatusLst = new List<SubmissionBatchStatus>();

            var nibrsBatchDal = new Nibrs_Batch();

            // Only process the agencyList with Environment C & P 
            agencyIncidentsCollection = agencyIncidentsCollection.Where(incList => incList.Environment != "T")?.ToList();

            List<string> oriList = new List<string>();

            // if agencyIncidentsCollection is provided stick to those ORIs
            if (agencyIncidentsCollection != null && agencyIncidentsCollection.Any())
                oriList = agencyIncidentsCollection.Select(incList => incList.OriNumber).ToList();
            // Else get all failed to save ORIs process them one after the other.
            else
            {
                var nibrsBatchdt = nibrsBatchDal.GetORIsWithPendingIncidentsToProcess();
                foreach (DataRow row in nibrsBatchdt.Rows)
                {
                    oriList.Add(row["ori_number"].ToString());
                }
            }


            foreach (var ori in oriList)
            {

                var lockKey = Guid.NewGuid().ToByteArray();
                try
                {
                    if (placeLock)
                    {
                        if (!PlaceLockOnAgency(agencyCode, lockKey, ori))
                        {
                            Log.WriteLog(ori, $"{DateTime.Now} : COULDN'T HOLD THE LOCK ON THE ORI: {ori}",
                                batchFolderName);
                            continue;
                        }
                    }
                    AgencyNibrsDirectoryInfo agencyXmlDirectoryInfo = new AgencyNibrsDirectoryInfo(ori);

                    // NIBRS Processor
                    var isOutOfSequence = await ReProcessPendingTransactionsAsync(ori, batchFolderName);

                    if (isOutOfSequence)
                        continue;

                    List<string> runNumbers = new List<string>();


                    var dt = nibrsBatchDal.Search(null, ori);
                    // if no records present for the ORI in NIBRS batch, then this is first ever nibrs processing for the ORI,so no need to check the next runnumber in sequence from database.
                    if (dt.Rows.Count == 0)
                    {
                        runNumbers = agencyIncidentsCollection.OrderBy(inc => inc.Runnumber)
                            .Select(incList => incList.Runnumber).ToList();
                    }
                    else
                    {
                        // the stored procedure gives the run-numbers that are to be NIBRS processed in sequence
                        var runNumbersdt = nibrsBatchDal.GetNextRunNumbersInSequence(ori);
                        if (runNumbersdt?.Rows != null)
                            foreach (DataRow runNumber in runNumbersdt?.Rows)
                            {
                                runNumbers.Add(runNumber["RunNumber"].ToString());
                            }
                    }

                    // foreach run number loop through and update the NIBRS  Batch
                    // Process the Batch in same order as it is received
                    foreach (var runNumber in runNumbers)
                    {


                        List<Submission> submissions = new List<Submission>();

                        // if the incident list for the next run-number not provided, build incident list using the delegate.
                        var incidentList = agencyIncidentsCollection.FirstOrDefault(incList => incList.Runnumber == runNumber) ??
                                           await buildLibrsIncidentsListFunc(runNumber, "NORMAL");

                        try
                        {
                            if (isOutOfSequence)
                                break;

                            submissions = SubmissionBuilder.BuildMultipleSubmission(incidentList)?.ToList();

                            // Update the Nibrs Batch table to have this run-number saying it is attempted to process. 
                            nibrsBatchDal.Add(runNumber, incidentList.Count(incList => !incList.HasErrors), submissions?.Count, DateTime.Now, null, false);

                            if (!submissions.Any())
                            {
                                Log.WriteLog(ori, $"{DateTime.Now} : NO NIBRS DATA TO PROCESS FOR RUN-NUMBER: {runNumber}",
                                    batchFolderName);
                                continue;
                            }
                            if (saveLocally)
                            {
                                var saveLocalPath = agencyXmlDirectoryInfo.GetArchiveLocation();

                                WriteSubmissions(submissions, saveLocalPath, ExceptionsLogger);
                                Log.WriteLog(ori,
                                    $"{DateTime.Now} : SAVED All XML FILES FOR RUNNUMBER: {runNumber} AT {saveLocalPath}",
                                    batchFolderName);
                            }

                            isOutOfSequence = await SubmitSubmissionsAsync(submissions, ori, runNumber, batchFolderName);

                            // Update the Nibrs Batch to have the RunNumber saying the data is processed
                            nibrsBatchDal.Edit(runNumber, null, null, null, DateTime.Now, !isOutOfSequence);

                            Log.WriteLog(ori,
                            $"{DateTime.Now} : COMPLETED PROCESSING  XML FILES PROCESSING FOR RUN-NUMBER : {runNumber}",
                            batchFolderName);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteLog(ori, "Exception:" + ex.Message, batchFolderName);
                            throw;
                        }
                        finally
                        {
                            var submissionBatchStatus = new SubmissionBatchStatus()
                            {
                                RunNumber = runNumber,
                                Ori = ori,
                                Environmennt = incidentList.Environment,
                                NoOfSubmissions = (submissions).Count(),
                                HasErrorOccured = isOutOfSequence
                            };
                            submissionBatchStatusLst.Add(submissionBatchStatus);
                        }
                    }

                    Log.WriteLog(ori, $"{DateTime.Now} :  PROCESSING NIBRS DATA COMPLETED",
                        batchFolderName);

                }
                catch (Exception ex)
                {
                    Log.WriteLog(ori, "Exception:" + ex.Message, batchFolderName);
                    ExceptionsLogger.Enqueue(Tuple.Create(ex, ObjectId.Empty));

                    Log.WriteLog(ori, DateTime.Now + " : " + "FAILED TO PROCESS NIBRS  DATA ",
                        batchFolderName);

                    var appSettingsReader = new AppSettingsReader();
                    var emails = Convert.ToString(appSettingsReader.GetValue("CriticalErrorToEmails", typeof(string)));

                    EmailSender emailSender = new EmailSender();

                    emailSender.SendCriticalErrorEmail(emails,
                        $"Something went wrong while trying to process the submission batch for ORI:{ori}",
                        $"Please check the logs for more details.{Environment.NewLine} Batch Folder Name {batchFolderName}  {Environment.NewLine} Exception {ex.Message} {ex.InnerException}", false, "donotreply@lcrx.librs.org", "");
                    throw;
                }
                finally
                {
                    if (placeLock)
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
        private static async Task<bool> CallApiToSaveInMongoDbAsync(NibrsXmlTransaction transaction, string endpointURL, HttpClient client)
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


        /// <summary>
        /// This method tries to reattempt the failed to save/upload transactions
        /// </summary>
        /// <param name="ori"></param>
        /// <param name="batchFolderName"></param>
        /// <returns></returns>
        private async Task<bool> ReProcessPendingTransactionsAsync(string ori, string batchFolderName)
        {
            var dal = new Nibrs_Batch();
            // get the paths to the folder 
            AgencyNibrsDirectoryInfo agencyNibrsDirectoryInfo = new AgencyNibrsDirectoryInfo(ori);
            var errorDirectory = agencyNibrsDirectoryInfo.GetErroredDirectory();
            var failedToSaveDir = agencyNibrsDirectoryInfo.GetFailedToSaveDirectory();
            bool isAnyFailedToSave = false;

            if (failedToSaveDir.GetDirectories().Any())
            {
                Log.WriteLog(ori, DateTime.Now + ": FOUND SOME FILES PENDING TO SAVE IN MONGODB", batchFolderName);

                foreach (var subDir in failedToSaveDir.GetDirectories().OrderBy(d => d.Name))
                {
                    var runNumber = subDir.Name;
                    try
                    {

                        Log.WriteLog(ori,
                            DateTime.Now + ": STARTING PROCESS TO  SAVE FILES FOR RUN-NUMBER: " + runNumber,
                            batchFolderName);

                        if (isAnyFailedToSave)
                        {
                            Log.WriteLog(ori,
                                DateTime.Now + ": SKIPPING THE PROCESS FOR RUN-NUMBER: " + runNumber,
                                batchFolderName);
                            break;
                        }

                        // Update the last attempted date 
                        dal.Edit(runNumber, null, null, null, DateTime.Now, null);


                        if (!subDir.GetFiles().Any())
                        {
                            Log.WriteLog(ori,
                                DateTime.Now + ": NO FILES FOUND, DELETING THE FOLDER" + subDir.FullName,
                                batchFolderName);
                            Directory.Delete(subDir.FullName);
                            continue;
                        }


                        var nibrsXmlTransactions = subDir.GetFiles()
                            .Select(fileInfo => NibrsXmlTransaction.Deserialize(fileInfo.FullName));
                        var pendingDeletes = nibrsXmlTransactions.Where(trans =>
                            trans.Submission.Reports[0].Header.ReportActionCategoryCode == "D");
                        var pendingTransactions = nibrsXmlTransactions.Where(trans =>
                            trans.Submission.Reports[0].Header.ReportActionCategoryCode != "D");

                        var failedToSaveDeletes = new List<NibrsXmlTransaction>();
                        // only deletes
                        if (pendingDeletes.Any())
                        {
                            failedToSaveDeletes =
                                await AttemptToSaveTransactionInMongoDbAsync(errorDirectory.FullName, pendingDeletes);
                        }

                        if (!failedToSaveDeletes.Any())
                        {
                            var failedToSave =
                                await AttemptToSaveTransactionInMongoDbAsync(errorDirectory.FullName, pendingTransactions);

                            if (!failedToSave.Any())
                            {
                                Log.WriteLog(ori, DateTime.Now + ": DELETING THE FOLDER" + subDir.FullName,
                                    batchFolderName);
                                Directory.Delete(subDir.FullName, true);
                            }
                            else
                            {
                                Log.WriteLog(ori,
                                    DateTime.Now +
                                    ": FAILED TO REPORT FBI OR SAVE IN MONGODB FOR RUN-NUMBER:" + runNumber,
                                    batchFolderName);
                                isAnyFailedToSave = true;

                                // delete saved files from pendingTransactions
                                Parallel.ForEach(pendingTransactions.Where(trans => failedToSave.All(failTrans => trans.Id != failTrans.Id)),
                                    transaction =>
                                    {
                                        File.Delete(subDir.FullName + "\\" + transaction.Id + ".json");
                                    });
                                // delete saved files from pending Deletes
                                Parallel.ForEach(pendingDeletes,
                                    transaction =>
                                    {
                                        File.Delete(subDir.FullName + "\\" + transaction.Id + ".json");
                                    });

                                // save only that failed to Save in the failedToSave folder
                                Parallel.ForEach(failedToSave,
                                    transaction =>
                                    {
                                        // update the document response or/and attempt count 
                                        File.WriteAllText(subDir.FullName + "\\" + transaction.Id + ".json", transaction.JsonString);
                                    });
                            }
                        }
                        else
                        {
                            isAnyFailedToSave = true;
                            Log.WriteLog(ori,
                                DateTime.Now + ": FAILED TO REPORT FBI OR SAVE IN MONGODB FOR RUN-NUMBER:" +
                                runNumber, batchFolderName);
                            // delete saved files from pendingDeletes
                            Parallel.ForEach(pendingDeletes.Where(trans => failedToSaveDeletes.All(failTrans => trans.Id != failTrans.Id)),
                                transaction =>
                                {

                                    File.Delete(subDir.FullName + "\\" + transaction.Id + ".json");
                                });
                            // save only that failed to Save in the failedToSave folder
                            Parallel.ForEach(failedToSaveDeletes,
                                transaction =>
                                {
                                    // update the document response or/and attempt count 
                                    File.WriteAllText(subDir.FullName + "\\" + transaction.Id + ".json", transaction.JsonString);
                                });
                        }

                        dal.Edit(runNumber, null, null, null, null, isAnyFailedToSave);

                    }
                    catch (Exception ex)
                    {
                        ExceptionsLogger.Enqueue(Tuple.Create(ex, ObjectId.Empty));
                        throw;
                    }
                    finally
                    {
                        // log exceptions
                        PrintExceptions(ExceptionsLogger, Log, ori, batchFolderName);
                        Log.WriteLog(ori,
                            DateTime.Now + ": COMPLETED PROCESSING FOR THE RUN-NUMBER:" + runNumber,
                            batchFolderName);
                    }

                }
            }

            return isAnyFailedToSave;
        }


        /// <summary>
        ///  This method will submit the NibrsXml to FBI and attempt to save the NibrsXmlTransaction in MongoDb using LCRX API, returns the NibrsXmlTransaction failed to save in MongoDb or failed to send FBI.
        /// Warning: This process does-not guarantee that all the requests made in the order as provided. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pathToSaveErrorTransactions"></param>
        /// <param name="documents"></param>
        /// <returns></returns>
        private async Task<List<NibrsXmlTransaction>> AttemptToSaveTransactionInMongoDbAsync<T>(
            string pathToSaveErrorTransactions, IEnumerable<T> documents)
        {

            var appSettingsReader = new AppSettingsReader();

            var baseURL = Convert.ToString(appSettingsReader.GetValue("LcrxAPIURL", typeof(string)));
            var endpoint = appSettingsReader.GetValue("SaveNibrsXmlEndpoint", typeof(String)).ToString();

            var maxDegreeOfParallelism = Convert.ToInt32(appSettingsReader.GetValue("MaxDegreeOfParallelism", typeof(int)));

            ConcurrentBag<NibrsXmlTransaction> failedToSave = new ConcurrentBag<NibrsXmlTransaction>();
            ConcurrentBag<NibrsXmlTransaction> errorTransactions = new ConcurrentBag<NibrsXmlTransaction>();

            var requestTasks = new List<Task>();

            // The purpose of the semaphoreSlim to control the max number of concurrent tasks that can be ran in the requestTasks
            // MaxDegreeOfParallelism defines max number of tasks that can be at a time.
            SemaphoreSlim semaphoreSlim = new SemaphoreSlim(maxDegreeOfParallelism, maxDegreeOfParallelism);

            foreach (var doc in documents)
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
                            var response = sub.IsNibrsReportable ? NibrsSubmitter.SendReport(sub.Xml) : null;
                            //Wrap both response and submission and then save to database
                            nibrsTrans = new NibrsXmlTransaction(sub, response);
                        }
                        else
                        {
                            nibrsTrans = doc as NibrsXmlTransaction;

                            // Check if any failed to upload and reattempt to report FBI.
                            if (nibrsTrans.Status == NibrsSubmissionStatusCodes.UploadFailed)
                            {
                                var response = NibrsSubmitter.SendReport(nibrsTrans.Submission.Xml);

                                nibrsTrans.SetNibrsXmlSubmissionResponse(response);
                            }
                        }

                        // If upload failed don't attempt to save in mongoDB.
                        if (nibrsTrans.Status == NibrsSubmissionStatusCodes.UploadFailed)
                        {
                            failedToSave.Add(nibrsTrans);
                            return;
                        }

                        try
                        {
                            isSaved = await CallApiToSaveInMongoDbAsync(nibrsTrans, baseURL + endpoint, HttpClient);
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
                SendErrorEmail(pathToSaveErrorTransactions);
            }

            return failedToSave.ToList();

        }

        private async Task<bool> SubmitSubmissionsAsync(List<Submission> subs, string ori, string runNumber,
            string batchFolderName)
        {

            try
            {
                var agencyDir = new AgencyNibrsDirectoryInfo(ori);
                var failedToSavePath = agencyDir.GetFailedToSaveDirectory().FullName;
                var errorPath = agencyDir.GetErroredDirectory().FullName;

                Log.WriteLog(ori,
                    DateTime.Now + " : " + "STARTED FILES PROCESSING FOR RUN-NUMBER : " +
                    runNumber,
                    batchFolderName);


                var failedToSave =
                    await AttemptToSaveTransactionInMongoDbAsync(errorPath, subs);

                if (failedToSave.Any())
                {
                    // save documents
                    WriteTransactions(failedToSave, failedToSavePath, ExceptionsLogger);

                    Log.WriteLog(ori,
                        DateTime.Now + " : " +
                        "FILES  EITHER FAILED TO UPLOAD TO FBI PROPERLY OR ERROR IN SAVING THE FILES IN MONGODB, MOVED TO" +
                        failedToSavePath +
                        " , RUNNUMBER : " + runNumber,
                        batchFolderName);

                    return true;
                }
            }
            finally
            {

                PrintExceptions(ExceptionsLogger, Log, ori, batchFolderName);
                Log.WriteLog(ori,
                    DateTime.Now + " : " +
                    "COMPLETED FILES PROCESSING FOR RUN-NUMBER : " + runNumber,
                    batchFolderName);
            }

            return false;
        }

        #region Helpers
        private static void SendErrorEmail(string errorFileLocation)
        {
            var appSettingsReader = new AppSettingsReader();
            var emails =
                Convert.ToString(appSettingsReader.GetValue("CriticalErrorToEmails", typeof(string)));

            EmailSender emailSender = new EmailSender();

            emailSender.SendCriticalErrorEmail(emails,
                "Found Some Error out  save in the MongoDb Clusters, that needs your attention for ORI:",
                "Please check the logs and Directory:" + errorFileLocation + "for more details." +
                Environment.NewLine, false,
                "donotreply@lcrx.librs.org", "");
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


        public static void ClearPendingToProcess(AgencyNibrsDirectoryInfo agencyNibrsDirectoryInfo, string runNumber)
        {


            var failedToSaveDirectory = agencyNibrsDirectoryInfo.GetFailedToSaveDirectory();
            var dataFolderInfo = agencyNibrsDirectoryInfo.GetDataDirectoryInfo();


            string failedToSaveFullPath = failedToSaveDirectory.FullName + "\\" + runNumber;

            // clear the Documents pending to be either uploaded or reported to the FBI, as we are trying to delete all incidents in this run number
            if (Directory.Exists(failedToSaveFullPath))
            {
                Directory.Delete(failedToSaveFullPath, true);
            }

            string dataFullPath = dataFolderInfo.FullName + "\\" + runNumber;

            if (Directory.Exists(dataFullPath))
            {
                Directory.Delete(dataFullPath, true);
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
