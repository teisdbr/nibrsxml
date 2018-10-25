using System.Xml.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Person
{
    /// <summary>
    ///     This class contains mechanisms that make its range and value properties mutually exclusive
    ///     so that only one can get serialized at a time
    /// </summary>
    [XmlRoot("PersonAgeMeasure", Namespace = Namespaces.niemCore)]
    public class PersonAgeMeasure
    {
        public PersonAgeMeasure()
        {
        }

        public PersonAgeMeasure(int value)
        {
            Value = value.ToString();
        }

        public PersonAgeMeasure(int max, int min)
        {
            Range = new MeasureIntegerRange(max, min);
        }

        [XmlElement("MeasureIntegerRange", Namespace = Namespaces.niemCore)]
        public MeasureIntegerRange Range { get; set; }

        [XmlElement("MeasureValueText", Namespace = Namespaces.niemCore)]
        public string ValueText { get; set; }
        
        [XmlElement("MeasureIntegerValue", Namespace = Namespaces.niemCore)]
        public string Value { get; set; }

        [BsonIgnore]
        [XmlIgnore]
        public string RangeOrValue
        {
            get { return Value ?? Range.Min + "-" + Range.Max; }
        }

        [BsonIgnore]
        [XmlIgnore]
        public bool IsJuvenile
        {
            get
            {
                //Verify there is a value, not a range and if so indicate whether individual is juvenile or not.
                int ageValue;
                if (int.TryParse(Value, out ageValue)) return ageValue < 18;
                return Range.Max >= 1 && Range.Max < 18;
            }
        }

        public PersonAgeMeasure(string textvalue)
        {
            this.ValueText = textvalue;
        }
    }
}