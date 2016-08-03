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
        //private MeasureIntegerRange _range;
        //private int _value;
        
        //[XmlElement("MeasureIntegerRange", Namespace = Namespaces.niemCore)]
        //public MeasureIntegerRange range
        //{
        //    get
        //    {
        //        return _range;
        //    }
        //    set
        //    {
        //        if (_value != null)
        //            _value = null;
        //        _range = value;
        //    }
        //}

        //[XmlElement("MeasureIntegerValue", Namespace = Namespaces.niemCore)]
        //public int value 
        //{ 
        //    get
        //    {
        //        return _value;
        //    }
        //    set
        //    {
        //        if (_range != null)
        //            _range = null;
        //        _value = value;
        //    }
        //}

        [XmlElement("MeasureIntegerRange", Namespace = Namespaces.niemCore)]
        public MeasureIntegerRange range { get; set; }

        [XmlElement("MeasureIntegerValue", Namespace = Namespaces.niemCore)]
        public string value { get; set; }

        public PersonAgeMeasure() { }

        public PersonAgeMeasure(int value)
        {
            this.value = value.ToString();
        }

        public PersonAgeMeasure(int max, int min)
        {
            this.range = new MeasureIntegerRange(max, min);
        }
    }
}
