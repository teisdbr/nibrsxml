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

            List<SubmissionBatchStatus> submissionBatchStatusLst = new List<SubmissionBatchStatus>();

            // Only process the agencyList with Environment C & P 
            agencyIncidentsCollection = agencyIncidentsCollection.Where(incList => incList.Environment != "T").ToList();


            foreach (var agencyGrp in agencyIncidentsCollection.GroupBy(collection => collection.OriNumber))
            {
                var ori = agencyGrp.Key;

                AgencyXmlDirectoryInfo agencyXmlDirectoryInfo = new AgencyXmlDirectoryInfo(ori);

                var isOutOfSequence = false;

                // Check If anything pending to reprocess
                await ReProcessPendingSubmissionsAsync(ori, batchFolderName);

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

                       

                        subs = SubmissionBuilder.BuildMultipleSubmission(new List<IncidentList>() {incidentList});

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

                        var failedToSavePath = agencyXmlDirectoryInfo.GetFailedToSaveLocation();

                        if (agencyXmlDirectoryInfo.GetFailedToSaveDirectory().GetDirectories().Length != 0)
                        {
                            log.WriteLog(ori,
                                DateTime.Now.ToString() + " : " + "Out Of SEQUENCE, RUNNUMBER : " + runNumber,
                                batchFolderName);

                            isOutOfSequence = true;
                        }

                        var failedToSave = await SubmitSubToFbiAndAttemptSaveInMongoAsync(submissions, exceptionsLogger,
                            !isOutOfSequence);


                        if (failedToSave.Any())
                        {

                            if (forceDelete)
                            {
                                string fileName = failedToSavePath + "\\" + runNumber;
                                // clear the Documents pending to be either uploaded or reported to the FBI, as we are trying to delete all incidents in this run number
                                if (Directory.Exists(fileName))
                                {
                                    Directory.Delete(fileName, true);
                                }
                            }

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
        private static async Task<bool> CallApiToSaveInMongoDbAsync(string jsonString, string endpointURL, HttpClient client)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(jsonString);
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
        /// This method will submit the NibrsXml to FBI and attempt to save the NibrsXmlTransaction in MongoDb using LCRX API, returns the NibrsXmlTransaction failed to save in MongoDb or failed to send FBI.
        /// </summary>
        /// <param name="submissions"></param>
        /// <param name="exceptions"></param>
        /// <param name="isOutSequence"></param>
        /// <returns></returns>
        private static async Task<List<NibrsXmlTransaction>> SubmitSubToFbiAndAttemptSaveInMongoAsync(
            List<Submission> submissions, ConcurrentQueue<Tuple<Exception, ObjectId>> exceptions,
            bool attemptToSaveInMongo)
        {
            HttpClient httpClient = new HttpClient();

            var appSettingsReader = new AppSettingsReader();

            List<NibrsXmlTransaction> failedToSave = new List<NibrsXmlTransaction>();

            // List<Submission> failedToUpload = new List<Submission>();

            var baseURL = System.Convert.ToString(appSettingsReader.GetValue("LcrxAPIURL", typeof(string)));
            var endpoint = appSettingsReader.GetValue("SaveNibrsXmlEndpoint", typeof(String)).ToString();

            //List<Task> tasks = new List<Task>();

            foreach (var submission in submissions)
            {
                var isSaved = false;
                try
                {
                    // var response = NibrsSubmitter.SendReport(submission.Xml);
                    var response = submission.IsNibrsReportable ? NibrsSubmitter.SendReport(submission.Xml) : null;
                    //Wrap both response and submission and then save to database
                    NibrsXmlTransaction nibrsXmlTransaction = new NibrsXmlTransaction(submission, response);
                    var jsonContent = nibrsXmlTransaction.JsonString;

                    // If failed to report FBI. Don't save in MongoDB. Reattempt to Report FBI and save in MongoDb later.
                    if (nibrsXmlTransaction.Status == NibrsSubmissionStatusCodes.UploadFailed)
                    {
                        failedToSave.Add(nibrsXmlTransaction);
                        continue;
                    }


                    if (attemptToSaveInMongo)
                    {
                        try
                        {
                            isSaved = await CallApiToSaveInMongoDbAsync(jsonContent, baseURL + endpoint, httpClient);
                        }
                        catch (Exception ex)
                        {
                            exceptions.Enqueue(Tuple.Create(ex, submission.Id));

                        }
                    }
                    if (!isSaved)
                    {
                        failedToSave.Add(nibrsXmlTransaction);
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Enqueue(Tuple.Create(ex, submission.Id));

                }

            };

            httpClient.Dispose();

            return failedToSave;

        }

        public static async Task ReProcessPendingSubmissionsAsync(string ori, string batchFolderName)
        {

            var log = new Logger();

            var appSettingsReader = new AppSettingsReader();

            var baseURL = System.Convert.ToString(appSettingsReader.GetValue("LcrxAPIURL", typeof(string)));

            var endpointUrl = System.Convert.ToString(appSettingsReader.GetValue("SaveNibrsXmlEndpoint", typeof(string)));


            // get the paths to the folder 
            AgencyXmlDirectoryInfo agencyXmlDirectoryInfo = new AgencyXmlDirectoryInfo(ori);
            var failedFileLocation = agencyXmlDirectoryInfo.GetFailedLocation();
            var failedToSaveDir = agencyXmlDirectoryInfo.GetFailedToSaveDirectory();
            
            var exceptionsLogger = new ConcurrentQueue<Tuple<Exception, ObjectId>>();



            if (failedToSaveDir.GetDirectories().Any())
            {
                HttpClient client = new HttpClient();
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

                    foreach (var fileInfo in subDir.GetFiles().OrderBy( info => info.CreationTime))
                    {
                       
                        NibrsXmlTransaction nibsXmlTransaction = NibrsXmlTransaction.Deserialize(fileInfo.FullName);
                        
                      var  isSaved = await ReattemptToSaveTransactionInMongoDbAsync(nibsXmlTransaction, baseURL + endpointUrl,
                              client, exceptionsLogger);

                      if (isSaved)
                            File.Delete(fileInfo.FullName);
                        else
                        {
                            // if number of attempts are more than Threshold
                            if (nibsXmlTransaction.NumberOfAttempts > 5)
                            {
                                // logic to move the file to the Failed folder
                                SaveTrans(nibsXmlTransaction,failedFileLocation);
                                File.Delete(fileInfo.FullName);
                            }
                            else
                            {
                                // update the document response or attempt count 
                                File.WriteAllText(fileInfo.FullName, nibsXmlTransaction.JsonString);
                            }
                            isAnyFailedToSave = true;
                        }

                    }

                    if (isAnyFailedToSave)
                        log.WriteLog(ori, DateTime.Now.ToString() + ": Failed to report FBI or Save in MongoDb For RunNumber:" + runNumber, batchFolderName);


                    if (Directory.GetFiles(subDir.FullName).Length == 0 &&
                        Directory.GetDirectories(subDir.FullName).Length == 0)
                    {

                        log.WriteLog(ori, DateTime.Now.ToString() + ": Deleting the Folder" + subDir.FullName, batchFolderName);
                        Directory.Delete(subDir.FullName, true);
                    }

                    log.WriteLog(ori, DateTime.Now.ToString() + ": Completed Processing for the runNumber:" + runNumber, batchFolderName);

                }
            }

            // log exceptions
             ExceptionLogger(exceptionsLogger, log, ori, batchFolderName);

        }


       


        private static async Task<bool> ReattemptToSaveTransactionInMongoDbAsync(
            NibrsXmlTransaction nibrsXmlTransaction, string endpointURL, HttpClient client,
            ConcurrentQueue<Tuple<Exception, ObjectId>> exceptionLogger)
        {

            bool isSaved = false;
            try
            {

                // Check if any failed to upload and reattempt to report FBI.
                if (nibrsXmlTransaction.Status == NibrsSubmissionStatusCodes.UploadFailed)
                {
                    var response = NibrsSubmitter.SendReport(nibrsXmlTransaction.Submission.Xml);

                    nibrsXmlTransaction.SetNibrsXmlSubmissionResponse(response);

                    if (response.IsUploadFailed)
                        return false;
                }

                // increment the attempt count 
                nibrsXmlTransaction.IncrementAttemptCount();

                isSaved = await CallApiToSaveInMongoDbAsync(nibrsXmlTransaction.JsonString, endpointURL, client);

               
            }
            catch (AggregateException aex)
            {
                foreach (var exception in aex.InnerExceptions)
                {
                    exceptionLogger.Enqueue(Tuple.Create(exception, nibrsXmlTransaction.Id));
                }
            }
            catch (Exception ex)
            {
                exceptionLogger.Enqueue(Tuple.Create(ex, nibrsXmlTransaction.Id));
            }

            return isSaved;

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
                    SaveTrans(trans, path);
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


        private static void SaveTrans(NibrsXmlTransaction trans, string path)
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
        }


        private static void ExceptionLogger(ConcurrentQueue<Tuple<Exception, ObjectId>> exceptionsLogger, Logger log , string ori , string batchFolderName)
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
