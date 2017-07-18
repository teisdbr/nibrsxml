using System;
using System.Collections.Generic;
using System.Linq;
using NibrsXml.NibrsReport.Person;
using NibrsXml.NibrsReport.Victim;
using NibrsXml.NibrsReport.Subject;
using NibrsXml.NibrsReport.Arrestee;
using NibrsXml.NibrsReport.EnforcementOfficial;
using NibrsXml.NibrsReport.Associations;
using NibrsXml.Constants;
using LoadBusinessLayer;
using LoadBusinessLayer.LIBRSErrorConstants;
using NibrsXml.Utility;
using TeUtil.Extensions;

namespace NibrsXml.Builder
{
    class PersonBuilder
    {
        public static void Build(List<Person> persons, List<Victim> victims, List<Subject> subjects,
            List<Arrestee> arrestees, List<SubjectVictimAssociation> subjectVictimAssocs,
            List<EnforcementOfficial> officers, LIBRSIncident incident, String uniquePrefix)
        {
            #region Victims & EnforcementOfficials

            foreach (var victim in incident.Victim)
            {
                //Initialize victim variable to null
                Victim newVictim = null;

                //Get injury if applicable for current victim
                var librsVictimInjuries = incident.VicInjury.Where(injury => injury.VictimSeqNum == victim.VictimSeqNum);
                
                //Only instantiate injuries if at least one exists.
                List<VictimInjury> nibrsVictimInjuries = null;
                if (librsVictimInjuries != null && librsVictimInjuries.Count() > 0)
                    nibrsVictimInjuries = librsVictimInjuries.Select(i => new VictimInjury(i.InjuryType)).ToList();
                
                if (victim.VictimType == LIBRSErrorConstants.VTIndividual || victim.VictimType == LIBRSErrorConstants.VTLawEnfOfficer)
                {
                    var newPerson = new Person(
                        id: uniquePrefix,
                        ageMeasure: LibrsAgeMeasureParser(victim.Age),
                        ethnicityCode: victim.Ethnicity,
                        raceCode: victim.Race,
                        residentCode: victim.ResidentStatus,
                        sexCode: victim.Sex,
                        augmentation: LibrsAgeCodeParser(victim.Age));

                    //First create the new list of aggravated assault homicide to use when creating the new victim
                    var aggAssaults = new List<String>();
                    aggAssaults.TryAdd(
                        victim.AggravatedAssault1.TrimNullIfEmpty(),
                        victim.AggravatedAssault2.TrimNullIfEmpty(),
                        victim.AggravatedAssault3.TrimNullIfEmpty());

                    //Initialize officer variable to null
                    EnforcementOfficial newOfficer = null;

                    //Create a new EnforcementOfficial object if this person is an officer
                    //Since Victim takes
                    if (victim.VictimType == LIBRSErrorConstants.VTLawEnfOfficer)
                    {
                        newOfficer = new EnforcementOfficial(
                            person: newPerson,
                            victimSeqNum: victim.VictimSeqNum,
                            activityCategoryCode: victim.OfficerActivityCircumstance.TrimNullIfEmpty(),
                            assignmentCategoryCode: victim.OfficerAssignmentType.TrimNullIfEmpty(),
                            agencyOri: victim.OfficerOri != incident.Admin.ORINumber ? victim.OfficerOri : null);

                        //Add each of the new objects above to their respective lists
                        newVictim = new Victim(
                            officer: newOfficer,
                            injuries: nibrsVictimInjuries,
                            aggravatedAssaultHomicideFactorCode: aggAssaults,
                            justifiableHomicideFactorCode: victim.AdditionalHomicide.TrimNullIfEmpty());
                    }
                    else
                    {
                        newVictim = new Victim(
                            person: newPerson,
                            seqNum: victim.VictimSeqNum,
                            injuries: nibrsVictimInjuries,
                            categoryCode: victim.VictimType,
                            aggravatedAssaultHomicideFactorCodes: aggAssaults,
                            justifiableHomicideFactorCode: victim.AdditionalHomicide.TrimNullIfEmpty());
                    }

                    //Add related offenders for establishing relationships later on.
                    var applicableRelationships =
                        incident.VicOff.Where(vo => vo.VictimSeqNum == victim.VictimSeqNum).ToList();
                    //Translate {NM : CS, XB : BG, ES : SE} All others remain as is.
                    if (applicableRelationships.Count > 0)
                    {
                        newVictim.RelatedOffenders = applicableRelationships.Select(r =>
                        {
                            switch (r.VictimOffenderRelation)
                            {
                                case "NM":
                                    r.VictimOffenderRelation = "CS";
                                    break;
                                case "XB":
                                    r.VictimOffenderRelation = "BG";
                                    break;
                                case "ES":
                                    r.VictimOffenderRelation = "SE";
                                    break;
                            }
                            return r;
                        }).ToList();
                    }
                    //Add each of the new person objects above to their respective lists
                    persons.Add(newPerson);
                    officers.TryAdd(newOfficer);
                }
                else
                {
                    newVictim = new Victim(
                        person: null,
                        seqNum: victim.VictimSeqNum,
                        injuries: nibrsVictimInjuries,
                        categoryCode: victim.VictimType,
                        aggravatedAssaultHomicideFactorCodes: null,
                        justifiableHomicideFactorCode: null);
                }

                //Add the newly created victim to the report's victim list
                victims.TryAdd(newVictim);
            }

            #endregion

            #region Subjects

            foreach (var offender in incident.Offender)
            {
                //Create new person
                var newPerson =
                    new Person(
                        id: uniquePrefix,
                        ageMeasure: LibrsAgeMeasureParser(offender.Age),
                        ethnicityCode: null,
                        raceCode: offender.Race,
                        residentCode: null,
                        sexCode: offender.Sex,
                        augmentation: null //This person should never be a NB, BB, or NN.
                    );

                //Create new subject
                var newSubject = new Subject(newPerson, offender.OffenderSeqNum);

                //Add each of the new objects above to their respective lists
                persons.Add(newPerson);
                subjects.Add(newSubject);
            }

            #endregion

            #region SubjectVictimAssociations

            //Match victims to subjects and create relationships
            if (
                victims.Any(
                    v => v.CategoryCode.MatchOne(LIBRSErrorConstants.VTIndividual, LIBRSErrorConstants.VTLawEnfOfficer)))
            {
                var humanVictims =
                    victims.Where(
                        victim =>
                            victim.CategoryCode.MatchOne(LIBRSErrorConstants.VTIndividual,
                                LIBRSErrorConstants.VTLawEnfOfficer));
                foreach (var victim in humanVictims)
                {
                    foreach (var relatedOffender in victim.RelatedOffenders)
                    {
                        //Find matching subjects
                        var matchingSubjects =
                            subjects.Where(subject => subject.SeqNum == relatedOffender.OffenderNumberRelated.TrimStart('0'));

                        //Create relationships
                        foreach (var subject in matchingSubjects)
                        {
                            //Create association
                            var subVicAssoc = new NibrsReport.Associations.SubjectVictimAssociation(
                                uniquePrefix: uniquePrefix,
                                id: (subjectVictimAssocs.Count() + 1).ToString(),
                                subject: subject,
                                victim: victim,
                                relationshipCode: TranslateRelationship(victim,relatedOffender.VictimOffenderRelation.TrimNullIfEmpty()));

                            //Add Association to list
                            subjectVictimAssocs.Add(subVicAssoc);
                        }
                    }
                }
            }

            #endregion

            #region Arrestees

            var nibrsArrestees = incident.Arrestee.Select(
                librsArrestee =>
                {
                    //Translate juvenile disposition code
                    var juvenileDispositionCode = librsArrestee.DispositionUnder17;
                    switch (juvenileDispositionCode)
                    {
                        case LIBRSErrorConstants.DispDepartment:
                            juvenileDispositionCode = JuvenileDispositionCode.HANDLED_WITHIN_DEPARTMENT.NibrsCode();
                            break;
                        case LIBRSErrorConstants.DispPoliceAgency:
                            juvenileDispositionCode = JuvenileDispositionCode.OTHER_AUTHORITIES.NibrsCode();
                            break;
                        case LIBRSErrorConstants.DispAdultCourt:
                            juvenileDispositionCode = JuvenileDispositionCode.CRIMINAL_COURT.NibrsCode();
                            break;
                        case LIBRSErrorConstants.DispJuvenileCourt:
                        case LIBRSErrorConstants.DispWelfareAgency:
                            break;
                        default:
                            juvenileDispositionCode = null;
                            break;
                    }

                    return new Arrestee(
                        person: new Person(
                            id: uniquePrefix,
                            ageMeasure: LibrsAgeMeasureParser(librsArrestee.Age),
                            ethnicityCode: librsArrestee.Ethnicity,
                            raceCode: librsArrestee.Race,
                            residentCode: librsArrestee.ResidentStatus,
                            sexCode: librsArrestee.Sex,
                            augmentation: LibrsAgeCodeParser(librsArrestee.Age)),
                        seqId: librsArrestee.ArrestSeqNum,
                        clearanceIndicator:
                        //Translate LIBRS OtherExceptionalClearances to NIBRS NotApplicable; other clearance codes do not require translation
                        librsArrestee.ClearanceIndicator == LIBRSErrorConstants.CEOther
                            ? IncidentExceptionalClearanceCode.NOT_APPLICABLE.NibrsCode()
                            : librsArrestee.ClearanceIndicator,
                        //todo: ??? Does LIBRS Clearance Indicator of "O" translate to NIBRS of "N"?
                        armedWithCode:
                        incident.ArrArm.Where(armm => armm.ArrestSeqNum == librsArrestee.ArrestSeqNum)
                            .Select(aarm => aarm.ArrestArmedWith.TrimNullIfEmpty())
                            .ToList(),
                        juvenileDispositionCode: TranslateJuvenileDispositionCode(juvenileDispositionCode),
                        subjectCountCode: librsArrestee.MultipleArresteeIndicator);
                }).ToList();

            //var removedDupArrestees = nibrsArrestees.GroupBy(
            //    keySelector: arrtee => arrtee.Person.Id).Select(group => group.First()).ToList();

            foreach (var arrestee in nibrsArrestees)
            {
                persons.Add(arrestee.Person);
                arrestees.Add(arrestee);
            }

            #endregion
        }

