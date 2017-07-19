using System;

namespace NibrsXml.Builder
{
    internal static class LibrsExtensions
    {
        public static string NibrsYearMonth(this DateTime date)
        {
            return date.ToString("yyyy-MM");
        }

        /// <summary>
        /// Converts the calling string which is of format MMDDYYYY to YYYY-MM-DD
        /// </summary>
        /// <param name="monthDayYear"></param>
        /// <returns></returns>
        public static string ConvertToNibrsYearMonthDay(this string monthDayYear)
        {
            return      monthDayYear.Substring(4, 4)    // year
                + "-" + monthDayYear.Substring(0, 2)    // month
                + "-" + monthDayYear.Substring(2, 2);   // day
        }
    }       
}
