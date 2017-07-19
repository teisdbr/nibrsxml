using NibrsXml.Constants;
using NibrsXml.NibrsReport;
using NibrsXml.NibrsReport.Subject;
using NibrsXml.Ucr.DataCollections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NibrsXml.Utility;
using TeUtil.Extensions;

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
                    ol.RelatedOffense.FactorBiasMotivationCodes.Any(bias => NibrsCodeGroups.HateCrimeBiasMotivationCodes.Contains(bias)) &&
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
                var biases = offLocGroup
                    .SelectMany(ol => ol.RelatedOffense.FactorBiasMotivationCodes)
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
                    .Select(ov => ov.RelatedVictim.Person.Id)
                    .Distinct()
                    .ToList();

                var victims = report.Victims
                    .Where(v => victimsIds.Contains(v.Person.Id))
                    .ToList();

                //Get victim types
                var victimTypes = victims
                    .Select(v => v.CategoryCode)
                    .OrderBy(victimType => victimType)
                    .Distinct()
                    .ToList();
                
                //Get victim counts (juvenile, adult, total-indivudual and total-overall)
                var individualVictims = victims
                    .Where(v => v.CategoryCode.MatchOne(NibrsCodeGroups.HumanVictims))
                    .ToList();
                var juvenileVictimCount = individualVictims.Count(v => v.Person.AgeMeasure.IsJuvenile);
                var adultVictimCount = individualVictims.Count(v => !v.Person.AgeMeasure.IsJuvenile);
                var totalIndividualVictimCount = juvenileVictimCount + adultVictimCount;

                //Extract offenders for this offense
                //???: How do we get suspects for victims who are not "I" or "L" since there is no subject victim association for those?
                //temp-soln: when any victim is not I or L, use all offenders
                List<Subject> offenders;
                if (victims.Any(v => !NibrsCodeGroups.HumanVictims.Contains(v.CategoryCode)))
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
                var juvenileOffenderCount = 0;
                var adultOffenderCount = 0;
                var totalOffenderCount = 0;
                var offenderGroupRace = RACCode.UNKNOWN.NibrsCode();
                var offenderGroupEthnicity = EthnicityCode.UNKNOWN.NibrsCode();

                if (offenders.Count > 1 || offenders[0].SeqNum != "00")
                {
                    //Get offender counts (juvenile, adult, and total)
                    juvenileOffenderCount = offenders.Count(o => o.Person.AgeMeasure.IsJuvenile);
                    adultOffenderCount = offenders.Count(o => !o.Person.AgeMeasure.IsJuvenile);
                    totalOffenderCount = juvenileOffenderCount + adultOffenderCount;

                    //Get offender races
                    offenderGroupRace = offenders[0].Person.RaceCode;
                    var race = offenderGroupRace;
                    if (offenders.All(o => o.Person.RaceCode != race))
                        offenderGroupRace = "M";
                    
                    //Get offender ethnicities
                    offenderGroupEthnicity = offenders[0].Person.EthnicityCode;
                    var ethnicity = offenderGroupEthnicity;
                    if (offenders.All(o => o.Person.EthnicityCode != ethnicity))
                        offenderGroupEthnicity = "M";
                }

                return new {
                    HateCrimeOffense = new HateCrime.Offense
                    {
                        Code = offenseCode,
                        AdultVictimCount = adultVictimCount,
                        JuvenileVictimCount = juvenileVictimCount,
                        TotalVictimCount = totalIndividualVictimCount,
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
                    AdultOffenderCount = totalOffenderCount == 0 ? (int?)adultOffenderCount : null,
                    JuvenileOffenderCount = totalOffenderCount == 0 ? (int?)juvenileOffenderCount : null,
                    TotalOffenderCount = totalOffenderCount,
                    OffenderGroupRace = offenderGroupRace,
                    OffenderGroupEthnicity = offenderGroupEthnicity
                };
            });

            var hateCrimeIncident = new HateCrime.Incident
            {
                Date = report.Incident.ActivityDate.DateTime,
                Id = report.Incident.ActivityId.Id.PadRight(12, ' ').Substring(0, 12)
            };

        }
    }
}
