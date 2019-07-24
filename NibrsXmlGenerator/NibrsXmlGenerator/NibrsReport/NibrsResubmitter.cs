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
        internal static IMongoCollection<NibrsXmlTransaction> Collection { get; set; }

        internal NibrsResubmitter(IMongoCollection<NibrsXmlTransaction> collection)
        {
            Collection = collection;
        }


        public static async Task ResbumitNibrsXml(List<FilterDefinition<NibrsXmlTransaction>> reUploadFilter)
        {
            try
            {

                AppSettingsReader objAppsettings = new AppSettingsReader();
                var nibrsDb = new DatabaseClient(objAppsettings);

                var options = new AggregateOptions()
                {
                    AllowDiskUse = false
                };




                var nibrsXmlTransacCursor = await Collection.FindAsync(Builders<NibrsXmlTransaction>.Filter.And(reUploadFilter));
                
                var result = await nibrsXmlTransacCursor.ToListAsync();

                foreach (NibrsXmlTransaction nibrsXmlTransaction in result)
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