        #region Helper Static Methods

        public static PersonAgeMeasure LibrsAgeMeasureParser(String age)
        {
            //Integer to hold possible estimated age.
            int calculatedAge;

            //Make sure the first two digit values are a valid integer.
            int.TryParse(age.Substring(0, 2), out calculatedAge);

            //Do not create a PersonAgeMeasure if no valid integer age was obtained
            if (calculatedAge == 0 || age == "NB" || age == "BB" || age == "NN")
                return null;

            //Determine if age is numeric or not. If it is not numeric and contains an E 
            //Parse out equivalent age range.
            else if (age.Contains("E"))
            {
                //Approximate age.
                var range = LibrsAgeAproximator(calculatedAge);
                return new PersonAgeMeasure(range.Max, range.Min);
            }
            else
            {
                return new PersonAgeMeasure(calculatedAge);
            }
        }

        public static PersonAugmentation LibrsAgeCodeParser(string age)
        {
            switch (age)
            {
                case "NN": return new PersonAugmentation(PersonAgeCode.NEONATAL.NibrsCode());
                case "NB": return new PersonAugmentation(PersonAgeCode.NEWBORN.NibrsCode());
                case "BB": return new PersonAugmentation(PersonAgeCode.BABY.NibrsCode());
                case "00": return new PersonAugmentation(PersonAgeCode.UNKNOWN.NibrsCode());
            }

            return null;
        }

