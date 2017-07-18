using NibrsXml.NibrsReport;
using NibrsXml.Ucr.DataCollections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Subject;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataMining
{
    internal class HateCrimeMiner
    {
        public static void Mine(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report)
        {
            if (report.OffenseLocationAssocs.IsNothingOrEmpty() || report.OffenseVictimAssocs == null)
                return;
            
            //Use offense-location-association here because we will need to group the offenses with the same location
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
            //Hate crime report only allows up to 10 offenses in a single incident
            //question: What do we do when there are more than 10 offenses in an incident?
            //temp-soln: sort by most critical ascending & take only first ten
            var groupedOffenseLocationAssocs = hateCrimeOffenseLocationAssocs
                .GroupBy(ol => ol.RelatedOffense.UcrCode)
                .OrderBy(group => group.Key)
                .Take(10)
                .ToList();

            var hateCrimeIncident = new HateCrime.Incident
            {
                Date = report.Header.ReportDate.YearMonthDate,
                Id = report.Incident.ActivityId.Id.PadRight(12, ' ').Substring(0, 12),
                OffenderRace = 'U',
                OffenderEthnicity = 'U'
            };

            //Offense Data Group determinations
            foreach (var offLocGroup in groupedOffenseLocationAssocs)
            {
                //Extract all offense IDs from this group to get join on the victims and offenders
                var offenseIds = offLocGroup
                    .Select(ol => ol.RelatedOffense.Id)
                    .Distinct()
                    .ToList();

                //Extract bias motivations
                //Note: UCR only accepts up to 5 biases per offense
                var biases = offLocGroup
                    .SelectMany(ol => ol.RelatedOffense.FactorBiasMotivationCodes)
                    .Distinct()
                    .Take(5)
                    .ToList();

                //Extract location - only one location is allowed per offense group
                //question: How do we report different locations when an incident contains many of the same offense?
                //temp-soln: the location of the first offense will be used
                var location = offLocGroup.First().RelatedLocation.CategoryCode;

                //Extract victims for this offense
                var victims = report.OffenseVictimAssocs
                    .Join(offenseIds, ov => ov.RelatedOffense.Id, id => id, (ov, id) => ov.RelatedVictim)
                    .ToList();

                //question: How do we get suspects for victims who are not "I" or "L" since there is no subject victim association for those?
                //temporary solution -> when any victim is not I or L, use all offenders
                List<Subject> offenders;
                if (victims.Any(v => !NibrsCodeGroups.HumanVicitms.Contains(v.CategoryCode)))
                    offenders = report.Subjects;
                else
                    //Extract offenders for this offense
                    offenders = report.SubjectVictimAssocs
                        .Join(victims, sv => sv.RelatedVictim.Person.Id, v => v.Person.Id, (sv, v) => sv.RelatedSubject)
                        .ToList();

                var hateCrimeOffense = new HateCrime.Offense(offLocGroup.Key, victims, location, biases);
                hateCrimeIncident.AddOffense(hateCrimeOffense, offenders);
            }
        }
    }
}
