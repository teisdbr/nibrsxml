using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Ucr.DataMining;

namespace NibrsXml.Ucr.DataCollections
{
    public class ReturnA : GeneralSummaryData
    {
        public override string XmlRootName
        {
            get { return "ReturnASummary"; }
        }

        public override string XslFileName
        {
            get { return "returna.xsl"; }
        }

        internal void ScoreHomicides(List<NibrsReport.Offense.Offense> homicideOffenses)
        {
            throw new NotImplementedException();
        }

        internal void ScoreRapeFunctions(List<NibrsReport.Offense.Offense> rapeOffenses)
        {
            throw new NotImplementedException();
        }

        internal void ScoreVehicleOffenses(List<NibrsReport.Offense.Offense> vehicularOffenses)
        {
            throw new NotImplementedException();
        }

        internal void ScoreAssaultOffenses(List<NibrsReport.Offense.Offense> assaultOffenses)
        {
            throw new NotImplementedException();
        }

        #region Burglaries

        internal void ScoreBurglaries(NibrsReport.Offense.Offense robberyOffense)
        {
            //Column 4 - Increment Actual Offense
            IncrementBurglaryActualOffenses(robberyOffense);

            //Column 5 - Increment Total Offenses Cleared By Arrest of Exceptional Means
            IncremenetBurglaryExceptionalClearance();

            //Column 6 - Increment Number of Clearances involving Juveniles
            IncrementBurglaryExceptionalClearanceWithJuveniles();



        }

        private void IncremenetBurglaryExceptionalClearance()
        {
            throw new NotImplementedException();
        }

        private void IncrementBurglaryExceptionalClearanceWithJuveniles()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function determines which line to increment
        /// of 5[abc]
        /// </summary>
        private void IncrementBurglaryActualOffenses(NibrsReport.Offense.Offense robberyOffense)
        {
            switch (robberyOffense.AttemptedIndicator)
            {
                //Offense is attempted
                case "true":
                    IncrementActualOffense("5c");
                    break;
                //Offense is completed
                case "false":
                    switch (robberyOffense.EntryPoint.PassagePointMethodCode)
                    {
                        //Force was used
                        case "F":
                            IncrementActualOffense("5a");
                            break;
                        //No Force was used
                        case "N":
                            IncrementActualOffense("5b");
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion


        #region GenericClearanceHelper
        //public Boolean 
        #endregion

        internal ReturnAMiner.ClearanceDetails ScoreClearances(List<NibrsReport.Arrest.Arrest> list, string p)
        {
            throw new NotImplementedException();
        }
    }
}