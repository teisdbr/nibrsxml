using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Substance
{
    [XmlRoot("Substance", Namespace = Namespaces.niemCore)]
    public class Substance
    {
        [XmlElement("DrugCategoryCode", Namespace = Namespaces.justice, Order = 1)]
        public string drugCategoryCode { get; set; }

        [XmlElement("SubstanceQuantityMeasure", Namespace = Namespaces.niemCore, Order = 2)]
	    public SubstanceQuantityMeasure quantityMeasure { get; set; }

        public Substance() { }

        public Substance(string drugCategoryCode, SubstanceQuantityMeasure quantityMeasure)
        {
            this.drugCategoryCode = drugCategoryCode;
            this.quantityMeasure = quantityMeasure;
        }
    }
}
