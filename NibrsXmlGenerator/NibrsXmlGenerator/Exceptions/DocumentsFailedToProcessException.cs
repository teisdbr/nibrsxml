using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibrsXml.Exceptions
{
   public class DocumentsFailedToProcessException : Exception
    {
       

        public DocumentsFailedToProcessException(string runnumber, string path, NibrsXmlTransaction doc)
            : base($"Operation Aborted  while processing the runnumber {runnumber}. Please see inner exception along with logs for more info. " +
                  $"Files failed to save in mongo clusters will be attempted to save in Directory:{path}." +
                  $" Incident Number: { doc?.Submission?.IncidentNumber ?? ""} , Arrest ID: {doc?.Submission?.Reports[0]?.Arrests?.FirstOrDefault()?.ActivityId?.Id}.")
        {
        }

        public DocumentsFailedToProcessException(string runnumber, string path, NibrsXmlTransaction doc, Exception inner)
             : base($"Operation Aborted  while processing the runnumber {runnumber}. Please see inner exception along with logs for more info." +
                  $"Files failed to save in mongo clusters will be attempted to save in Directory:{path}." +
                   $" Incident Number: { doc?.Submission?.IncidentNumber ?? ""} , Arrest ID: {doc?.Submission?.Reports[0]?.Arrests?.FirstOrDefault()?.ActivityId?.Id }. " 
                   , inner)
        {

        }
    }
}
