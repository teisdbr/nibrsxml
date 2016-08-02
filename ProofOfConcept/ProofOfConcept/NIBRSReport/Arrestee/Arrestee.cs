using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.NibrsReport.Person;

namespace NibrsXml.NibrsReport.Arrestee
{
    [XmlRoot("Arrestee", Namespace = Namespaces.justice)]
    public class Arrestee 
    {
        [XmlIgnore]
        public Person.Person person { get; set; }

        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string id { get; set; }
        
        [XmlElement("RoleOfPerson", Namespace = Namespaces.niemCore, Order = 1)]
        public RoleOfPerson role { get; set; }
        
        [XmlElement("ArrestSequenceID", Namespace = Namespaces.justice, Order = 2)]
        public string seqId { get; set; }

        [XmlElement("ArresteeClearanceIndicator", Namespace = Namespaces.justice, Order = 3)]
        public string clearanceIndicator { get; set; }

        [XmlElement("ArresteeArmedWithCode", Namespace = Namespaces.justice, Order = 4)]
        public string armedWithCode { get; set; }

        [XmlElement("ArresteeJuvenileDispositionCode", Namespace = Namespaces.justice, Order = 5)]
        public string juvenileDispositionCode { get; set; }

        public Arrestee() { }

        public Arrestee(
            Person.Person person,
            int seqId,
            bool clearanceIndicator,
            string armedWithCode,
            string juvenileDispositionCode)
        {
            this.person = person;
            this.person.id = "PersonArrestee" + seqId.ToString();
            this.id = "Arrestee" + seqId.ToString();
            this.role = new RoleOfPerson(this.person.id);
            this.seqId = seqId.ToString();
            this.clearanceIndicator = clearanceIndicator.ToString().ToLower();
            this.armedWithCode = armedWithCode;
            this.juvenileDispositionCode = juvenileDispositionCode;
        }
    }
}
