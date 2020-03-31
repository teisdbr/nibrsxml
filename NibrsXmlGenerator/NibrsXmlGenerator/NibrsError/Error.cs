using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NibrsXml.NibrsError
{
    [BsonIgnoreExtraElements]
    public class Error
    {
        public ObjectId Id { get; set; }

        public ObjectId SubmissionId { get; set; }
    }
}