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

       
        public nibrsResponse NibrsResponse { get; set; } = new nibrsResponse();

        public bool IsFileValid { get; set; } = true;

        public string LastException { get; set; } 

        public NIbrsXmlTransaction(Submission submission, nibrsResponse nibrsResponse, bool isFileValid, string lastException)
        {
            Id = submission.Id;
            Submission = submission;
            NibrsResponse = nibrsResponse;
            IsFileValid = isFileValid;
            LastException = lastException;
        }
       
    }
}
