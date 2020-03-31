using System;
using MongoDB.Driver;
using NibrsXml.NibrsReport;
using MongoDB.Bson;
using NibrsInterface;

namespace NibrsXml.DataAccess.Collection
{
    public class SubmissionCollection
    {
        private IMongoCollection<Submission> Collection { get; set; }
        public IMongoCollection<NibrsXmlTransaction> Trans { get; set; }

        internal SubmissionCollection(IMongoCollection<Submission> collection)
        {
            Collection = collection;
        }

        internal SubmissionCollection(IMongoCollection<NibrsXmlTransaction> collections)
        {
            Trans = collections;
        }

        public void InsertMany(params Submission[] subs)
        {
            Collection.InsertMany(subs);
        }

        public void InsertOne(NibrsXmlTransaction transaction)
        {
            Trans.InsertOne(transaction);
        }

        public void UpdateResponse( NibrsXmlSubmissionResponse Response, ObjectId id)
        {
            // also need to update LsFileValid and LastTransaction fields 

            var filter = Builders<NibrsXmlTransaction>.Filter.Eq(x => x.Id, id);

            var updateDef = Builders<NibrsXmlTransaction>.Update
                .Set(o => o.NibrsSubmissionResponse, Response)
                .Set(o => o.TransactionDate, DateTime.Now);
                

            var result = Trans.UpdateOneAsync(filter, updateDef).Result;
        }

    }

}