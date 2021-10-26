using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibrsXml
{
    public class BatchResponseReport : ResponseReport
    {
        public BatchResponseReport(bool isFailedToFBI)
        {
            this.IsFailedToFBI = isFailedToFBI;            
        }

        public BatchResponseReport(ResponseReport[] responseReport)
        {
            this.IsFailedToFBI = responseReport.Any(res => res.IsFailedToFBI);
            this.IsFailedToSaveInDB = responseReport.Any(res => res.IsFailedToSaveInDB);
        }

        /// <summary>
        /// Returns a boolean  to indicate whether to abort the current process based on the response results.
        /// </summary>
        /// <returns></returns>
        public bool CheckIfSomethingFailedToSaveDB()
        {
           return  IsFailedToSaveInDB;
        }
    }


    public class ResponseReport
    {
        public bool IsFailedToFBI { get; set; }

        public bool IsFailedToSaveInDB { get; set; }
    }
}
