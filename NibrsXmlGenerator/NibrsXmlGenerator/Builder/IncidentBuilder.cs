using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.NibrsReport.Incident;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.Utility;
using LoadBusinessLayer;
using LoadBusinessLayer.LIBRSAdmin;
using LoadBusinessLayer.LIBRSErrorConstants;

namespace NibrsXml.Builder
{
    class IncidentBuilder
    {
        public static Incident Build(LIBRSAdmin admin)
        {
            Incident inc = new Incident();
            inc.ActivityId = new ActivityIdentification(admin.IncidentNumber.Trim());
            inc.ActivityDate = ExtractNibrsIncidentDateTime(admin);
            //todo: ??? Will the IncidentReportDateIndicator in CjisIncidentAugmentation always be false?
            inc.CjisIncidentAugmentation = new CjisIncidentAugmentation(false, false); // There will be a cargo theft indicator that has to be initialized in this builder sometime in the future
            inc.JxdmIncidentAugmentation = new JxdmIncidentAugmentation(ExtractNibrsClearanceCode(admin), new IncidentExceptionalClearanceDate(ExtractNibrsClearanceDate(admin)));
            return inc;
        }

        /// <summary>
        /// Since LIBRS incidents don't record time, this translates LIBRS date format to NIBRS datetime format
        /// </summary>
        private static ActivityDate ExtractNibrsIncidentDateTime(LIBRSAdmin admin)
        {
            string month, day, year, hour, date, time;
            try
            {
                month = admin.IncidentDate.Substring(0, 2);
                day = admin.IncidentDate.Substring(2, 2);
                year = admin.IncidentDate.Substring(4, 4);
                hour = admin.IncidentDate.Substring(8, 2).Trim();
                date = String.Format("{0}-{1}-{2}", year, month, day);
                time = String.Format("{0}:00:00", hour == String.Empty ? "00" : hour);
            }
            catch (Exception e)
            {
                throw new Exception("There was an error parsing the LIBRS incident date.", e);
            }
            return new ActivityDate(date, time);
        }

        private static string ExtractNibrsClearanceCode(LIBRSAdmin admin)
        {
            if (admin.ClearedExceptionally == LIBRSErrorConstants.CEOther)
                return LIBRSErrorConstants.CENotApp;
            return admin.ClearedExceptionally;
        }

        private static string ExtractNibrsClearanceDate(LIBRSAdmin admin)
        {
            string month, day, year;
            try
            {
                month = admin.IncidentDate.Substring(0, 2);
                day = admin.IncidentDate.Substring(2, 2);
                year = admin.IncidentDate.Substring(4, 4);
            }
            catch (Exception e)
            {
                throw new Exception("There was an error parsing the LIBRS incident date.", e);
            }
            return String.Format("{0}-{1}-{2}", year, month, day);
        }
    }
}
