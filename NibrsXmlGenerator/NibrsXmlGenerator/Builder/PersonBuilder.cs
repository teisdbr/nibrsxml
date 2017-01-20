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
                var victimInjury = incident.VicInjury.Where(injury => injury.VictimSeqNum == victim.VictimSeqNum);
                PersonInjury newInjury = null;

                //Only instantiate newInjury if one exists.
                //Todo: ??? Do we want to handle multiple injuries per victim.
                if (victimInjury.Count() > 0)
                    newInjury = victimInjury.First().InjuryType.TrimNullIfEmpty() == null
                        ? null
                        : new PersonInjury(victimInjury.First().InjuryType);

                if (victim.VictimType == LIBRSErrorConstants.VTIndividual ||
                    victim.VictimType == LIBRSErrorConstants.VTLawEnfOfficer)
                {
                    var newPerson = new Person(
                        id: uniquePrefix,
                        ageMeasure: LibrsAgeMeasureParser(victim.Age),
                        ethnicityCode: victim.Ethnicity,
                        injury: newInjury,
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
                            agencyOri: incident.Admin.ORINumber);

                        //Add each of the new objects above to their respective lists
                        newVictim = new Victim(
                            officer: newOfficer,
                            aggravatedAssaultHomicideFactorCode: aggAssaults,
                            justifiableHomicideFactorCode: victim.AdditionalHomicide.TrimNullIfEmpty());
                    }
                    else
                    {
                        newVictim = new Victim(
                            person: newPerson,
                            seqNum: victim.VictimSeqNum,
                            categoryCode: victim.VictimType,
                            aggravatedAssaultHomicideFactorCode: aggAssaults,
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
                        categoryCode: victim.VictimType,
                        aggravatedAssaultHomicideFactorCode: null,
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
                        injury: null, //Injury is not collected for offenders
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
                            subjects.Where(subject => subject.SeqNum == relatedOffender.OffenderNumberRelated);

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
                            injury: null,
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
                        juvenileDispositionCode: juvenileDispositionCode);
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
                case "NB":
                case "BB":
                case "NN":
                    return new PersonAugmentation(age);
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

            return VictimOffenderRelationshipLibrsNibrsTranslation[derivedVicOffRelationship];
        }

        #endregion

        #region Static Dictionaries
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
