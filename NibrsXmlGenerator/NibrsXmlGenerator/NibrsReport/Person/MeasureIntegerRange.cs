using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Person
{
    [XmlRoot("MeasureIntegerRange", Namespace = Namespaces.niemCore)]
    public class MeasureIntegerRange
    {
        public MeasureIntegerRange()
        {
        }

        public MeasureIntegerRange(int max, int min)
        {
            Max = max;
            Min = min;
        }

        [XmlElement("RangeMaximumIntegerValue", Namespace = Namespaces.niemCore, Order = 1)]
        public int Max { get; set; }

        [XmlElement("RangeMinimumIntegerValue", Namespace = Namespaces.niemCore, Order = 2)]
        public int Min { get; set; }
    }
}