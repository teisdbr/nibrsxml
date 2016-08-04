using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Person
{
    /// <summary>
    /// This class contains mechanisms that make its range and value properties mutually exclusive
    /// so that only one can get serialized at a time
    /// </summary>
    [XmlRoot("PersonAgeMeasure", Namespace = Namespaces.niemCore)]
    public class PersonAgeMeasure
    {
        [XmlElement("MeasureIntegerRange", Namespace = Namespaces.niemCore)]
        public MeasureIntegerRange Range { get; set; }

        [XmlElement("MeasureIntegerValue", Namespace = Namespaces.niemCore)]
        public string Value { get; set; }

        public PersonAgeMeasure() { }

        public PersonAgeMeasure(int value)
        {
            this.Value = value.ToString();
        }

        public PersonAgeMeasure(int max, int min)
        {
            this.Range = new MeasureIntegerRange(max, min);
        }
    }
}
