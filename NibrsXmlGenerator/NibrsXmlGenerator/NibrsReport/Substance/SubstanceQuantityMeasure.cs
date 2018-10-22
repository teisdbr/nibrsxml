using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Substance
{
    [XmlRoot("SubstanceQuantityMeasure", Namespace = Namespaces.niemCore)]
    public class SubstanceQuantityMeasure
    {
        public SubstanceQuantityMeasure()
        {
        }

        public SubstanceQuantityMeasure(string decimalValue, string substanceUnitCode)
        {
            DecimalValue = decimalValue;
            SubstanceUnitCode = substanceUnitCode;
        }

        [XmlElement("MeasureDecimalValue", Namespace = Namespaces.niemCore)]
        public string DecimalValue { get; set; }

        [XmlElement("SubstanceUnitCode", Namespace = Namespaces.justice)]
        public string SubstanceUnitCode { get; set; }
    }
}