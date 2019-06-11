using MongoDB.Driver;
using NibrsXml.NibrsReport;
using MongoDB.Bson;
using NibrsInterface;

namespace NibrsXml.DataAccess.Collection
{
    public class SubmissionCollection
    {
        private IMongoCollection<Submission> Collection { get; set; }
        public IMongoCollection<NIbrsXmlTransaction> Trans { get; set; }

        internal SubmissionCollection(IMongoCollection<Submission> collection)
        {
            Collection = collection;
        }

        internal SubmissionCollection(IMongoCollection<NIbrsXmlTransaction> collections)
        {
            Trans = collections;
        }

        public void InsertMany(params Submission[] subs)
        {
            Collection.InsertMany(subs);
        }

        public void InsertOne(NIbrsXmlTransaction transaction)
        {
            Trans.InsertOne(transaction);
        }

        public void UpdateResponse( NibrsXmlSubmissionResponse Response, ObjectId id)
        {
            // also need to update LsFileValid and LastTransaction fields 

            var filter = Builders<NIbrsXmlTransaction>.Filter.Eq(x => x.Id, id);

            var updateDef = Builders<NIbrsXmlTransaction>.Update
                .Set(o => o.NibrsSubmissionResponse, Response);
                

            var result = Trans.UpdateOneAsync(filter, updateDef).Result;
        }

    }

}