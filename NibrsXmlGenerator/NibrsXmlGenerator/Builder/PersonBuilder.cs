using System.Collections.Generic;
using System.Linq;
using NibrsModels.NibrsReport.Person;
using NibrsModels.NibrsReport.Victim;
using NibrsModels.NibrsReport.Subject;
using NibrsModels.NibrsReport.Arrestee;
using NibrsModels.NibrsReport.EnforcementOfficial;
using NibrsModels.NibrsReport.Associations;
using NibrsModels.Constants;
using LoadBusinessLayer;
using LoadBusinessLayer.LibrsErrorConstants;
using LoadBusinessLayer.LIBRSOffense;
using NibrsModels.Utility;
using System.Security.Cryptography;
using LoadBusinessLayer.LIBRSVictim;
using Util.Extensions;

namespace NibrsXml.Builder
{
    internal class PersonBuilder
    {
        private int PersonSeqNum = 1;

        private Person BuildPerson (
            string id,
            PersonAgeMeasure ageMeasure,
            string ethnicityCode,
            string raceCode,
            string residentCode,
            string sexCode,
            string personType,
            PersonAugmentation augmentation){

                return new Person(id+"Person"+PersonSeqNum++, ageMeasure, ethnicityCode, raceCode, residentCode, sexCode, personType, augmentation 
                    );

        }

        public bool IsGroupAVictim(LoadBusinessLayer.LIBRSVictim.LIBRSVictim victim, List<LoadBusinessLayer.LIBRSOffense.LIBRSOffense> offenses) 
        {
            var matchingOffenses = offenses.Where(o => o.OffConnecttoVic == victim.VictimSeqNum);
            return matchingOffenses.Any(o => o.OffenseGroup.Equals("A", System.StringComparison.OrdinalIgnoreCase));         
        }

