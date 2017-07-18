using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NibrsXml.Constants;
using System.Text.RegularExpressions;
using NibrsXml.Utility;
using TeUtil.Extensions;

namespace NibrsXml.NibrsReport
{
    [XmlRoot("Report", Namespace = Namespaces.cjisNibrs)]
    public class Report : NibrsSerializable
    {
        [XmlElement("ReportHeader", Namespace = Namespaces.cjisNibrs, Order = 1)]
        public ReportHeader.ReportHeader Header { get; set; }

        [XmlElement("Incident", Namespace = Namespaces.niemCore, Order = 2)]
        public Incident.Incident Incident { get; set; }

        [XmlElement("Offense", Namespace = Namespaces.justice, Order = 3)]
        public List<Offense.Offense> Offenses { get; set; }

        [XmlElement("Location", Namespace = Namespaces.niemCore, Order = 4)]
        public List<Location.Location> Locations { get; set; }

        [XmlElement("Item", Namespace = Namespaces.niemCore, Order = 5)]
        public List<Item.Item> Items { get; set; }

        [XmlElement("Substance", Namespace = Namespaces.niemCore, Order = 6)]
        public List<Substance.Substance> Substances { get; set; }

        [XmlElement("Person", Namespace = Namespaces.niemCore, Order = 7)]
        public List<Person.Person> Persons;

        [XmlElement("EnforcementOfficial", Namespace = Namespaces.justice, Order = 8)]
        public List<EnforcementOfficial.EnforcementOfficial> Officers { get; set; }

        [XmlElement("Victim", Namespace = Namespaces.justice, Order = 9)]
        public List<Victim.Victim> Victims { get; set; }

        [XmlElement("Subject", Namespace = Namespaces.justice, Order = 10)]
        public List<Subject.Subject> Subjects { get; set; }

        [XmlElement("Arrestee", Namespace = Namespaces.justice, Order = 11)]
        public List<Arrestee.Arrestee> Arrestees { get; set; }

        [XmlElement("Arrest", Namespace = Namespaces.justice, Order = 12)]
        public List<Arrest.Arrest> Arrests { get; set; }

        [XmlElement("ArrestSubjectAssociation", Namespace = Namespaces.justice, Order = 13)]
        public List<Associations.ArrestSubjectAssociation> ArrestSubjectAssocs { get; set; }

        [XmlElement("OffenseLocationAssociation", Namespace = Namespaces.justice, Order = 14)]
        public List<Associations.OffenseLocationAssociation> OffenseLocationAssocs { get; set; }

        [XmlElement("OffenseVictimAssociation", Namespace = Namespaces.justice, Order = 15)]
        public List<Associations.OffenseVictimAssociation> OffenseVictimAssocs { get; set; }

        [XmlElement("SubjectVictimAssociation", Namespace = Namespaces.justice, Order = 16)]
        public List<Associations.SubjectVictimAssociation> SubjectVictimAssocs { get; set; }

        [XmlIgnore]
        private static NibrsSerializer.NibrsSerializer serializer = new NibrsSerializer.NibrsSerializer(typeof(Report));

        [XmlIgnore]
        public List<Item.Item> StolenVehicles => Items.Where(i => i.NibrsPropertyCategoryCode.MatchOne(NibrsCodeGroups.VehicleProperties) && i.Status.Code == ItemStatusCode.STOLEN.NibrsCode()).ToList();

        [XmlIgnore]
        public string Xml => Regex.Replace(serializer.Serialize(this), ".*\\n<nibrs:Report [\\w\\s\"/\\.:=\\-\\d_]+\">", "<nibrs:Report>");
        public Report() { 
            //Initialize Locations
            this.Locations = new List<Location.Location>();

            //Initialize Location Associations
            this.OffenseLocationAssocs = new List<Associations.OffenseLocationAssociation>();

            //Initialize Items
            this.Items = new List<Item.Item>();
            
            //Initialize Substances
            this.Substances = new List<Substance.Substance>();

            //Initialize Persons, Victims, EnforcementOfficial, Subjects, Arrestees, Arrests, ArrestSubjectAssocs ,OffenseVictimAssocs, SubjectVictimAssocs
            this.Persons = new List<Person.Person>();
            this.Victims = new List<Victim.Victim>();
            this.Officers = new List<EnforcementOfficial.EnforcementOfficial>();
            this.Subjects = new List<Subject.Subject>();
            this.Arrestees = new List<Arrestee.Arrestee>();
            this.Arrests = new List<Arrest.Arrest>();
            this.ArrestSubjectAssocs = new List<Associations.ArrestSubjectAssociation>();
            this.OffenseVictimAssocs = new List<Associations.OffenseVictimAssociation>();
            this.SubjectVictimAssocs = new List<Associations.SubjectVictimAssociation>();

        }

        public Report(
            ReportHeader.ReportHeader header,
            Incident.Incident incident)
        {
            this.Header = header;
            this.Incident = incident;
            this.Offenses = new List<Offense.Offense>();
            this.Locations = new List<Location.Location>();
            this.Items = new List<Item.Item>();
            this.Substances = new List<Substance.Substance>();
            this.Persons = new List<Person.Person>();
            this.Officers = new List<EnforcementOfficial.EnforcementOfficial>();
            this.Victims = new List<Victim.Victim>();
            this.Subjects = new List<Subject.Subject>();
            this.Arrestees = new List<Arrestee.Arrestee>();
            this.Arrests = new List<Arrest.Arrest>();
            this.ArrestSubjectAssocs = new List<Associations.ArrestSubjectAssociation>();
            this.OffenseLocationAssocs = new List<Associations.OffenseLocationAssociation>();
            this.OffenseVictimAssocs = new List<Associations.OffenseVictimAssociation>();
            this.SubjectVictimAssocs = new List<Associations.SubjectVictimAssociation>();
        }

