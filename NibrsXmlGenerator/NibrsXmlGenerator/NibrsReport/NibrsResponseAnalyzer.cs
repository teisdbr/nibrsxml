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
                if (response.NibrsResponse != null && response.NibrsResponse.success)
                    return "accepted";
                if (response.NibrsResponse != null && response.NibrsResponse.success == false)
                    return "rejected";
                if (response.NibrsResponse == null && response.IsFileValid)
                    return "uploadFailed";
                if (response.IsFileValid == false)
                    return "formatError";
            }
            return null;
        }
    }
}