        private static MeasureIntegerRange LibrsAgeAproximator(int age)
        {
            var range = new MeasureIntegerRange();

            if (age < 10)
            {
                range.Min = 0;
                range.Max = 10;
            }
            else
            {
                //By dividing by 10 and multiplying by 10 a number like 35 becomes 30.
                range.Min = age / 10 * 10;

                //By dividing by 10, adding 1, and multiplying by 10 a number like 35 becomes 40.
                range.Max = ((age / 10) + 1) * 10;
            }

            return range;
        }

        private static string TranslateJuvenileDispositionCode(string librsJuvenileDisposition)
        {
            if (librsJuvenileDisposition.IsNullBlankOrEmpty())
                return null;

            if (JuvenileDispositionCodeLibrsNibrsTranslations.ContainsKey(librsJuvenileDisposition))
                return JuvenileDispositionCodeLibrsNibrsTranslations[librsJuvenileDisposition];

            //Intentionally return the input value as a last resort so that the xml validator can pick up the error and report it at the end
            return librsJuvenileDisposition;
        }

        private static string TranslateRelationship(Victim victim,string relationship)
        {
            if (relationship.IsNullBlankOrEmpty()) return null;

            //Derived relationship
            String derivedVicOffRelationship = relationship;

            //If boyfriend or girlfriend related, add gender for specific translation
            if (relationship.MatchOne("XB", "BG"))
            {
                //Append the sex for dictionary lookup.
                derivedVicOffRelationship = victim.Person.SexCode == "U" ? "OK" : derivedVicOffRelationship + victim.Person.SexCode;
            }
            else if (!VictimOffenderRelationshipLibrsNibrsTranslation.ContainsKey(relationship))
            {
                return VictimOffenderRelationshipLibrsNibrsTranslation["RU"];
            }

            return VictimOffenderRelationshipLibrsNibrsTranslation[derivedVicOffRelationship];
        }

