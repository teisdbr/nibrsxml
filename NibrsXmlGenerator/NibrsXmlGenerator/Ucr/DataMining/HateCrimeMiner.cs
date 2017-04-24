using NibrsXml.NibrsReport;
using NibrsXml.Ucr.DataCollections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Subject;
using NibrsXml.Ucr.DataCollections;

namespace NibrsXml.Ucr.DataMining
{
    class HateCrimeMiner
    {
        public static void Mine(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report)
        {
            if (report.OffenseLocationAssocs == null || report.OffenseVictimAssocs == null)
                return;
            
            //Use offense-location-association here because we will need to group the offenses with the same location
            var hateCrimeOffenseLocationAssocs = report.OffenseLocationAssocs
                .Where(ol => ol.RelatedOffense.FactorBiasMotivationCodes.Any(bias => NibrsCodeGroups.BiasMotivationCodes.Contains(bias)))
                .ToList();

            if (hateCrimeOffenseLocationAssocs.Count == 0)
                return;

            //Group offenses with the same location so that we do not report the same offense-location-pair twice in a single incident
            //Hate crime report only allows 10 offenses in a single incident
            //question: What do we do when there are more than 10 offenses in an incident?
            //temp-soln: take only first ten
            var groupedOffenseLocationAssocs = hateCrimeOffenseLocationAssocs
                .GroupBy(ol => Tuple.Create(ol.RelatedOffense.UcrCode, ol.RelatedLocation.CategoryCode))
                .OrderBy(group => group.Key.Item1)
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
                    .Take(5);

                //Extract location
                var location = offLocGroup.Key.Item2;

                //Extract victims for this offense
                var victims = report.OffenseVictimAssocs
                    .Join(offenseIds, ov => ov.RelatedOffense.Id, id => id, (ov, id) => ov.RelatedVictim)
                    .ToList();

                //question: How do get suspects for victims who are not "I" or "L" since there is no subject victim association for those?
                //temporary solution -> when any victim is not I or L, use all offenders
                List<Subject> offenders;
                if (victims.Any(v => NibrsCodeGroups.HumanVicitms.Contains(v.CategoryCode)))
                    offenders = report.Subjects;
                else
                    //Extract offenders for this offense
                    offenders = report.SubjectVictimAssocs
                        .Join(victims, sv => sv.RelatedVictim.Person.Id, v => v.Person.Id, (sv, v) => sv.RelatedSubject)
                        .ToList();
            }
        }
    }
}
