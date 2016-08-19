using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.NibrsReport.Person;
using NibrsXml.Utility;

namespace NibrsXml.NibrsReport.Arrestee
{
    [XmlRoot("Arrestee", Namespace = Namespaces.justice)]
    public class Arrestee
    {
        [XmlIgnore]
        public Person.Person Person { get; set; }

        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string Id { get; set; }

        /// <summary>
        /// This property is public only For serialization.
        /// It should only be set by using the Arrestee(string) constructor and accessed using the reference property.
        /// </summary>
        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string ArresteeRef { get; set; }

        [XmlElement("RoleOfPerson", Namespace = Namespaces.niemCore, Order = 1)]
        public RoleOfPerson Role { get; set; }

        [XmlElement("ArrestSequenceID", Namespace = Namespaces.justice, Order = 2)]
        public string SeqId { get; set; }

        [XmlElement("ArresteeClearanceIndicator", Namespace = Namespaces.justice, Order = 3)]
        public string ClearanceIndicator { get; set; }

        [XmlElement("ArresteeArmedWithCode", Namespace = Namespaces.justice, Order = 4)]
        public string ArmedWithCode { get; set; }

        [XmlElement("ArresteeJuvenileDispositionCode", Namespace = Namespaces.justice, Order = 5)]
        public string JuvenileDispositionCode { get; set; }

        public Arrestee Reference { get { return new Arrestee(Person.Id); } }

        public Arrestee() { }

        public Arrestee(string arresteeId)
        {
            this.ArresteeRef = arresteeId;
        }

        public Arrestee(
            Person.Person person,
            string seqId,
            string clearanceIndicator,
            string armedWithCode,
            string juvenileDispositionCode)
        {
            this.Person = person;
            this.Id = this.Person.Id; //Since person should already contain a unique prefix by now, we can reuse it here for the arrestee id
            this.Person.Id += "PersonArrestee" + seqId.ToString();
            this.Id += "Arrestee" + seqId.ToString();
            this.Role = new RoleOfPerson(this.Person.Id);
            this.SeqId = seqId;
            this.ClearanceIndicator = clearanceIndicator.ToLower().TrimNullIfEmpty();
            this.ArmedWithCode = armedWithCode.TrimNullIfEmpty();
            this.JuvenileDispositionCode = juvenileDispositionCode;
        }
    }
}
