﻿using System.Linq;
using NibrsXml.NibrsReport;
using NibrsXml.Ucr.DataCollections;
using System.Collections.Concurrent;
using NibrsXml.Constants;
using NibrsXml.Utility;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataMining
{
    internal class AsreMiner
    {
        public static void MineAdd(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report)
        {
            //Filter Conditions:
            //All arrestees must have a multiple count indicator of C or N
            //All arrestees must have known sexes, races, and ethnicities.
            var validAssocs = report.ArrestSubjectAssocs.Where(assoc =>
                assoc.RelatedArrestee.SubjectCountCode.MatchOne(MultipleArresteeSegmentsCode.COUNT.NibrsCode(), MultipleArresteeSegmentsCode.NOT_APPLICABLE.NibrsCode()) &&
                assoc.RelatedArrestee.Person.SexCode.MatchOne(NibrsCodeGroups.KnownSexCodes) &&
                assoc.RelatedArrestee.Person.RaceCode.MatchOne(NibrsCodeGroups.KnownRaceCodes) &&
                assoc.RelatedArrestee.Person.EthnicityCode.MatchOne(NibrsCodeGroups.KnownEthnicityCodes));
            foreach (var assoc in validAssocs)
            {
                //Gather arrest data from the association
                var ucrReportKey = ClearanceMiner.ConvertNibrsDateToDateKeyPrefix(assoc.RelatedArrest.Date.Date) + report.Header.ReportingAgency.OrgAugmentation.OrgOriId.Id;
                var chargeUcrCode = assoc.RelatedArrest.Charge.UcrCode;
                
                //Gather arrestee data from the association
                var arresteeAge = assoc.RelatedArrestee.Person.AgeMeasure.Value;
                var arresteeSex = assoc.RelatedArrestee.Person.SexCode;
                var arresteeRace = assoc.RelatedArrestee.Person.RaceCode;
                var arresteeEthnicity = assoc.RelatedArrestee.Person.EthnicityCode;

                monthlyReportData.TryAdd(ucrReportKey, new ReportData());

                if (chargeUcrCode == "35A")
                {
                    //Drug/Narcotic offenses are handled differently than all others
                    var drugs = report.Substances.ToList();
                    var drugOffenses = report.Offenses.Where(o => o.UcrCode == "35A").ToList();

                    monthlyReportData[ucrReportKey].AsreData.AddDrugOffenseCounts(drugs, drugOffenses, arresteeAge, arresteeSex, arresteeRace, arresteeEthnicity);
                }
                else
                    monthlyReportData[ucrReportKey].AsreData.AddNonDrugOffenseCounts(chargeUcrCode, arresteeAge, arresteeSex, arresteeRace, arresteeEthnicity);
            }
        }
    }
}
