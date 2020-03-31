using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NibrsInterface;

namespace NibrsXml.NibrsReport
{
    [BsonIgnoreExtraElements]
    public class NibrsXmlTransaction
    {

        [JsonConverter(typeof(ObjectIdConverter))]

        public ObjectId Id { get; set; }       
           


        public Submission Submission { get; set; } = new Submission();

        public DateTime TransactionDate { get; private set; }


        public int NumberOfAttempts { get; private set; }


        public NibrsXmlSubmissionResponse NibrsSubmissionResponse { get; private set; } = new NibrsXmlSubmissionResponse();

        /// <summary>
        /// This property will Analyize the Response and give the status of the Response.
        /// </summary>

        public string Status { get; private set; }

        /// <summary>
        /// This property will indicate if there are any operations happening on this document. No operations are happening if ProcessingId is Null.
        /// </summary>

        public string ProcessingId { get; set; }


        [JsonIgnore]
        [BsonIgnore]
        public string JsonString
        {
            get
            {
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore,
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                };

                return JsonConvert.SerializeObject(this);
            }
        }


        



        [JsonConstructor]
        private NibrsXmlTransaction(NibrsXmlSubmissionResponse nibrsSubmissionResponse, DateTime transactionDate, int numberOfAttempts, string status)
        {
            TransactionDate = transactionDate;
            NibrsSubmissionResponse = nibrsSubmissionResponse;
            NumberOfAttempts = numberOfAttempts;
            Status = status;
        }



        /// <summary>
        /// Use this Constructor when trying create a new NibrsXml Inicident Transaction
        /// </summary>
        /// <param name="submission"></param>
        /// <param name="nibrsSubmissionResponse"></param>
        public NibrsXmlTransaction(Submission submission, NibrsXmlSubmissionResponse nibrsSubmissionResponse)
        {
            Id = submission.Id;
            Submission = submission;
            NibrsSubmissionResponse = nibrsSubmissionResponse;
            TransactionDate = DateTime.Now;
            NumberOfAttempts = 1;
            Status = NibrsResponseAnalyzer.AnalyzeResponse(NibrsSubmissionResponse);
        }

        /// <summary>
        /// Call this method only to update the new FBI response to the exsisting NibrsXmlTransaction. Eg:- To Update the response after re-attempt.
        /// </summary>
        /// <param name="nibrsSubmissionResponse"></param>
        public void SetNibrsXmlSubmissionResponse(NibrsXmlSubmissionResponse nibrsSubmissionResponse)
        {
            NibrsSubmissionResponse = nibrsSubmissionResponse;
            TransactionDate = DateTime.Now;
            NumberOfAttempts += 1;
            Status = NibrsResponseAnalyzer.AnalyzeResponse(NibrsSubmissionResponse);
            //NumberOfAttempts = numberOfAttempts + 1;
        }

        
        /// <summary>
        /// Deserializes the given  JSON file string into NibrsXmlTransaction.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public NibrsXmlTransaction DeserializeNibrsXmlTransaction(string filepath)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };

            var jsonFile = new FileStream(filepath, FileMode.Open);
            var streamReader = new StreamReader(jsonFile, new UTF8Encoding());
            string json = streamReader.ReadToEnd();
            var NibrsTrans =  JsonConvert.DeserializeObject<NibrsXmlTransaction>(json);
            streamReader.Dispose();
            jsonFile.Close();
            return NibrsTrans;
        }



    }
}
