using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Person
{
    [XmlRoot("PersonInjury", Namespace = Namespaces.niemCore)]
    public class PersonInjury
    {
        [XmlElement("InjuryCategoryCode", Namespace = Namespaces.justice)]
        public string categoryCode { get; set; }

        public PersonInjury() { }

        public PersonInjury(string categoryCode)
        {
            this.categoryCode = categoryCode;
        }
    }
}
