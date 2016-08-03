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
        private const int nil = -1;
        private MeasureIntegerRange _range;
        private int _value = nil;
        
        [XmlElement("MeasureIntegerRange", Namespace = Namespaces.niemCore)]
        public MeasureIntegerRange range
        {
            get
            {
                return _range;
            }
            set
            {
                if (_value != nil)
                    _value = nil;
                _range = value;
            }
        }

        [XmlElement("MeasureIntegerValue", Namespace = Namespaces.niemCore)]
        public int value 
        { 
            get
            {
                return _value;
            }
            set
            {
                if (_range != null)
                    _range = null;
                _value = value;
            }
        }

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
