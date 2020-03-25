using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NibrsXml.Constants;
using NibrsXml.NibrsReport;
using NibrsXml.NibrsReport.Associations;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.Ucr.DataCollections;
using NibrsXml.Utility;
using TeUtil.Extensions;

namespace NibrsXml.Ucr.DataMining
{
    internal class SupplementaryHomicideMiner
    {
        public static void Mine(ConcurrentDictionary<string, ReportData> monthlyReportData, Report report)
        {
            //Get all homicides
            var homicideOffenseVicAssocs = report.OffenseVictimAssocs.Where(ov => ov.RelatedOffense.UcrCode.MatchOne("09A", "09B", "09C")).ToList();
            if (!homicideOffenseVicAssocs.Any())
                return;
            
            //Filter associations to query from by victims of homicide
            var homicideVictimSeqNums = homicideOffenseVicAssocs.Select(ov => ov.RelatedVictim.SeqNum);
            var homicideSubjectVictimAssocs = report.SubjectVictimAssocs.Where(sv => homicideVictimSeqNums.Contains(sv.RelatedVictim.SeqNum)).ToArray();

            //Extract negligence
            var offenseCodes = homicideOffenseVicAssocs.Select(ov => ov.RelatedOffense.UcrCode).Distinct().ToArray();
            var isHomicideNegligent = offenseCodes.Contains("09A") ? false : offenseCodes.Contains("09B");
            
            //Extract weapon (used for all offenders)
            var mostCriticalOffense = homicideOffenseVicAssocs.First(ov => ov.RelatedOffense.UcrCode == offenseCodes.OrderBy(o => o).First()).RelatedOffense;
            var primaryForce = mostCriticalOffense.Forces.OrderBy(f => f.CategoryCode).First();
            var weaponUsed = Translate.TranslateSupplementaryHomicideWeaponForceCode(primaryForce.CategoryCode);

            //Extract relationships
            var homicideRelationships = homicideSubjectVictimAssocs
                .Select(sv => new SupplementaryHomicide.Relationship
                {
                    VictimSequenceNumber = sv.RelatedVictim.SeqNum,
                    OffenderSequenceNumber = sv.RelatedSubject.SeqNum,
                    RelationshipOfVictimToOffender = Translate.TranslateSupplementaryHomicideRelationship(sv.RelationshipCode, sv.RelatedVictim.Person.SexCode)
                })
                .ToList();

            //Extract victims
            var homicideVictims = homicideVictimSeqNums
                .Select(seqNum => homicideSubjectVictimAssocs.First(sv => sv.RelatedVictim.SeqNum == seqNum).RelatedVictim)
                .Select(victim => new SupplementaryHomicide.Victim
                {
                    SequenceNumber = victim.SeqNum,
                    Age = victim.Person.AgeMeasure.RangeOrValue ?? "00",
                    Sex = victim.Person.SexCode,
                    Ethnicity = victim.Person.EthnicityCode,
                    Race = victim.Person.RaceCode,
                    WasKilledByNegligence = homicideOffenseVicAssocs.Any(ov => ov.RelatedOffense.UcrCode == "09B"),
                    Circumstance = GetCircumstance(
                        homicideOffenseVicAssocs.Where(ov => ov.RelatedVictim.Person.Id == victim.Person.Id)
                        .Select(ov => ov.RelatedOffense)
                        .ToList(),
                        victim.AggravatedAssaultHomicideFactorCodes,
                        homicideRelationships.Where(rel => rel.VictimSequenceNumber == victim.SeqNum).ToList(),
                        report.OffenseLocationAssocs,
                        victim.JustifiableHomicideFactorCode),
                    Subcircumstance = victim.JustifiableHomicideFactorCode == string.Empty ? null : victim.JustifiableHomicideFactorCode
                })
                .ToList();

            //Extract offenders
            var homicideSubjects = homicideSubjectVictimAssocs
                .Select(sv => sv.RelatedSubject.SeqNum)
                .Select(seqNum => homicideSubjectVictimAssocs.First(sv => sv.RelatedSubject.SeqNum == seqNum).RelatedSubject)
                .Select(subject => new SupplementaryHomicide.Offender
                {
                    SequenceNumber = subject.SeqNum,
                    Age = subject.Person?.AgeMeasure != null ? subject.Person?.AgeMeasure.RangeOrValue : "00",
                    Sex = subject.Person?.SexCode,
                    Ethnicity = subject.Person?.EthnicityCode,
                    Race = subject.Person?.RaceCode,
                    WeaponUsed = weaponUsed
                })
                .ToList();

            //Construct the homicide incident with the previous filtered data
            var homicideIncident = new SupplementaryHomicide.Incident{
                //Incident number is determined by TryAddIncident()
                IsNegligent = isHomicideNegligent,
                Victims = homicideVictims,
                Offenders = homicideSubjects,
                Relationships = homicideRelationships,
                Situation = GetSituation(homicideSubjects, homicideVictims)
            };

            //Add data to the appropriate SHR
            var ucrReportkey = report.UcrKey();
            monthlyReportData.TryAdd(ucrReportkey, new ReportData());
            monthlyReportData[ucrReportkey].SupplementaryHomicideData.TryAddIncident(homicideIncident);
        }

