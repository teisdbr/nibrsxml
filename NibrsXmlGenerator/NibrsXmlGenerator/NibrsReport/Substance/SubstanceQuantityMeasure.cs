using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Substance
{
    [XmlRoot("SubstanceQuantityMeasure", Namespace = Namespaces.niemCore)]
    public class SubstanceQuantityMeasure
    {
        [XmlElement("MeasureDecimalValue", Namespace = Namespaces.niemCore)]
        public string DecimalValue { get; set; }

        [XmlElement("SubstanceUnitCode", Namespace = Namespaces.justice)]
        public string SubstanceUnitCode { get; set; }

        public SubstanceQuantityMeasure() { }

        public SubstanceQuantityMeasure(string decimalValue, string substanceUnitCode)
        {
            this.DecimalValue = decimalValue;
            this.SubstanceUnitCode = substanceUnitCode;
        }
    }
}
