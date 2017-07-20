using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Substance
{
    [XmlRoot("Substance", Namespace = Namespaces.niemCore)]
    public class Substance
    {
        [XmlElement("DrugCategoryCode", Namespace = Namespaces.justice, Order = 1)]
        public string DrugCategoryCode { get; set; }

        [XmlElement("SubstanceQuantityMeasure", Namespace = Namespaces.niemCore, Order = 2)]
        public SubstanceQuantityMeasure QuantityMeasure { get; set; }

        public Substance() { }

        public Substance(string drugCategoryCode, SubstanceQuantityMeasure quantityMeasure)
        {
            this.DrugCategoryCode = drugCategoryCode;
            this.QuantityMeasure = quantityMeasure;
        }
        public Substance(string drugCategoryCode, string measureDecimalValue, string substanceUnitCode)
        {
            this.DrugCategoryCode = drugCategoryCode;
            this.QuantityMeasure = new SubstanceQuantityMeasure(decimalValue: measureDecimalValue, substanceUnitCode: substanceUnitCode);
        }
    }
}
