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

namespace NibrsXml
{
    public class SubmissionProcessor
    {

        //public HttpClient httpClient = new HttpClient();

        //private bool isOutOfSequence { get; set; }

        public static void ProcessSubmissionsBatch(List<IncidentList> agencyIncidentsCollection, string batchFolderName, bool saveLocally = true)
        {
            var log = new Logger();

            var appSettingsReader = new AppSettingsReader();
            var nibrsXmlFilesFolderLocation = System.Convert.ToString(appSettingsReader.GetValue("IncomingNibrsXmlFilesFolderLocation", typeof(string)));
            var strIncomingFolderLocation = System.Convert.ToString(appSettingsReader.GetValue("ReadDirectoryPath", typeof(string)));
            var failedToUploadLocation = System.Convert.ToString(appSettingsReader.GetValue("FailedToUploadNibrsXmlFilesFolderLocation", typeof(string)));
            var failedToSaveLocation = System.Convert.ToString(appSettingsReader.GetValue("FailedToSaveNibrsXmlFilesFolderLocation", typeof(string)));

            var archiveXmlLocation = System.Convert.ToString(appSettingsReader.GetValue("ArchiveNibrsXmlFilesFolderLocation", typeof(string)));
            var nibrsXmlFolder = strIncomingFolderLocation + "//" + nibrsXmlFilesFolderLocation;

            foreach (var agencyIncidentsGrp in agencyIncidentsCollection.GroupBy(collection => collection.OriNumber))
            {
                var ori = agencyIncidentsGrp.Key;




                var isOutOfSequence = false;

                try
                {

                    log.WriteLog(ori, DateTime.Now.ToString() + " : " + "--------- PROCESSING NIBRS DATA--------------",
                                 batchFolderName);

                    var subs = SubmissionBuilder.BuildMultipleSubmission(agencyIncidentsGrp.ToList());


                    if (subs.Count() == 0)
                    {
                        log.WriteLog(ori, DateTime.Now.ToString() + " : " + "NO NIBRS DATA TO PROCESS",
                                 batchFolderName);
                        continue;
                    }



                    var archiveNibrsXmlPath = appSettingsReader.GetValue("ArchiveNibrsXmlFilesFolderLocation", typeof(string)).ToString();

                    // if multiple files are submitted, process them in asec order of runnumbers.
                    var submissionsGrp = subs.GroupBy(sub => sub.Runnumber).OrderBy(grp => grp.Key).ToArray();

                    foreach (var grpsub in submissionsGrp)
                    {
                        try
                        {
                            var submissions = grpsub.ToList();
                            var directory = archiveNibrsXmlPath + "\\" + ori + "\\" + grpsub.Key.ToString();

                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            if (saveLocally)
                            {
                                // save xml file locally 
                                Parallel.ForEach(submissions, new ParallelOptions { MaxDegreeOfParallelism = 5 }, async submission =>
                                {
                                    var docFileName = submission.Id.ToString() + ".xml";
                                    string[] paths = { directory, docFileName };
                                    string nibrsSchemaLocation = NibrsXml.Constants.Misc.schemaLocation;
                                    //Save locally
                                    submission.XsiSchemaLocation = nibrsSchemaLocation;
                                    var xdoc = new XmlDocument();

                                    xdoc.LoadXml(submission.Xml);

                                    string fullPath = Path.Combine(paths);
                                    xdoc.Save(fullPath);
                                });
                            }


                            log.WriteLog(ori, DateTime.Now.ToString() + " : " + "SAVED All XML FILES FOR RUNNUMBER: " + grpsub.Key + "AT " + directory,
                                    batchFolderName);


                            log.WriteLog(ori, DateTime.Now.ToString() + " : " + "STARTED XML FILES PROCESSING FOR RUNNUMBER : " + grpsub.Key,
                                   batchFolderName);


                            if (new DirectoryInfo(nibrsXmlFolder + "//" +  ))


                                var exceptions = new ConcurrentQueue<Tuple<Exception, ObjectId>>();
                            // ParallelOptions parlleloptions = new ParallelOptions();

                            var tasks = SubmitSubToFBIAndAttemptSaveInMongoAsync(submissions, exceptions, !isOutOfSequence);
                            //   Task.WaitAll(delTasks);

                            //var insertTasks = SubmitSubToFBIAndSaveTransInMongoAsync(submissions.Where(sub => sub.Reports[0].Header.ReportActionCategoryCode != "D")?.ToList(), exceptions);
                            Task.WaitAll(tasks);

                            if (tasks.Result.Item1.Any() || tasks.Result.Item2.Any())
                            {
                                // failed to upload 
                                Parallel.ForEach(tasks.Result.Item1, submission =>
                                {
                                    // save failed files.
                                    string errorFileName = appSettingsReader.GetValue("FailedNibrsXmlFilesFolderLocation", typeof(String)).ToString() + "//" + submission.Runnumber;
                                    var docName = submission.Id + ".xml";
                                    string[] errorFilePaths = { errorFileName, docName };
                                    string errorPath = Path.Combine(errorFilePaths);
                                    var xdoc = new XmlDocument();

                                    xdoc.LoadXml(submission.Xml);
                                    xdoc.Save(errorPath);
                                });

                                // failed to save in MongoDb
                                Parallel.ForEach(tasks.Result.Item2, trans =>
                                {
                                    // save failed files.
                                    string errorFileName = appSettingsReader.GetValue("FailedNibrsXmlFilesFolderLocation", typeof(String)).ToString() + "//" + trans.Submission.Runnumber;
                                    var docName = trans.Submission.Id + ".json";
                                    string[] errorFilePaths = { errorFileName, docName };
                                    string errorPath = Path.Combine(errorFilePaths);
                                    File.WriteAllText(errorPath, trans.JsonString);
                                });

                            }

                            if (exceptions.Any())
                            {
                                foreach (var tuple in exceptions)
                                {
                                    log.WriteLog(ori, "Message :" + tuple.Item1.Message + "<br/>" + Environment.NewLine + "StackTrace :" + tuple.Item1.StackTrace +
          "" + Environment.NewLine + " File:" + tuple.Item2 + ".xml" + "Date :" + DateTime.Now.ToString(), batchFolderName);
                                    log.WriteLog(ori, Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine, batchFolderName);

                                }

                                isOutOfSequence = true;

                            }

                            log.WriteLog(ori, DateTime.Now.ToString() + " : " + "COMPLETED PROCESSING  XML FILES PROCESSING FOR RUNNUMBER : " + grpsub.Key,
                                   batchFolderName);

                        }
                        catch (AggregateException exception)
                        {
                            foreach (var message in exception.Flatten().Message)
                            {
                                log.WriteLog(ori, "Exception:" + message, batchFolderName);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.WriteLog(ori, "Exception:" + ex.Message, batchFolderName);
                        }

                    }
                    log.WriteLog(ori, DateTime.Now.ToString() + " : " + " PROCESSING NIBRS DATA COMPLETED",
                                     batchFolderName);
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
            catch (Exception ex)
            {

                throw ex;
            }
            //finally
            //{
            //    client.Dispose();

            //}

        }







        /// <summary>
        /// This method will submit the NibrsXml to FBI and attempt to save the NibrsXmlTransaction in MongoDb using LCRX API, returns the NibrsXmlTransaction failed to save in MongoDb.
        /// </summary>
        /// <param name="submissions"></param>
        /// <param name="exceptions"></param>
        /// <param name="isOutSequence"></param>
        /// <returns></returns>
        private async static Task<Tuple<List<Submission>, List<NibrsXmlTransaction>>> SubmitSubToFBIAndAttemptSaveInMongoAsync(List<Submission> submissions, ConcurrentQueue<Tuple<Exception, ObjectId>> exceptions, bool attemptToSaveInMongo)
        {
            HttpClient httpClient = new HttpClient();

            var appSettingsReader = new AppSettingsReader();

            List<NibrsXmlTransaction> failedToSave = new List<NibrsXmlTransaction>();

            List<Submission> failedToUpload = new List<Submission>();

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
                    if (nibrsXmlTransaction.Status == NibrsSubmissionStatusCodes.UploadFailed)
                    {
                        failedToUpload.Add(submission);
                        throw new Exception("Failed To Upload the Nibrs Xml");
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

            return Tuple.Create(failedToUpload, failedToSave);

        }


        public static async Task<bool> ReattemptToSaveNibrsXmlInMongoDbAsync(string filepath, string endpointURL, HttpClient client)
        {

            try
            {
                NibrsXmlTransaction nibrsXmlTransaction = NibrsXmlTransaction.DeserializeNibrsXmlTransaction(filepath);
                if (nibrsXmlTransaction.Status == NibrsSubmissionStatusCodes.UploadFailed)
                {
                    var response = NibrsSubmitter.SendReport(nibrsXmlTransaction.Submission.Xml);
                    nibrsXmlTransaction.SetNibrsXmlSubmissionResponse(response);
                }
                var isSaved = await CallApiToSaveInMongoDbAsync(nibrsXmlTransaction.JsonString, endpointURL, client);
                return isSaved;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
