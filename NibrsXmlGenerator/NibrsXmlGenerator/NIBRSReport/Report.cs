using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;
using System.Text.RegularExpressions;

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
        public List<Offense.Offense> offenses { get; set; }

        [XmlElement("Location", Namespace = Namespaces.niemCore, Order = 4)]
        public List<Location.Location> locations { get; set; }

        [XmlElement("Item", Namespace = Namespaces.niemCore, Order = 5)]
        public List<Item.Item> items { get; set; }

        [XmlElement("Substance", Namespace = Namespaces.niemCore, Order = 6)]
        public List<Substance.Substance> substances { get; set; }

        [XmlElement("Person", Namespace = Namespaces.niemCore, Order = 7)]
        public List<Person.Person> persons;

        [XmlElement("EnforcementOfficial", Namespace = Namespaces.justice, Order = 8)]
        public List<EnforcementOfficial.EnforcementOfficial> officers { get; set; }

        [XmlElement("Victim", Namespace = Namespaces.justice, Order = 9)]
        public List<Victim.Victim> victims { get; set; }

        [XmlElement("Subject", Namespace = Namespaces.justice, Order = 10)]
        public List<Subject.Subject> subjects { get; set; }

        [XmlElement("Arrestee", Namespace = Namespaces.justice, Order = 11)]
        public List<Arrestee.Arrestee> arrestees { get; set; }

        [XmlElement("Arrest", Namespace = Namespaces.justice, Order = 12)]
        public List<Arrest.Arrest> arrests { get; set; }

        [XmlElement("ArrestSubjectAssociation", Namespace = Namespaces.justice, Order = 13)]
        public List<Associations.ArrestSubjectAssociation> arrestSubjectAssocs { get; set; }

        [XmlElement("OffenseLocationAssociation", Namespace = Namespaces.justice, Order = 14)]
        public List<Associations.OffenseLocationAssociation> offenseLocationAssocs { get; set; }

        [XmlElement("OffenseVictimAssociation", Namespace = Namespaces.justice, Order = 15)]
        public List<Associations.OffenseVictimAssociation> offenseVictimAssocs { get; set; }

        [XmlElement("SubjectVictimAssociation", Namespace = Namespaces.justice, Order = 16)]
        public List<Associations.SubjectVictimAssociation> subjectVictimAssocs { get; set; }

        [XmlIgnore]
        private static NibrsSerializer.NibrsSerializer serializer = new NibrsSerializer.NibrsSerializer(typeof(Report));

        [XmlIgnore]
        public string xml { get { return Regex.Replace(serializer.Serialize(this), ".*\\n<nibrs:Report [\\w\\s\"/\\.:=\\-\\d_]+\">", "<nibrs:Report>"); } }

        public Report() { }

        public Report(
            ReportHeader.ReportHeader header,
            Incident.Incident incident)
        {
            this.header = header;
            this.incident = incident;
            offenses = new List<Offense.Offense>();
            locations = new List<Location.Location>();
            items = new List<Item.Item>();
            substances = new List<Substance.Substance>();
            persons = new List<Person.Person>();
            officers = new List<EnforcementOfficial.EnforcementOfficial>();
            victims = new List<Victim.Victim>();
            subjects = new List<Subject.Subject>();
            arrestees = new List<Arrestee.Arrestee>();
            arrests = new List<Arrest.Arrest>();
            arrestSubjectAssocs = new List<Associations.ArrestSubjectAssociation>();
            offenseLocationAssocs = new List<Associations.OffenseLocationAssociation>();
            offenseVictimAssocs = new List<Associations.OffenseVictimAssociation>();
            subjectVictimAssocs = new List<Associations.SubjectVictimAssociation>();
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
            }
        }

        public void AddVictims(params Victim.Victim[] victims)
        {
            foreach (Victim.Victim victim in victims)
            {
                this.victims.Add(victim);
                this.persons.Add(victim.person);
            }
        }

        public void AddSubjects(params Subject.Subject[] subjects)
        {
            foreach (Subject.Subject subject in subjects)
            {
                this.subjects.Add(subject);
                this.persons.Add(subject.person);
            }
        }

        public void AddArrestees(params Arrestee.Arrestee[] arrestees)
        {
            foreach (Arrestee.Arrestee arrestee in arrestees)
            {
                this.arrestees.Add(arrestee);
                this.persons.Add(arrestee.person);
            }
        }

        public void AddArrests(params Arrest.Arrest[] arrests)
        {
            foreach (Arrest.Arrest arrest in arrests)
                this.arrests.Add(arrest);
        }

        public void AddArrestSubjectAssociations(params Associations.ArrestSubjectAssociation[] associations)
        {
            foreach (Associations.ArrestSubjectAssociation association in associations)
                this.arrestSubjectAssocs.Add(association);
        }

        public void AddOffenseLocationAssociations(params Associations.OffenseLocationAssociation[] associations)
        {
            foreach (Associations.OffenseLocationAssociation association in associations)
                this.offenseLocationAssocs.Add(association);
        }

        public void AddOffenseVictimAssociations(params Associations.OffenseVictimAssociation[] associations)
        {
            foreach (Associations.OffenseVictimAssociation association in associations)
                this.offenseVictimAssocs.Add(association);
        }

        public void AddSubjectVictimAssociations(params Associations.SubjectVictimAssociation[] associations)
        {
            foreach (Associations.SubjectVictimAssociation association in associations)
                this.subjectVictimAssocs.Add(association);
        }
    }
}
