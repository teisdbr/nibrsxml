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
        public string maxValue { get; set; }

        [XmlElement("RangeMinimumIntegerValue", Namespace = Namespaces.niemCore, Order = 2)]
        public string minValue { get; set; }

        public MeasureIntegerRange() { }

        public MeasureIntegerRange(int maxValue, int minValue)
        {
            this.maxValue = maxValue.ToString();
            this.minValue = minValue.ToString();
        }
    }
}
