using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Person
{
    [XmlRoot("MeasureIntegerRange", Namespace = Namespaces.niemCore)]
    public class MeasureIntegerRange
    {
        [XmlElement("RangeMaximumIntegerValue", Namespace = Namespaces.niemCore, Order = 1)]
        public int max { get; set; }

        [XmlElement("RangeMinimumIntegerValue", Namespace = Namespaces.niemCore, Order = 2)]
        public int min { get; set; }

        public MeasureIntegerRange() { }

        public MeasureIntegerRange(int max, int min)
        {
            this.max = max;
            this.min = min;
        }
    }
}
