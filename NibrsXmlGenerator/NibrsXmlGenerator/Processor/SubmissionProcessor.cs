using LoadBusinessLayer;
using NibrsXml.Builder;
using NibrsXml.NibrsReport;
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
using NibrsXml.Constants;
using NibrsXml.Utility;
using Newtonsoft.Json;
using System.Net;
using System.Web.Util;
using System.ServiceModel.PeerResolvers;
using System.Data;

namespace NibrsXml.Processor
{
    public class SubmissionProcessor
    {

        

        public static void ProcessSubmissionsBatch(List<IncidentList> agencyIncidentsCollection, string batchFolderName, bool saveLocally = true)
        {
            var log = new Logger();


          

            foreach (var agencyIncidentsGrp in agencyIncidentsCollection.GroupBy(collection => new { collection.OriNumber, collection.Runnumber },
                (key,g) => new  { Ori = key.OriNumber, Runnumber = key.Runnumber, Incidents = g  } ).OrderBy(grp => grp.Ori).ThenBy( grp => grp.Runnumber))
            {
                var ori = agencyIncidentsGrp.Ori;
                var runnumber = agencyIncidentsGrp.Runnumber;

                AgencyXmlDirectoryInfo agencyXmlDirectoryInfo = new AgencyXmlDirectoryInfo(ori);
               
               
                var isOutOfSequence = false;

                try
                {

                    log.WriteLog(ori, DateTime.Now.ToString() + " : " + "--------- PROCESSING NIBRS DATA--------------",
                                 batchFolderName);

                    var subs = SubmissionBuilder.BuildMultipleSubmission(agencyIncidentsGrp.Incidents.ToList());


                    if (subs.Count() == 0)
                    {
                        log.WriteLog(ori, DateTime.Now.ToString() + " : " + "NO NIBRS DATA TO PROCESS",
                                 batchFolderName);
                        continue;
                    }

                    //// if multiple files are submitted, process them in asec order of runnumbers.
                    //var submissionsGrp = subs.GroupBy(sub => sub.Runnumber).OrderBy(grp => grp.Key).ToArray();

                    //foreach (var grpsub in submissionsGrp)
                    //{
                        //try
                        //{
                          var submissions = subs.ToList();

                            var exceptionsLogger = new ConcurrentQueue<Tuple<Exception, ObjectId>>();

                            var saveLocalPath = agencyXmlDirectoryInfo.GetArchiveLocation();
                            if (saveLocally)
                            {
                                SaveSubXml(submissions, saveLocalPath,exceptionsLogger);
                            }


                            log.WriteLog(ori, DateTime.Now.ToString() + " : " + "SAVED All XML FILES FOR RUNNUMBER: " + runnumber + "AT " + saveLocalPath,
                                    batchFolderName);


                            log.WriteLog(ori, DateTime.Now.ToString() + " : " + "STARTED XML FILES PROCESSING FOR RUNNUMBER : " + runnumber,
                                   batchFolderName);


                            var failedToUploadPath= agencyXmlDirectoryInfo.GetFailedToUploadLocation();

                            var failedToSavePath = agencyXmlDirectoryInfo.GetFailedToSaveLocation();

                            //if (agencyXmlDirectoryInfo.GetFailedToUploadDirectory().GetDirectories().Length == 0)
                            //{                              

                                if (agencyXmlDirectoryInfo.GetFailedToSaveDirectory().GetDirectories().Length != 0)
                                    isOutOfSequence = true;
                             
                              
                                // ParallelOptions parlleloptions = new ParallelOptions();

                                var tasks = SubmitSubToFBIAndAttemptSaveInMongoAsync(submissions, exceptionsLogger, !isOutOfSequence);
                                //   Task.WaitAll(delTasks);

                                //var insertTasks = SubmitSubToFBIAndSaveTransInMongoAsync(submissions.Where(sub => sub.Reports[0].Header.ReportActionCategoryCode != "D")?.ToList(), exceptions);
                               Task.WaitAll(tasks);

                                 if (tasks.Result.Any())
                                {
                                   
                                   // SaveSubXml(tasks.Result.Item1, failedToUploadPath,exceptionsLogger);

                                    SaveTrans(tasks.Result, failedToSavePath,exceptionsLogger);                                 

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
                            //}
                           // else
                            //{
                               
                            //    SaveSubXml(submissions, failedToUploadPath,exceptionsLogger);
                            //}

                            log.WriteLog(ori, DateTime.Now.ToString() + " : " + "COMPLETED PROCESSING  XML FILES PROCESSING FOR RUNNUMBER : " + runnumber,
                                   batchFolderName);

                       

                    
                    log.WriteLog(ori, DateTime.Now.ToString() + " : " + " PROCESSING NIBRS DATA COMPLETED",
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
        }



        /// <summary>
        /// This Method calls the LCRx API.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="endpointURL"></param>
        private static async Task<bool> CallApiToSaveInMongoDbAsync(string jsonString, string endpointURL, HttpClient client)
        {
            //var client = new HttpClient();
            // var appSettingsReader = new AppSettingsReader();

            // var baseURL = System.Convert.ToString(appSettingsReader.GetValue("LcrxAPIURL", typeof(string)));

            try
            {

                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonString);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var httpresponses = await client.PostAsync(endpointURL, byteContent);
                return httpresponses.IsSuccessStatusCode;
            }
            catch (Exception ex)            {

                throw ;
            }
            //finally
            //{
            //    client.Dispose();

            //}

        }


        /// <summary>
        /// This method will submit the NibrsXml to FBI and attempt to save the NibrsXmlTransaction in MongoDb using LCRX API, returns the NibrsXmlTransaction failed to save in MongoDb or failed to send FBI.
        /// </summary>
        /// <param name="submissions"></param>
        /// <param name="exceptions"></param>
        /// <param name="isOutSequence"></param>
        /// <returns></returns>
        private async static Task<List<NibrsXmlTransaction>> SubmitSubToFBIAndAttemptSaveInMongoAsync(List<Submission> submissions, ConcurrentQueue<Tuple<Exception, ObjectId>> exceptions, bool attemptToSaveInMongo)
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

                    var response = NibrsSubmitter.SendReport(submission.Xml);
                    //Wrap both response and submission and then save to database
                    NibrsXmlTransaction nibrsXmlTransaction = new NibrsXmlTransaction(submission, response);
                    var jsonContent = nibrsXmlTransaction.JsonString;

                    //If failed to upload then also outofsequence we have to stop sending files to FBI
                    //if (nibrsXmlTransaction.Status == NibrsSubmissionStatusCodes.UploadFailed)
                    //{
                    //    failedToUpload.Add(submission);
                    //    throw new Exception("Failed To Upload the Nibrs Xml");
                    //}
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

        public static Task ReProcessSubmissions(string ori, string batchFolderName, string endpointURL)
        {


        var task =    new Task(() =>
            {

                var log = new Logger();
                // get the paths to the folder 
                AgencyXmlDirectoryInfo agencyXmlDirectoryInfo = new AgencyXmlDirectoryInfo(ori);
              //  var failedToUploadDir = agencyXmlDirectoryInfo.GetFailedToUploadDirectory();
                var failedToSaveDir = agencyXmlDirectoryInfo.GetFailedToSaveDirectory();
             //   var isOutOfSequence = false;
                var exceptionsLogger = new ConcurrentQueue<Tuple<Exception, ObjectId>>();

                //if (failedToUploadDir.GetDirectories().Any())
                //{
                //    log.WriteLog(ori, DateTime.Now.ToString() +  ": Found some files pending to upload", batchFolderName);

                //    //foreach (var subDir in failedToUploadDir.GetDirectories().OrderBy(d => d.Name))
                //    //{
                //    //    var runnumber = subDir.Name;

                //    //    if (failedToSaveDir.GetDirectories().Any())
                //    //        isOutOfSequence = true;

                //    //    log.WriteLog(ori, DateTime.Now.ToString() + ": Started uploading the files for runnumber: " + runnumber, batchFolderName);

                //    //    var submissions = DeserilzeFiles(subDir, log, batchFolderName, ori, Submission.Deserialize);
                //    //    var tasks = SubmitSubToFBIAndAttemptSaveInMongoAsync(submissions, exceptionsLogger, !isOutOfSequence);

                //    //    Task.WaitAll(tasks);

                //    //    if (tasks.Result.Item2.Any())
                //    //    {
                //    //        SaveTrans(tasks.Result.Item2, agencyXmlDirectoryInfo.GetFailedToSaveLocation(), exceptionsLogger);
                //    //    }

                //    //    if (tasks.Result.Item1.Any())
                //    //    {
                //    //        var failedToUploadSubs = tasks.Result.Item1;

                //    //        subDir.GetFiles().ToList().ForEach(fileInfo =>
                //    //        {
                //    //            if (failedToUploadSubs.Any(sub => sub.Id + ".xml" == fileInfo.Name))
                //    //                return;
                //    //            File.Delete(fileInfo.FullName);
                //    //        });

                //    //        log.WriteLog(ori, DateTime.Now.ToString() + ": Found some files that failed to upload  for runnumber: " + runnumber, batchFolderName);
                //    //        // stop uploading.
                //    //        break;

                //    //    }

                //    //    if (Directory.GetFiles(subDir.FullName).Length == 0 &&
                //    //            Directory.GetDirectories(subDir.FullName).Length == 0)
                //    //    {
                //    //        Directory.Delete(subDir.FullName, true);
                //    //    }

                       
                //    //}

                //}

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
                            Task<bool> taskToSave = ReattemptToSaveTransactionInMongoDbAsync(fileInfo.FullName, endpointURL, client);
                            taskToSave.Wait();

                            if (taskToSave.Result)
                                File.Delete(fileInfo.FullName);
                            else
                            {
                               
                                isAnyFailedToSave = true;
                            }                          

                        }


                        if (Directory.GetFiles(subDir.FullName).Length == 0 &&
                                    Directory.GetDirectories(subDir.FullName).Length == 0)
                        {

                            log.WriteLog(ori, DateTime.Now.ToString() + ": Deleting the Folder" + subDir.FullName, batchFolderName);
                            Directory.Delete(subDir.FullName, true);
                        }

                        log.WriteLog(ori, DateTime.Now.ToString() + ": Completed Processing for the runnumber:" + runnumber, batchFolderName);

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

            });

            task.Start();

            return task;           

        }


        private static List<T> DeserilzeFiles<T>(DirectoryInfo directoryInfo, Logger log, string batchFolderName, string ori, Func<string,T> deserialize) 
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

            //TODO: Move the failed files to someother folder
            return deserilizeTasks.Where(task => task.Status == TaskStatus.RanToCompletion).Select(task => task.Result)?.ToList();
        }


        private static async Task<bool> ReattemptToSaveTransactionInMongoDbAsync(string filepath, string endpointURL, HttpClient client)
        {

            try
            {
                NibrsXmlTransaction nibrsXmlTransaction = NibrsXmlTransaction.Deserialize(filepath);               
                var isSaved = await CallApiToSaveInMongoDbAsync(nibrsXmlTransaction.JsonString, endpointURL, client);
                return isSaved;
            }
            catch (Exception ex)
            {
                throw ex;
            }

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
                    string nibrsSchemaLocation = NibrsXml.Constants.Misc.schemaLocation;
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
