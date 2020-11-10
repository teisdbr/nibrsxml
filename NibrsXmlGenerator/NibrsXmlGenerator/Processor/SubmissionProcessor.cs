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



        public static List<SubmissionBatchStatus> ProcessSubmissionsBatch(List<IncidentList> agencyIncidentsCollection, string batchFolderName,
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
                ReProcessSubmissions(ori, batchFolderName);

                // Process the Batch in same order as it is received
                foreach (var incidentList in agencyGrp.ToList().OrderBy(grp => grp.Runnumber))
                {
                    var runNumber = incidentList.Runnumber;


                    try
                    {

                        log.WriteLog(ori,
                            DateTime.Now.ToString() + " : " + "--------- PROCESSING NIBRS DATA--------------",
                            batchFolderName);

                        IEnumerable<Submission> subs;

                        subs = SubmissionBuilder.BuildMultipleSubmission(new List<IncidentList>() { incidentList });

                        if (!subs.Any())
                        {
                            log.WriteLog(ori, DateTime.Now.ToString() + " : " + "NO NIBRS DATA TO PROCESS FOR RUNNUMBER: " +
                            runNumber,
                                batchFolderName);
                            var submissionStatus = new SubmissionBatchStatus()
                            {
                                RunNumber = runNumber,
                                Ori = ori,
                                Environmennt = incidentList.Environment,
                                NoOfSubmissions = subs.Count(),
                                HasErrorOccured = isOutOfSequence
                            };

                            submissionBatchStatusLst.Add(submissionStatus);
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

                        var tasks = SubmitSubToFbiAndAttemptSaveInMongoAsync(submissions, exceptionsLogger,
                            !isOutOfSequence);

                        Task.WaitAll(tasks);

                        if (tasks.Result.Any())
                        {
                            SaveTrans(tasks.Result, failedToSavePath, exceptionsLogger);
                            log.WriteLog(ori,
                                DateTime.Now.ToString() + " : " + "FILES  EITHER FAILED TO UPLOAD TO FBI PROPERLY OR ERROR IN SAVING THE FILES IN MONGO, moved to " + failedToSavePath +
                                " , RUNNUMBER : " + runNumber,
                                batchFolderName);
                            isOutOfSequence = true;
                        }

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

                        var submissionBatchStatus = new SubmissionBatchStatus()
                        {
                            RunNumber = runNumber,
                            Ori = ori,
                            Environmennt = incidentList.Environment,
                            NoOfSubmissions = subs.Count(),
                            HasErrorOccured = isOutOfSequence
                        };

                        submissionBatchStatusLst.Add(submissionBatchStatus);

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
            var httpresponses = await client.PostAsync(endpointURL, byteContent);
            if (!httpresponses.IsSuccessStatusCode)
            {
                var content = await httpresponses.Content.ReadAsStringAsync();
                Console.Write(content);
                var code = (int) httpresponses.StatusCode;
                
                throw new HttpException(code,content);

            }
            return httpresponses.IsSuccessStatusCode;
        }


        /// <summary>
        /// This method will submit the NibrsXml to FBI and attempt to save the NibrsXmlTransaction in MongoDb using LCRX API, returns the NibrsXmlTransaction failed to save in MongoDb or failed to send FBI.
        /// </summary>
        /// <param name="submissions"></param>
        /// <param name="exceptions"></param>
        /// <param name="isOutSequence"></param>
        /// <returns></returns>
        private async static Task<List<NibrsXmlTransaction>> SubmitSubToFbiAndAttemptSaveInMongoAsync(
            List<Submission> submissions, ConcurrentQueue<Tuple<Exception, ObjectId>> exceptions,
            bool attemptToSaveInMongo, bool attemptToSendFBI = true)
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
                    var response = submission.IsNibrsReportable && attemptToSendFBI ? NibrsSubmitter.SendReport(submission.Xml) : null;
                    //Wrap both response and submission and then save to database
                    NibrsXmlTransaction nibrsXmlTransaction = new NibrsXmlTransaction(submission, response);
                    var jsonContent = nibrsXmlTransaction.JsonString;

                    // If failed to report FBI. Don't save in MongoDB. Reattempt to Report FBI and save in MongoDb later.
                    if (nibrsXmlTransaction.Status == NibrsSubmissionStatusCodes.UploadFailed && attemptToSendFBI)
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

        public static void ReProcessSubmissions(string ori, string batchFolderName)
        {



            var log = new Logger();

            var appSettingsReader = new AppSettingsReader();

            var baseURL = System.Convert.ToString(appSettingsReader.GetValue("LcrxAPIURL", typeof(string)));

            var endpointURL = System.Convert.ToString(appSettingsReader.GetValue("SaveNibrsXmlEndpoint", typeof(string)));


            // get the paths to the folder 
            AgencyXmlDirectoryInfo agencyXmlDirectoryInfo = new AgencyXmlDirectoryInfo(ori);
            //  var failedToUploadDir = agencyXmlDirectoryInfo.GetFailedToUploadDirectory();
            var failedToSaveDir = agencyXmlDirectoryInfo.GetFailedToSaveDirectory();
            //   var isOutOfSequence = false;
            var exceptionsLogger = new ConcurrentQueue<Tuple<Exception, ObjectId>>();



            if (failedToSaveDir.GetDirectories().Any())
            {
                HttpClient client = new HttpClient();
                bool isAnyFailedToSave = false;
                log.WriteLog(ori, DateTime.Now.ToString() + ": Found some files pending to save in MongoDb", batchFolderName);

                foreach (var subDir in failedToSaveDir.GetDirectories().OrderBy(d => d.Name))
                {
                    var runnumber = subDir.Name;
                    log.WriteLog(ori, DateTime.Now.ToString() + ": Starting Process to  save files for runnumber: " + runnumber, batchFolderName);

                    if (isAnyFailedToSave)
                    {
                        log.WriteLog(ori, DateTime.Now.ToString() + ": Skipping the Process for runnumber: " + runnumber, batchFolderName);
                        break;
                    }

                    foreach (var fileInfo in subDir.GetFiles())
                    {
                        bool isSaved = false;

                        try
                        {
                            var reattemptTask = ReattemptToSaveTransactionInMongoDbAsync(fileInfo.FullName, baseURL + endpointURL,
                               client);

                            reattemptTask.Wait();
                            isSaved = reattemptTask.Result;
                        }

                        catch (AggregateException aex)
                        {
                            foreach (var exception in aex.InnerExceptions)
                            {
                                exceptionsLogger.Enqueue(Tuple.Create(exception, ObjectId.Empty));
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionsLogger.Enqueue(Tuple.Create(ex, ObjectId.Empty));
                        }


                        if (isSaved)
                            File.Delete(fileInfo.FullName);
                        else
                        {
                            isAnyFailedToSave = true;
                        }

                    }

                    if (isAnyFailedToSave)
                        log.WriteLog(ori, DateTime.Now.ToString() + ": Failed to report FBI or Save in MongoDb For RunNumber:" + runnumber, batchFolderName);


                    if (Directory.GetFiles(subDir.FullName).Length == 0 &&
                        Directory.GetDirectories(subDir.FullName).Length == 0)
                    {

                        log.WriteLog(ori, DateTime.Now.ToString() + ": Deleting the Folder" + subDir.FullName, batchFolderName);
                        Directory.Delete(subDir.FullName, true);
                    }

                    log.WriteLog(ori, DateTime.Now.ToString() + ": Completed Processing for the runNumber:" + runnumber, batchFolderName);

                }
            }

            if (exceptionsLogger.Any())
            {
                foreach (var tuple in exceptionsLogger)
                {
                    log.WriteLog(ori, "Message :" + tuple.Item1.Message + "<br/>" + Environment.NewLine + "StackTrace :" + tuple.Item1.StackTrace +
"" + Environment.NewLine + " File:" + tuple.Item2 + ".xml" + "Date :" + DateTime.Now.ToString(), batchFolderName);
                    log.WriteLog(ori, Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine, batchFolderName);

                }

            }




        }


        private static List<T> DeserilzeFiles<T>(DirectoryInfo directoryInfo, Logger log, string batchFolderName, string ori, Func<string, T> deserialize)
        {
            var deserilizeTasks = directoryInfo.GetFiles().Select(file => Task.Run(() => deserialize(file.FullName)));

            // failed to deserilaize  t.Status == TaskStatus.RanToCompletion
            try
            {
                Task.WaitAll(deserilizeTasks.ToArray());
            }
            catch (AggregateException aex)
            {
                foreach (var message in aex.Message)
                {

                    log.WriteLog(ori, "Exception while deserializing:" + message, batchFolderName);
                }
            }

            //TODO: Move the failed files to some other folder
            return deserilizeTasks.Where(task => task.Status == TaskStatus.RanToCompletion).Select(task => task.Result)?.ToList();
        }


        private static async Task<bool> ReattemptToSaveTransactionInMongoDbAsync(string filepath, string endpointURL, HttpClient client)
        {
            NibrsXmlTransaction nibrsXmlTransaction = NibrsXmlTransaction.Deserialize(filepath);

            // Check if any failed to upload and reattempt to report FBI.
            if (nibrsXmlTransaction.Status == NibrsSubmissionStatusCodes.UploadFailed)
            {
                var response = NibrsSubmitter.SendReport(nibrsXmlTransaction.Submission.Xml);
                if (response.IsUploadFailed)
                    return false;
                nibrsXmlTransaction.SetNibrsXmlSubmissionResponse(response);
            }


            var isSaved = await CallApiToSaveInMongoDbAsync(nibrsXmlTransaction.JsonString, endpointURL, client);
            return isSaved;
        }

        // This is deprecated because it might corrupt the prod database by hard deleting the data that is already reported to the FBI

        //public static async Task<List<SubmissionBatchStatus>> DataSync(List<IncidentList> agencyIncidentsCollection, string batchFolderName)
        //{

        //    List<SubmissionBatchStatus> submissionBatchStatusLst = new List<SubmissionBatchStatus>();

        //    agencyIncidentsCollection = agencyIncidentsCollection.Where(incList => incList.Environment != "T").ToList();


        //    foreach (var agencyGrp in agencyIncidentsCollection.GroupBy(collection => collection.OriNumber))
        //    {
        //        var log = new Logger();
        //        var ori = agencyGrp.Key;

        //        var exceptionsLogger = new ConcurrentQueue<Tuple<Exception, ObjectId>>();

        //        log.WriteLog(ori, " DATA SYNC PROCESS STARTED", batchFolderName);


        //        // Process the Batch in same order as it is received
        //        foreach (var incidentList in agencyGrp.ToList().OrderBy(grp => grp.Runnumber))
        //        {
        //            var runNumber = incidentList.Runnumber;

        //            log.WriteLog(ori,
        //                DateTime.Now.ToString() + " : " + "STARTED XML FILES PROCESSING FOR RUNNUMBER : " +
        //                runNumber,
        //                batchFolderName);

        //            var subs = SubmissionBuilder.BuildMultipleSubmission(new List<IncidentList>() { incidentList });


        //            var failedToProcess
        //                = await SubmitSubToFbiAndAttemptSaveInMongoAsync(subs?.ToList(), exceptionsLogger,
        //              attemptToSaveInMongo: true, attemptToSendFBI: false);



        //            if (failedToProcess.Any())
        //            {
        //                log.WriteLog(ori,
        //                    DateTime.Now.ToString() + " : " + "SOMETHING WENT WRONG WHILE PROCESSING FILES; RUNNUMBER : " +
        //                    runNumber,
        //                    batchFolderName);

        //            }


        //            var submissionBatchStatus = new SubmissionBatchStatus()
        //            {
        //                RunNumber = runNumber,
        //                Ori = ori,
        //                Environmennt = incidentList.Environment,
        //                NoOfSubmissions = subs.Count(),
        //                HasErrorOccured = failedToProcess.Any()
        //            };

        //            submissionBatchStatusLst.Add(submissionBatchStatus);

        //            if (exceptionsLogger.Any())
        //            {
        //                foreach (var tuple in exceptionsLogger)
        //                {
        //                    log.WriteLog(ori,
        //                        "Message :" + tuple.Item1.Message + "<br/>" + Environment.NewLine + "StackTrace :" +
        //                        tuple.Item1.StackTrace +
        //                        "" + Environment.NewLine + " File:" + tuple.Item2 + ".xml" + "Date :" +
        //                        DateTime.Now.ToString(), batchFolderName);
        //                    log.WriteLog(ori,
        //                        Environment.NewLine +
        //                        "-----------------------------------------------------------------------------" +
        //                        Environment.NewLine, batchFolderName);

        //                }

        //                log.WriteLog(ori,
        //                    DateTime.Now.ToString() + " : " + "SOMETHING FAILED TO SAVE IN MONGODB, SO ABORTING THE PROCESS",
        //                    batchFolderName);
        //                break;
        //            }

        //            log.WriteLog(ori,
        //              DateTime.Now.ToString() + " : " + "COMPLETED FILE PROCESSING FOR RUNNUMBER : " +
        //              runNumber,
        //              batchFolderName);

        //        }

        //        log.WriteLog(ori, " DATA SYNC PROCESS COMPLETED", batchFolderName);
        //    }



        //    return submissionBatchStatusLst;
        //}


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


    }
}
