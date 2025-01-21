using System;
using NibrsModels.NibrsReport.Incident;
using NibrsModels.NibrsReport.Misc;
using LoadBusinessLayer.LIBRSAdmin;
using LoadBusinessLayer.LibrsErrorConstants;
using TeUtil.Extensions;

namespace NibrsXml.Builder
{
    internal class IncidentBuilder
    {
        public static Incident Build(LIBRSAdmin admin)
        {
            CjisIncidentAugmentation cjisincidentAugmentation = new CjisIncidentAugmentation(false, null);

            if (!admin.CargoTheft.IsNullBlankOrEmpty())
            {
                cjisincidentAugmentation = new CjisIncidentAugmentation(false, admin.CargoTheft.ToUpper() == "Y" ? true : false);
            }

            var inc = new Incident
            {
                ActivityId = new ActivityIdentification(admin.IncidentNumber.Trim()),
                ActivityDate = ExtractNibrsIncidentDateTime(admin),
                CjisIncidentAugmentation = cjisincidentAugmentation,
 
                // Ignore incident augmentation when action type is D 
                JxdmIncidentAugmentation = admin.ActionType != "D" ? new JxdmIncidentAugmentation(ExtractNibrsClearanceCode(admin) ,
                ExtractIncidentExceptionalClearanceDate(admin)) : null
            };
            //todo: ??? Will the IncidentReportDateIndicator in CjisIncidentAugmentation always be false?
            // There will be a cargo theft indicator that has to be initialized in this builder sometime in the future
            return inc;
        }

        /// <summary>
        /// Since LIBRS incidents don't record time, this translates LIBRS date format to NIBRS datetime format
        /// </summary>
        public static ActivityDate ExtractNibrsIncidentDateTime(LIBRSAdmin admin)
        {
            string date, time;
            try
            {
                var month = admin.IncidentDate.Substring(0, 2);
                var day = admin.IncidentDate.Substring(2, 2);
                var year = admin.IncidentDate.Substring(4, 4);
                var hour = admin.IncidentDate.Substring(9, 2).Trim();
                date = string.Format("{0}-{1}-{2}", year, month, day);
                time = string.Format("{0}:00:00", hour == string.Empty ? "00" : hour.PadLeft(2,'0'));
            }
            catch (Exception e)
            {
                throw new Exception("There was an error parsing the LIBRS incident date.", e);
            }
            return new ActivityDate(date, time);
        }

        /// <summary>
        /// Since LIBRS incidents don't record time, this translates LIBRS date format to NIBRS datetime format
        /// </summary>
        public static ActivityDate ExtractNibrsIncidentDateTime(string incidentDate)
        {
            string date, time;
            try
            {
                var month = incidentDate.Substring(0, 2);
                var day = incidentDate.Substring(2, 2);
                var year = incidentDate.Substring(4, 4);
                var hour = incidentDate.Substring(9, 2).Trim();
                date = string.Format("{0}-{1}-{2}", year, month, day);
                time = string.Format("{0}:00:00", hour == string.Empty ? "00" : hour.PadLeft(2, '0'));
            }
            catch (Exception e)
            {
                throw new Exception("There was an error parsing the LIBRS incident date.", e);
            }
            return new ActivityDate(date, time);
        }

        private static string ExtractNibrsClearanceCode(LIBRSAdmin admin)
        {
            // If action type is D ignore Exceptional Clearence Code            
            if (admin.ActionType == "I")
            {
                return admin.ClearedExceptionally == LibrsErrorConstants.CEOther ? LibrsErrorConstants.CENotApp : admin.ClearedExceptionally;
            }
            else return null;
        }

        private static IncidentExceptionalClearanceDate ExtractIncidentExceptionalClearanceDate(LIBRSAdmin admin)
        {
            
            
            // If empty or action type = D  or Exception clearance type is other (Exceptional Clearance type O is translated to N) return null
            if (admin.ExcpClearDate.IsNullBlankOrEmpty() || admin.ActionType == "D"  || admin.ClearedExceptionally == LibrsErrorConstants.CEOther) return null;
                        
            string month, day, year;
            try
            {
                month = admin.ExcpClearDate.Substring(0, 2);
                day = admin.ExcpClearDate.Substring(2, 2);
                year = admin.ExcpClearDate.Substring(4, 4);
            }
            catch (Exception e)
            {
                throw new Exception("There was an error parsing the LIBRS incident date.", e);
            }
            return new IncidentExceptionalClearanceDate(string.Format("{0}-{1}-{2}", year, month, day));
        }
    }
}
