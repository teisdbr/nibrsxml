using System.Xml.Linq;

namespace NibrsXml.Ucr.DataCollections
{
    public class ReportData
    {
        #region Stored Properties

        public Asre AsreData { get; set; }
        public ReturnA ReturnAData { get; set; }
        public HumanTrafficking HumanTraffickingData { get; set; }
        public Arson ArsonData { get; set; }
        public Leoka LeokaData { get; set; }
        public SupplementaryHomicide SupplementaryHomicideData { get; set; }
        public HateCrime HateCrimeData { get; set; }

        #endregion
        
        #region Computed Properties

        public ReturnASupplement ReturnASupplementData
        {
            get
            {
                return ReturnAData.Supplement;
            }
        } 

        #endregion

        public ReportData()
        {
            AsreData = new Asre();
            ReturnAData = new ReturnA();
            HumanTraffickingData = new HumanTrafficking();
            ArsonData = new Arson();
            LeokaData = new Leoka();
            SupplementaryHomicideData = new SupplementaryHomicide();
            HateCrimeData = new HateCrime();
        }

        public XDocument Serialize()
        {
            return new XDocument(
                new XElement("UcrReports",
                    ReturnAData.Serialize().Root,
                    HumanTraffickingData.Serialize().Root,
                    ArsonData.Serialize().Root,
                    AsreData.Serialize().Root,
                    LeokaData.Serialize().Root,
                    HateCrimeData.Serialize().Root,
                    SupplementaryHomicideData.Serialize().Root));
        }
    }
}