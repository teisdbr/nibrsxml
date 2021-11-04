using LoadBusinessLayer;
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
using System.Threading;
using System.Threading.Tasks;
using WebhookProcessor;

namespace NibrsXml.Processor
{
    public abstract class BaseProcessor
    {
        protected LogManager LogManager { get; }
        protected string Ori { get; set; }
        protected string Environment { get; set; }
        protected AppSettingsReader _appSettingsReader;
        protected Nibrs_Batch _nibrsBatchDal;
        protected List<IncidentList> AgencyBatchCollection { get; set; }


        protected BaseProcessor(LogManager logManager, List<IncidentList> agencyIncidentsCollection, string environment)
        {
            LogManager = logManager;
            new ConcurrentQueue<Exception>();
            _nibrsBatchDal = new Nibrs_Batch();
            Ori = logManager.Ori;
            Environment = environment;
            _appSettingsReader = new AppSettingsReader();
            AgencyBatchCollection = agencyIncidentsCollection =
                agencyIncidentsCollection?.Where(incList => incList.Environment != "T" && incList.OriNumber == Ori)
                    ?.ToList() ??
                new List<IncidentList>();
        }

        public abstract Task ProcessAsync();
        /// <summary>
        /// Sends the Documents to the LCRX API, based on the reportDocuments boolean it will decide whether to report documents to the FBI. Returns true If all documets are processed succefully.
        /// </summary>
        /// <param name="runNumber"></param>
        /// <param name="documentsBatch"></param>
        /// <param name="reportDocuments"></param>
        /// <returns></returns>
        protected async Task<bool> AttemptToReportDocumentsAsync(string runNumber,
            IEnumerable<Submission> documentsBatch, bool reportDocuments = true)
        {
            var agencyDir = new AgencyNibrsDirectoryInfo(Ori);
            var pathToSaveErrorTransactions = agencyDir.GetErroredDirectory().FullName;
            var baseURL = Convert.ToString(_appSettingsReader.GetValue("LcrxAPIURL", typeof(string)));
            var endpoint = _appSettingsReader.GetValue("SaveNibrsXmlEndpoint", typeof(String)).ToString();
            var maxDegreeOfParallelism =
                Convert.ToInt32(_appSettingsReader.GetValue("MaxDegreeOfParallelism", typeof(int)));
            var reportToFbi = Convert.ToBoolean(_appSettingsReader.GetValue("ReportToFBI", typeof(Boolean)));
            ConcurrentBag<NibrsXmlTransaction> errorTransactions = new ConcurrentBag<NibrsXmlTransaction>();


            // The purpose of the semaphoreSlim to control the max number of concurrent tasks that can be ran in the requestTasks
            // MaxDegreeOfParallelism defines max number of tasks that can be at a time.
            SemaphoreSlim semaphoreSlim = new SemaphoreSlim(maxDegreeOfParallelism, maxDegreeOfParallelism);
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            var requestTasks = new List<Task<bool>>();
            foreach (var sub in documentsBatch)
            {
                requestTasks.Add(Task.Run(async () =>
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                    await semaphoreSlim.WaitAsync();
                    NibrsXmlTransaction nibrsTrans = null;
                    bool uploadSuccessFully = true;
                    try
                    {
                        var response = sub.IsNibrsReportable && reportToFbi && reportDocuments
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

                        cancellationTokenSource.Cancel();
                        throw new DocumentsFailedToSaveInDBException(runNumber, pathToSaveErrorTransactions, nibrsTrans,
                            ex);
                    }
                    finally
                    {
                        // Mark as upload failed.
                        if (nibrsTrans.Status == NibrsSubmissionStatusCodes.UploadFailed)
                        {
                            uploadSuccessFully = false;
                        }

                        //Release the semaphore. It is vital to ALWAYS release the semaphore so that tasks waiting to enter can takeover or else we will end up with a Semaphore that is forever locked.
                        //final block is recommended to release, program execution may crash or take a different path, this way you are guaranteed execution
                        lock (semaphoreSlim)
                        {
                            semaphoreSlim.Release();
                        }
                    }

                    return uploadSuccessFully;

                }, cancellationToken));
            }

            var attemptResults = await Task.WhenAll(requestTasks.ToArray());
            return attemptResults.All(reported => reported);
        }
    }
}