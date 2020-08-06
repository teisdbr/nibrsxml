using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using NibrsModels.NibrsReport;
using NibrsXml.Ucr.DataCollections;
using NibrsXml.Ucr.DataMining;
using System.Configuration;
using Util.Xml;

namespace NibrsXml.Ucr
{
    public class NibrsToUcrImport
    {
        public Submission NibrsSubmission { get; set; }
        public ConcurrentDictionary<string, ReportData> MonthlyOriReportData { get; set; }

        public NibrsToUcrImport(string xmlFilepath)
        {
            var validator = new XmlValidator(xmlFilepath);
            if (validator.HasErrors) return;

            NibrsSubmission = Submission.Deserialize(xmlFilepath);
            MonthlyOriReportData = ReportMiner.Mine(NibrsSubmission);
        }

        public NibrsToUcrImport(Submission submission, List<KeyValuePair<string, string>> schemasToUseForValidation = null)
        {
            //If no schemas were provided, use default. IBR should always provide it. It is optional for WinLIBRS.
            if (schemasToUseForValidation == null)
            {
                schemasToUseForValidation = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("http://fbi.gov/cjis/nibrs/4.2", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"NibrsXsd\xsd\nibrs\4.2\nibrs.xsd"),
                    new KeyValuePair<string, string>("http://fbi.gov/cjis/1.0", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"NibrsXsd\xsd\cjis\1.0\cjis.xsd"),
                    new KeyValuePair<string, string>("http://fbi.gov/cjis/cjis-codes/1.0", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"NibrsXsd\xsd\cjis\1.0\cjis-codes.xsd"),
                    new KeyValuePair<string, string>("http://release.niem.gov/niem/appinfo/3.0/", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"NibrsXsd\xsd\niem\appinfo\3.0\appinfo.xsd"),
                    new KeyValuePair<string, string>("http://release.niem.gov/niem/codes/fbi_ucr/3.2/", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"NibrsXsd\xsd\niem\codes\fbi_ucr\3.2\fbi_ucr.xsd"),
                    new KeyValuePair<string, string>("http://release.niem.gov/niem/domains/jxdm/5.2/", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"NibrsXsd\xsd\niem\domains\jxdm\5.2\jxdm.xsd"),
                    new KeyValuePair<string, string>("http://release.niem.gov/niem/localTerminology/3.0/", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"NibrsXsd\xsd\niem\localTerminology\3.0\localTerminology.xsd"),
                    new KeyValuePair<string, string>("http://release.niem.gov/niem/niem-core/3.0/", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"NibrsXsd\xsd\niem\niem-core\3.0\niem-core.xsd"),
                    new KeyValuePair<string, string>("http://release.niem.gov/niem/proxy/xsd/3.0/", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"NibrsXsd\xsd\niem\proxy\xsd\3.0\xs.xsd"),
                    new KeyValuePair<string, string>("http://release.niem.gov/niem/structures/3.0/", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"NibrsXsd\xsd\niem\structures\3.0\structures.xsd"),
                    new KeyValuePair<string, string>("http://fbi.gov/cjis/nibrs/nibrs-codes/4.2", ConfigurationManager.AppSettings[@"ReadDirectoryPath"] + @"NibrsXsd\xsd\nibrs\4.2\nibrs-codes.xsd")
                };
            }

            //Use the StringReader Overload constructor to validate the string directly instead of reading an xml file.
            var validator = new XmlValidator(new StringReader(submission.Xml), schemasToUseForValidation);
            if (validator.HasErrors) return;

            NibrsSubmission = submission;
            MonthlyOriReportData = ReportMiner.Mine(NibrsSubmission);
        }
    }
}