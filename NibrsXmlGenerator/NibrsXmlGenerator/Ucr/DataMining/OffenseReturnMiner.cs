using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using NibrsXml.Ucr.DataCollections;
using NibrsXml.NibrsReport;
using NibrsXml.NibrsReport.Arrestee;
using NibrsXml.NibrsReport.Associations;
using NibrsXml.Utility;
using System.Web.Script.Serialization;


namespace NibrsXml.Ucr.DataMining
{
    class OffenseReturnMiner
    {
        public static DataCollections.OffenseReturn Mine(NibrsReport.Submission submission)
        {
            return new DataCollections.OffenseReturn();
        }

        public static void MineAdd(ConcurrentDictionary<String, ReportData> monthlyReportData, Report nibrsIncidentReport)
        {
            // Return if no arrestee data to query.
            if (nibrsIncidentReport.Victims.Count == 0)
                return;

            //Get victim and offense relationships
            foreach (var record in nibrsIncidentReport.OffenseVictimAssocs)
            {
                //Count Offenses
                if (record.OffenseRef.UcrCode == "09A")
                {
                    monthlyReportData[nibrsIncidentReport.UcrKey].OffenseReturnData.OffenseTotals.TryAdd(OffenseReturn.NibrsCode.Murder).IncrementActualOffense();
                }
            }
            
            //Count Exceptional Clearances
            //Get Arrestee,Arrest, and Subject relationships
            var arresteeInfos = nibrsIncidentReport.ArrestSubjectAssocs.Join(
                    nibrsIncidentReport.Arrestees,
                    arrSubAssoc => arrSubAssoc.SubjectRef.Person.Id,
                    arr => arr.Person.Id,
                    (assoc, arr) =>  new ArresteeInfo
                    {
                        Arrestee = arr,
                        ArrestSubjectAssociation = assoc
                    }).ToList();
                       
            //Get only arrests that match report date
            foreach (var arrInfo in arresteeInfos)
            {
                if (arrInfo.ArrestSubjectAssociation.ActivityRef.Date.YearMonthDate == nibrsIncidentReport.Header.ReportDate.YearMonthDate || (nibrsIncidentReport.Incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceCode != null && nibrsIncidentReport.Incident.JxdmIncidentAugmentation.IncidentExceptionalClearanceDate.YearMonthDate == nibrsIncidentReport.Header.ReportDate.YearMonthDate ))
                {
                    //Increment Arrest Clearance
                    monthlyReportData[nibrsIncidentReport.UcrKey].OffenseReturnData.OffenseTotals.TryAdd(OffenseReturn.NibrsCode.Murder).IncrementAllClearences();

                    //Increment Juvenile Clearance if applicable
                    if (arrInfo.Arrestee.Person.AgeMeasure.IsJuvenile) { monthlyReportData[nibrsIncidentReport.UcrKey].OffenseReturnData.OffenseTotals.TryAdd(OffenseReturn.NibrsCode.Murder).IncrementJuvenileClearences(); }
                }
            }
        }

        private class ArresteeInfo
        {
            internal Arrestee Arrestee { get; set; }
            internal ArrestSubjectAssociation ArrestSubjectAssociation { get; set; }
        }
    }
}
