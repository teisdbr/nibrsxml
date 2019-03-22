using MongoDB.Driver;
using NibrsXml.NibrsReport;
using MongoDB.Bson;

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

        public void UpdateResponse(nibrsResponse Response, ObjectId id, string LastException, bool IsFileValid)
        {
            // also need to update LsFileValid and LastTransaction fields 

            var filter = Builders<NIbrsXmlTransaction>.Filter.Eq(x => x.Id, id);

            var updateDef = Builders<NIbrsXmlTransaction>.Update
                .Set(o => o.NibrsResponse, Response)
                .Set(o => o.LastException, LastException)
                .Set(o => o.IsFileValid, IsFileValid);

            var result = Trans.UpdateOneAsync(filter, updateDef).Result;
        }

    }

}