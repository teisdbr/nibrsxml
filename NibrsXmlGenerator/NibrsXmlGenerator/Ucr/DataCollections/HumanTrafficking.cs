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

        public override XElement MappingDictionary
        {
            get
            {
                return new XElement("UcrCodeDictionary",
                    new XElement("UCRDescription", new XAttribute("value", "A"), "A. Commercial Sex Acts"),
                    new XElement("UCRDescription", new XAttribute("value", "B"), "B. Involuntary Servitude"));
            }
        }
    }
}