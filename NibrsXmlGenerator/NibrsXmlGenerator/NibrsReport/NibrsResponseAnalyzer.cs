using NibrsInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibrsXml.NibrsReport
{
    public class NibrsResponseAnalyzer
    {
        public static string AnalyzeResponse(NibrsXmlSubmissionResponse response)
        {
            if (response != null)
            {
                if (response.NibrsResponse != null && response.NibrsResponse.success)
                    return "Accepted";
                if (response.NibrsResponse != null && response.NibrsResponse.success == false)
                    return "Rejected";
                if (response.NibrsResponse == null && response.IsFileValid)
                    return "UploadFailed";
                if (response.IsFileValid == false)
                    return "FormatError";
            }


            return null;
        }
    }
}
