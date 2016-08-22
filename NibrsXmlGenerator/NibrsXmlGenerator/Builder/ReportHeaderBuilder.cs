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
        private const string deleteActionType = "D";

        public static ReportHeader Build(List<LIBRSOffense> offenses, string actionType, LIBRSAdmin admin)
        {
            ReportHeader rptHeader = new ReportHeader();
            rptHeader.NibrsReportCategoryCode = actionType == deleteActionType ? NIBRSReportCategoryCode.A.NibrsCode() : DetermineNibrsReportCategoryCode(offenses);
            rptHeader.ReportActionCategoryCode = actionType;
            rptHeader.ReportDate = new ReportDate(DateTime.Now.NibrsYearMonth());
            rptHeader.ReportingAgency = new ReportingAgency(new OrganizationAugmentation(new OrganizationORIIdentification(admin.ORINumber)));
            return rptHeader;
        }

        private static string DetermineNibrsReportCategoryCode(List<LIBRSOffense> offenses)
        {
             // Determine NIBRS report category code based on provided offenses
            foreach (LIBRSOffense offense in offenses)
            {
                //Trim LRSNumber value in offense
                offense.LRSNumber = offense.LRSNumber.Trim();

                if (LarsList.LarsDictionary[offense.LRSNumber].lgroup == groupAIncidentReport)
                {
                    return NIBRSReportCategoryCode.A.NibrsCode();
                }
            }
            // If there are no group A offenses, this is a group B incident report
            return NIBRSReportCategoryCode.B.NibrsCode();
        }
    }
}