        public void AddOffenses(params Offense.Offense[] offenses)
        {
            foreach (Offense.Offense offense in offenses)
                this.Offenses.Add(offense);
        }

        public void AddLocations(params Location.Location[] locations)
        {
            foreach (Location.Location location in locations)
                this.Locations.Add(location);
        }

        public void AddItems(params Item.Item[] items)
        {
            foreach (Item.Item item in items)
                this.Items.Add(item);
        }

        public void AddSubstances(params Substance.Substance[] substances)
        {
            foreach (Substance.Substance substance in substances)
                this.Substances.Add(substance);
        }

        public void AddEnforcementOfficials(params EnforcementOfficial.EnforcementOfficial[] officers)
        {
            foreach (EnforcementOfficial.EnforcementOfficial officer in officers)
            {
                this.Officers.Add(officer);
            }
        }

        public void AddVictims(params Victim.Victim[] victims)
        {
            foreach (Victim.Victim victim in victims)
            {
                this.Victims.Add(victim);
                this.Persons.Add(victim.Person);
            }
        }

        public void AddSubjects(params Subject.Subject[] subjects)
        {
            foreach (Subject.Subject subject in subjects)
            {
                this.Subjects.Add(subject);
                this.Persons.Add(subject.Person);
            }
        }

        public void AddArrestees(params Arrestee.Arrestee[] arrestees)
        {
            foreach (Arrestee.Arrestee arrestee in arrestees)
            {
                this.Arrestees.Add(arrestee);
                this.Persons.Add(arrestee.Person);
            }
        }

        public void AddArrests(params Arrest.Arrest[] arrests)
        {
            foreach (Arrest.Arrest arrest in arrests)
                this.Arrests.Add(arrest);
        }

        public void AddArrestSubjectAssociations(params Associations.ArrestSubjectAssociation[] associations)
        {
            foreach (Associations.ArrestSubjectAssociation association in associations)
                this.ArrestSubjectAssocs.Add(association);
        }

        public void AddOffenseLocationAssociations(params Associations.OffenseLocationAssociation[] associations)
        {
            foreach (Associations.OffenseLocationAssociation association in associations)
                this.OffenseLocationAssocs.Add(association);
        }

        public void AddOffenseVictimAssociations(params Associations.OffenseVictimAssociation[] associations)
        {
            foreach (Associations.OffenseVictimAssociation association in associations)
                this.OffenseVictimAssocs.Add(association);
        }

        public void AddSubjectVictimAssociations(params Associations.SubjectVictimAssociation[] associations)
        {
            foreach (Associations.SubjectVictimAssociation association in associations)
                this.SubjectVictimAssocs.Add(association);
        }

        /// <summary>
        /// To be called only after deserialization occurs. Because of the nature of deserialization, the full context of
        /// the associations, arrestees, enforcement officials, subjects, and victims is missing. Person objects only
        /// have the person ref and the association objects only have the refs of the objects they associate.
        /// </summary>
        public void RebuildCrossReferencedRelationships()
        {
            //Get all person victims so that these loops/lambdas do not try to match victims that are business, gov't, religious org, etc.
            var personVictims = Victims.Where(v => v.CategoryCode.MatchOne(VictimCategoryCode.INDIVIDUAL.NibrsCode(), VictimCategoryCode.LAW_ENFORCEMENT_OFFICER.NibrsCode())).ToArray();

            //Rebuild persons
            foreach (var arrestee in Arrestees) arrestee.Person = Persons.First(p => p.Id == arrestee.Role.PersonId);
            foreach (var leo in Officers) leo.Person = Persons.First(p => p.Id == leo.Role.PersonId);
            foreach (var subject in Subjects) subject.Person = Persons.First(p => p.Id == subject.Role.PersonId);
            foreach (var victim in personVictims) victim.Person = Persons.First(p => p.Id == victim.Role.PersonId);

            //Rebuild associations
            foreach (var assoc in ArrestSubjectAssocs)
            {
                assoc.RelatedArrest = Arrests.First(a => a.Id == assoc.ActivityRef.ArrestRef);
                assoc.RelatedArrestee = Arrestees.First(a => a.Role.PersonId == assoc.SubjectRef.PersonRef);
            }

            foreach (var assoc in OffenseLocationAssocs)
            {
                assoc.RelatedOffense = Offenses.First(o => o.Id == assoc.OffenseRef.OffenseRef);
                assoc.RelatedLocation = Locations.First(l => l.Id == assoc.LocationRef.LocationRef);
                assoc.RelatedOffense.Location = assoc.RelatedLocation;
            }

            foreach (var assoc in OffenseVictimAssocs)
            {
                assoc.RelatedOffense = Offenses.First(o => o.Id == assoc.OffenseRef.OffenseRef);
                assoc.RelatedVictim = personVictims.First(v => v.Role.PersonId == assoc.VictimRef.VictimRef);
            }

            foreach (var assoc in SubjectVictimAssocs)
            {
                assoc.RelatedSubject = Subjects.First(s => s.Role.PersonId == assoc.SubjectRef.SubjectRef);
                assoc.RelatedVictim = personVictims.First(v => v.Role.PersonId == assoc.VictimRef.VictimRef);
            }
        }
    }
}
