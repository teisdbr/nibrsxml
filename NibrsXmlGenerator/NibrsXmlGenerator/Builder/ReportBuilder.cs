using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoadBusinessLayer;
using NibrsXml.NibrsReport;

namespace NibrsXml.Builder
{
    class ReportBuilder
    {
        public static NibrsReport.Report Build(LIBRSIncident incident)
        {
            Report rpt = new Report();
            return rpt;
        }
    }
}
