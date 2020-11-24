using LoadBusinessLayer;
using NibrsModels.NibrsReport;
using System;
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
using Newtonsoft.Json;
using System.Net;
using System.Web.Util;
using System.ServiceModel.PeerResolvers;
using System.Data;
using System.Web;
using LoadBusinessLayer.Interfaces;
using NibrsXml;
using NibrsXml.Builder;
using NibrsXml.Constants;
using NibrsXml.Ucr.DataCollections;

namespace NibrsXml.Processor
{
    public class SubmissionProcessor
    {



        public static async Task<List<SubmissionBatchStatus>> ProcessSubmissionsBatchAsync(
            List<IncidentList> agencyIncidentsCollection, string batchFolderName,
            bool saveLocally = true, bool forceDelete = false)
        {
            var log = new Logger();

            var httpClient = new HttpClient();


            List<SubmissionBatchStatus> submissionBatchStatusLst = new List<SubmissionBatchStatus>();

            // Only process the agencyList with Environment C & P 
            agencyIncidentsCollection = agencyIncidentsCollection.Where(incList => incList.Environment != "T").ToList();


            foreach (var agencyGrp in agencyIncidentsCollection.GroupBy(collection => collection.OriNumber))
            {
                var ori = agencyGrp.Key;



                AgencyXmlDirectoryInfo agencyXmlDirectoryInfo = new AgencyXmlDirectoryInfo(ori);

                var isOutOfSequence = false;

                var failedToSavePath = agencyXmlDirectoryInfo.GetFailedToSaveLocation();



                if (forceDelete)
                {
                    foreach (var incList in agencyGrp.ToList())
                    {

                        string fileName = failedToSavePath + "\\" + incList.Runnumber;
                        // clear the Documents pending to be either uploaded or reported to the FBI, as we are trying to delete all incidents in this run number
                        if (Directory.Exists(fileName))
                        {
                            Directory.Delete(fileName, true);
                        }
                    }
                }



                // Check If anything pending to reprocess
                await ReProcessPendingSubmissionsAsync(ori, batchFolderName, httpClient);

                // Process the Batch in same order as it is received
                foreach (var incidentList in agencyGrp.ToList().OrderBy(grp => grp.Runnumber))
                {
                    var runNumber = incidentList.Runnumber;
                    IEnumerable<Submission> subs = new List<Submission>();

                    try
                    {

                        log.WriteLog(ori,
                            DateTime.Now.ToString() + " : " + "--------- PROCESSING NIBRS DATA--------------",
                            batchFolderName);



                        subs = SubmissionBuilder.BuildMultipleSubmission(new List<IncidentList>() { incidentList });

                        if (!subs.Any())
                        {
                            log.WriteLog(ori, DateTime.Now.ToString() + " : " +
                                              "NO NIBRS DATA TO PROCESS FOR RUNNUMBER: " +
                                              runNumber,
                                batchFolderName);


                            continue;
                        }

                        if (forceDelete)
                        {
                            log.WriteLog(ori,
                                DateTime.Now.ToString() + " : " +
                                "--------- RUNNING THE PROCESS IN THE FORCE DELETE MODE--------------",
                                batchFolderName);

                            subs = DeleteTransformer.TransformIntoDeletes(subs);

                        }

                        var submissions = subs.ToList();

                        var exceptionsLogger = new ConcurrentQueue<Tuple<Exception, ObjectId>>();

                        var saveLocalPath = agencyXmlDirectoryInfo.GetArchiveLocation();

                        if (saveLocally)
                        {

                            SaveSubXml(submissions, saveLocalPath, exceptionsLogger);

                            log.WriteLog(ori,
                                DateTime.Now.ToString() + " : " + "SAVED All XML FILES FOR RUNNUMBER: " + runNumber +
                                "AT " + saveLocalPath,
                                batchFolderName);
                        }

                        log.WriteLog(ori,
                            DateTime.Now.ToString() + " : " + "STARTED XML FILES PROCESSING FOR RUNNUMBER : " +
                            runNumber,
                            batchFolderName);


                        if (agencyXmlDirectoryInfo.GetFailedToSaveDirectory().GetDirectories().Length != 0)
                        {
                            log.WriteLog(ori,
                                DateTime.Now.ToString() + " : " + "Out Of SEQUENCE, RUNNUMBER : " + runNumber,
                                batchFolderName);

                            isOutOfSequence = true;

                            // save documents
                            SaveTrans(submissions.Select(sub => new NibrsXmlTransaction(sub, null)), failedToSavePath, exceptionsLogger);

                            log.WriteLog(ori,
                                DateTime.Now.ToString() + " : " +
                                "FILES  EITHER FAILED TO UPLOAD TO FBI PROPERLY OR ERROR IN SAVING THE FILES IN MONGO, moved to " +
                                failedToSavePath +
                                " , RUNNUMBER : " + runNumber,
                                batchFolderName);

                            continue;
                        }


                          var failedToSave = await AttemptToSaveTransactionInMongoDbAsync(submissions, httpClient, exceptionsLogger);


                        if (failedToSave.Any())
                        {

                            // save documents
                            SaveTrans(failedToSave, failedToSavePath, exceptionsLogger);

                            log.WriteLog(ori,
                                DateTime.Now.ToString() + " : " +
                                "FILES  EITHER FAILED TO UPLOAD TO FBI PROPERLY OR ERROR IN SAVING THE FILES IN MONGO, moved to " +
                                failedToSavePath +
                                " , RUNNUMBER : " + runNumber,
                                batchFolderName);

                            isOutOfSequence = true;
                        }

                        ExceptionLogger(exceptionsLogger, log, ori, batchFolderName);

                        log.WriteLog(ori,
                            DateTime.Now.ToString() + " : " +
                            "COMPLETED PROCESSING  XML FILES PROCESSING FOR RUNNUMBER : " + runNumber,
                            batchFolderName);
                    }
                    catch (AggregateException exception)
                    {
                        foreach (var innerexception in exception.InnerExceptions)
                        {
                            log.WriteLog(ori, "Exception:" + innerexception.Message, batchFolderName);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLog(ori, "Exception:" + ex.Message, batchFolderName);
                    }
                    finally
                    {
                        var submissionBatchStatus = new SubmissionBatchStatus()
                        {
                            RunNumber = runNumber,
                            Ori = ori,
                            Environmennt = incidentList.Environment,
                            NoOfSubmissions = subs.Count(),
                            HasErrorOccured = isOutOfSequence
                        };

                        submissionBatchStatusLst.Add(submissionBatchStatus);
                    }
                }

                if (isOutOfSequence)
                {
                    var appSettingsReader = new AppSettingsReader();
                    var emails = Convert.ToString(appSettingsReader.GetValue("CriticalErrorToEmails", typeof(string)));

                    EmailSender emailSender = new EmailSender();

                    emailSender.SendCriticalErrorEmail(emails, "Something went wrong while trying to process the submission batch for ORI:" + ori, "Please check the logs for more details." + Environment.NewLine, false, "donotreply@lcrx.librs.org", "");
                }

                log.WriteLog(ori, DateTime.Now.ToString() + " : " + " PROCESSING NIBRS DATA COMPLETED",
                    batchFolderName);


            }

            return submissionBatchStatusLst;
        }



        /// <summary>
        /// This Method calls the LCRx API.
        /// </summary>
        /// <param name="content"></param>
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
        /// <param name="submissions"></param>
        /// <param name="exceptions"></param>
        /// <param name="isOutSequence"></param>
        /// <returns></returns>
        public static async Task ReProcessPendingSubmissionsAsync(string ori, string batchFolderName,
            HttpClient httpClient)
        {

            var log = new Logger();
         
            // get the paths to the folder 
            AgencyXmlDirectoryInfo agencyXmlDirectoryInfo = new AgencyXmlDirectoryInfo(ori);
            var failedFileLocation = agencyXmlDirectoryInfo.GetFailedLocation();
            var failedToSaveDir = agencyXmlDirectoryInfo.GetFailedToSaveDirectory();
            var exceptionsLogger = new ConcurrentQueue<Tuple<Exception, ObjectId>>();

            if (failedToSaveDir.GetDirectories().Any())
            {
              
                bool isAnyFailedToSave = false;
                log.WriteLog(ori, DateTime.Now.ToString() + ": Found some files pending to save in MongoDb", batchFolderName);

                foreach (var subDir in failedToSaveDir.GetDirectories().OrderBy(d => d.Name))
                {
                    var runNumber = subDir.Name;

                    log.WriteLog(ori, DateTime.Now.ToString() + ": Starting Process to  save files for run-number: " + runNumber, batchFolderName);

                    if (isAnyFailedToSave)
                    {
                        log.WriteLog(ori, DateTime.Now.ToString() + ": Skipping the Process for run-number: " + runNumber, batchFolderName);
                        break;
                    }

                    if (!subDir.GetFiles().Any())
                    {
                  
                        log.WriteLog(ori, DateTime.Now.ToString() + ": No Files found, Deleting the Folder" + subDir.FullName, batchFolderName);
                        Directory.Delete(subDir.FullName);
                        continue;
                    }


                    var nibrsXmlTransactions = subDir.GetFiles().Select(fileInfo => NibrsXmlTransaction.Deserialize(fileInfo.FullName));
                    var pendingDeletes = nibrsXmlTransactions.Where(trans =>
                        trans.Submission.Reports[0].Header.ReportActionCategoryCode == "D");
                    var pendingTransactions = nibrsXmlTransactions.Where(trans =>
                        trans.Submission.Reports[0].Header.ReportActionCategoryCode != "D");

                    var failedToSaveDeletes = new List<NibrsXmlTransaction>();
                    // only deletes
                    if (pendingDeletes.Any())
                    {
                        failedToSaveDeletes = await AttemptToSaveTransactionInMongoDbAsync(pendingDeletes, httpClient, exceptionsLogger);
                    }

                    if (!failedToSaveDeletes.Any() && pendingTransactions.Any())
                    {
                        var failedToSave = await AttemptToSaveTransactionInMongoDbAsync(nibrsXmlTransactions, httpClient, exceptionsLogger);


                        if (!failedToSave.Any())
                        {
                            log.WriteLog(ori, DateTime.Now.ToString() + ": Deleting the Folder" + subDir.FullName, batchFolderName);
                            Directory.Delete(subDir.FullName, true);
                        }
                        else
                        {
                            log.WriteLog(ori, DateTime.Now.ToString() + ": Failed to report FBI or Save in MongoDb For RunNumber:" + runNumber, batchFolderName);
                            isAnyFailedToSave = true;
                            SaveFailedTransactions(failedToSave, subDir, failedFileLocation, exceptionsLogger);
                        }
                    }
                    else
                    {
                        isAnyFailedToSave = true;
                        log.WriteLog(ori, DateTime.Now.ToString() + ": Failed to report FBI or Save in MongoDb For RunNumber:" + runNumber, batchFolderName);
                        SaveFailedTransactions(failedToSaveDeletes, subDir, failedFileLocation, exceptionsLogger);
                    }

                    log.WriteLog(ori, DateTime.Now.ToString() + ": Completed Processing for the runNumber:" + runNumber, batchFolderName);

                }

            }

            // log exceptions
            ExceptionLogger(exceptionsLogger, log, ori, batchFolderName);

        }


        private static void SaveFailedTransactions(IEnumerable<NibrsXmlTransaction> nibrsXmlTransactions, DirectoryInfo subDir, string failedFileLocation, ConcurrentQueue<Tuple<Exception, ObjectId>> exceptionsLogger)
        {
            var maxAllowedAttempts = 3;
            // if number of attempts are more than Threshold
            if (nibrsXmlTransactions.Any(trans => trans.NumberOfAttempts > maxAllowedAttempts))
            {
                var failedTrans = nibrsXmlTransactions.Where(trans => trans.NumberOfAttempts > maxAllowedAttempts);
                // logic to move the file to the Failed folder
                SaveTrans(failedTrans, failedFileLocation, exceptionsLogger);
                foreach (var nibrsXmlTransaction in failedTrans)
                {
                    // delete from the failedToSave folder
                    File.Delete(subDir.FullName + "/" + nibrsXmlTransaction.Id + ".json");
                }
            }

            foreach (var transaction in nibrsXmlTransactions.Where(trans => trans.NumberOfAttempts <= maxAllowedAttempts))
            {
                // update the document response or attempt count 
                File.WriteAllText(subDir.FullName + "/" + transaction.Id + ".json", transaction.JsonString);
            }
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

            ConcurrentBag<NibrsXmlTransaction> failedToSave = new ConcurrentBag<NibrsXmlTransaction>();

            var requestTasks = new List<Task>();

            // The purpose of the semaphoreSlim to control the max number of concurrent tasks that can be ran in the requestTasks
            var maxDegreeOfParallelism = 50; // defines max number of tasks that can be at a time.
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




        private static void SaveSubXml(IEnumerable<Submission> submissions, string path, ConcurrentQueue<Tuple<Exception, ObjectId>> exceptions)
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
                    var docName = submission.Id.ToString() + ".xml";
                    string[] fullpath = { fileName, docName };
                    string nibrsSchemaLocation = NibrsModels.Constants.Misc.schemaLocation;
                //Save locally
                submission.XsiSchemaLocation = nibrsSchemaLocation;
                    var xdoc = new XmlDocument();

                    xdoc.LoadXml(submission.Xml);

                    string fullPath = Path.Combine(fullpath);
                    xdoc.Save(fullPath);
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

        private static void ExceptionLogger(ConcurrentQueue<Tuple<Exception, ObjectId>> exceptionsLogger, Logger log, string ori, string batchFolderName)
        {
            if (exceptionsLogger.Any())
            {
                foreach (var tuple in exceptionsLogger)
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



    }
}
