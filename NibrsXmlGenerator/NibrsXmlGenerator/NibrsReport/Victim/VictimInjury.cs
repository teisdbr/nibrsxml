using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Victim
{
    [XmlRoot("VictimInjury", Namespace = Namespaces.niemCore)]
    public class VictimInjury
    {
        [XmlElement("InjuryCategoryCode", Namespace = Namespaces.justice)]
        public string CategoryCode { get; set; }

        public VictimInjury() { }

        public VictimInjury(string categoryCode)
        {
            this.CategoryCode = categoryCode;
        }
    }
}
