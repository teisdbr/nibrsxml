using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NibrsXml.Utility;
using NibrsXml.Constants;

namespace NibrsXml.Ucr.DataCollections
{
    public class OffenseReturn : Data
    {
        public XDocument Serialize() { 
            return new XDocument(
                    new XProcessingInstruction(
                        "xml-stylesheet",
                        "type=\"text/xsl\" href=\"returna.xsl\""),
                        new XElement("ReturnASummary", 
                            this.OffenseTotals.Select(classif => new XElement("Classification", 
                                                                                new XAttribute("name", classif.Key.NibrsCodeDescription()),
                                                                                new XElement("Actual", classif.Value.ActualOffenses),
                                                                                new XElement("ClearedByArrest", classif.Value.ClearedByArrestOrExcepMeans),
                                                                                new XElement("ClearedByJuvArrest", classif.Value.ClearencesInvolvingJuveniles)))));
        }

        public enum NibrsCode
        {
            [CodeDescription("Murder & NonNegligent Homicide")]
            Murder,                                 //09A

            [CodeDescription("Manslaughter By Negligence")]
            Manslaughter,                           //09B

            [CodeDescription("Rape")]
            RapeCompleted,                          //11A|11B|11C,C          

            [CodeDescription("Attempts to commit Rape")]
            RapeAttempted,                          //11A|11B|11C,A 

            [CodeDescription("Firearm")]
            RobberyFirearm,                         //120, Weapon: 11|12|13|14|15|11A|12A|14A|15A

            [CodeDescription("Knife or Cutting Instrument")]
            RobberyKnife,                           //120, Weapon: 20

            [CodeDescription("Other Dangerous Weapon")]
            RobberyOther,                           //120, Weapon: 30,35,50,60,65,70,85,90,95

            [CodeDescription("Strong-Arm (Hands, Fists, Feet, Etc.)")]
            RobberyStrongArm,                       //120, Weapon: 40,99 

            [CodeDescription("Firearm")]
            AggrAssaultFireArm,                     //13A, Weapon: 11|12|13|14|15|11A|12A|14A|15A

            [CodeDescription("Knife or Cutting Instrument")]
            AggrAssaultKnife,                       //13A, Weapon: 20

            [CodeDescription("Other Dangerous Weapon")]
            AggrAssaultOther,                       //13A, Weapon: 30,35,50,60,65,70,85,90,95

            [CodeDescription("Strong-Arm (Hands, Fists, Feet, Etc.)")]
            AggrAssaultHandsFists,                  //13A, Weapon: 40,99

            [CodeDescription("Other Assaults - Simple, Not Aggravated")]
            OtherAssaults,                          //13B|13C, Weapon: 40,90,95,99

            [CodeDescription("Forcible Entry")]
            BurglaryForcibleEntry,                  //220,C,F

            [CodeDescription("Unlawful Entry - No Force")]
            BurglaryUnlawfulEntry,                  //220,C,N

            [CodeDescription("Attempted Forcible Entry")]
            BurglaryAttemptedForcibleEntry,         //220,A

            [CodeDescription("Larceny-Theft")]
            LarcenyTheft,                           //23A-H

            [CodeDescription("Autos")]
            MotorVehicleTheftAuto,                  //240,A|C,03

            [CodeDescription("Trucks & Buses")]
            MotorVehicleTheftTrucksAndBuses,        //240,C,05 28 37

            [CodeDescription("Other Vehicles")]
            MotorVehicleTheftOther                  //240,C,24
        }

        public Dictionary<NibrsCode, Counts> OffenseTotals { get; set; }

        public struct Counts
        {
            public int? ActualOffenses { get; private set; }
            public int? ClearedByArrestOrExcepMeans { get; private set; }
            public int? ClearencesInvolvingJuveniles { get; private set; }

            public void IncrementActualOffense(int byValue = 1){
                //Verify not null before adding
                if (this.ActualOffenses.HasValue == false) { this.ActualOffenses = 0; }

                this.ActualOffenses += byValue;
            }

            public void IncrementAllClearences(int byValue = 1)
            {
                //Verify not null before adding
                if (this.ActualOffenses.HasValue == false) { this.ActualOffenses = 0; }

                this.ClearedByArrestOrExcepMeans += byValue;
            }

            public void IncrementJuvenileClearences(int byValue = 1)
            {
                //Verify not null before adding
                if (this.ActualOffenses.HasValue == false) { this.ActualOffenses = 0; }

                this.ClearencesInvolvingJuveniles += byValue;
            }
        }
    }
}
