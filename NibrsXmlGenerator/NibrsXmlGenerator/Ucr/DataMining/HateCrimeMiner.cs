using NibrsXml.Constants;
using NibrsModels.NibrsReport;
using NibrsModels.NibrsReport.Subject;
using NibrsXml.Ucr.DataCollections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NibrsModels.Constants;
using NibrsModels.Utility;
using NibrsXml.Constants.Ucr;
using NibrsXml.Utility;
using Util.Extensions;

namespace NibrsXml.Ucr.DataMining
{
    internal class HateCrimeMiner
    {
        public static void Mine(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report)
        {
            if (report.OffenseLocationAssocs.IsNothingOrEmpty() || report.OffenseVictimAssocs == null)
                return;
            
            //Use offense-location-association here because we will need to group the offenses with the same location later
            //Qualified offenses have both:
            //1) a valid bias motivation code
            //2) a specific group A offense determined by the hate crime specification
            var hateCrimeOffenseLocationAssocs = report.OffenseLocationAssocs
                .Where(ol =>
                    ol.RelatedOffense.FactorBiasMotivationCodes.Any(bias => UcrCodeGroups.HateCrimeBiasMotivationCodes.Contains(bias)) &&
                    Translate.HateCrimeOffenseCodeTranslations.ContainsKey(ol.RelatedOffense.UcrCode))
                .ToList();

            if (hateCrimeOffenseLocationAssocs.Count == 0)
                return;

            //Group offenses so that we do not report the same offense twice in a single incident (incident cannot contain duplicate offense codes)
            // - Group offenses by a composite of offense code and location
            //Hate crime report only allows up to 10 offenses in a single incident
            //???: What do we do when there are more than 10 offenses in an incident?
            //temp-soln: sort by most critical ascending & take only first ten
            var groupedOffenseLocationAssocs = hateCrimeOffenseLocationAssocs
                .GroupBy(ol => ol.RelatedOffense.UcrCode + ol.RelatedLocation.CategoryCode)
                .OrderBy(group => group.Key)
                .Take(10)
                .ToList();

            //Offense Data Group determinations
            var offenses = groupedOffenseLocationAssocs.Select(offLocGroup =>
            {
                //Extract and translate offense code
                var offenseCode = Translate.HateCrimeOffenseCodeTranslations[offLocGroup.First().RelatedOffense.UcrCode];

                //Extract and translate location code
                var locationCode = Translate.TranslateHateCrimeLocationCode(offLocGroup.First().RelatedLocation.CategoryCode);

                //Extract and translate bias motivations
                //Note: UCR only accepts up to 5 biases per offense
                //Nore: Excluding the NONE and UNKNOW bias motivations
                var biases = offLocGroup
                    .SelectMany(ol => ol.RelatedOffense.FactorBiasMotivationCodes).Where( bias => bias != "UNKNOWN" && bias != "NONE")
                    .Distinct()
                    .Take(5)
                    .Select(bias => Translate.HateCrimeBiasMotivationTranslations[bias])
                    .ToList();

                //Extract all offense IDs from this group to get join on the victims and offenders
                var offenseIds = offLocGroup
                    .Select(ol => ol.RelatedOffense.Id)
                    .ToList();

                //Extract victims for this offense
                //Get IDs first because we need to filter out duplicate victim IDs in the event a single victim is associated to many offenses
                var victimsIds = report.OffenseVictimAssocs
                    .Where(ov => offenseIds.Contains(ov.RelatedOffense.Id))
                    .Select(ov => ov.RelatedVictim.Id)
                    .Distinct()
                    .ToList();

                var victims = report.Victims
                    .Where(v => victimsIds.Contains(v.Id))
                    .ToList();

                //Get victim types
                var victimTypes = victims
                    .Select(v => v.CategoryCode)
                    .OrderBy(victimType => victimType)
                    .Distinct()
                    .ToList();
                
                //Get victim counts (juvenile, adult, total-indivudual and total-overall)
                var offenseIndividualVictims = victims
                    .Where(v => v.CategoryCode.MatchOne(UcrCodeGroups.HumanVictims))
                    .ToList();
                var offenseAdultVictimCount = offenseIndividualVictims.Count(v => !v.Person.AgeMeasure.IsJuvenile);
                var offenseJuvenileVictimCount = offenseIndividualVictims.Count(v => v.Person.AgeMeasure.IsJuvenile);
                var offenseTotalIndividualVictimCount = offenseJuvenileVictimCount + offenseAdultVictimCount;

                //Extract offenders for this offense
                //???: How do we get suspects for victims who are not "I" or "L" since there is no subject victim association for those?
                //temp-soln: when any victim is not I or L, use all offenders
                List<Subject> offenders;
                if (victims.Any(v => !UcrCodeGroups.HumanVictims.Contains(v.CategoryCode)))
                    offenders = report.Subjects;
                else
                {
                    //Get IDs first because we need to filter out duplicate offender IDs in the event a single offender has multiple victims
                    var offenderIds = report.SubjectVictimAssocs
                        .Where(sv => victimsIds.Contains(sv.RelatedVictim.Person.Id))
                        .Select(sv => sv.RelatedSubject.Person.Id)
                        .Distinct();

                    offenders = report.Subjects
                        .Where(s => offenderIds.Contains(s.Person.Id))
                        .ToList();
                }

                //Initialize default values for counts, races, and ethnicities
                var offenseAdultOffenderCount = 0;
                var offenseJuvenileOffenderCount = 0;
                var offenseTotalOffenderCount = 0;
                var offenseOffenderGroupRace = RACCode.UNKNOWN.NibrsCode();
                var offenseOffenderGroupEthnicity = EthnicityCode.UNKNOWN.NibrsCode();

                if (offenders.Count > 0 && offenders[0].SeqNum != HateCrimeConstants.UnknownOffenderSequenceCode)
                {
                    //Get offender counts (juvenile, adult, and total)
                    offenseJuvenileOffenderCount = offenders.Count(o => o.Person.AgeMeasure.IsJuvenile);
                    offenseAdultOffenderCount = offenders.Count(o => !o.Person.AgeMeasure.IsJuvenile);
                    offenseTotalOffenderCount = offenseJuvenileOffenderCount + offenseAdultOffenderCount;

                    //Get offender races
                    offenseOffenderGroupRace = offenders[0].Person.RaceCode;
                    if (offenders.All(o => o.Person.RaceCode != offenseOffenderGroupRace))
                        offenseOffenderGroupRace = HateCrimeConstants.HateCrimeMultipleRaceCode;
                    
                    //Get offender ethnicities
                    offenseOffenderGroupEthnicity = offenders[0].Person.EthnicityCode;
                    if (offenders.All(o => o.Person.EthnicityCode != offenseOffenderGroupEthnicity))
                        offenseOffenderGroupEthnicity = HateCrimeConstants.HateCrimeMultipleEthnicityCode;
                }

                return new {
                    HateCrimeOffense = new HateCrime.Offense
                    {
                        Code = offenseCode,
                        AdultVictimCount = offenseAdultVictimCount,
                        JuvenileVictimCount = offenseJuvenileVictimCount,
                        VictimCount = offenseTotalIndividualVictimCount,
                        Location = locationCode,
                        BiasMotivation1 = biases.ElementAtOrDefault(0),
                        BiasMotivation2 = biases.ElementAtOrDefault(1),
                        BiasMotivation3 = biases.ElementAtOrDefault(2),
                        BiasMotivation4 = biases.ElementAtOrDefault(3),
                        BiasMotivation5 = biases.ElementAtOrDefault(4),
                        VictimTypeIndividual = victimTypes.Contains(VictimCategoryCode.INDIVIDUAL.NibrsCode()),
                        VictimTypeBusiness = victimTypes.Contains(VictimCategoryCode.BUSINESS.NibrsCode()),
                        VictimTypeFinancialInstitution = victimTypes.Contains(VictimCategoryCode.FINANCIAL_INSTITUTION.NibrsCode()),
                        VictimTypeGovernment = victimTypes.Contains(VictimCategoryCode.GOVERNMENT.NibrsCode()),
                        VictimTypeReligiousOrg = victimTypes.Contains(VictimCategoryCode.RELIGIOUS_ORGANIZATION.NibrsCode()),
                        VictimTypeOther = victimTypes.Contains(VictimCategoryCode.OTHER.NibrsCode()),
                        VictimTypeUnknown = victimTypes.Contains(VictimCategoryCode.UNKNOWN.NibrsCode())
                    },
                    AdultOffenderCount = offenseTotalOffenderCount > 0 ? (int?) offenseAdultOffenderCount : null,
                    JuvenileOffenderCount = offenseTotalOffenderCount > 0 ? (int?) offenseJuvenileOffenderCount : null,
                    TotalOffenderCount = offenseTotalOffenderCount,
                    OffenderGroupRace = offenseOffenderGroupRace,
                    OffenderGroupEthnicity = offenseOffenderGroupEthnicity
                };
            }).ToList();

            //Aggregate offenders
            var incidentAdultOffenderCount = offenses.Aggregate(0, (sum, offenseData) => sum + offenseData.AdultOffenderCount ?? sum );
            var incidentJuvenileOffenderCount = offenses.Aggregate(0, (sum, offenseData) => sum + offenseData.JuvenileOffenderCount ?? sum);
            var incidentTotalOffenderCount = incidentAdultOffenderCount + incidentJuvenileOffenderCount;

            //Aggregate races
            var incidentOffenderGroupRace = offenses[0].OffenderGroupRace;
            if (offenses.All(offenseData => offenseData.OffenderGroupRace != incidentOffenderGroupRace))
                incidentOffenderGroupRace = HateCrimeConstants.HateCrimeMultipleRaceCode;

            //Aggregate ethnicities
            var incidentOffenderGroupEthnicities = offenses[0].OffenderGroupEthnicity;
            if (offenses.All(offenseData => offenseData.OffenderGroupEthnicity != incidentOffenderGroupEthnicities))
                incidentOffenderGroupEthnicities = HateCrimeConstants.HateCrimeMultipleEthnicityCode;

            //Construct full hate crime incident
            var hateCrimeIncident = new HateCrime.Incident
            {
                Id = report.Incident.ActivityId.Id.PadRight(12, ' ').Substring(0, 12),
                Date = report.Incident.ActivityDate.DateTime,
                AdultOffenderCount = incidentAdultOffenderCount,
                JuvenileOffenderCount = incidentJuvenileOffenderCount,
                TotalOffenderCount = incidentTotalOffenderCount,
                OffenderRace = incidentOffenderGroupRace,
                OffenderEthnicity = incidentOffenderGroupEthnicities,
                Offenses = offenses.Select(offenseData => offenseData.HateCrimeOffense).ToList()
            };

            //Add the hate crime incident to the appropriate HCR
            var ucrReportkey = report.UcrKey();
            monthlyReportData.TryAdd(ucrReportkey, new ReportData());
            monthlyReportData[ucrReportkey].HateCrimeData.Incidents.Add(hateCrimeIncident);
        }
    }
}
