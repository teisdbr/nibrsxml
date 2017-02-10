using System;

namespace NibrsXml.Ucr.DataCollections
{
    public class GeneralSummaryCounts
    {
        public int? ActualOffenses { get; private set; }
        public int? ClearedByArrestOrExcepMeans { get; private set; }
        public int? ClearencesInvolvingJuveniles { get; private set; }

        public void IncrementActualOffense(int byValue = 1, Action<int> incrementHandler = null)
        {
            //If byValue is 0, do not do anything. Property would remain null as opposed to zero.
            if (byValue == 0) return;

            //Verify not null before adding
            if (this.ActualOffenses.HasValue == false) { this.ActualOffenses = 0; }

            this.ActualOffenses += byValue;

            //Perform increment handler if provided
            if (incrementHandler != null) incrementHandler(byValue);
        }

        public void IncrementAllClearences(int byValue = 1, Action<int> incrementHandler = null)
        {
            //If byValue is 0, do not do anything. Property would remain null as opposed to zero.
            if (byValue == 0) return;

            //Verify not null before adding
            if (this.ClearedByArrestOrExcepMeans.HasValue == false) { this.ClearedByArrestOrExcepMeans = 0; }

            this.ClearedByArrestOrExcepMeans += byValue;

            //Perform increment handler if provided
            if (incrementHandler != null) incrementHandler(byValue);
        }

        public void IncrementJuvenileClearences(int byValue = 1, Action<int> incrementHandler = null)
        {
            //If byValue is 0, do not do anything. Property would remain null as opposed to zero.
            if (byValue == 0) return;

            //Verify not null before adding
            if (this.ClearencesInvolvingJuveniles.HasValue == false) { this.ClearencesInvolvingJuveniles = 0; }

            this.ClearencesInvolvingJuveniles += byValue;

            //Perform increment handler if provided
            if (incrementHandler != null) incrementHandler(byValue);
        }
    }
}