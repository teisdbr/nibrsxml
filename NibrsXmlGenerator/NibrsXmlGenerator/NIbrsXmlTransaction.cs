using System;
using System.IO;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NibrsInterface;
using NibrsModels.NibrsReport;
using NibrsXml.Constants;

namespace NibrsXml
{
    [BsonIgnoreExtraElements]
    public class NibrsXmlTransaction 
    {

        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }                 


        public Submission Submission { get; set; } = new Submission();

        public DateTime TransactionDate { get; private set; }

        /// <summary>
        /// This gives the count of attempts made to report FBI.
        /// </summary>
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


        


        /// <summary>
        /// This constructor is used by the json serlizer, all the private properties should be set in this constructor.
        /// </summary>
        /// <param name="nibrsSubmissionResponse"></param>
        /// <param name="transactionDate"></param>
        /// <param name="numberOfAttempts"></param>
        /// <param name="status"></param>
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
            IncrementAttemptCount();
            Status = SetTransactionStatus();
        }

        /// <summary>
        /// Call this method only to update the new FBI response to the exsisting NibrsXmlTransaction. Eg:- To Update the response after re-attempt.
        /// </summary>
        /// <param name="nibrsSubmissionResponse"></param>
        public void SetNibrsXmlSubmissionResponse(NibrsXmlSubmissionResponse nibrsSubmissionResponse)
        {
            NibrsSubmissionResponse = nibrsSubmissionResponse;
            TransactionDate = DateTime.Now;
            Status = SetTransactionStatus();
            IncrementAttemptCount();
        }


        public void IncrementAttemptCount()
        {
            NumberOfAttempts += 1;
        }

        /// <summary>
        /// Deserializes the given  JSON file string into NibrsXmlTransaction.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static NibrsXmlTransaction Deserialize(string filepath)
        {
            var jsonFile = new FileStream(filepath, FileMode.Open);
            var streamReader = new StreamReader(jsonFile, new UTF8Encoding());
            try
            {
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore,
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                };               
                string json = streamReader.ReadToEnd();
                var NibrsTrans = JsonConvert.DeserializeObject<NibrsXmlTransaction>(json);
               
                return NibrsTrans;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                streamReader.Dispose();
                jsonFile.Close();
            }
           
        }


        private string  SetTransactionStatus()
        {
            return Submission.IsNibrsReportable ? AnalyzeResponse() : NibrsSubmissionStatusCodes.NotReported;
        }

        public  string AnalyzeResponse()
        {
            if (NibrsSubmissionResponse != null)
            {
                 if (NibrsSubmissionResponse.NibrsResponse != null && NibrsSubmissionResponse.NibrsResponse?.queryErrors?.Length > 0)
                    return NibrsSubmissionStatusCodes.FaultedResponse;
                if (NibrsSubmissionResponse.NibrsResponse != null 
                    && (NibrsSubmissionResponse.NibrsResponse?.ingestResponse?.status == NibrsResponseCodes.Accepted || NibrsSubmissionResponse?.NibrsResponse?.ingestResponse?.status == NibrsResponseCodes.Warnings))
                    return NibrsSubmissionStatusCodes.Accepted;
                if (NibrsSubmissionResponse.NibrsResponse != null && NibrsSubmissionResponse.NibrsResponse?.ingestResponse?.status == NibrsResponseCodes.Errors)
                    return NibrsSubmissionStatusCodes.Rejected;               
                if ( NibrsSubmissionResponse.IsUploadFailed)
                    return NibrsSubmissionStatusCodes.UploadFailed;
                if (NibrsSubmissionResponse.IsFormatError)
                    return NibrsSubmissionStatusCodes.FormatError;
            }
            
            // Assuming if no Response/no match treat it as upload failed.
            return NibrsSubmissionStatusCodes.NotReported;

        }





    }
}