        #endregion

        #region Shared Variables
        private static Dictionary<String, String> JuvenileDispositionCodeLibrsNibrsTranslations = new Dictionary<string, string>
        {
            {LIBRSErrorConstants.DispDepartment, JuvenileDispositionCode.HANDLED_WITHIN_DEPARTMENT.NibrsCode()},
            {LIBRSErrorConstants.DispJuvenileCourt, JuvenileDispositionCode.JUVENILE_COURT.NibrsCode()},
            {LIBRSErrorConstants.DispWelfareAgency, JuvenileDispositionCode.WELFARE.NibrsCode()},
            {LIBRSErrorConstants.DispPoliceAgency, JuvenileDispositionCode.OTHER_AUTHORITIES.NibrsCode()},
            {LIBRSErrorConstants.DispAdultCourt, JuvenileDispositionCode.CRIMINAL_COURT.NibrsCode()}
        };

        private static Dictionary<String, String> VictimOffenderRelationshipLibrsNibrsTranslation = new Dictionary
            <string, string>()
            {
                {"SE", "Family Member_Spouse"},
                {
                    "CS",
                    "Family Member_Spouse_Common Law"
                },

                {
                    "PA",
                    "Family Member_Parent"
                },
                {
                    "SB",
                    "Family Member_Sibling"
                },
                {
                    "CH",
                    "Family Member_Child"
                },
                {
                    "GP",
                    "Family Member_Grandparent"
                },
                {
                    "GC",
                    "Family Member_Grandchild"
                },
                {
                    "IL",
                    "Family Member_In-Law"
                },
                {
                    "SP",
                    "Family Member_Stepparent"
                },
                {
                    "SC",
                    "Family Member_Stepchild"
                },
                {
                    "SS",
                    "Family Member_Stepsibling"
                },
                {
                    "OF",
                    "Family Member"
                },
                {
                    "NM",
                    "Family Member_Spouse_Common Law"
                },
                {
                    "VO",
                    "Victim Was Offender"
                },
                {
                    "AQ",
                    "Acquaintance"
                },
                {
                    "FR",
                    "Friend"
                },
                {
                    "NE",
                    "Neighbor"
                },
                {
                    "BE",
                    "Babysittee"
                },
                {
                    "BGM",
                    "Boyfriend"
                },
                {
                    "BGF",
                    "Girlfriend"
                },
                {
                    "XBM",
                    "Boyfriend"
                },
                {
                    "XBF",
                    "Girlfriend"
                },
                {
                    "CF",
                    "Child of Boyfriend_Girlfriend"
                },
                {
                    "HR",
                    "Homosexual relationship"
                },
                {
                    "XS",
                    "Ex_Spouse"
                },
                {
                    "EE",
                    "Employee"
                },
                {
                    "ER",
                    "Employer"
                },
                {
                    "OK",
                    "NonFamily_Otherwise Known"
                },
                {
                    "ES",
                    "Family Member_Spouse"
                },
                {
                    "RU",
                    "Relationship Unknown"
                },
                {
                    "ST",
                    "Stranger"
                }
            };
    }
        #endregion
}
