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
    public abstract class PersonAgeMeasure
    {
        [XmlElement("MeasureIntegerValue", Namespace = Namespaces.niemCore)]
		public string value { get; set; }

        public PersonAgeMeasure() { }

        public PersonAgeMeasure(int value)
        {
            this.value = value.ToString();
        }
    }
}
