using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using LoadBusinessLayer;
using NibrsXml.Constants.Ucr;
using NibrsXml.Utility;
using Util.Extensions;

namespace NibrsXml.Ucr.ReportRendering
{
    public class HtmlRenderer
    {
        public static string reportPrefix;
        public static void RenderUcrFromSubmission(NibrsToUcrImport ucrReports, string ori, int desiredYear, int desiredMonth)
        {
            //Get folder path
            var path = new CommonFunctions();

            //Ucr Criteria
            var ucrCriteria = string.Format("{0:0000}{1:00}", desiredYear, desiredMonth);

            //Filter Only the One report - More than one reports may have been created
            var reportKey = ucrReports.MonthlyOriReportData.Keys.FirstOrDefault(
                key => key.Contains(ucrCriteria));

            //Exit if the key does not exist!
            if (reportKey == null || !ucrReports.MonthlyOriReportData.ContainsKey(reportKey)) return;
            
            //Get desired Report Data
            var reportData = ucrReports.MonthlyOriReportData[reportKey];

            //Get Ucr Report Paths
            var ucrReportsPath = path.GetUcrFilesFolderLocation(ori) + @"\" + desiredYear + @"\" + desiredMonth.ToString("00") + @"\" ;

            //Make sure directory is created
            if (!Directory.Exists(ucrReportsPath)) Directory.CreateDirectory(ucrReportsPath);

            //Generate the xmlfile for all reports at once, XSLT will render reports individual 
            var xmlfile = reportData.Serialize("", ori, desiredYear, desiredMonth);

            reportPrefix = desiredYear.ToString("00") + desiredMonth.ToString("00") + ori;
            //Output all reports
            //Return A
            RenderUcrReport(UcrReportType.ReturnA, ucrReportsPath, CreateXmlReaderFromXmlString(xmlfile.ToString()));
            //Supplement to Return A
            RenderUcrReport(UcrReportType.SupplementToReturnA, ucrReportsPath, CreateXmlReaderFromXmlString(xmlfile.ToString()));
            //Arson
            RenderUcrReport(UcrReportType.Arson, ucrReportsPath, CreateXmlReaderFromXmlString(xmlfile.ToString()));
            //Asre
            RenderUcrReport(UcrReportType.Asre, ucrReportsPath, CreateXmlReaderFromXmlString(xmlfile.ToString()));
            //Human Trafficking
            RenderUcrReport(UcrReportType.HumanTrafficking, ucrReportsPath, CreateXmlReaderFromXmlString(xmlfile.ToString()));
            //Leoka
            RenderUcrReport(UcrReportType.Leoka, ucrReportsPath, CreateXmlReaderFromXmlString(xmlfile.ToString()));
            //Accepted or Rejected
            RenderUcrReport(UcrReportType.IncidentsAcceptedOrRejected, ucrReportsPath, CreateXmlReaderFromXmlString(xmlfile.ToString()));

        }
        public static void RenderUcrReport(UcrReportType ucrReport, string ucrFileNamePrefix, XmlReader ucrReportXmlReader)
        {
            var assembly = Assembly.GetExecutingAssembly();

            //Get details for selected report
            var details = ucrReport.GetDescriptionForAttributeType<UcrReportAttribute>();

            //Read Xsl
            var xslStream = assembly.GetManifestResourceStream(details.Assembly);

            //Create Transform
            var xslt = new XslCompiledTransform();
            xslt.Load(XmlReader.Create(xslStream));

            //Create Html Writer
            var xmlWriter = new XmlTextWriter(ucrFileNamePrefix + @"\" + reportPrefix + details.HtmlOutputName, Encoding.UTF8);

            //Write Report
            xslt.Transform(new XPathDocument(ucrReportXmlReader), xmlWriter);

            //Close the Writer
            xmlWriter.Close();
        }

        public static XmlReader CreateXmlReaderFromXmlString(string xmlDataString)
        {
            return  System.Xml.XmlReader.Create(XmlValidator.ConvertStringToStream(xmlDataString));
        }
    }
}
