using NibrsXml.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NibrsInterface
{
    public class NibrsResponseAnalyzer
    {
        public static string AnalyzeResponse(NibrsXmlSubmissionResponse response)
        {
            if (response != null)
            {
                if (response.NibrsResponse != null && response.NibrsResponse.ingestResponse.status == "ACCEPTED")
                    return NibrsSubmissionStatusCodes.Accepted;
                if (response.NibrsResponse != null && response.NibrsResponse.ingestResponse.status != "ACCEPTED")
                    return  NibrsSubmissionStatusCodes.Rejected; ;
                if (response.NibrsResponse == null && response.IsFileValid)
                    return   NibrsSubmissionStatusCodes.UploadFailed; ;
                if (response.IsFileValid == false)
                    return  NibrsSubmissionStatusCodes.FormatError; ;
            }
            return "noResponse";
        }
    }
}
