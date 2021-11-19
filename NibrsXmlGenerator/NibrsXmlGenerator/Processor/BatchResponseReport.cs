using System.Linq;

namespace NibrsXml.Processor
{
    public class BatchResponseReport : ResponseReport
    {
        public BatchResponseReport(bool uploadedToFbi)
        {
            this.UploadedToFbi = uploadedToFbi;            
        }

        public BatchResponseReport(ResponseReport[] responseReport)
        {
            this.UploadedToFbi = responseReport.All(res => res.UploadedToFbi);
            this.SavedInDb = responseReport.All(res => res.SavedInDb);
        }
    }
    
    public class ResponseReport
    {
        public bool UploadedToFbi { get; set; }

        public bool SavedInDb { get; set; }
    }
    
    
    
}