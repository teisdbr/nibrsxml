﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoadBusinessLayer;
using LoadBusinessLayer.LIBRSOffense;
using NibrsXml.Utility;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.ReportHeader;
using NibrsXml.NibrsReport.Misc;

namespace NibrsXml.Builder
{
    class ReportHeaderBuilder 
    {
        private LIBRSIncident currentIncident { get; set; }

        public ReportHeaderBuilder(LIBRSIncident incident)
        {
            this.currentIncident = incident;

            report = new NibrsReport.ReportHeader.ReportHeader();

            DetermineNibrsReportCatCode();

            this.report.ReportActionCategoryCode = incident.ActionType;
            //TODO: BOBBY!!!
            this.report.ReportDate = new ReportDate(DateTime.Now.ToString("yyyy-MM"));
            this.report.ReportingAgency = new ReportingAgency(new OrganizationAugmentation(new OrganizationORIIdentification(incident.Admin.ORINumber)));
        }

        public ReportHeader report { get; private set; }

        public void DetermineNibrsReportCatCode()
        {

                //Boolean groupAOffenseExists = false;
                foreach (LIBRSOffense offense in currentIncident.Offense)
                {
                    if (LarsList.LarsDictionary[offense.LRSNumber].lgroup == "A")
                    {
                        this.report.NibrsReportCategoryCode = NIBRSCodeAttribute.GetDescription(NIBRSReportCategoryCode.A);
                        return;
                    }

                }
                this.report.NibrsReportCategoryCode = NIBRSCodeAttribute.GetDescription(NIBRSReportCategoryCode.B);
        }
    }
}
