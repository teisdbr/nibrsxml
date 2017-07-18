namespace NibrsXml.Ucr.DataCollections
{
    public class GeneralSummaryCounts
    {
        public int? ActualOffenses { get; private set; }
        public int? ClearedByArrestOrExcepMeans { get; private set; }
        public int? ClearencesInvolvingJuveniles { get; private set; }

        /// <summary>
        /// To be used only by the monthly Arson Report.
        /// </summary>
        public long? EstimatedValueOfPropertyDamage { get; private set; }

        public void IncrementActualOffense(int byValue = 1)
        {
            //If byValue is 0, do not do anything. Property would remain null as opposed to zero.
            if (byValue == 0) return;

            //Verify not null before adding
            if (this.ActualOffenses.HasValue == false) { this.ActualOffenses = 0; }

            this.ActualOffenses += byValue;
        }

        public void IncrementAllClearences(int byValue = 1)
        {
            //If byValue is 0, do not do anything. Property would remain null as opposed to zero.
            if (byValue == 0) return;

            //Verify not null before adding
            if (this.ClearedByArrestOrExcepMeans.HasValue == false) { this.ClearedByArrestOrExcepMeans = 0; }

            this.ClearedByArrestOrExcepMeans += byValue;
        }

        public void IncrementJuvenileClearences(int byValue = 1)
        {
            //If byValue is 0, do not do anything. Property would remain null as opposed to zero.
            if (byValue == 0) return;

            //Verify not null before adding
            if (this.ClearencesInvolvingJuveniles.HasValue == false) { this.ClearencesInvolvingJuveniles = 0; }

            this.ClearencesInvolvingJuveniles += byValue;
        }

        public void IncrementEstimatedValueOfPropertyDamage(long byValue = 1)
        {
            //If byValue is 0, do not do anything. Property would remain null as opposed to zero.
            if (byValue == 0) return;

            //Verify not null before adding
            if (this.EstimatedValueOfPropertyDamage.HasValue == false) { this.EstimatedValueOfPropertyDamage = 0; }

            this.EstimatedValueOfPropertyDamage += byValue;
        }
    }
}