using LoadBusinessLayer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Processor;

namespace NibrsXml
{
   public  class LogManager
    {
        private Logger Log { get; }

        public string Ori { get; }

       public string BatchFolderName { get; }

        public LogManager( string ori, string batchFolderName )
        {
            Log = new Logger(); 
            this.Ori = ori;
            BatchFolderName = batchFolderName;
        }

        public void PrintSubmissionSummary(int count, string runnumber)
        {
            Log.WriteLog(Ori,
                       $"{DateTime.Now} : TOTAL {count} SUBMISSIONS BUILT TO PROCESS FOR RUN-NUMBER: {runnumber}",
                       BatchFolderName);
        }

        public void PrintStartOfProcess()
        {
            Log.WriteLog(Ori,
               $"{DateTime.Now} : -------------------  RUNNING NIBRS BATCH PROCESS--------------------",
               BatchFolderName);
        }

        public void PrintEndOfProcess()
        {
            Log.WriteLog(Ori,
              $"{DateTime.Now} : -------------------  PROCESSING NIBRS BATCH COMPLETED--------------------",
              BatchFolderName);
        }

        public void PrintFailedToProcess()
        {
            Log.WriteLog(Ori, DateTime.Now + " : " + "FAILED TO PROCESS NIBRS  DATA ",
                       BatchFolderName);
        }

        public void PrintGeneratedZeroReport(string runNumber)
        {
            Log.WriteLog(Ori,
                            $"{DateTime.Now} : GENERATING ZERO REPORT AS TOTAL SUBMISSIONS TO PROCESS FOR RUN-NUMBER: {runNumber} IS ZERO",
                            BatchFolderName);
        }

        public void  PrintTransformIntoReplace(string runNumber)
        {
            Log.WriteLog(Ori,
                           $"{DateTime.Now} : TRANSFORMING THE INSERT AS REPLACE BECAUSE REPROCESS MODE IS ENABLED FOR RUNNUMBER : {runNumber}",
                           BatchFolderName);
        }


        public void PrintAddingBatchDetailsToDatabase(string runNumber)
        {
            Log.WriteLog(Ori,
                        $"{DateTime.Now} : ADDING BATCH DETAILS TO THE DATABASE RUN-NUMBER : {runNumber}",
                        BatchFolderName);
        }

        public void PrintStartedProcessForRunNumber(string runnumber)
        {
            Log.WriteLog(Ori,
                        DateTime.Now + " : " + "STARTED FILES PROCESSING FOR RUN-NUMBER : " +
                        runnumber,
                        BatchFolderName);
        }     
                    

       public void PrintProcessCompletedForRunNumber(string runnumber)
        {
            Log.WriteLog(Ori,
                      DateTime.Now + " : " +
                       "COMPLETED FILES PROCESSING FOR RUN-NUMBER : " + runnumber,
                       BatchFolderName);
        }

        public void PrintStatusAfterProcessForRunNumber(string runnumber, BatchResponseReport status)
        {
            Log.WriteLog(Ori,
                        $"{DateTime.Now} : Uploaded To FBI :  {status.UploadedToFbi} , Saved In Database : {status.SavedInDb} FOR RUN-NUMBER: {runnumber}",
                        BatchFolderName);
        }

        public void PrintTransformIntoDelete(string runnumber)
        {
            Log.WriteLog(Ori,
                       $"{DateTime.Now} : TRANSFORMED NIBRS DATA INTO DELETE  FOR RUN-NUMBER: {runnumber}",
                       BatchFolderName);
        }

        public void PrintRunningInForceDeleteMode()
        {
            Log.WriteLog(Ori,
               $"{DateTime.Now} : --------- RUNNING THE PROCESS IN THE FORCE DELETE MODE--------------",
               BatchFolderName);
        }


        public void PrintWhetherDeletesReportToFBI(bool status)
        {
            Log.WriteLog(Ori,
               $"{DateTime.Now} : Will this Batch report to FBI? : ${status} ",
               BatchFolderName);
            if(!status)
            {
                string strComments = $"{DateTime.Now} :WARNING: If you intent to report these deletes to FBI, please make sure you include all runnumbers that are pending to report FBI, may be by adjusting date range";
                Log.WriteLog(Ori,
               strComments,
               BatchFolderName);
            }
        }

        public void PrintFailedToPlaceLock()
        {
            Log.WriteLog(Ori, $"{DateTime.Now} : COULDN'T PLACE THE LOCK ON THE ORI: {Ori}",
                           BatchFolderName);
        }

        public void PrintExeption(Exception e)
        {
            Log.WriteLog(Ori,
                                   $"Message :{e.Message} {Environment.NewLine} Inner Exception: {e.InnerException} {Environment.NewLine}   StackTrace :{e.StackTrace}{Environment.NewLine}Date :{DateTime.Now}", BatchFolderName);
            Log.WriteLog(Ori,
                Environment.NewLine +
                "-----------------------------------------------------------------------------" +
                Environment.NewLine, BatchFolderName);
        }

     

        public  void PrintExceptions(ConcurrentQueue<Exception> exceptionsLogger)
        {
            if (exceptionsLogger.Any())
            {
                while (exceptionsLogger.TryDequeue(out Exception ex))
                {
                    Log.WriteLog(Ori,
                        $"Message :{ex.Message} {Environment.NewLine}  Inner Exception: {ex.InnerException} {Environment.NewLine}   StackTrace :{ex.StackTrace}{Environment.NewLine}Date :{DateTime.Now}", BatchFolderName);
                    Log.WriteLog(Ori,
                        Environment.NewLine +
                        "-----------------------------------------------------------------------------" +
                        Environment.NewLine, BatchFolderName);
                }
            }
        }
    }
}
