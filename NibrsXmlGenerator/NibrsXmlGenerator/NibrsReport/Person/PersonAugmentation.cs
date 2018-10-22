using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Person
{
    [XmlRoot("PersonAugmentation", Namespace = Namespaces.justice)]
    public class PersonAugmentation
    {
        public PersonAugmentation()
        {
        }

        public PersonAugmentation(string ageCode)
        {
            AgeCode = ageCode;
        }

        [XmlElement("PersonAgeCode", Namespace = Namespaces.cjisNibrs)]
        public string AgeCode { get; set; }
    }
}