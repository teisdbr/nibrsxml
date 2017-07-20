namespace NibrsXml.Ucr.DataCollections
{
    public class HumanTrafficking : GeneralSummaryData
    {
        public override string XmlRootName
        {
            get
            {
                return "HumanTraffickingSummary";
            }
        } 
        public override string XslFileName
        {
            get
            {
                return "ht.xsl";
            }
        }
        protected override void ClassificationCountEntryInstantiations()
        {
            ClassificationCounts.Add("A", new GeneralSummaryCounts());
            ClassificationCounts.Add("B", new GeneralSummaryCounts());
        }
    }
}