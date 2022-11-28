using LoadBusinessLayer;
using NibrsInterface;
using NibrsModels.NibrsReport;
using NibrsXml.Constants;
using NibrsXml.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TeUtil.Extensions;
using WebhookProcessor;
using Newtonsoft.Json;
using LoadBusinessLayer.LIBRSAdmin;
using LoadBusinessLayer.LIBRSArrestee;
using LibrsModels.Classes;
using NibrsModels.Constants;
using NibrsModels.Utility;

namespace NibrsXml.Processor
{
    public abstract class BaseProcessor
    {
        protected LogManager LogManager { get; }
        protected string Ori { get; set; }
        protected string Environment { get; set; }
        protected readonly AppSettingsReader _appSettingsReader;
        protected readonly Nibrs_Batch _nibrsBatchDal;
        protected List<IncidentList> AgencyBatchCollection { get; set; }

        protected BaseProcessor(LogManager logManager, List<IncidentList> agencyIncidentsCollection, string environment)
        {
            LogManager = logManager;
            _nibrsBatchDal = new Nibrs_Batch();
            Ori = logManager.Ori;
            Environment = environment;
            _appSettingsReader = new AppSettingsReader();
            AgencyBatchCollection =
                agencyIncidentsCollection?.Where(incList => (incList.Environment == Environment && Environment != "T") && incList.OriNumber == Ori)
                    ?.ToList() ??
                new List<IncidentList>();
        }

        protected bool CheckIfBatchIsNibrsReportable()
        {
            return Environment == "C";
        }

        public abstract Task ProcessAsync();
        /// <summary>
        /// Sends the Documents to the LCRX API, based on the reportDocuments boolean it will decide whether to report documents to the FBI. Returns true If all documets are processed succefully.
        /// </summary>
        /// <param name="runNumber"></param>
        /// <param name="documentsBatch"></param>
        /// <param name="reportDocuments"></param>
        /// <returns></returns>
        protected async Task<BatchResponseReport> AttemptToReportDocumentsAsync(string runNumber,
            IEnumerable<Submission> documentsBatch, IncidentList incidentList, DataTable larsDatatable, bool reportDocuments = true)
        {
            var agencyDir = new AgencyNibrsDirectoryInfo(Ori);
            var pathToSaveErrorTransactions = agencyDir.GetErroredDirectory().FullName;
            var baseURL = Convert.ToString(_appSettingsReader.GetValue("LcrxAPIURL", typeof(string)));
            var endpoint = _appSettingsReader.GetValue("SaveNibrsXmlEndpoint", typeof(String)).ToString();
            var maxDegreeOfParallelism =
                Convert.ToInt32(_appSettingsReader.GetValue("MaxDegreeOfParallelism", typeof(int)));
            var reportToFbi = Convert.ToBoolean(_appSettingsReader.GetValue("ReportToFBI", typeof(Boolean)));

            // The purpose of the semaphoreSlim to control the max number of concurrent tasks that can be ran in the requestTasks
            // MaxDegreeOfParallelism defines max number of tasks that can be at a time.
            SemaphoreSlim semaphoreSlim = new SemaphoreSlim(maxDegreeOfParallelism, maxDegreeOfParallelism);
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            var requestTasks = new List<Task<ResponseReport>>();

            foreach (var sub in documentsBatch)
            {
                NibrsXmlTransaction nibsTrans = null;
                requestTasks.Add(Task.Run(async () =>
                {
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }
                await semaphoreSlim.WaitAsync(cancellationToken);
                ResponseReport responseReport = new ResponseReport();
                try
                {
                    var response = CheckIfBatchIsNibrsReportable() && reportToFbi && reportDocuments
                        ? NibrsSubmitter.SendReport(sub.Xml)
                        : null;
                    //Wrap both response and submission and then save to database
                    nibsTrans = new NibrsXmlTransaction(sub, response);

                    if (nibsTrans.Status != NibrsSubmissionStatusCodes.UploadFailed && nibsTrans.Status != NibrsSubmissionStatusCodes.NotReported)
                    {
                        responseReport.UploadedToFbi = true;
                    }
                        //Remove delete incident and send the incident pertaining that the submission report only to lessen the data transfer
                        //Sending the filterIncidentList is not related to the FBI submission data. This is for retrieving the lrs number
                        //Scenario: 1 - ZeroReport doesn't have any incident
                        //          2 - If submission.report.incidentNumber is not Null. use sub.incidentNumber to filter the incidentlist
                        //          3 - If submission.report.incidentNumber is Null then get IncidentNumber from ArrestId to filter incidentlist

                        LIBRSIncident filterIncidentList = new LIBRSIncident(); //Use for getting the LRSCode in the LCRX. Nothing else for now. This doesn't affect the submissionReport.
                        filterIncidentList = null;
                     
                        if (sub.IncidentNumber != null)
                        {
                            filterIncidentList = incidentList.Where(i => i.ActionType.Trim() != "D" && i.IncidentNumber.Trim() == sub.IncidentNumber).FirstOrDefault();
                            RemoveRelationshipFields(filterIncidentList);
                        }
                        else
                        {
                            List<string> ArrestIncidentNumber = new List<string>();
                            if (sub.Reports[0].Arrests.Count > 0)
                            {
                                //Retrieve incidentNumber in submission Report ArrestId then filter the IncidentList object.
                                var ArrestId = sub.Reports[0].Arrests[0].Id;
                                var oriNumber = ArrestId.Substring(0, ArrestId.IndexOf("-"));
                                var withoutOriNumber = ArrestId.Replace(oriNumber, "").Remove(0, 1);
                                var arrestIncidentNumber = "";

                                if (withoutOriNumber.IndexOf("-I") > 0)
                                {
                                    arrestIncidentNumber = withoutOriNumber.Substring(0, withoutOriNumber.IndexOf("-I"));
                                }
                                else if(withoutOriNumber.IndexOf("-D") > 0)
                                {
                                    arrestIncidentNumber = withoutOriNumber.Substring(0, withoutOriNumber.IndexOf("-D"));
                                }

                                filterIncidentList = incidentList.Where(i => i.ActionType.Trim() != "D" && i.IncidentNumber.Trim() == arrestIncidentNumber).FirstOrDefault();
                                RemoveRelationshipFields(filterIncidentList);
                            }
                        }
                        
                        //Call LCRX API
                        await HttpActions.Post<HTTPDataObjectTransport<NibrsXmlTransaction, LIBRSIncident, DataTable>, object>(new HTTPDataObjectTransport<NibrsXmlTransaction, LIBRSIncident, DataTable>(nibsTrans, filterIncidentList, larsDatatable),
                        baseURL + endpoint, null, true);
                    responseReport.SavedInDb = true;
                }
                catch (Exception ex)
                {
                    cancellationTokenSource.Cancel();
                    if (nibsTrans != null)
                    {
                        WriteTransactions(nibsTrans, pathToSaveErrorTransactions);
                    }
                    throw new DocumentsFailedToProcessException(runNumber, pathToSaveErrorTransactions, nibsTrans,
                    ex);
                }
                finally
                {

                    //Release the semaphore. It is vital to ALWAYS release the semaphore so that tasks waiting to enter can takeover or else we will end up with a Semaphore that is forever locked.
                    //final block is recommended to release, program execution may crash or take a different path, this way you are guaranteed execution
                    lock (semaphoreSlim)
                    {
                        semaphoreSlim.Release();
                    }
                }

                    return responseReport;

                }, cancellationToken));
            }

