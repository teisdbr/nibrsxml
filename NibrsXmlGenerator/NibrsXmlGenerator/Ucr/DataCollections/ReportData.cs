using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibrsXml.Ucr.DataCollections
{
    public class ReportData
    {
        public Asra AsraData { get; set; }
        public ReturnA ReturnAData { get; set; }
        public HumanTrafficking HumanTraffickingData { get; set; }
        public Arson ArsonData { get; set; }

        public ReportData()
        {
            this.AsraData = new Asra();
            this.ReturnAData = new ReturnA();
            this.HumanTraffickingData = new HumanTrafficking();
            this.ArsonData = new Arson();
        }
    }
}
