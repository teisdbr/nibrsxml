using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibrsXml.Exceptions
{
   public class DocumentsFailedToSaveInDBException : Exception
    {
       

        public DocumentsFailedToSaveInDBException(string runnumber, string path, NibrsXmlTransaction doc)
            : base($"Operation Aborted  while processing the runnumber {runnumber}, This could happend if it cannot reach to LCRx server or didnot recieve the success response. " +
                  $"Files failed to save in mongo clusters will be attempted to save in Directory:{path}." +
                  $" Incident Number: { doc?.Submission?.IncidentNumber ?? ""} , Arrest ID: {doc?.Submission?.Reports[0]?.Arrests?.FirstOrDefault()?.ActivityId?.Id}. Please check the logs and files to analyze the issue.")
        {
        }

        public DocumentsFailedToSaveInDBException(string runnumber, string path, NibrsXmlTransaction doc, Exception inner)
             : base($"Operation Aborted  while processing the runnumber {runnumber}, This could happend if it cannot reach to LCRx server or didnot recieve the success response. " +
                  $"Files failed to save in mongo clusters will be attempted to save in Directory:{path}." +
                   $" Incident Number: { doc?.Submission?.IncidentNumber ?? ""} , Arrest ID: {doc?.Submission?.Reports[0]?.Arrests?.FirstOrDefault()?.ActivityId?.Id }. " +
                   $"Please check the logs and files to analyze the issue.", inner)
        {

        }
    }
}
