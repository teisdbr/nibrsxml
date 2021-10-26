using LoadBusinessLayer;
using LoadBusinessLayer.Interfaces;
using NibrsInterface;
using NibrsModels.NibrsReport;
using NibrsXml.Constants;
using NibrsXml.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebhookProcessor;

namespace NibrsXml.Processor
{
  public abstract class BaseProcessor
    {
        public LogManager LogManager { get; }
        public ConcurrentQueue<Tuple<Exception, string>> ExceptionsLogger { get; }
        public string Ori { get; set; }
        public AppSettingsReader _appSettingsReader;
        public Nibrs_Batch _nibrsBatchDal;
        public List<IncidentList> AgencyBatchCollection { get; set; }


        public BaseProcessor(LogManager logManager, List<IncidentList> agencyIncidentsCollection)
        {
            LogManager = logManager;
            ExceptionsLogger = new ConcurrentQueue<Tuple<Exception, string>>();
            _nibrsBatchDal = new Nibrs_Batch();
            Ori = logManager.Ori;
            _appSettingsReader = new AppSettingsReader();
            AgencyBatchCollection = agencyIncidentsCollection =
                agencyIncidentsCollection?.Where(incList => incList.Environment != "T" && incList.OriNumber == Ori)?.ToList() ??
                new List<IncidentList>();
        }

        public abstract  Task ProcessAsync();

        public  async Task<BatchResponseReport> AttemptToReportDocumentsAsync( string runNumber,
           IEnumerable<Submission> documentsBatch, bool isAnyPendingToUpload = false)
        {
            var agencyDir = new AgencyNibrsDirectoryInfo(Ori);          
            var pathToSaveErrorTransactions = agencyDir.GetErroredDirectory().FullName;
            var baseURL = Convert.ToString(_appSettingsReader.GetValue("LcrxAPIURL", typeof(string)));
            var endpoint = _appSettingsReader.GetValue("SaveNibrsXmlEndpoint", typeof(String)).ToString();
            var maxDegreeOfParallelism =
                Convert.ToInt32(_appSettingsReader.GetValue("MaxDegreeOfParallelism", typeof(int)));
            var reportToFbi = Convert.ToBoolean(_appSettingsReader.GetValue("ReportToFBI", typeof(Boolean)));
            ConcurrentBag<NibrsXmlTransaction> errorTransactions = new ConcurrentBag<NibrsXmlTransaction>();
            var requestTasks = new List<Task<ResponseReport>>();
            
            // The purpose of the semaphoreSlim to control the max number of concurrent tasks that can be ran in the requestTasks
            // MaxDegreeOfParallelism defines max number of tasks that can be at a time.
            SemaphoreSlim semaphoreSlim = new SemaphoreSlim(maxDegreeOfParallelism, maxDegreeOfParallelism);

            foreach (var sub in documentsBatch)
            {
                requestTasks.Add(Task.Run(async () =>
                {
                    ResponseReport responseReport = new ResponseReport();

                    await semaphoreSlim.WaitAsync();
                    NibrsXmlTransaction nibrsTrans = null;
                    try
                    {
                        var response = sub.IsNibrsReportable && reportToFbi && !isAnyPendingToUpload
                            ? NibrsSubmitter.SendReport(sub.Xml)
                            : null;
                        //Wrap both response and submission and then save to database
                        nibrsTrans = new NibrsXmlTransaction(sub, response);

                        await HttpActions.Post<NibrsXmlTransaction, object>(nibrsTrans,
                            baseURL + endpoint, null, true);
                    }
                    catch (Exception ex)
                    {
                        if (ex is HttpRequestException)
                        {
                            errorTransactions.Add(nibrsTrans);
                        }
                        ExceptionsLogger.Enqueue(Tuple.Create(ex,
                              $"Incident Number: {nibrsTrans?.Submission?.IncidentNumber ?? String.Empty} , Arrest ID: {nibrsTrans?.Submission?.Reports[0]?.Arrests?.FirstOrDefault()?.ActivityId?.Id ?? "N/A"}. "));
                        responseReport.IsFailedToSaveInDB = true;
                    }
                    finally
                    {
                        // Mark as upload failed.
                        if (nibrsTrans.Status == NibrsSubmissionStatusCodes.UploadFailed)
                        {
                            responseReport.IsFailedToFBI = true;
                        }
                        //Release the semaphore. It is vital to ALWAYS release the semaphore so that tasks waiting to enter can takeover or else we will end up with a Semaphore that is forever locked.
                        //final block is recommended to release, program execution may crash or take a different path, this way you are guaranteed execution
                        lock (semaphoreSlim)
                        {
                            semaphoreSlim.Release();
                        }
                    }

                    return responseReport;
                }));
            }

            var attemptResults = await Task.WhenAll(requestTasks.ToArray());

            // save in failed folder
            if (errorTransactions.Any())
            {
                WriteTransactions(errorTransactions, pathToSaveErrorTransactions);                
            }

            var batchResponseStatus = new BatchResponseReport(attemptResults);

            if (batchResponseStatus.CheckIfSomethingFailedToSaveDB())
                throw new DocumentsFailedToSaveInDBException(runNumber, pathToSaveErrorTransactions);

            return batchResponseStatus;
        }

        private static void SendErrorEmail(string subject, string body)
        {
            try
            {
                var _appSettingsReader = new AppSettingsReader();
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
        private static void WriteTransactions(IEnumerable<NibrsXmlTransaction> transactions, string path)
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

            }
        }


    }
}
