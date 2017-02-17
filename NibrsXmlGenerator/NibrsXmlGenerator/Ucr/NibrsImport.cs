using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using NibrsXml.Constants;
using NibrsXml.NibrsReport;
using NibrsXml.Ucr.DataCollections;
using NibrsXml.Ucr.DataMining;
using NibrsXml.Utility;

namespace NibrsXml.Ucr
{
    public class NibrsImport
    {
        public List<Report> Reports { get; private set; }
        public ConcurrentDictionary<string, ReportData> MonthlyOriReportData { get; private set; }

        public NibrsImport(string xmlFilepath)
        {
            XmlValidator validator = new XmlValidator(xmlFilepath);
            if (!validator.HasErrors)
            {
                Reports = Submission.Deserialize(xmlFilepath).Reports.Where(r => r.Header.ReportActionCategoryCode == ReportActionCategoryCode.I.NibrsCode()).ToList();
                MonthlyOriReportData = ReportDataMiner.Mine(Reports);
            }
        }

        public NibrsImport(Submission submission)
        {
            //Use the StringReader Overload constructor to validate the string directly instead of reading an xml file.
            XmlValidator validator = new XmlValidator(new StringReader(submission.Xml));
            if (!validator.HasErrors)
            {
                Reports = submission.Reports.Where(r => r.Header.ReportActionCategoryCode == ReportActionCategoryCode.I.NibrsCode()).ToList();
                MonthlyOriReportData = ReportDataMiner.Mine(Reports);
            }
        }
    }
}
