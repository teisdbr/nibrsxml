using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Item;
using NibrsXml.Utility;

namespace NibrsXml.Ucr.DataCollections
{
    public class Arson : GeneralSummaryData
    {
        public override string XmlRootName
        {
            get { return "ArsonSummary"; }
        }

        public override string XslFileName
        {
            get { return "arson.xsl"; }
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

        public Arson() : base()
        {
            
        }

        #region Constants

        private const String ArsonUcrCode = "200";

        #endregion

        public void CalculateTotalStructures()
        {
            
        }
    }
}