        public void Build(List<Person> persons, List<Victim> victims, List<Subject> subjects,
            List<Arrestee> arrestees, List<SubjectVictimAssociation> subjectVictimAssocs,
            List<EnforcementOfficial> officers, LIBRSIncident incident, string uniquePrefix)
        {
            #region Victims & EnforcementOfficials

            foreach (var victim in incident.Victim)
            {
                // Ensure its groupA victim
                if (!IsGroupAVictim(victim, incident.Offense))
                    continue;

                // Initialize victim variable to null
                Victim newVictim = null;

                //Get injury if applicable for current victim
                var librsVictimInjuries = incident.VicInjury.Where(injury => injury.VictimSeqNum == victim.VictimSeqNum);
                
                //Only instantiate injuries if at least one exists.
                List<VictimInjury> nibrsVictimInjuries = null;
                if (librsVictimInjuries != null && librsVictimInjuries.Count() > 0 && IsInjuryValidToOffense(librsVictimInjuries.ToList(), incident.Offense))
                    nibrsVictimInjuries = librsVictimInjuries.Select(i => new VictimInjury(i.InjuryType)).ToList();
                
                if (victim.VictimType == LibrsErrorConstants.VTIndividual || victim.VictimType == LibrsErrorConstants.VTLawEnfOfficer)
                {
                    var newPerson = BuildPerson(
                        id: uniquePrefix,
                        ageMeasure: LibrsAgeMeasureParser(victim.Age),
                        ethnicityCode: victim.Ethnicity,
                        raceCode: victim.Race,
                        residentCode: victim.ResidentStatus,
                        sexCode: victim.Sex,
                        augmentation: LibrsAgeCodeParser(victim.Age),
                        personType: "Victim"                        
                        );
                    

                    // First create the new list of aggravated assault homicide to use when creating the new victim
                                     
                    var aggAssaults = new List<string>();
                    aggAssaults.TryAdd(
                        victim.AggravatedAssault1.TrimNullIfEmpty(),
                        victim.AggravatedAssault2.TrimNullIfEmpty(),
                        victim.AggravatedAssault3.TrimNullIfEmpty());

                    var offenses = incident.Offense.Where(o => o.OffConnecttoVic == victim.VictimSeqNum).ToList();
                                       
                    bool is09B = offenses.Any(o => (o.AgencyAssignedNibrs.HasValue(trim: true) ? o.AgencyAssignedNibrs : LarsList.LarsDictionaryBuildNibrsXmlForUcrExtract[o.LrsNumber.Trim()].Nibr) == "09B");
                             
                    
                    //Initialize officer variable to null
                    EnforcementOfficial newOfficer = null;

                    //Create a new EnforcementOfficial object if this person is an officer
                    //Since Victim takes
                    if (victim.VictimType == LibrsErrorConstants.VTLawEnfOfficer)
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

                            // Convert aggAssault 40 to 34 if offense is 09B else convert to 09 
                            aggravatedAssaultHomicideFactorCode: aggAssaults.Select(a => (a == "40")  ?  (is09B ? "34" : "09") : a).ToList(),
                            justifiableHomicideFactorCode: victim.AdditionalHomicide.TrimNullIfEmpty(),
                            uniquePrefix: uniquePrefix);
                    }
                    else
                    {
                        newVictim = new Victim(
                            person: newPerson,
                            seqNum: victim.VictimSeqNum,
                            injuries: nibrsVictimInjuries,
                            categoryCode: victim.VictimType,

                            // Convert aggAssault 40 to 34 if offense is 09B else convert to 09 
                            aggravatedAssaultHomicideFactorCodes: aggAssaults.Select(a => (a == "40") ? (is09B ? "34" : "09") : a).ToList(),
                            justifiableHomicideFactorCode: victim.AdditionalHomicide.TrimNullIfEmpty(),
                            uniquePrefix: uniquePrefix);
                    }

                    //Add related offenders for establishing relationships later on.
                    var applicableRelationships =
                        incident.VicOff.Where(vo => vo.VictimSeqNum == victim.VictimSeqNum).ToList();
                    //Translate {NM : CS, XB : BG, ES : SE} All others remain as is.
                    if (applicableRelationships.Count > 0)
                    {
                        newVictim.RelatedOffenders =
                            applicableRelationships.Select(r =>
                        {

                           var  VictimOffenderRelation = new VictimOffenderRelation
                           {
                               ORINumber = r.ORINumber,
                               IncidentNumber = r.IncidentNumber,
                               VictimSeqNum =  r.VictimSeqNum,
                               OffenderNumberRelated =  r.OffenderNumberRelated,
                               VictimOffenderRelationCode = r.VictimOffenderRelation
                           };


                            switch (r.VictimOffenderRelation)
                            {
                                case "NM":
                                    VictimOffenderRelation.VictimOffenderRelationCode = "CS";
                                    break;
                                case "XB":
                                    VictimOffenderRelation.VictimOffenderRelationCode = "BG";
                                    break;
                                case "ES":
                                    VictimOffenderRelation.VictimOffenderRelationCode = "SE";
                                    break;
                            }


                            return VictimOffenderRelation;
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
                        justifiableHomicideFactorCode: null,
                        uniquePrefix: uniquePrefix);
                }

                //Add the newly created victim to the report's victim list
                victims.TryAdd(newVictim);
            }

            #endregion

            #region Subjects

            foreach (var offender in incident.Offender)
            {
                if(offender.OffenderSeqNum != "000")
                {
                //Create new person
                var newPerson =
                    BuildPerson(
                        id: uniquePrefix,
                        ageMeasure: offender.OffenderSeqNum == "000" ? null : LibrsAgeMeasureParser(offender.Age),
                        ethnicityCode: offender.Ethnicity.MatchOne(EthnicityCode.HISPANIC_OR_LATINO.NibrsCode(), EthnicityCode.NOT_HISPANIC_OR_LATINO.NibrsCode())
                        ? offender.Ethnicity
                        : EthnicityCode.UNKNOWN.NibrsCode(),
                        raceCode: offender.Race,
                        residentCode: ResidentCode.UNKNOWN.NibrsCode(),
                        sexCode: offender.Sex,
                        personType: "Offender",
                        augmentation: null //This person should never be a NB, BB, or NN.
                    ); ;

                //Create new subject
                var newSubject = new Subject(newPerson, offender.OffenderSeqNum, uniquePrefix);

                //Add each of the new objects above to their respective lists
                persons.Add(newPerson);
                subjects.Add(newSubject);
            }
                else
                {
                     //Create new subject for Unknow Subject
                    var newSubject = new Subject(null, "000", uniquePrefix);
                    subjects.Add(newSubject);
                }
                

            }

            #endregion

            #region SubjectVictimAssociations

            //Match victims to subjects and create relationships
            if (
                victims.Any(
                    v => v.CategoryCode.MatchOne(LibrsErrorConstants.VTIndividual, LibrsErrorConstants.VTLawEnfOfficer)))
            {
                var humanVictims =
                    victims.Where(
                        victim =>
                            victim.CategoryCode.MatchOne(LibrsErrorConstants.VTIndividual,
                                LibrsErrorConstants.VTLawEnfOfficer));
                foreach (var victim in humanVictims)
                {
                    foreach (var relatedOffender in victim.RelatedOffenders)
                    {
                        //Find matching subjects
                        var matchingSubjects =
                            subjects.Where(subject => subject.SeqNum == relatedOffender.OffenderNumberRelated.Substring(1));

                       

                        //Create relationships
                        foreach (var subject in matchingSubjects)
                        {
                            //Create association
                            var subVicAssoc = new NibrsModels.NibrsReport.Associations.SubjectVictimAssociation(
                                uniquePrefix: uniquePrefix,
                                id: (subjectVictimAssocs.Count() + 1).ToString(),
                                subject: subject,
                                victim: victim,
                                relationshipCode: TranslateRelationship(victim,relatedOffender.VictimOffenderRelationCode?.TrimNullIfEmpty()));

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
                        case LibrsErrorConstants.DispDepartment:
                            juvenileDispositionCode = JuvenileDispositionCode.HANDLED_WITHIN_DEPARTMENT.NibrsCode();
                            break;
                        case LibrsErrorConstants.DispPoliceAgency:
                            juvenileDispositionCode = JuvenileDispositionCode.OTHER_AUTHORITIES.NibrsCode();
                            break;
                        case LibrsErrorConstants.DispAdultCourt:
                            juvenileDispositionCode = JuvenileDispositionCode.CRIMINAL_COURT.NibrsCode();
                            break;
                        case LibrsErrorConstants.DispJuvenileCourt:
                        case LibrsErrorConstants.DispWelfareAgency:
                            break;
                        default:
                            juvenileDispositionCode = null;
                            break;
                    }

                    return new Arrestee(
                        person: BuildPerson(
                            id: uniquePrefix,
                            ageMeasure: LibrsAgeMeasureParser(librsArrestee.Age),
                            ethnicityCode: librsArrestee.Ethnicity,
                            raceCode: librsArrestee.Race,
                            residentCode: librsArrestee.ResidentStatus,
                            sexCode: librsArrestee.Sex,
                            personType: "Arrestee",
                            augmentation: LibrsAgeCodeParser(librsArrestee.Age)),
                        seqId: librsArrestee.ArrestSeqNum,
                        clearanceIndicator:
                        //Translate LIBRS OtherExceptionalClearances to NIBRS NotApplicable; other clearance codes do not require translation
                        librsArrestee.ClearanceIndicator == LibrsErrorConstants.CEOther
                            ? IncidentExceptionalClearanceCode.NOT_APPLICABLE.NibrsCode()
                            : librsArrestee.ClearanceIndicator,
                        //todo: ??? Does LIBRS Clearance Indicator of "O" translate to NIBRS of "N"?
                        armedWithCode:
                        incident.ArrArm.Where(armm => armm.ArrestSeqNum == librsArrestee.ArrestSeqNum)
                            .Select(aarm => aarm.ArrestArmedWith.TrimNullIfEmpty())
                            .ToList(),
                        juvenileDispositionCode: TranslateJuvenileDispositionCode(juvenileDispositionCode,librsArrestee.Age),
                        subjectCountCode: librsArrestee.MultipleArresteeIndicator,
                        uniquePerfix: uniquePrefix);
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

        public static PersonAgeMeasure LibrsAgeMeasureParser(string age)
        {
            // Integer to hold possible estimated age.
            int calculatedAge;

            // Make sure the first two digit values are a valid integer.
            int.TryParse(age.Substring(0, 2), out calculatedAge);

            // Do not create a PersonAgeMeasure if no valid integer age was obtained
            if (calculatedAge == 0 || age == "NB" || age == "BB" || age == "NN")
            {

                if (calculatedAge == 0)
                {
                    return new PersonAgeMeasure("UNKNOWN");
                }
                else if(age == "NB")
                { return new PersonAgeMeasure("NEWBORN");}
                else if(age == "BB")
                { return new PersonAgeMeasure("BABY");}
                else
                { return new PersonAgeMeasure("NEONATAL"); }

            }
                
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

        private static string TranslateJuvenileDispositionCode(string librsJuvenileDisposition, string age)
        {
            int parsedAge;
            int.TryParse(age, out parsedAge);
            
            if(parsedAge == 17)
            {
                // Has to be R we have picked Criminal court as one of the possible cases that can be translated to R. 
                return JuvenileDispositionCode.CRIMINAL_COURT.NibrsCode();
            }
            else if (librsJuvenileDisposition.IsNullBlankOrEmpty())
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
            var derivedVicOffRelationship = relationship;

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

        private static bool IsInjuryValidToOffense(List<LIBRSVictimInjury> librsvictimInjuries, List<LIBRSOffense> librsoffenses)
        {
            // Librs allows Group B offenses to have Victim Injury, but in nibrs we build victim only if we have group A offense. This causes some minor Nibrs Extraction issues which is explained below.
            //Eg:
            //1.  If the Incident has both Group A and Group B offense, only Group A offense is considered filtering out Group B to build Nibrs.
            //2.  If same victim is connected to both  above offenses, and Group A offense doesn't require victim injury and Group B does. 
            //3.  Librs validations consider Victim Injury is valid as atleast one offense connected to the victim requires injury, 
            //4.  Now in the Nibrs Only Group A is considered and it doesn't not require Victim Injury, below code makes sure only with  offenses require injury can have victim injuries.
            var offensesValidToVictimInjuries = new HashSet<string>
            {
                "100",
                "11A",
                "11C",
                "11D",
                "120",
                "13A",
                "13B",
                "210",
                "64A",
                "64B"

            };

            // Matching Group A offeneses that requires Victim Injury.
          return  librsoffenses.Where(of => of.OffenseGroup.Equals("A", System.StringComparison.OrdinalIgnoreCase) && librsvictimInjuries.Any(injury => injury.VictimSeqNum == of.OffConnecttoVic))?.Any(o => offensesValidToVictimInjuries.Contains(o.AgencyAssignedNibrs)) ?? false;
        }



        #endregion

        #region Shared Variables
        private static Dictionary<string, string> JuvenileDispositionCodeLibrsNibrsTranslations = new Dictionary<string, string>
        {
            {LibrsErrorConstants.DispDepartment, JuvenileDispositionCode.HANDLED_WITHIN_DEPARTMENT.NibrsCode()},
            {LibrsErrorConstants.DispJuvenileCourt, JuvenileDispositionCode.JUVENILE_COURT.NibrsCode()},
            {LibrsErrorConstants.DispWelfareAgency, JuvenileDispositionCode.WELFARE.NibrsCode()},
            {LibrsErrorConstants.DispPoliceAgency, JuvenileDispositionCode.OTHER_AUTHORITIES.NibrsCode()},
            {LibrsErrorConstants.DispAdultCourt, JuvenileDispositionCode.CRIMINAL_COURT.NibrsCode()}
        };

        private static Dictionary<string, string> VictimOffenderRelationshipLibrsNibrsTranslation = new Dictionary
            <string, string>
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
                    "Boyfriend_Girlfriend"
                },
                {
                    "BGF",
                    "Boyfriend_Girlfriend"
                },
                {
                    "XBM",
                    "Ex_Relationship"
                },
                {
                    "XBF",
                    "Ex_Relationship"
                },
                {
                    "XR",
                    "Ex_Relationship"
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
