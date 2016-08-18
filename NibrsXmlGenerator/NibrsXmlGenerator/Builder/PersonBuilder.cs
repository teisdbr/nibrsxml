using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.NibrsReport.Person;
using NibrsXml.NibrsReport.Victim;
using LoadBusinessLayer;
using LoadBusinessLayer.LIBRSVictim;
using System.Text.RegularExpressions;
using NibrsXml.Utility;

namespace NibrsXml.Builder
{
    class PersonBuilder
    {

        public static void Build(List<Person> persons, List<Victim> victims, LIBRSIncident incident, String uniquePrefix)
        {  
            //Collect all victims
            foreach (var victim in incident.Victim)
            {
                //Get injury if applicable for current victim
                var victimInjury = incident.VicInjury.Where(injury => injury.VictimSeqNum == victim.VictimSeqNum);
                PersonInjury newInjury = null;

                //Only instantiate newInjury if one exists.
                if (victimInjury.Count() > 0)
                    newInjury = new PersonInjury(victimInjury.First().InjuryType);

                if (victim.VictimType == "I")
                {
                    var newPerson =
                        new Person(
                                uniquePrefix,
                                LibrsAgeMeasureParser(victim.Age),
                                victim.Ethnicity,
                                newInjury,
                                victim.Race,
                                victim.ResidentStatus,
                                victim.Sex,
                                LibrsAgeCodeParser(victim.Age)
                            );

                    //First create the new list of aggravated assault homicide to use when creating the new victim
                    var aggAssaults = new List<String>();
                    aggAssaults.TryAdd(victim.AggravatedAssault1.TrimNullIfEmpty(), victim.AggravatedAssault2.TrimNullIfEmpty(), victim.AggravatedAssault3.TrimNullIfEmpty());

                    var newVictim =
                        new Victim(newPerson, victim.VictimSeqNum, victim.VictimType, aggAssaults, victim.AdditionalHomicide.TrimNullIfEmpty());


                    //Add each of the new objects above to their respective lists
                    persons.Add(newPerson);
                    victims.Add(newVictim);
                }
                else if (victim.VictimType == "L")
                {

                }
                else
                {

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
