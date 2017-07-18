namespace NibrsXml.Ucr.DataCollections
{
    public class ReportData
    {
        #region Stored Properties

        public Asre AsreData { get; set; } = new Asre();

        public ReturnA ReturnAData { get; set; } = new ReturnA();

        public HumanTrafficking HumanTraffickingData { get; set; } = new HumanTrafficking();

        public Arson ArsonData { get; set; } = new Arson();

        public Leoka LeokaData { get; set; } = new Leoka();

        public SupplementaryHomicide SupplementaryHomicideData { get; set; } = new SupplementaryHomicide();

        public HateCrime HateCrimeData { get; set; } = new HateCrime();

        #endregion
        
        #region Computed Properties

        public ReturnASupplement ReturnASupplementData => ReturnAData.Supplement;

        #endregion
    }
}