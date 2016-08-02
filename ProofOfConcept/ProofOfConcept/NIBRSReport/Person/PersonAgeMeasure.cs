using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Person
{
    [XmlRoot("PersonAgeMeasure", Namespace = Namespaces.niemCore)]
    public class PersonAgeMeasure
    {
        [XmlElement("MeasureIntegerRange", Namespace = Namespaces.niemCore)]
        public MeasureIntegerRange range;

        [XmlElement("MeasureIntegerValue", Namespace = Namespaces.niemCore)]
        public int value { get; set; }

        public PersonAgeMeasure() { }

        public PersonAgeMeasure(int value)
        {
            this.value = value;
        }

        public PersonAgeMeasure(int max, int min)
        {
            this.range = new MeasureIntegerRange(max, min);
        }
    }
}
