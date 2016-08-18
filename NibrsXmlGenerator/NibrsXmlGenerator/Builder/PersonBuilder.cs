using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.NibrsReport.Person;
using NibrsXml.NibrsReport.Victim;
using NibrsXml.NibrsReport.Subject;
using NibrsXml.NibrsReport.Arrestee;
using NibrsXml.NibrsReport.Arrest;
using NibrsXml.NibrsReport.EnforcementOfficial;
using NibrsXml.NibrsReport.Associations;
using LoadBusinessLayer;
using LoadBusinessLayer.LIBRSVictim;
using LoadBusinessLayer.LIBRSErrorConstants;
using System.Text.RegularExpressions;
using NibrsXml.Utility;

namespace NibrsXml.Builder
{
    class PersonBuilder
    {

        public static void Build(List<Person> persons, List<Victim> victims, List<Subject> subjects, List<Arrestee> arrestees, List<Arrest> arrests, List<SubjectVictimAssociation> subjectVictimAssocs,List<EnforcementOfficial> officers, LIBRSIncident incident, String uniquePrefix)
        {  
            //Collect all victims
            foreach (var victim in incident.Victim)
            {
                //Initialize victim variable to null
                Victim newVictim = null;
                
                //Get injury if applicable for current victim
                var victimInjury = incident.VicInjury.Where(injury => injury.VictimSeqNum == victim.VictimSeqNum);
                PersonInjury newInjury = null; 

                //Only instantiate newInjury if one exists.
                if (victimInjury.Count() > 0)
                    newInjury = new PersonInjury(victimInjury.First().InjuryType);

                if (victim.VictimType == LIBRSErrorConstants.VTIndividual || victim.VictimType == LIBRSErrorConstants.VTLawEnfOfficer)
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

                    //Add related offenders for establishing relationships later on.
                    newVictim.RelatedOffenders = incident.VicOff.Where(vo => vo.VictimSeqNum == victim.VictimSeqNum).ToList();

                    //Add each of the new objects above to their respective lists
                        newVictim = new Victim(
                            officer: newOfficer,
                            aggravatedAssaultHomicideFactorCode: aggAssaults,
                            justifiableHomicideFactorCode: victim.AdditionalHomicide);
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
            //Collect all subjects (offenders)
            foreach (var offender in incident.Offender)
            {
                //Create new person
                var newPerson =
                    new Person(
                            id: uniquePrefix,
                            ageMeasure: LibrsAgeMeasureParser(offender.Age),
                            ethnicityCode: null,
                            injury: null,   //Injury is not collected for offenders
                            raceCode: offender.Race,
                            residentCode: null,
                            sexCode: offender.Sex,
                            augmentation: null    //This person should never be a NB, BB, or NN.
                        );

                //Create new subject
                var newSubject = new Subject(newPerson, offender.OffenderSeqNum);

                //Add each of the new objects above to their respective lists
                persons.Add(newPerson);
                subjects.Add(newSubject);
            }
            //Match victims to subjects and create relationships
            foreach (var victim in victims)
            {
                foreach (var relatedOffender in victim.RelatedOffenders) {
                    //Find matching subjects
                    var matchingSubjects = subjects.Where(subject => subject.SeqNum == relatedOffender.OffenderNumberRelated);

                    //Create relationships
                    foreach (var subject in matchingSubjects)
	                {
                        //Create association
		                var subVicAssoc = new NibrsReport.Associations.SubjectVictimAssociation(
                                                                                            uniquePrefix: uniquePrefix,
                                                                                            id: (subjectVictimAssocs.Count() + 1).ToString(),
                                                                                            subject: subject,
                                                                                            victim: victim,
                                                                                            relationshipCode: relatedOffender.VictimOffenderRelation);

                        //Add Association to list
                        subjectVictimAssocs.Add(subVicAssoc);
	                }
                }
            }
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
            switch (age) {
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
        #endregion
    }
}
