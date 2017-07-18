using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Person
{
    [XmlRoot("PersonAugmentation", Namespace = Namespaces.justice)]
    public class PersonAugmentation
    {
        [XmlElement("PersonAgeCode", Namespace = Namespaces.cjisNibrs)]
        public string AgeCode { get; set; }

        public PersonAugmentation() { }

        public PersonAugmentation(string ageCode)
        {
            this.AgeCode = ageCode;
        }
    }
}
