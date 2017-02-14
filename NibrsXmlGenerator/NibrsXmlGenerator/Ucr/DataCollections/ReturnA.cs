using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibrsXml.Ucr.DataCollections
{
    class ReturnA : GeneralSummaryData
    {
        public override string XmlRootName
        {
            get { return "ReturnASummary"; }
        }

        public override string XslFileName
        {
            get { return "returna.xsl"; }
        }

        internal static void ScoreHomicides(List<NibrsReport.Offense.Offense> homicideOffenses)
        {
            throw new NotImplementedException();
        }

        internal static void ScoreRapeFunctions(List<NibrsReport.Offense.Offense> rapeOffenses)
        {
            throw new NotImplementedException();
        }

        internal static void ScoreVehicleOffenses(List<NibrsReport.Offense.Offense> vehicularOffenses)
        {
            throw new NotImplementedException();
        }

        internal static void ScoreAssaultOffenses(List<NibrsReport.Offense.Offense> assaultOffenses)
        {
            throw new NotImplementedException();
        }
    }
}
