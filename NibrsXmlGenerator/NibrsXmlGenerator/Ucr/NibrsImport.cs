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
using System.Configuration;

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
            var schemasToUseForValidation = new List<KeyValuePair<String, String>>()
            {
                new KeyValuePair<String, String>("http://fbi.gov/cjis/nibrs/4.0", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"xsd\nibrs\4.0\nibrs.xsd"),
                new KeyValuePair<String, String>("http://fbi.gov/cjis/1.0", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"xsd\cjis\1.0\cjis.xsd"),
                new KeyValuePair<String, String>("http://fbi.gov/cjis/cjis-codes/1.0", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"xsd\cjis\1.0\cjis-codes.xsd"),
                new KeyValuePair<String, String>("http://release.niem.gov/niem/appinfo/3.0/", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"xsd\niem\appinfo\3.0\appinfo.xsd"),
                new KeyValuePair<String, String>("http://release.niem.gov/niem/codes/fbi_ucr/3.0/", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"xsd\niem\codes\fbi_ucr\3.0\fbi_ucr.xsd"),
                new KeyValuePair<String, String>("http://release.niem.gov/niem/domains/jxdm/5.1/", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"xsd\niem\domains\jxdm\5.1\jxdm.xsd"),
                new KeyValuePair<String, String>("http://release.niem.gov/niem/localTerminology/3.0/",
                    ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"xsd\niem\localTerminology\3.0\localTerminology.xsd"),
                new KeyValuePair<String, String>("http://release.niem.gov/niem/niem-core/3.0/", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"xsd\niem\niem-core\3.0\niem-core.xsd"),
                new KeyValuePair<String, String>("http://release.niem.gov/niem/proxy/xsd/3.0/", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"xsd\niem\proxy\xsd\3.0\xs.xsd"),
                new KeyValuePair<String, String>("http://release.niem.gov/niem/structures/3.0/", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"xsd\niem\structures\3.0\structures.xsd"),
                new KeyValuePair<String, String>("http://fbi.gov/cjis/nibrs/nibrs-codes/4.0", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"xsd\nibrs\4.0\nibrs-codes.xsd"),
            };

            //Use the StringReader Overload constructor to validate the string directly instead of reading an xml file.
            XmlValidator validator = new XmlValidator(new StringReader(submission.Xml), schemasToUseForValidation);
            if (!validator.HasErrors)
            {
                Reports = submission.Reports.Where(r => r.Header.ReportActionCategoryCode == ReportActionCategoryCode.I.NibrsCode()).ToList();
                MonthlyOriReportData = ReportDataMiner.Mine(Reports);
            }
        }
    }
}