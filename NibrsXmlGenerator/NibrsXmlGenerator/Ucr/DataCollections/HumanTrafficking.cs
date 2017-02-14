using System.Xml.Linq;

namespace NibrsXml.Ucr.DataCollections
{
    public class HumanTrafficking : GeneralSummaryData
    {
        public override string XmlRootName
        {
            get { return "HumanTraffickingSummary"; }
        }

        public override string XslFileName
        {
            get { return "ht.xsl"; }
        }
    }
}