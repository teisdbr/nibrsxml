﻿using System.Collections.Generic;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;
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
        public string PersonRef { get; set; }

        [XmlElement("RoleOfPerson", Namespace = Namespaces.niemCore, Order = 1)]
        public RoleOfPerson Role { get; set; }

        [XmlElement("ArrestSequenceID", Namespace = Namespaces.justice, Order = 2)]
        public string SeqId { get; set; }

        [XmlElement("ArresteeClearanceIndicator", Namespace = Namespaces.justice, Order = 3)]
        public string ClearanceIndicator { get; set; }

        [XmlElement("ArresteeArmedWithCode", Namespace = Namespaces.justice, Order = 4)]
        public List<string> ArmedWithCodes { get; set; }

        [XmlElement("ArresteeJuvenileDispositionCode", Namespace = Namespaces.justice, Order = 5)]
        public string JuvenileDispositionCode { get; set; }

        [XmlElement("ArrestSubjectCountCode", Namespace = Namespaces.justice, Order = 6)]
        public string SubjectCountCode { get; set; }

        public Arrestee Reference
        {
            get
            {
                return new Arrestee(Person.Id);
            }
        }
        public Arrestee() { }

        public Arrestee(string arresteeId)
        {
            this.PersonRef = arresteeId;
        }

        public Arrestee(
            Person.Person person,
            string seqId,
            string clearanceIndicator,
            List<string> armedWithCode,
            string juvenileDispositionCode,
            string subjectCountCode,
            string uniquePerfix)
        {
            this.Person = person;
            this.Id = uniquePerfix + "Arrestee" + seqId.TrimStart('0'); 
            //this.Person.Id += "PersonArrestee" + seqId.TrimStart('0');
            this.Role = new RoleOfPerson(this.Person.Id);
            this.SeqId = seqId.TrimStart('0');
            this.ClearanceIndicator = clearanceIndicator.ToLower().TrimNullIfEmpty();
            this.ArmedWithCodes = armedWithCode;
            this.JuvenileDispositionCode = juvenileDispositionCode;
            this.SubjectCountCode = subjectCountCode;
        }
    }
}
