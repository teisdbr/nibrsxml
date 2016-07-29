using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Offense
{
    [XmlRoot("Offense", Namespace = Namespaces.justice)]
    [XmlType("Offense")]
    public class OffenseList : List<Offense>
    {
        public OffenseList() { }

        public OffenseList(params Offense[] offenses)
        {
            foreach (Offense offense in offenses)
                this.Add(offense);
        }
    }
}
