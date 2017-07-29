using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using LoadBusinessLayer;
using NibrsXml.Constants;
using NibrsXml.Constants.Ucr;
using NibrsXml.NibrsReport;
using NibrsXml.Utility;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.ReportRendering
{
    public class HtmlRenderer
    {
        public static void RenderUcrFromSubmission(NibrsImport ucrReports, string ori, int desiredYear, int desiredMonth)
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
            var ucrReportsPath = path.GetUCRFilesFolderLocation(ori) + @"\" + desiredYear + @"\" + desiredMonth.ToString("00");

            //Make sure directory is created
            if (!Directory.Exists(ucrReportsPath)) Directory.CreateDirectory(ucrReportsPath);

            //Output all reports
            //Return A
            RenderUcrReport(UcrReportType.ReturnA, ucrReportsPath, CreateXmlReaderFromXmlString(reportData.ReturnAData.Serialize().ToString()));
            //Supplement to Return A
            RenderUcrReport(UcrReportType.SupplementToReturnA, ucrReportsPath, CreateXmlReaderFromXmlString(reportData.ReturnASupplementData.Serialize().ToString()));
            //Arson
            RenderUcrReport(UcrReportType.Arson, ucrReportsPath, CreateXmlReaderFromXmlString(reportData.ArsonData.Serialize().ToString()));
            //Asre
            RenderUcrReport(UcrReportType.Asre, ucrReportsPath, CreateXmlReaderFromXmlString(reportData.AsreData.Serialize().ToString()));
            //Human Trafficking
            RenderUcrReport(UcrReportType.HumanTrafficking, ucrReportsPath, CreateXmlReaderFromXmlString(reportData.HumanTraffickingData.Serialize().ToString()));
            //Leoka
            RenderUcrReport(UcrReportType.Leoka, ucrReportsPath, CreateXmlReaderFromXmlString(reportData.LeokaData.Serialize().ToString()));

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
            var xmlWriter = new XmlTextWriter(ucrFileNamePrefix + @"\" + details.HtmlOutputName, Encoding.UTF8);

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
