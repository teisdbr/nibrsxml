using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using LoadBusinessLayer;
using LoadBusinessLayer.Interfaces;
using MongoDB.Driver.Linq;

namespace NibrsXml.Processor
{
    public static class NibrsReportingProcessor
    {
        /// <summary>
        /// Process the Nibrs Batch for the given LIBRS Batch of Incidents
        /// </summary>
        /// <param name="agencyIncidentsCollection"></param>
        /// <param name="batchFolderName"></param>
        /// <param name="buildLibrsIncidentsListFunc"></param>
        /// <returns></returns>
        public static async Task ProcessAgenciesBatchAsync(
            List<IncidentList> agencyIncidentsCollection, string batchFolderName,
            Func<string, string, Task<IncidentList>> buildLibrsIncidentsListFunc)
        {
            AgencyCode agencyCode = new AgencyCode();
            List<string> oriList = new List<string>();

            // if agencyIncidentsCollection is provided stick to those ORIs
            if (agencyIncidentsCollection.Any())
                oriList = agencyIncidentsCollection.Select(incList => incList.OriNumber)?.Distinct().ToList();


            foreach (var ori in oriList)
            {
                var lockKey = Guid.NewGuid().ToByteArray();
                var log = new LogManager(ori, batchFolderName);
                try
                {
                    if (!PlaceLockOnAgency(agencyCode, lockKey, ori))
                    {
                        log.PrintFailedToPlaceLock();

                        continue;
                    }

                    await new AgencyInsertOrReplaceProcessor(log, agencyIncidentsCollection,
                        buildLibrsIncidentsListFunc).ProcessAsync();
                }
                catch (AggregateException aex)
                {
                    foreach (Exception e in aex.Flatten().InnerExceptions)
                    {
                        log.PrintExeption(e);
                    }

                    SendErrorEmail($"Something went wrong while trying to process the submission batch for ORI:{ori}",
                        $"Please check the logs for more" +
                        $" details.{Environment.NewLine} Batch Folder Name {batchFolderName} {Environment.NewLine} " +
                        $"{aex.ToFalttenString()}");
                }
                catch (Exception ex)
                {
                    log.PrintExeption(ex);
                    log.PrintFailedToProcess();
                    SendErrorEmail($"Something went wrong while trying to process the submission batch for ORI:{ori}",
                        $"Please check the logs for more" +
                        $" details.{Environment.NewLine} Batch Folder Name {batchFolderName} {Environment.NewLine} {ex.ToReableString()}");
                }
                finally
                {
                    ReleaseLockOnAgency(agencyCode, lockKey, ori);
                }
            }
        }


        #region Helpers

        private static void SendErrorEmail(string subject, string body)
        {
            var _appSettingsReader = new AppSettingsReader();
            try
            {
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

        private static bool PlaceLockOnAgency(AgencyCode agencyCode, byte[] lockKey, string ori)
        {
            // lock processing agency
            agencyCode.LockAgency(ori, lockKey);
            var dtLockedAgency = agencyCode.GetLockedAgency(ori, lockKey);
            if (dtLockedAgency.Rows.Count == 0)
            {
                return false;
            }

            return true;
        }

        private static void ReleaseLockOnAgency(AgencyCode agencyCode, byte[] lockKey, string ori)
        {
            var dtLockedAgency = agencyCode.GetLockedAgency(ori, lockKey);
            if (dtLockedAgency.Rows.Count > 0)
            {
                agencyCode.UnLockAgency(ori);
            }
        }

        #endregion
    }
}