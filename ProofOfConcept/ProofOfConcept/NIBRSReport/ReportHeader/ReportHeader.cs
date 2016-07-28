using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NIBRSXML.Constants;

namespace NIBRSReport
{
    class ReportHeader
    {
        public string NIBRSReportCategoryCode { get; set; }

        public string ReportActionCategoryCode { get; set; }

        public ReportDate ReportDate { get; set; }

        public ReportingAgency ReportingAgency { get; set; }

        public ReportHeader() { }

        public ReportHeader(string nibrsCode, string actionCode, ReportDate date, ReportingAgency agency)
        {
            this.NIBRSReportCategoryCode = nibrsCode;
            this.ReportActionCategoryCode = actionCode;
            this.ReportDate = date;
            this.ReportingAgency = agency;
        }
    }
}
