using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Offense
{
    [XmlRoot("OffenseForce", Namespace = Namespaces.justice)]
    public class OffenseForce
    {
        [XmlElement("ForceCategoryCode", Namespace = Namespaces.justice)]
        public string CategoryCode { get; set; }
        
        public OffenseForce() { }

        public OffenseForce(string categoryCode)
        {
            this.CategoryCode = categoryCode;
        }
    }
}
