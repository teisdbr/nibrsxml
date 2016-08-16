using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.NibrsReport;
using NibrsXml.Ucr.DataCollections;
using System.Collections.Concurrent;

namespace NibrsXml.Ucr.DataMining
{
    class AsreMiner
    {
        public static void MineAdd(ConcurrentDictionary<String, ReportData> monthlyReportData, Report nibrsIncidentReport)
        {
            // Return if no arrestee data to query
            if (nibrsIncidentReport.Arrestees.Count == 0)
                return;

            // Query data
            var arrestRef = nibrsIncidentReport.Arrestees.Join(
                inner: nibrsIncidentReport.ArrestSubjectAssocs,
                outerKeySelector: arrestee => arrestee.ArresteeRef,
                innerKeySelector: assoc => assoc.ArresteeRef.ArresteeRef,
                resultSelector: (arrestee, assoc) => assoc.ActivityRef.ArrestRef);
            string offenseUcrCode = nibrsIncidentReport.Arrests.Where((arrest) => arrest.Id == arrestRef.ToString()).ElementAt(0).Charge.UcrCode;
            var asre = nibrsIncidentReport.Arrestees.Join(
                nibrsIncidentReport.Persons,
                arrestee => arrestee.ArresteeRef,
                person => person.Id,
                (arrestee, person) => new { Age = person.AgeMeasure.RangeOrValue, Sex = person.SexCode, Race = person.RaceCode, Ethnicity = person.EthnicityCode });
            
            // Use query results to add ASRA and ASRJ counts
            monthlyReportData[nibrsIncidentReport.Header.ReportDate.YearMonthDate].AsraData.AddCounts(
                offenseUcrCode,
                asre.ElementAt(0).Age,
                asre.ElementAt(0).Sex,
                asre.ElementAt(0).Race,
                asre.ElementAt(0).Ethnicity);
        }
    }
}
