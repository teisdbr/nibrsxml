using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Arrest;
using NibrsXml.NibrsReport.Arrestee;
using NibrsXml.NibrsReport.Association;
using NibrsXml.NibrsReport.EnforcementOfficial;
using NibrsXml.NibrsReport.Incident;
using NibrsXml.NibrsReport.Item;
using NibrsXml.NibrsReport.Location;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.NibrsReport.Person;
using NibrsXml.NibrsReport.ReportHeader;
using NibrsXml.NibrsReport.Subject;
using NibrsXml.NibrsReport.Substance;
using NibrsXml.NibrsReport.Victim;

namespace NibrsXml.NibrsReport
{
    [XmlRoot("Report", Namespace = Namespaces.cjisNibrs)]
    public class Report : NibrsSerializable
    {
        [XmlElement("ReportHeader", Namespace = Namespaces.cjisNibrs, Order = 1)]
        public ReportHeader.ReportHeader header { get; set; }

        [XmlElement("Incident", Namespace = Namespaces.niemCore, Order = 2)]
        public Incident.Incident incident { get; set; }

        [XmlElement("Offense", Namespace = Namespaces.justice, Order = 3)]
        public List<Offense.Offense> offenses = new List<Offense.Offense>();

        [XmlElement("Location", Namespace = Namespaces.niemCore, Order = 4)]
        public List<Location.Location> locations { get; set; }

        [XmlElement("Item", Namespace = Namespaces.niemCore, Order = 5)]
        public List<Item.Item> items { get; set; }

        [XmlElement("Substance", Namespace = Namespaces.niemCore, Order = 6)]
        public List<Substance.Substance> substances { get; set; }

        [XmlElement("Person", Namespace = Namespaces.niemCore, Order = 7)]
        public List<Person.Person> persons;

        [XmlElement("EnforcementOfficial", Namespace = Namespaces.justice, Order = 8)]
        public List<Person.Person> officers { get; set; }

        [XmlElement("Victim", Namespace = Namespaces.justice, Order = 9)]
        public List<Person.Person> victims { get; set; }

        [XmlElement("Subject", Namespace = Namespaces.justice, Order = 10)]
        public List<Person.Person> subjects { get; set; }

        [XmlElement("Arrestee", Namespace = Namespaces.justice, Order = 11)]
        public List<Person.Person> arrestees { get; set; }

        [XmlElement("Arrest", Namespace = Namespaces.justice, Order = 12)]
        public List<Arrest.Arrest> arrests { get; set; }

        [XmlElement("ArrestSubjectAssociation", Namespace = Namespaces.justice, Order = 13)]
        public List<Association.Association> arrestSubjectAssocs { get; set; }

        [XmlElement("OffenseLocationAssociation", Namespace = Namespaces.justice, Order = 14)]
        public List<Association.Association> offenseLocationAssocs { get; set; }

        [XmlElement("OffenseVictimAssociation", Namespace = Namespaces.justice, Order = 15)]
        public List<Association.Association> offenseVictimAssocs { get; set; }

        [XmlElement("SubjectVictimAssociation", Namespace = Namespaces.justice, Order = 16)]
        public List<Association.Association> subjectVictimAssocs { get; set; }

        [XmlIgnore]
        public string xml { get { return NibrsSerializer.NibrsSerializer.SerializeReport(this); } }

        public Report() { }

        public Report(
            ReportHeader.ReportHeader header,
            Incident.Incident incident)
        {
            this.header = header;
            this.incident = incident;
        }

        public void AddOffenses(params Offense.Offense[] offenses)
        {
            foreach (Offense.Offense offense in offenses)
                this.offenses.Add(offense);
        }

        public void AddLocations(params Location.Location[] locations)
        {
            foreach (Location.Location location in locations)
                this.locations.Add(location);
        }

        public void AddItems(params Item.Item[] items)
        {
            foreach (Item.Item item in items)
                this.items.Add(item);
        }

        public void AddSubstances(params Substance.Substance[] substances)
        {
            foreach (Substance.Substance substance in substances)
                this.substances.Add(substance);
        }

        public void AddEnforcementOfficials(params EnforcementOfficial.EnforcementOfficial[] officers)
        {
            foreach (EnforcementOfficial.EnforcementOfficial officer in officers)
            {
                this.officers.Add(officer);
                this.persons.Add(officer);
            }
        }

        public void AddVictims(params Victim.Victim[] victims)
        {
            foreach (Victim.Victim victim in victims)
            {
                this.victims.Add(victim);
                this.persons.Add(victim);
            }
        }

        public void AddSubjects(params Subject.Subject[] victims)
        {
            foreach (Subject.Subject subject in subjects)
            {
                this.subjects.Add(subject);
                this.persons.Add(subject);
            }
        }

        public void AddArrestees(params Arrestee.Arrestee[] arrestees)
        {
            foreach (Arrestee.Arrestee arrestee in arrestees)
            {
                this.arrestees.Add(arrestee);
                this.persons.Add(arrestee);
            }
        }

        public void AddArrests(params Arrest.Arrest[] arrests)
        {
            foreach (Arrest.Arrest arrest in arrests)
                this.arrests.Add(arrest);
        }

        public void AddArrestSubjectAssociations(params Association.ArrestSubjectAssociation[] associations)
        {
            foreach (Association.ArrestSubjectAssociation association in associations)
                this.arrestSubjectAssocs.Add(association);
        }

        public void AddOffenseLocationAssociations(params Association.OffenseLocationAssociation[] associations)
        {
            foreach (Association.OffenseLocationAssociation association in associations)
                this.offenseLocationAssocs.Add(association);
        }

        public void AddOffenseVictimAssociations(params Association.OffenseVictimAssociation[] associations)
        {
            foreach (Association.OffenseVictimAssociation association in associations)
                this.offenseVictimAssocs.Add(association);
        }

        public void AddSubjectVictimAssociations(params Association.SubjectVictimAssociation[] associations)
        {
            foreach (Association.SubjectVictimAssociation association in associations)
                this.subjectVictimAssocs.Add(association);
        }

        
    }
}
