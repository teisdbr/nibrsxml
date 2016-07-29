using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Offense
{
    public class OffenseList
    {
        [XmlElement("Offense")]
        public List<Offense> offenses = new List<Offense>();
        
        public OffenseList() { }

        public OffenseList(Offense offense)
        {
            this.offenses.Add(offense);
        }
        
        public OffenseList(params Offense[] offenses)
        {
            foreach (Offense offense in offenses)
                this.offenses.Add(offense);
        }
    }
}
