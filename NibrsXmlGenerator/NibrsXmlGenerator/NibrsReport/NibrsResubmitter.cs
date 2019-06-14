using NibrsXml.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using NibrsInterface;
using System.Xml;
using MongoDB.Bson.IO;
using System.Xml.Linq;



namespace NibrsXml.NibrsReport
{
   public class NibrsResubmitter
    {
        public static void ResbumitNibrsXml()
        {
            var x = _Main();

            Task.WaitAll(x);
        }

        static async Task _Main()
        {
            try
            {

                AppSettingsReader objAppsettings = new AppSettingsReader();
                var nibrsDb = new DatabaseClient(objAppsettings);

                var options = new AggregateOptions()
                {
                    AllowDiskUse = false
                };


                //BsonDocument filter = new BsonDocument();
                //filter.Add("isFileValid", new BsonBoolean(true));
                //filter.Add("nibrsResponse", new BsonDocument()
                //        .Add("$exists", new BsonBoolean(false))
                //        );


                BsonDocument filter = new BsonDocument();
                filter.Add("_id", new BsonObjectId(new ObjectId("5c92aabb89ae1755e49fac6e")));


                var cursor = await nibrsDb.Submissions.Trans.FindAsync(filter);
                var result = await cursor.ToListAsync();

                foreach (NIbrsXmlTransaction nibrsXmlTransaction in result)
                {
                    Submission submission = nibrsXmlTransaction.Submission;
                    
                    string nibrsSchemaLocation = Constants.Misc.schemaLocation;
                    submission.XsiSchemaLocation = nibrsSchemaLocation;

                    // send report to FBI and get response
                    // IsFileValid  parameter is passed by ref, value is set in SendReport method. 
                    var response = NibrsSubmitter.SendReport(submission.Xml);

                    // Updating old nibrs response with the current response. 
                    nibrsDb.Submissions.UpdateResponse(response, nibrsXmlTransaction.Id);

                    
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

        }


    }
}
