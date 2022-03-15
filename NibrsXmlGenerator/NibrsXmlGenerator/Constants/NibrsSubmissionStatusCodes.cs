using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibrsXml.Constants
{
   public class NibrsSubmissionStatusCodes
    {
       
       
            public const string Accepted = "accepted";
            public const string Rejected = "rejected";
            public const string UploadFailed = "upload Failed";
            public const string FormatError = "format Error";
            public const string NotReported = "not Reported";
            public const string FaultedResponse = "faulted Response";
            public const string NotReportable = "not Reportable";
        
    }

    public class NibrsResponseCodes
    {
        public const string Accepted = "ACCEPTED";
        public const string Warnings = "WARNINGS";
        public const string Errors = "ERRORS";
    }
}
