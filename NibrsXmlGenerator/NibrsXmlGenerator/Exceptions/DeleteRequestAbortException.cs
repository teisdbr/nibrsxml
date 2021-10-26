using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibrsXml.Exceptions
{
    public class DeleteRequestAbortException : Exception
    {
        public DeleteRequestAbortException()
        {
        }

                           
        public DeleteRequestAbortException(string runnumber) : base($"Exception Occured while processing the Deletes, Cannot Report Deletes to incidents that are to reported to the FBI previously, So deleting the current batch with runnumber {runnumber} can cause duplicate incidents errors in future reporting")
        {
        }

        public DeleteRequestAbortException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
