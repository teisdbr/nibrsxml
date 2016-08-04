using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Substance
{
    [XmlRoot("SubstanceQuantityMeasure", Namespace = Namespaces.niemCore)]
    public class SubstanceQuantityMeasure
    {
        [XmlElement("MeasureDecimalValue", Namespace = Namespaces.niemCore)]
        public double DecimalValue { get; set; }

        [XmlElement("SubstanceUnitCode", Namespace = Namespaces.justice)]
        public string SubstanceUnitCode { get; set; }

        public SubstanceQuantityMeasure() { }

        public SubstanceQuantityMeasure(double decimalValue, string substanceUnitCode)
        {
            this.DecimalValue = decimalValue;
            this.SubstanceUnitCode = substanceUnitCode;
        }
    }
}
