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
            if (trans.Submission.Reports[0]?.Incident?.ActivityDate.DateTime != null)
            {
                var dateTime = trans.Submission.Reports[0]?.Incident?.ActivityDate.DateTime;
                if (DateTime.TryParse(dateTime, out DateTime output))
                {
                    return output;
                }
            }
            else
            {
                List<DateTime> dateTimes = new List<DateTime>();
                foreach (var arrest in trans.Submission.Reports[0].Arrests)
                {

                    if (DateTime.TryParse(arrest.Date.DateTime, out DateTime output))
                    {
                        dateTimes.Add(output);
                    }
                }
                if (dateTimes.Any())
                    return dateTimes.Min();
            }
            return null;

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
