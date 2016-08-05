using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoadBusinessLayer.LIBRSAdmin;
using LoadBusinessLayer.LIBRSOffender;
using LoadBusinessLayer.LIBRSOffense;
using LoadBusinessLayer.LIBRSErrorConstants;
using NibrsXml.NibrsReport;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.Utility;

namespace NibrsXml.Builder
{
    static class LibrsExtensions
    {
        public static string NibrsYearMonth(this DateTime date)
        {
            return date.ToString("yyyy-MM");
        }
    }       
}
