using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.NibrsReport;
using NibrsXml.Ucr.DataCollections;
using LoadBusinessLayer.LIBRSErrorConstants;
using System.Collections.Concurrent;
using NibrsXml.NibrsReport.Arrestee;
using NibrsXml.NibrsReport.Associations;

namespace NibrsXml.Ucr.DataMining
{
    class AsreMiner
    {
        public static void MineAdd(ConcurrentDictionary<String, ReportData> monthlyReportData, Report nibrsIncidentReport)
        {
            // Return if no arrestee data to query
            if (nibrsIncidentReport.Arrestees.Count == 0)
                return;

            var associationsToPersons = nibrsIncidentReport.ArrestSubjectAssocs.Join(
                nibrsIncidentReport.Persons,
                assoc => assoc.SubjectRef.PersonRef,
                person => person.Id,
                (assoc, person) => new {
                    ActivityRef = assoc.ActivityRef.ArrestRef,
                    Person = person
                }).ToList();
            var associationsToPersonsToArrests = associationsToPersons.Join(
                nibrsIncidentReport.Arrests.Where(arrest => arrest.SubjectCountCode != LIBRSErrorConstants.MAMultiple).ToList(), // Count only arrestees that do not have the multiple count indicator of 'M'
                assocPerson => assocPerson.ActivityRef,
                arrest => arrest.Id,
                (assocPerson, arrest) => new
                {
                    Person = assocPerson.Person,
                    OffenseUcrCode = arrest.Charge.UcrCode
                }).ToList();
    
            //Use query results to add ASRA and ASRJ counts
            foreach (var arrest in associationsToPersonsToArrests)
            {
                monthlyReportData[nibrsIncidentReport.UcrKey].AsraData.AddCounts(
                    offenseUcrCode: arrest.OffenseUcrCode,
                    age: arrest.Person.AgeMeasure.RangeOrValue,
                    sex: arrest.Person.SexCode,
                    race: arrest.Person.RaceCode,
                    ethnicity: arrest.Person.EthnicityCode);
            }
        }
    }
}
