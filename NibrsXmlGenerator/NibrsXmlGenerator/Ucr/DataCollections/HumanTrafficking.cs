using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
