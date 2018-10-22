using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Substance
{
    [XmlRoot("Substance", Namespace = Namespaces.niemCore)]
    public class Substance : Item.Item
    {
        public Substance()
        {
        }

        public Substance(string drugCategoryCode, SubstanceQuantityMeasure quantityMeasure)
        {
            DrugCategoryCode = drugCategoryCode;
            QuantityMeasure = quantityMeasure;
        }

        public Substance(string drugCategoryCode, string measureDecimalValue, string substanceUnitCode,
            string statusCode, string valueAmount, string valueDate, string nibrsPropertyCategoryCode, string quantity)
            : base(statusCode, valueAmount, valueDate, nibrsPropertyCategoryCode, quantity)
        {
            DrugCategoryCode = drugCategoryCode;
            QuantityMeasure = new SubstanceQuantityMeasure(measureDecimalValue, substanceUnitCode);
        }

        [XmlElement("DrugCategoryCode", Namespace = Namespaces.justice, Order = 1)]
        public string DrugCategoryCode { get; set; }

        [XmlElement("SubstanceQuantityMeasure", Namespace = Namespaces.niemCore, Order = 2)]
        public SubstanceQuantityMeasure QuantityMeasure { get; set; }
    }
}