using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using NibrsInterface;

namespace NibrsXml.NibrsReport
{
  [BsonIgnoreExtraElements]
  public  class NibrsXmlTransaction
  {
       
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }


        public Submission Submission { get; set; }  = new Submission();

        public DateTime TransactionDate { get; private set; }


        public int NumberOfAttempts { get; private set; }

        public NibrsXmlSubmissionResponse NibrsSubmissionResponse { get; private set; } = new NibrsXmlSubmissionResponse();

        /// <summary>
        /// This property will Analyize the Response and give the status of the Response.
        /// </summary>
        [BsonIgnore]
        [JsonIgnore]
        public string Status { get => NibrsResponseAnalyzer.AnalyzeResponse(NibrsSubmissionResponse); }

        [JsonConstructor]
        public NibrsXmlTransaction()
        {
           
        }

        public NibrsXmlTransaction(Submission submission, NibrsXmlSubmissionResponse nibrsSubmissionResponse)
        {
            Id = submission.Id;
            Submission = submission;
            NibrsSubmissionResponse = nibrsSubmissionResponse;
            TransactionDate = DateTime.Now;          
            NumberOfAttempts =  1;
        }

        /// <summary>
        /// Call this method only to update the new FBI response.
        /// </summary>
        /// <param name="nibrsSubmissionResponse"></param>
        public void SetNibrsXmlSubmissionResponse(NibrsXmlSubmissionResponse nibrsSubmissionResponse)
        {
            NibrsSubmissionResponse = nibrsSubmissionResponse;
            TransactionDate = DateTime.Now;
            NumberOfAttempts += 1;
            //NumberOfAttempts = numberOfAttempts + 1;
        }


    }
}
