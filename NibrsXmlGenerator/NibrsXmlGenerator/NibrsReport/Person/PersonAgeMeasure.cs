﻿using NibrsXml.Constants;
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

        [XmlIgnore]
        public string RangeOrValue
        {
            get
            {
                return Value ?? Range.Min + "-" + Range.Max;
            }
        }

        [XmlIgnore]
        public bool IsJuvenile
        {
            get
            {
                //Verify there is a value, not a range and if so indicate whether individual is juvenile or not.
                int ageValue;
                if (int.TryParse(this.Value, out ageValue)){
                    return ageValue < 18;
                }
                return this.Range.Max >= 1 && this.Range.Max < 18;
            }
        }

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