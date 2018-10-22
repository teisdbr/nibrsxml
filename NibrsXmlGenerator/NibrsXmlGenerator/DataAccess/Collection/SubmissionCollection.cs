using MongoDB.Driver;
using NibrsXml.NibrsReport;

namespace NibrsXml.DataAccess.Collection
{
    public class SubmissionCollection
    {
        private IMongoCollection<Submission> Collection { get; set; }

        internal SubmissionCollection(IMongoCollection<Submission> collection)
        {
            Collection = collection;
        }

        public void InsertMany(params Submission[] subs)
        {
            Collection.InsertMany(subs);
        }
    }
}