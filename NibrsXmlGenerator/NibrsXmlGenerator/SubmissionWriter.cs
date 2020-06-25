using System.Collections.Generic;
using System.Xml;
using LoadBusinessLayer;
using NibrsModels.NibrsReport;
using NibrsXml.Builder;

namespace NibrsXml
{
  public  class SubmissionWriter
    {
        /// <summary>
        ///  Use to write NIBRS XML for  UCR extraction  of multiple agencies/WinLIBRS runnumbers. 
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="fileName"></param>
        /// <param name="nibrsSchemaLocation"></param>
        /// <returns></returns>
        public static Submission WriteXmlForUCR(List<IncidentList> lists, string fileName, string ori,
            string nibrsSchemaLocation = NibrsModels.Constants.Misc.schemaLocation)
        {

            var submission = SubmissionBuilder.Build(lists, ori);


            // Save locally
            submission.XsiSchemaLocation = nibrsSchemaLocation;
            var xdoc = new XmlDocument();
            xdoc.LoadXml(submission.Xml);
            xdoc.Save(fileName);

            return submission;

        }


        /// <summary>
        ///     Use to write NIBRS XML for a single agency/WinLIBRS runnumber.
        /// </summary>
        /// <param name="list">Incident data to be used</param>
        /// <param name="fileName">Complete file name with path prefixed</param>
        public static void WriteXml(IncidentList list, string fileName)
        {
            WriteXml(new List<IncidentList> { list }, fileName);
        }

        /// <summary>
        ///     Use to write NIBRS XML for multiple agencies/WinLIBRS runnumbers.
        /// </summary>
        /// <param name="lists">Incident data to be used</param>
        /// <param name="fileName">Complete file name with path prefixed</param>
        /// <param name="nibrsSchemaLocation"></param>
        public static Submission WriteXml(List<IncidentList> lists, string fileName,
            string nibrsSchemaLocation = NibrsModels.Constants.Misc.schemaLocation)
        {
            // NibrsResubmitter.ResbumitNibrsXml();

            var submissions = SubmissionBuilder.BuildMultipleSubmission(lists);

            //Allows overriding of the location, primarily for individual ORI xmls at this point.  /ORI/NIBRS

            foreach (var submission in submissions)
            {
                // Save locally
                submission.XsiSchemaLocation = nibrsSchemaLocation;
                var xdoc = new XmlDocument();
                xdoc.LoadXml(submission.Xml);
                xdoc.Save(fileName.Replace(".xml", submission + ".xml"));
            }

            // Return submission created above
            return submissions[0];
        }


    }
}
