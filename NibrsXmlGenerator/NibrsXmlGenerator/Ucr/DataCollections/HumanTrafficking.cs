namespace NibrsXml.Ucr.DataCollections
{
    public class HumanTrafficking : GeneralSummaryData
    {
        public override string XmlRootName => "HumanTraffickingSummary";
        public override string XslFileName => "ht.xsl";
        protected override void ClassificationCountEntryInstantiations()
        {
            ClassificationCounts.Add("A", new GeneralSummaryCounts());
            ClassificationCounts.Add("B", new GeneralSummaryCounts());
        }
    }
}