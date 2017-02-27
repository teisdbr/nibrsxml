namespace NibrsXml.Ucr.DataCollections
{
    public class ReportData
    {
        public ReportData()
        {
            AsreData = new Asre();
            ReturnAData = new ReturnA();
            HumanTraffickingData = new HumanTrafficking();
            ArsonData = new Arson();
            LeokaData = new Leoka();
        }

        public Asre AsreData { get; set; }
        public ReturnA ReturnAData { get; set; }
        public HumanTrafficking HumanTraffickingData { get; set; }
        public Arson ArsonData { get; set; }
        public Leoka LeokaData { get; set; }

        public ReturnASupplement ReturnASupplementData
        {
            get { return ReturnAData.Supplement; }
        }
    }
}