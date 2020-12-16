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

        public static int SaveSubmissions(IncidentList incidentList, bool forceDelete, string ori, string batchFolderName)
        {
            var exceptionsLogger = new ConcurrentQueue<Tuple<Exception, ObjectId>>();
            var log = new Logger();
            IEnumerable<Submission> subs = new List<Submission>();

            AgencyNibrsDirectoryInfo agencyNibrsDirectoryInfo = new AgencyNibrsDirectoryInfo(ori);

            var dataFolderInfo = agencyNibrsDirectoryInfo.GetDataDirectoryInfo();

            // Process the Batch in same order as it is received

            try
            {
                log.WriteLog(ori, $"{DateTime.Now} : SAVING THE NIBRS FILES FOR RUNNUMBER: {incidentList.Runnumber}",
                    batchFolderName);

                subs = SubmissionBuilder.BuildMultipleSubmission(incidentList);

                if (forceDelete)
                {
                    log.WriteLog(ori,
                        $"{DateTime.Now} : TRANSFORMING THE FILES INTO DELETES AS THE FORCE DELETE MODE IS TRUE",
                        batchFolderName);

                    subs = DeleteTransformer.TransformIntoDeletes(subs);
                }

                WriteSubmissions(subs, dataFolderInfo.FullName, exceptionsLogger);

                log.WriteLog(ori,
                    $"{DateTime.Now} : SAVING THE NIBRS FILES FOR RUNNUMBER: {incidentList.Runnumber} COMPLETE",
                    batchFolderName);
            }
            catch (Exception ex)
            {
                exceptionsLogger.Enqueue(Tuple.Create(ex, ObjectId.Empty));
               ClearPendingToProcess(agencyNibrsDirectoryInfo,incidentList.Runnumber);
                throw;
            }
            finally
            {
                PrintExceptions(exceptionsLogger, log, ori, batchFolderName);
            }
            return subs.Count();
        }

        public static void WriteSubmissionsBatch(List<IncidentList> agencyIncidentsCollection,
            string batchFolderName,
            bool forceDelete = false)
        {
            var log = new Logger(); 

            // Only process the agencyList with Environment C & P 
            agencyIncidentsCollection = agencyIncidentsCollection.Where(incList => incList.Environment != "T").ToList();

            foreach (var agencyGrp in agencyIncidentsCollection.GroupBy(collection => collection.OriNumber))
            {
                var ori = agencyGrp.Key;
               
                log.WriteLog(ori,
                    $"{DateTime.Now} : --------- WRITING NIBRS DATA--------------",
                    batchFolderName);

                foreach (var incidentList in agencyIncidentsCollection.ToList().OrderBy(grp => grp.Runnumber))
                {
                    // write xml files
                    SaveSubmissions(incidentList, forceDelete, ori, batchFolderName);
                }
            }
        }

        public static async Task ProcessSubmissionsBatchAsync(string batchFolderName)

        {
            var httpClient = new HttpClient();
            var exceptionsLogger = new ConcurrentQueue<Tuple<Exception, ObjectId>>();
            var log = new Logger();
            // to place locks
            var agencyCode = new AgencyCode(null);

            var dtAgencies = agencyCode.GetAllAgencies(false);

            foreach (var agency in dtAgencies.Rows)
            {
                var dr = agency as DataRow;
                var ori = dr["SYSTEM_ORI_NUMBER"].ToString();

                AgencyNibrsDirectoryInfo agencyNibrsDirectoryInfo = new AgencyNibrsDirectoryInfo(ori);

                var dataFolderInfo = agencyNibrsDirectoryInfo.GetDataDirectoryInfo();
               
                var lockKey = Guid.NewGuid().ToByteArray();
                try
                {
                    // lock processing agency
                    agencyCode.LockAgency(ori, lockKey);
                    var dtLockedAgency = agencyCode.GetLockedAgency(ori, lockKey);
                    if (dtLockedAgency.Rows.Count == 0)
                    {
                        continue;
                    }

                    // NIBRS Processor
                    var isOutOfSequence = await ReProcessPendingTransactionsAsync(ori, batchFolderName, httpClient, exceptionsLogger);

                    // Process the Batch in same order as it is received
                    foreach (var subDir in dataFolderInfo.GetDirectories().OrderBy(dir => dir.Name))
                    {

                        if (isOutOfSequence)
                            break;
                        var runNumber = subDir.Name;
                        log.WriteLog(ori, $"{DateTime.Now} :STARTED PROCESSING NIBRS DATA  FOR RUN-NUMBER: {runNumber}",
                            batchFolderName);
                        List<Submission> subs = subDir.GetFiles()
                            .Select(fileInfo => Submission.DeserializeJson(fileInfo.FullName))?.ToList();

                        if (subs == null || !subs.Any())
                        {
                            log.WriteLog(ori, $"{DateTime.Now} : NO NIBRS DATA TO PROCESS FOR RUN-NUMBER: {runNumber}",
                                batchFolderName);
                            Directory.Delete(subDir.FullName, true);
                            continue;
                        }
                        isOutOfSequence = await SubmitSubmissionsAsync(subs, ori, runNumber, batchFolderName,
                            exceptionsLogger, httpClient);
                        string dataFullPath = dataFolderInfo.FullName + "\\" + runNumber;

                        if (!isOutOfSequence)
                        {
                            // Update the Nibrs Batch 
                            var dal = new Nibrs_Batch();
                            dal.Edit(runNumber, null, null, null, true);
                        }
                     

                        // Delete Processed
                        Directory.Delete(dataFullPath, true);

                        log.WriteLog(ori, $"{DateTime.Now} :COMPLETED PROCESSING NIBRS DATA FOR RUN-NUMBER: {runNumber}",
                           batchFolderName);
                    }

                    if (isOutOfSequence)
                    {
                        log.WriteLog(ori, $"{DateTime.Now} :SOMETHING WENT WRONG WHILE PROCESSING NIBRS DATA",
                            batchFolderName);
                        var appSettingsReader = new AppSettingsReader();
                        var emails =
                            Convert.ToString(appSettingsReader.GetValue("CriticalErrorToEmails", typeof(string)));

                        EmailSender emailSender = new EmailSender();

                        emailSender.SendCriticalErrorEmail(emails,
                            "Something went wrong while trying to process the submission batch for ORI:" + ori,
                            "Please check the logs for more details." + Environment.NewLine, false,
                            "donotreply@lcrx.librs.org", "");
                    }

                }
                catch (AggregateException exception)
                {
                    foreach (var innerException in exception.InnerExceptions)
                    {
                        log.WriteLog(ori, "Exception:" + innerException.Message, batchFolderName);
                        exceptionsLogger.Enqueue(Tuple.Create(innerException, ObjectId.Empty));
                    }
                }
                catch (Exception ex)
                {
                    log.WriteLog(ori, "Exception:" + ex.Message, batchFolderName);
                    exceptionsLogger.Enqueue(Tuple.Create(ex, ObjectId.Empty));
                    //TODO SEND EXCEPTION EMAIL
                    log.WriteLog(ori, DateTime.Now + " : " + "FAILED TO PROCESS NIBRS XML DATA ",
                        batchFolderName);
                    throw;
                }
                finally
                {

                    var dtLockedAgency = agencyCode.GetLockedAgency(ori, lockKey);
                    if (dtLockedAgency.Rows.Count > 0)
                    {
                        agencyCode.UnLockAgency(ori);
                    }

                    PrintExceptions(exceptionsLogger, log, ori, batchFolderName);
                   
                }
            }
           
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
        /// <param name="httpClient"></param>
        /// <param name="exceptionsLogger"></param>
        /// <returns></returns>
        public static async Task<bool> ReProcessPendingTransactionsAsync(string ori, string batchFolderName,
            HttpClient httpClient, ConcurrentQueue<Tuple<Exception, ObjectId>> exceptionsLogger
            )
        {

            var log = new Logger();

            // get the paths to the folder 
            AgencyNibrsDirectoryInfo agencyNibrsDirectoryInfo = new AgencyNibrsDirectoryInfo(ori);
            var errorDirectory = agencyNibrsDirectoryInfo.GetErroredDirectory();
            var failedToSaveDir = agencyNibrsDirectoryInfo.GetFailedToSaveDirectory();
            bool isAnyFailedToSave = false;

            if (failedToSaveDir.GetDirectories().Any())
            {
                log.WriteLog(ori, DateTime.Now + ": FOUND SOME FILES PENDING TO SAVE IN MONGODB", batchFolderName);

                foreach (var subDir in failedToSaveDir.GetDirectories().OrderBy(d => d.Name))
                {
                    var runNumber = subDir.Name;
                    try
                    {

                        log.WriteLog(ori,
                            DateTime.Now + ": STARTING PROCESS TO  SAVE FILES FOR RUN-NUMBER: " + runNumber,
                            batchFolderName);

                        if (isAnyFailedToSave)
                        {
                            log.WriteLog(ori,
                                DateTime.Now + ": SKIPPING THE PROCESS FOR RUN-NUMBER: " + runNumber,
                                batchFolderName);
                            break;
                        }

                        if (!subDir.GetFiles().Any())
                        {
                            log.WriteLog(ori,
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
                                await AttemptToSaveTransactionInMongoDbAsync(errorDirectory.FullName, pendingDeletes, httpClient,
                                    exceptionsLogger);
                        }

                        if (!failedToSaveDeletes.Any())
                        {
                            var failedToSave =
                                await AttemptToSaveTransactionInMongoDbAsync(errorDirectory.FullName,pendingTransactions, httpClient,
                                    exceptionsLogger);

                            if (!failedToSave.Any())
                            {
                                log.WriteLog(ori, DateTime.Now + ": DELETING THE FOLDER" + subDir.FullName,
                                    batchFolderName);
                                Directory.Delete(subDir.FullName, true);
                            }
                            else
                            {
                                log.WriteLog(ori,
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
                            log.WriteLog(ori,
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
                    }
                    catch (Exception ex)
                    {
                        exceptionsLogger.Enqueue(Tuple.Create(ex, ObjectId.Empty));
                        throw;
                    }
                    finally
                    {
                        // log exceptions
                        PrintExceptions(exceptionsLogger, log, ori, batchFolderName);
                        log.WriteLog(ori,
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
        /// <param name="client"></param>
        /// <param name="exceptionLogger"></param>
        /// <returns></returns>
        private static async Task<List<NibrsXmlTransaction>> AttemptToSaveTransactionInMongoDbAsync<T>(
           string pathToSaveErrorTransactions, IEnumerable<T> documents, HttpClient client,
            ConcurrentQueue<Tuple<Exception, ObjectId>> exceptionLogger)
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
                            isSaved = await CallApiToSaveInMongoDbAsync(nibrsTrans, baseURL + endpoint, client);
                        }
                        catch (Exception ex)
                        {
                            exceptionLogger.Enqueue(Tuple.Create(ex, nibrsTrans.Id));
                            if (ex is HttpException)
                            {
                                var httpException = (HttpException) ex;
                                if (httpException.GetHttpCode() == (int) HttpStatusCode.InternalServerError)
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
                        exceptionLogger.Enqueue(Tuple.Create(ex, ObjectId.Empty));
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
                WriteTransactions(errorTransactions, pathToSaveErrorTransactions, exceptionLogger);
                SendErrorEmail(pathToSaveErrorTransactions);
            }
        
            return failedToSave.ToList();

        }

        private static async Task<bool> SubmitSubmissionsAsync(List<Submission> subs, string ori, string runNumber,
            string batchFolderName, ConcurrentQueue<Tuple<Exception, ObjectId>> exceptionsLogger, HttpClient httpClient)
        {
            var log = new Logger();

            try
            {
                var agencyDir = new AgencyNibrsDirectoryInfo(ori);
                var failedToSavePath = agencyDir.GetFailedToSaveDirectory().FullName;
                var errorPath = agencyDir.GetErroredDirectory().FullName;

                log.WriteLog(ori,
                    DateTime.Now + " : " + "STARTED FILES PROCESSING FOR RUN-NUMBER : " +
                    runNumber,
                    batchFolderName);


                var failedToSave =
                    await AttemptToSaveTransactionInMongoDbAsync(errorPath,subs, httpClient, exceptionsLogger);

                if (failedToSave.Any())
                {
                    // save documents
                    WriteTransactions(failedToSave, failedToSavePath, exceptionsLogger);

                    log.WriteLog(ori,
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

                PrintExceptions(exceptionsLogger, log, ori, batchFolderName);
                log.WriteLog(ori,
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

        #endregion

    }
}
