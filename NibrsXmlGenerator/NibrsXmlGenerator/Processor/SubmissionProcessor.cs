using LoadBusinessLayer;
using NibrsModels.NibrsReport;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net.Http.Headers;
using NibrsInterface;
using System.Collections.Concurrent;
using MongoDB.Bson;
using System.Threading;
using System.Web;
using LoadBusinessLayer.Interfaces;
using NibrsXml.Builder;
using NibrsXml.Constants;

namespace NibrsXml.Processor
{
    public class SubmissionProcessor
    {

        public static void SaveSubmissions(List<IncidentList> agencyIncidentsCollection,bool forceDelete, string ori, string batchFolderName)
        {
            var exceptionsLogger = new ConcurrentQueue<Tuple<Exception, ObjectId>>();
            var log = new Logger();


            AgencyXmlDirectoryInfo agencyXmlDirectoryInfo = new AgencyXmlDirectoryInfo(ori);

            var dataFolderInfo = agencyXmlDirectoryInfo.GetDataDirectoryInfo();

            // Process the Batch in same order as it is received
            foreach (var incidentList in agencyIncidentsCollection.ToList().OrderBy(grp => grp.Runnumber))
            {
                try
                {
                   log.WriteLog(ori, $"{DateTime.Now} : SAVING THE NIBRS FILES FOR RUNNUMBER: {incidentList.Runnumber}",
                        batchFolderName);
                    IEnumerable<Submission> subs = new List<Submission>();
                    subs = SubmissionBuilder.BuildMultipleSubmission(incidentList);

                    if (forceDelete)
                    {
                        log.WriteLog(ori,
                            $"{DateTime.Now} : TRANSFORMING THE FILES INTO DELETES AS THE FORCE DELETE MODE IS TRUE",
                            batchFolderName);

                        subs = DeleteTransformer.TransformIntoDeletes(subs);
                    }

                    SaveSub(subs, dataFolderInfo.FullName, exceptionsLogger);

                    log.WriteLog(ori, $"{DateTime.Now} : SAVING THE NIBRS FILES FOR RUNNUMBER: {incidentList.Runnumber} COMPLETE",
                        batchFolderName);
                }
                catch (Exception ex)
                {
                    exceptionsLogger.Enqueue(Tuple.Create(ex, ObjectId.Empty));
                    throw ;
                }
            }

            PrintExceptions(exceptionsLogger, log, ori, batchFolderName);
        }

        public static async Task<List<SubmissionBatchStatus>> ProcessSubmissionsBatchAsync(
            List<IncidentList> agencyIncidentsCollection, string batchFolderName,
            bool forceDelete = false)
        
