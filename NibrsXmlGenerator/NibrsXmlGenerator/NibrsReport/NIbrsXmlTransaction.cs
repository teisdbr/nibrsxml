using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsInterface;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NibrsXml.NibrsReport
{
  [BsonIgnoreExtraElements]
  public  class NIbrsXmlTransaction
    {
        public ObjectId Id { get; set; }

        public Submission Submission { get; set; }  = new Submission();

       
        public NibrsXmlSubmissionResponse NibrsSubmissionResponse { get; set; } = new NibrsXmlSubmissionResponse();


        public NIbrsXmlTransaction(Submission submission, NibrsXmlSubmissionResponse nibrsSubmissionResponse )
        {
            Id = submission.Id;
            Submission = submission;
            NibrsSubmissionResponse = nibrsSubmissionResponse;
        }

       
    }
}
