using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.NibrsReport.Arrest;

namespace NibrsXml.Ucr
{
    public static class Extensions
    {
        #region Nibrs Extensions

        public static String ArrestUcrKey(this List<Arrest> arrests, String ori)
        {
            var earliestArrest = arrests.OrderBy(a => a.Date.YearMonthDate).FirstOrDefault();
            return earliestArrest.Date.YearMonthDate.Replace("-", "").Substring(0,6) + ori;
        }

        #endregion
    }
}
