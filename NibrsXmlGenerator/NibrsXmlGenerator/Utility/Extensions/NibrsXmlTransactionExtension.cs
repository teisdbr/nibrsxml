using NibrsModels.Constants;
using NibrsModels.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibrsXml.Utility.Extensions
{
    public static class NibrsXmlTransactionExtension
    {
        /// <summary>
        /// If the Report doesnt have incident date,
        /// it will return the earliest arrest date, if they are multiple arrests.
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static DateTime? GetActivityDate(this NibrsXmlTransaction trans)
        {
            if (trans.Submission.Reports[0]?.Header.NibrsReportCategoryCode ==  NibrsReportCategoryCode.A.NibrsCode())
            {
                return trans.Submission.Reports[0]?.Incident?.ActivityDate.RealDateTime;
            }
            else
            {
               
                    return trans.Submission.Reports[0].Arrests.Min(arr => arr.Date.RealDateTime);
            }
           

        }

        /// <summary>
        /// If the Report doesnt have incident date,
        /// it will return the earliest arrest date, if they are multiple arrests.
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static int? GetActivityYear(this NibrsXmlTransaction trans)
        {
            // If the incident date is not present return the any arrest activity date.
            var dateTime = trans.Submission.Reports[0]?.Incident?.ActivityDate.DateTime ??
                 trans.Submission.Reports[0].Arrests.Select(arr => int.Parse(arr.Date.DateTime.Substring(0, 4))).Min().ToString();
            if (DateTime.TryParse(dateTime, out DateTime output))
            {
                return output.Year;
            }
            else
                return null;

        }

    }
}
