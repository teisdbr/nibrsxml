using System.Linq;
using NibrsModels.NibrsReport;
using NibrsXml.Ucr.DataCollections;
using System.Collections.Concurrent;
using NibrsModels.Constants;
using NibrsModels.Utility;
using NibrsXml.Constants;
using NibrsXml.Constants.Ucr;
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
                assoc.RelatedArrestee.Person.SexCode.MatchOne(UcrCodeGroups.KnownSexCodes) &&
                assoc.RelatedArrestee.Person.RaceCode.MatchOne(UcrCodeGroups.KnownRaceCodes) &&
                assoc.RelatedArrestee.Person.EthnicityCode.MatchOne(UcrCodeGroups.KnownEthnicityCodes));
            foreach (var assoc in validAssocs)
            {
                //Gather arrest data from the association
                var ucrReportKey = ClearanceMiner.ConvertNibrsDateToDateKeyPrefix(assoc.RelatedArrest.Date.Date) + report.Header.ReportingAgency.OrgAugmentation.OrgOriId.Id;
                var chargeUcrCode = assoc.RelatedArrest.Charge.UcrCode;
                
                //Gather arrestee data from the association
                var arresteeAge = assoc.RelatedArrestee.Person.AgeMeasure.Value ?? assoc.RelatedArrestee.Person.AgeMeasure.RangeOrValue;
                var arresteeSex = assoc.RelatedArrestee.Person.SexCode;
                var arresteeRace = assoc.RelatedArrestee.Person.RaceCode;
                var arresteeEthnicity = assoc.RelatedArrestee.Person.EthnicityCode;
                var arresteeJuvDispCode = assoc.RelatedArrestee.JuvenileDispositionCode;

                //Ensure object is instantiated before being operated on
                monthlyReportData.TryAdd(ucrReportKey, new ReportData());
                var asreData = monthlyReportData[ucrReportKey].AsreData;
                //Begin scoring

                if (arresteeJuvDispCode != null)
                    asreData.AddJuvenileDispositionCounts(arresteeJuvDispCode);

                if (chargeUcrCode == "35A")
                {
                    //Drug/Narcotic offenses are handled differently than all others
                    var drugs = report.Substances.ToList();
                    var drugOffenses = report.Offenses.Where(o => o.UcrCode == "35A").ToList();

                    asreData.AddDrugOffenseCounts(drugs, drugOffenses, arresteeAge, arresteeSex, arresteeRace, arresteeEthnicity);
                }
                else
                    asreData.AddNonDrugOffenseCounts(chargeUcrCode, arresteeAge, arresteeSex, arresteeRace, arresteeEthnicity);
            }
        }
    }
}