        private static string GetCircumstance(
            List<Offense> offenses,
            ICollection<string> victimAssaultCircumstanceCodes,
            IEnumerable<SupplementaryHomicide.Relationship> relationships,
            IEnumerable<OffenseLocationAssociation> offenseLocationAssociations,
            string justifiableHomicideFactorCode)
        {
            if (offenses.Any(o => o.UcrCode == OffenseCode.MURDER_NONNEGLIGENT.NibrsCode()))
            {
                //Part I Offenses
                if (offenses.Any(o => o.UcrCode.MatchOne(OffenseCode.RAPE.NibrsCode(), OffenseCode.SODOMY.NibrsCode(), OffenseCode.SEXUAL_ASSAULT_WITH_OBJECT.NibrsCode())))
                    return "02"; //Rape

                if (offenses.Any(o => o.UcrCode.MatchOne("120")))
                    return "03"; //Robbery

                if (offenses.Any(o => o.UcrCode.MatchOne("220")))
                    return "05"; //Burglary

                if (offenses.Any(o => o.UcrCode.MatchOne("23A", "23B", "23C", "23D", "23E", "23F", "23G", "23H")))
                    return "06"; //Larceny

                if (offenses.Any(o => o.UcrCode.MatchOne("240")))
                    return "07"; //Motor Vehicle Theft

                if (offenses.Any(o => o.UcrCode.MatchOne("200")))
                    return "09"; //Arson

                if (offenses.Any(o => o.UcrCode.MatchOne("64A")))
                    return "30"; //Human Trafficking - Commercial Sex Acts

                if (offenses.Any(o => o.UcrCode.MatchOne("64B")))
                    return "31"; //Human Trafficking - Involuntary Servitude

                //Part II Offenses
                if (offenses.Any(o => o.UcrCode.MatchOne("35A")) || victimAssaultCircumstanceCodes.Contains("03"))
                    return "18"; //Narcotic Drug Laws
                
                if (offenses.Any(o => o.UcrCode.MatchOne("11D", "36A", "36B")))
                    return "17"; //Other Sex Offenses

                if (offenses.Any(o => o.UcrCode.MatchOne("40A", "40B", "40C")))
                    return "10"; //Prostitution and Commercialized Vice

                if (offenses.Any(o => o.UcrCode.MatchOne("39A", "39B", "39C", "39D")))
                    return "19"; //Gambling

                if (victimAssaultCircumstanceCodes.Contains("08"))
                    return "26"; //Other-Not Specified

                if (victimAssaultCircumstanceCodes.Contains("09") && relationships.Select(rel => rel.RelationshipOfVictimToOffender).Contains("BE"))
                    return "41"; //Child Killed by Babysitter

                if (victimAssaultCircumstanceCodes.ContainsAny("01", "06"))
                    return "45"; //Other Arguments

                if (victimAssaultCircumstanceCodes.Contains("04"))
                    return "41"; //Gangland Killings

                if (victimAssaultCircumstanceCodes.Contains("05"))
                    return "47"; //Juvenile Gang Killings

                if (offenses.Any(o => o.Factors.Any(f => f.Code == "A")))
                    return "42"; //Brawl Due to Influence of Alcohol

                if (offenses.Any(o => o.Factors.Any(f => f.Code == "D")))
                    return "43"; //Brawl Due to Influence of Narcotics

                var locations = offenseLocationAssociations.Join(offenses, ol => ol.RelatedOffense.Id, o => o.Id,
                    (ol, o) => ol.RelatedLocation.CategoryCode).ToList();
                if (locations.Contains("15"))
                    return "48"; //Institutional Killings

                if (victimAssaultCircumstanceCodes.ContainsAny("02", "07", "09"))
                    return "60"; //Other
            }

            if (offenses.Any(o => o.UcrCode == "09C"))
            {
                if (victimAssaultCircumstanceCodes.Contains("20") && justifiableHomicideFactorCode.MatchOne("C", "D", "E", "F", "G"))
                    return "80"; //Felon Killed by Private Citizen

                if (victimAssaultCircumstanceCodes.Contains("21") && justifiableHomicideFactorCode.MatchOne("A", "B", "C", "D", "E", "F", "G"))
                    return "81"; //Felon Killed by Police Officer
            }

            return "99"; //Unable to determine
        }

        private static string GetSituation(IReadOnlyList<SupplementaryHomicide.Offender> offenders, IReadOnlyCollection<SupplementaryHomicide.Victim> victims)
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
                //This allows multiple offenders where at least 1 is known and at least 1 is unknown
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
            if (offenders.All(o => o.SequenceNumber == "00"))
                return "F";

            //None of the above are valid
            return null;
        }
    }
}