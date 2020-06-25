using System.Configuration;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using NibrsXml.DataAccess.Collection;

namespace NibrsXml.DataAccess
{
    public class DatabaseClient
    {
        private readonly IMongoDatabase _db;

        public DatabaseClient(AppSettingsReader settingsReader)
        {
            // GET CONFIGURATION VALUES
            var dbConnectionUrl = (string) settingsReader.GetValue(ConfigurationKey.MongoConnectionUrl, typeof(string));
            var dbName = (string) settingsReader.GetValue(ConfigurationKey.MongoDatabaseName, typeof(string));

            // INITIALIZATIONS
            IMongoClient server = new MongoClient(dbConnectionUrl);
            _db = server.GetDatabase(dbName);
            
            // Ignore serializing data if null
            ConventionRegistry.Register("IgnoreIfNull",
                new ConventionPack {new IgnoreIfNullConvention(true)},
                t => true);

            ConventionRegistry.Register("CamelCaseElements",
                new ConventionPack { new CamelCaseElementNameConvention() },
                t => true);
        }

        public SubmissionCollection Submissions
        {
            //get { return new SubmissionCollection(_db.GetCollection<Submission>(CollectionName.Submission)); }
            get { return new SubmissionCollection(_db.GetCollection<NibrsXmlTransaction>(CollectionName.ModifiedTest)); }

        }



        private static class ConfigurationKey
        {
            public const string MongoConnectionUrl = "MongoConnectionUrl";
            public const string MongoDatabaseName = "MongoDatabaseName";
        }

        private static class CollectionName
        {
            public const string Submission = "submission";
            public const string Transaction = "transaction";
            public const string Test = "test";
            public const string ModifiedTest = "Modified_test";

        }
    }
}