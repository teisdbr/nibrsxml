using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.NibrsReport;
using NibrsXml.Ucr.DataCollections;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataMining
{
    class SupplementaryHomicideMiner
    {
        public static void Mine(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report)
        {
            //Get all homicides
            var homicideOffenseVicAssocs = report.OffenseVictimAssocs.Where(ov => ov.RelatedOffense.UcrCode.Matches("09[ABC]")).ToList();
            if (!homicideOffenseVicAssocs.Any())
                return;
            
            //Filter associations to query from by victims of homicide
            var homicideVictimSeqNums = homicideOffenseVicAssocs.Select(ov => ov.RelatedVictim.SeqNum);
            var homicideSubjectVictimAssocs = report.SubjectVictimAssocs.Where(sv => homicideVictimSeqNums.Contains(sv.RelatedVictim.SeqNum)).ToArray();

            //Transform filtered result set to get objects we want
            var homicideVictims = homicideVictimSeqNums
                .Select(seqNum => homicideSubjectVictimAssocs.First(sv => sv.RelatedVictim.SeqNum == seqNum).RelatedVictim)
                .Select(victim => new SupplementaryHomicide.Victim
                {
                    SequenceNumber = victim.SeqNum,
                    Age = null, //todo: translate age
                    Sex = victim.Person.SexCode,
                    Ethnicity = victim.Person.EthnicityCode,
                    Race = victim.Person.RaceCode,
                    WasKilledByNegligence = homicideOffenseVicAssocs.Any(ov => ov.RelatedOffense.UcrCode == "09B"),
                    Circumstance = victim.AggravatedAssaultHomicideFactorCode[0], //todo: translate this code
                    Subcircumstance = victim.JustifiableHomicideFactorCode
                })
                .ToList();
            var homicideSubjects = homicideSubjectVictimAssocs
                .Select(sv => sv.RelatedSubject.SeqNum)
                .Select(seqNum => homicideSubjectVictimAssocs.First(sv => sv.RelatedSubject.SeqNum == seqNum).RelatedSubject)
                .Select(subject => new SupplementaryHomicide.Offender
                {
                    SequenceNumber = subject.SeqNum,
                    Age = null,
                    Sex = subject.Person.SexCode,
                    Ethnicity = subject.Person.EthnicityCode,
                    Race = subject.Person.RaceCode
                })
                .ToList();
            var homicideRelationships = homicideSubjectVictimAssocs
                .Select(sv => new SupplementaryHomicide.Relationship
                {
                    VictimSequenceNumber = sv.RelatedVictim.SeqNum,
                    OffenderSequenceNumber = sv.RelatedSubject.SeqNum,
                    RelationshipOfVictimToOffender = sv.RelationshipCode
                })
                .ToList();

            //todo: use sequence numbers provided and make new ones that are 1-based. reason is that we want to ensure that seq nums begin at 1 and increment by one for each subsequent seq num

            var homicideIncident = new SupplementaryHomicide.Incident{
                Victims = homicideVictims,
                Offenders = homicideSubjects,
                Relationships = homicideRelationships,
                Situation = GetSituation(homicideSubjects, homicideVictims)
            };

            //Get the incident date, if incident date is not available, use report date
            
            //Add data to the appropriate SHR
        }

        private static string GetSituation(List<SupplementaryHomicide.Offender> offenders, List<SupplementaryHomicide.Victim> victims)
        {
            if (victims.Count == 0 || offenders.Count == 0)
                return null;

            if (victims.Count == 1)
            {
                //Single victim/single offender
                if (offenders.Count == 1 && offenders[0].SequenceNumber != "00")
                    return "A";

                //Single victim/unknown offender(s)
                if (offenders.All(o => o.SequenceNumber == "00"))
                    return "B";

                //Single victim/multiple offenders
                //Currently, this allows multiple offenders where at least 1 is known and at least 1 is unknown
                if (offenders.Count > 1 && offenders.Any(o => o.SequenceNumber != "00"))
                    return "C";
            }

            //Multiple victims/single offender
            if (offenders.Count == 1 && offenders[0].SequenceNumber != "00")
                return "D";

            //Multiple victims/multiple offenders
            if (offenders.Count > 1 && offenders.Any(o => o.SequenceNumber != "00"))
                return "E";

            //Multiple victims/unknown offender(s)
            return offenders.All(o => o.SequenceNumber == "00") ? "F" : null;
        }
    }
}
