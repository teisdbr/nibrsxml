using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Person
{
    [XmlRoot("PersonAgeMeasure", Namespace = Namespaces.niemCore)]
    public class PersonAgeMeasureRange : PersonAgeMeasure
    {
        [XmlElement("MeasureIntegerRange", Namespace = Namespaces.niemCore)]
        public MeasureIntegerRange range { get; set; }

        public PersonAgeMeasureRange() { }

        public PersonAgeMeasureRange(MeasureIntegerRange range)
        {
            this.range = range;
        }
    }
}
