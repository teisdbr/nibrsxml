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
    public class PersonAgeMeasureValue : PersonAgeMeasure
    {
        [XmlElement("MeasureIntegerValue", Namespace = Namespaces.niemCore)]
        public string value { get; set; }

        public PersonAgeMeasureValue() { }

        public PersonAgeMeasureValue(int value)
        {
            this.value = value.ToString();
        }
    }
}
