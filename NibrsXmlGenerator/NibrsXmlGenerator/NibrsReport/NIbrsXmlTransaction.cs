using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsInterface;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace NibrsXml.NibrsReport
{
  [BsonIgnoreExtraElements]
  
  public  class NIbrsXmlTransaction
  {
       
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }


        public Submission Submission { get; set; }  = new Submission();

        public DateTime TransactionDate { get; set; }


        public NibrsXmlSubmissionResponse NibrsSubmissionResponse { get; set; } = new NibrsXmlSubmissionResponse();

        


        public NIbrsXmlTransaction(Submission submission, NibrsXmlSubmissionResponse nibrsSubmissionResponse )
        {
            Id = submission.Id;
            Submission = submission;
            NibrsSubmissionResponse = nibrsSubmissionResponse;
            TransactionDate = DateTime.Now;

        }

       
    }
}