            var attemptResults = await Task.WhenAll(requestTasks.ToArray());
            return new BatchResponseReport(attemptResults);
        }
        private void RemoveRelationshipFields(LIBRSIncident filterIncidentList)
        {
            //Clean up object because it is causing an issue on HTTP POST
            //Issue: Self referencing loop detected with type 
            //Fix: Must set these null inorder to pass the object over HTTP. This filterIncidentList is only use to extract the lrsCode.
            //     The submissionReport already has everything for fbi submission.
            if(filterIncidentList != null)
            {
                filterIncidentList.Offense.ForEach(g => g.RelationshipsToProperties = null);
                filterIncidentList.PropDesc.ForEach(g => g.RelationshipsToOffenses = null);
            }
            
        }

        public List<PendingRunNumbers> GetPendingRunNumbers()
        {
            List<PendingRunNumbers> runNumbers = new List<PendingRunNumbers>();

            // get the pending runnumbers of the ORI
            var nibrsBatchdt = _nibrsBatchDal.GetORIsWithPendingIncidentsToProcess(Ori, Environment);
            foreach (DataRow row in nibrsBatchdt.Rows)
            {
                if (row["ori_number"].ToString() == Ori)
                {
                    runNumbers.Add(new PendingRunNumbers(row));
                }
            }

            return runNumbers;
        }

        private static void WriteTransactions(NibrsXmlTransaction trans, string path)
        {
            try
            {
                // save failed files.
                string fileName = path + "\\" + trans?.Submission?.Runnumber;
                if (!Directory.Exists(fileName))
                {
                    Directory.CreateDirectory(fileName);
                }

                var docName = trans?.Submission?.Id + ".json";
                string[] filePath = { fileName, docName };
                string errorPath = Path.Combine(filePath);
                File.WriteAllText(errorPath, trans?.JsonString);

            }
            catch (AggregateException exception)
            {

            }
        }

    }
     public class PendingRunNumbers
    {
        public PendingRunNumbers(DataRow dataRow)
        {
            RunNumber = dataRow["runnumber"].ToString();
            IsUploadedToFBI = dataRow["is_uploaded_to_fbi"] != null && dataRow["is_uploaded_to_fbi"].ToString() != "" ? Boolean.Parse(dataRow["is_uploaded_to_fbi"].ToString()) : (bool?)null;
            IsSavedToDb = dataRow["is_saved_to_db"] != null  && dataRow["is_saved_to_db"].ToString() != "" ? Boolean.Parse(dataRow["is_saved_to_db"].ToString()) : (bool?)null;           
            PendingDeletes = dataRow["pending_deletes"] != null && dataRow["pending_deletes"].ToString() != "" ? Boolean.Parse(dataRow["pending_deletes"].ToString()) : (bool?)null;
        }
       public string RunNumber;
       public bool? IsUploadedToFBI;
       public bool? IsSavedToDb;
       public bool? PendingDeletes;
    }
}