        {
            List<SubmissionBatchStatus> submissionBatchStatusLst = new List<SubmissionBatchStatus>();
            var log = new Logger();
            var httpClient = new HttpClient();
            var exceptionsLogger = new ConcurrentQueue<Tuple<Exception, ObjectId>>();

            // Only process the agencyList with Environment C & P 
            agencyIncidentsCollection = agencyIncidentsCollection.Where(incList => incList.Environment != "T").ToList();

            foreach (var agencyGrp in agencyIncidentsCollection.GroupBy(collection => collection.OriNumber))
            {
                var ori = agencyGrp.Key;
                var isOutOfSequence = false;
                try
                {
                    log.WriteLog(ori,
                        $"{DateTime.Now} : --------- PROCESSING NIBRS DATA--------------",
                        batchFolderName);

                    AgencyXmlDirectoryInfo agencyXmlDirectoryInfo = new AgencyXmlDirectoryInfo(ori);
                   
                    var failedToSavePath = agencyXmlDirectoryInfo.GetFailedToSaveLocation();
                    var dataFolderInfo = agencyXmlDirectoryInfo.GetDataDirectoryInfo();

                    if (forceDelete)
                    {
                        foreach (var incList in agencyGrp.ToList())
                        {

                            string failedToSaveFullPath = failedToSavePath + "\\" + incList.Runnumber;
                            // clear the Documents pending to be either uploaded or reported to the FBI, as we are trying to delete all incidents in this run number
                            if (Directory.Exists(failedToSaveFullPath))
                            {
                                Directory.Delete(failedToSaveFullPath, true);
                            }

                            string dataFullPath = dataFolderInfo.FullName + "\\" + incList.Runnumber;

                            if (Directory.Exists(dataFullPath))
                            {
                                Directory.Delete(dataFullPath, true);
                            }
                        }
                    }

                    // write xml files
                    SaveSubmissions(agencyIncidentsCollection, forceDelete, ori, batchFolderName);

                    isOutOfSequence = await ReProcessPendingTransactionsAsync(ori, batchFolderName, httpClient, exceptionsLogger);

                    // Process the Batch in same order as it is received
                    foreach (var subDir in dataFolderInfo.GetDirectories().OrderBy(dir => dir.Name))
                    {
                       
                        if (isOutOfSequence)
                            break;

                        var runNumber = subDir.Name;
                        List<Submission> subs = subDir.GetFiles().Select(fileInfo => Submission.DeserializeJson(fileInfo.FullName))?.ToList();
                          
                        if (subs == null || !subs.Any())
                        {
                            log.WriteLog(ori, $"{DateTime.Now} : NO NIBRS DATA TO PROCESS FOR RUN-NUMBER: {runNumber}",
                                batchFolderName);
                            Directory.Delete(subDir.FullName, true);
                        }
                        isOutOfSequence = await SubmitSubmissionsAsync(subs, ori, runNumber, batchFolderName, exceptionsLogger, httpClient);
                        string dataFullPath = dataFolderInfo.FullName + "\\" + runNumber;
                        // Delete Processed
                        Directory.Delete(dataFullPath, true);
                    }

                    if (isOutOfSequence)
                    {
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
                    log.WriteLog(ori, DateTime.Now.ToString() + " : " + "FAILED TO PROCESS NIBRS XML DATA ",
                        batchFolderName);
                    throw;
                }
                finally
                {
                    submissionBatchStatusLst.Add(new SubmissionBatchStatus()
                    {
                        Ori = ori,
                        HasErrorOccured = isOutOfSequence
                    });
                    PrintExceptions(exceptionsLogger, log, ori, batchFolderName);
                    log.WriteLog(ori, DateTime.Now.ToString() + " : " + " PROCESSING NIBRS DATA COMPLETED",
                        batchFolderName);
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

            // increment the attempt count 
            transaction.IncrementAttemptCount();

            var buffer = System.Text.Encoding.UTF8.GetBytes(transaction.JsonString);
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
            AgencyXmlDirectoryInfo agencyXmlDirectoryInfo = new AgencyXmlDirectoryInfo(ori);
            var failedFileLocation = agencyXmlDirectoryInfo.GetFailedLocation();
            var failedToSaveDir = agencyXmlDirectoryInfo.GetFailedToSaveDirectory();
            bool isAnyFailedToSave = false;

            if (failedToSaveDir.GetDirectories().Any())
            {
                log.WriteLog(ori, DateTime.Now.ToString() + ": FOUND SOME FILES PENDING TO SAVE IN MONGODB", batchFolderName);

                foreach (var subDir in failedToSaveDir.GetDirectories().OrderBy(d => d.Name))
                {
                    var runNumber = subDir.Name;
                    try
                    {

                        log.WriteLog(ori,
                            DateTime.Now.ToString() + ": STARTING PROCESS TO  SAVE FILES FOR RUN-NUMBER: " + runNumber,
                            batchFolderName);

                        if (isAnyFailedToSave)
                        {
                            log.WriteLog(ori,
                                DateTime.Now.ToString() + ": SKIPPING THE PROCESS FOR RUN-NUMBER: " + runNumber,
                                batchFolderName);
                            break;
                        }

                        if (!subDir.GetFiles().Any())
                        {
                            log.WriteLog(ori,
                                DateTime.Now.ToString() + ": NO FILES FOUND, DELETING THE FOLDER" + subDir.FullName,
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
                                await AttemptToSaveTransactionInMongoDbAsync(pendingDeletes, httpClient,
                                    exceptionsLogger);
                        }

                        if (!failedToSaveDeletes.Any())
                        {
                            var failedToSave =
                                await AttemptToSaveTransactionInMongoDbAsync(pendingTransactions, httpClient,
                                    exceptionsLogger);

                            // delete saved files from pending Deletes
                            Parallel.ForEach(pendingDeletes,
                                transaction =>
                                {
                                    File.Delete(subDir.FullName + "\\" + transaction.Id + ".json");
                                });

                            if (!failedToSave.Any())
                            {
                                log.WriteLog(ori, DateTime.Now.ToString() + ": DELETING THE FOLDER" + subDir.FullName,
                                    batchFolderName);
                                Directory.Delete(subDir.FullName, true);
                            }
                            else
                            {
                                log.WriteLog(ori,
                                    DateTime.Now.ToString() +
                                    ": FAILED TO REPORT FBI OR SAVE IN MONGODB FOR RUN-NUMBER:" + runNumber,
                                    batchFolderName);
                                isAnyFailedToSave = true;
                                // delete saved files from pendingTransactions
                                Parallel.ForEach(pendingTransactions.Where(trans => failedToSave.All(failTrans => trans.Id != failTrans.Id)),
                                    transaction =>
                                    {
                                        File.Delete(subDir.FullName + "\\" + transaction.Id + ".json");
                                    });

                                SaveFailedTransactions(ori, failedToSave, subDir, failedFileLocation, exceptionsLogger);
                            }
                        }
                        else
                        {
                            isAnyFailedToSave = true;
                            log.WriteLog(ori,
                                DateTime.Now.ToString() + ": FAILED TO REPORT FBI OR SAVE IN MONGODB FOR RUN-NUMBER:" +
                                runNumber, batchFolderName);
                            // delete saved files from pendingDeletes
                            Parallel.ForEach(pendingDeletes.Where(trans => failedToSaveDeletes.All(failTrans => trans.Id != failTrans.Id)),
                                transaction =>
                                {

                                    File.Delete(subDir.FullName + "\\" + transaction.Id + ".json");
                                });
                            SaveFailedTransactions(ori, failedToSaveDeletes, subDir, failedFileLocation,
                                exceptionsLogger);
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
                            DateTime.Now.ToString() + ": COMPLETED PROCESSING FOR THE RUN-NUMBER:" + runNumber,
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
        /// <param name="documents"></param>
        /// <param name="client"></param>
        /// <param name="exceptionLogger"></param>
        /// <returns></returns>
        private static async Task<List<NibrsXmlTransaction>> AttemptToSaveTransactionInMongoDbAsync<T>(
            IEnumerable<T> documents, HttpClient client,
            ConcurrentQueue<Tuple<Exception, ObjectId>> exceptionLogger)
        {

            var appSettingsReader = new AppSettingsReader();

            var baseURL = System.Convert.ToString(appSettingsReader.GetValue("LcrxAPIURL", typeof(string)));
            var endpoint = appSettingsReader.GetValue("SaveNibrsXmlEndpoint", typeof(String)).ToString();

            var maxDegreeOfParallelism = Convert.ToInt32(appSettingsReader.GetValue("MaxDegreeOfParallelism", typeof(int)));

            ConcurrentBag<NibrsXmlTransaction> failedToSave = new ConcurrentBag<NibrsXmlTransaction>();

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

            return failedToSave.ToList();

        }

        private static async Task<bool> SubmitSubmissionsAsync(List<Submission> subs, string ori, string runNumber,
            string batchFolderName, ConcurrentQueue<Tuple<Exception, ObjectId>> exceptionsLogger, HttpClient httpClient)
        {
            var log = new Logger();

            try
            {

                var failedToSavePath = new AgencyXmlDirectoryInfo(ori).GetFailedToSaveLocation();

                log.WriteLog(ori,
                    DateTime.Now.ToString() + " : " + "STARTED FILES PROCESSING FOR RUN-NUMBER : " +
                    runNumber,
                    batchFolderName);


                var failedToSave =
                    await AttemptToSaveTransactionInMongoDbAsync(subs, httpClient, exceptionsLogger);

                if (failedToSave.Any())
                {
                    // save documents
                    SaveTrans(failedToSave, failedToSavePath, exceptionsLogger);

                    log.WriteLog(ori,
                        DateTime.Now.ToString() + " : " +
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
                    DateTime.Now.ToString() + " : " +
                    "COMPLETED FILES PROCESSING FOR RUN-NUMBER : " + runNumber,
                    batchFolderName);
            }

            return false;
        }

        #region Helpers

        private static void SaveFailedTransactions(string ori, IEnumerable<NibrsXmlTransaction> failedToSave, DirectoryInfo currentDirectoryInfo, string failedFileLocation, ConcurrentQueue<Tuple<Exception, ObjectId>> exceptionsLogger)
        {
            var maxAllowedAttempts = 4;
            try
            {

                //// save only that failed to Save in the failedToSave folder and Number of attempts less than threshold.
                Parallel.ForEach(failedToSave.Where(trans => trans.NumberOfAttempts <= maxAllowedAttempts),
                    transaction =>
                    {
                        // update the document response or/and attempt count 
                        File.WriteAllText(currentDirectoryInfo.FullName + "\\" + transaction.Id + ".json", transaction.JsonString);
                    });

                //SaveTrans(failedToSave.Where(trans => trans.NumberOfAttempts <= maxAllowedAttempts), currentDirectoryInfo.FullName, exceptionsLogger);

                // if number of attempts are more than Threshold
                if (failedToSave.Any(trans => trans.NumberOfAttempts > maxAllowedAttempts))
                {

                    var failedTrans = failedToSave.Where(trans => trans.NumberOfAttempts > maxAllowedAttempts);
                    // save in failed folder
                    SaveTrans(failedTrans, failedFileLocation, exceptionsLogger);
                    // Delete them from the failed to save folder
                    Parallel.ForEach(failedToSave.Where(trans => trans.NumberOfAttempts > maxAllowedAttempts),
                        transaction =>
                        {
                            // update the document response or/and attempt count 
                            File.Delete(currentDirectoryInfo.FullName + "\\" + transaction.Id + ".json");
                        });

                    var appSettingsReader = new AppSettingsReader();
                    var emails =
                        Convert.ToString(appSettingsReader.GetValue("CriticalErrorToEmails", typeof(string)));

                    EmailSender emailSender = new EmailSender();

                    emailSender.SendCriticalErrorEmail(emails,
                        "Found Some Files Failed to save in the MongoDb Clusters after multiple attempts, that needs your attention for ORI:" +
                        ori,
                        "Please check the logs and Directory:" + failedFileLocation + "for more details." +
                        Environment.NewLine, false,
                        "donotreply@lcrx.librs.org", "");
                }


            }
            catch (Exception ex)
            {
                exceptionsLogger.Enqueue(Tuple.Create(ex, ObjectId.Empty));
                throw;
            }



        }

        private static void SaveSub(IEnumerable<Submission> submissions, string path, ConcurrentQueue<Tuple<Exception, ObjectId>> exceptions)
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
                    var docName = submission.Id.ToString() + ".json";
                    string[] fullpath = { fileName, docName };
                    string nibrsSchemaLocation = NibrsModels.Constants.Misc.schemaLocation;
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


        private static void SaveTrans(IEnumerable<NibrsXmlTransaction> transactions, string path, ConcurrentQueue<Tuple<Exception, ObjectId>> exceptions)
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
                        DateTime.Now.ToString(), batchFolderName);
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
