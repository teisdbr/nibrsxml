using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoadBusinessLayer;
using LoadBusinessLayer.LIBRSOffense;
using LoadBusinessLayer.LIBRSAdmin;
using NibrsXml.Utility;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.ReportHeader;
using NibrsXml.NibrsReport.Misc;

namespace NibrsXml.Builder
{
    public class ReportHeaderBuilder 
    {
        private const string groupAIncidentReport = "A";

        public static ReportHeader Build(List<LIBRSOffense> offenses, string actionType, LIBRSAdmin admin)
        {
            ReportHeader rptHeader = new ReportHeader();
            rptHeader.NibrsReportCategoryCode = DetermineNibrsReportCategoryCode(ref offenses);
            rptHeader.ReportActionCategoryCode = actionType;
            rptHeader.ReportDate = new ReportDate(DateTime.Now.NibrsYearMonth());
            rptHeader.ReportingAgency = new ReportingAgency(new OrganizationAugmentation(new OrganizationORIIdentification(admin.ORINumber)));
            return rptHeader;
        }

        private static string DetermineNibrsReportCategoryCode(ref List<LIBRSOffense> offenses)
        {
             // Determine NIBRS report category code based on provided offenses
            foreach (LIBRSOffense offense in offenses)
            {
                if (LarsList.LarsDictionary[offense.LRSNumber].lgroup == groupAIncidentReport)
                {
                    return NIBRSCodeAttribute.GetDescription(NIBRSReportCategoryCode.A);
                }
            }
            // If there are no group A offenses, this is a group B incident report
            return NIBRSCodeAttribute.GetDescription(NIBRSReportCategoryCode.B);
        }
    }
}
