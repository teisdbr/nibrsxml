using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Associations;
using NibrsXml.Utility;
using TeUtil.Extensions;

namespace NibrsXml.NibrsReport
{
    [XmlRoot("Report", Namespace = Namespaces.cjisNibrs)]
    public class Report : INibrsSerializable
    {
        [BsonIgnore] [XmlIgnore] [JsonIgnore] private static readonly NibrsSerializer.NibrsSerializer Serializer =
            new NibrsSerializer.NibrsSerializer(typeof(Report));

        [XmlElement("Person", Namespace = Namespaces.niemCore, Order = 7)]
        public List<Person.Person> Persons;

        public Report()
        {
            //Initialize Locations
            Locations = new List<Location.Location>();

            //Initialize Location Associations
            OffenseLocationAssocs = new List<OffenseLocationAssociation>();

            //Initialize Items
            Items = new List<Item.Item>();

            //Initialize Substances
            Substances = new List<Substance.Substance>();

            //Initialize Persons, Victims, EnforcementOfficial, Subjects, Arrestees, Arrests, ArrestSubjectAssocs ,OffenseVictimAssocs, SubjectVictimAssocs
            Persons = new List<Person.Person>();
            Victims = new List<Victim.Victim>();
            Officers = new List<EnforcementOfficial.EnforcementOfficial>();
            Subjects = new List<Subject.Subject>();
            Arrestees = new List<Arrestee.Arrestee>();
            Arrests = new List<Arrest.Arrest>();
            ArrestSubjectAssocs = new List<ArrestSubjectAssociation>();
            OffenseVictimAssocs = new List<OffenseVictimAssociation>();
            SubjectVictimAssocs = new List<SubjectVictimAssociation>();
        }

        public Report(
            ReportHeader.ReportHeader header,
            Incident.Incident incident)
        {
            Header = header;
            Incident = incident;
            Offenses = new List<Offense.Offense>();
            Locations = new List<Location.Location>();
            Items = new List<Item.Item>();
            Substances = new List<Substance.Substance>();
            Persons = new List<Person.Person>();
            Officers = new List<EnforcementOfficial.EnforcementOfficial>();
            Victims = new List<Victim.Victim>();
            Subjects = new List<Subject.Subject>();
            Arrestees = new List<Arrestee.Arrestee>();
            Arrests = new List<Arrest.Arrest>();
            ArrestSubjectAssocs = new List<ArrestSubjectAssociation>();
            OffenseLocationAssocs = new List<OffenseLocationAssociation>();
            OffenseVictimAssocs = new List<OffenseVictimAssociation>();
            SubjectVictimAssocs = new List<SubjectVictimAssociation>();
        }

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
        public List<ArrestSubjectAssociation> ArrestSubjectAssocs { get; set; }

        [XmlElement("OffenseLocationAssociation", Namespace = Namespaces.justice, Order = 14)]
        public List<OffenseLocationAssociation> OffenseLocationAssocs { get; set; }

        [XmlElement("OffenseVictimAssociation", Namespace = Namespaces.justice, Order = 15)]
        public List<OffenseVictimAssociation> OffenseVictimAssocs { get; set; }

        [XmlElement("SubjectVictimAssociation", Namespace = Namespaces.justice, Order = 16)]
        public List<SubjectVictimAssociation> SubjectVictimAssocs { get; set; }

        [BsonIgnore]
        [XmlIgnore]
        [JsonIgnore]
        public bool HasFailedToBuildProperly { get; set; } = false;

        [BsonIgnore] [XmlIgnore] [JsonIgnore]
        public List<Item.Item> StolenVehicles
        {
            get
            {
                return Items.Where(i =>
                    i.NibrsPropertyCategoryCode.MatchOne(NibrsCodeGroups.VehicleProperties) &&
                    i.Status.Code == ItemStatusCode.STOLEN.NibrsCode()).ToList();
            }
        }

        [BsonIgnore] [XmlIgnore] [JsonIgnore]
        public string Xml
        {
            get
            {
                return Regex.Replace(Serializer.Serialize(this), ".*\\n<nibrs:Report [\\w\\s\"/\\.:=\\-\\d_]+\">",
                    "<nibrs:Report>");
            }
        }

        public void AddOffenses(params Offense.Offense[] offenses)
        {
            foreach (var offense in offenses)
                Offenses.Add(offense);
        }

        public void AddLocations(params Location.Location[] locations)
        {
            foreach (var location in locations)
                Locations.Add(location);
        }

        public void AddItems(params Item.Item[] items)
        {
            foreach (var item in items)
                Items.Add(item);
        }

        public void AddSubstances(params Substance.Substance[] substances)
        {
            foreach (var substance in substances)
                Substances.Add(substance);
        }

        public void AddEnforcementOfficials(params EnforcementOfficial.EnforcementOfficial[] officers)
        {
            foreach (var officer in officers) Officers.Add(officer);
        }

        public void AddVictims(params Victim.Victim[] victims)
        {
            foreach (var victim in victims)
            {
                Victims.Add(victim);
                Persons.Add(victim.Person);
            }
        }

        public void AddSubjects(params Subject.Subject[] subjects)
        {
            foreach (var subject in subjects)
            {
                Subjects.Add(subject);
                Persons.Add(subject.Person);
            }
        }

        public void AddArrestees(params Arrestee.Arrestee[] arrestees)
        {
            foreach (var arrestee in arrestees)
            {
                Arrestees.Add(arrestee);
                Persons.Add(arrestee.Person);
            }
        }

        public void AddArrests(params Arrest.Arrest[] arrests)
        {
            foreach (var arrest in arrests)
                Arrests.Add(arrest);
        }

        public void AddArrestSubjectAssociations(params ArrestSubjectAssociation[] associations)
        {
            foreach (var association in associations)
                ArrestSubjectAssocs.Add(association);
        }

        public void AddOffenseLocationAssociations(params OffenseLocationAssociation[] associations)
        {
            foreach (var association in associations)
                OffenseLocationAssocs.Add(association);
        }

        public void AddOffenseVictimAssociations(params OffenseVictimAssociation[] associations)
        {
            foreach (var association in associations)
                OffenseVictimAssocs.Add(association);
        }

        public void AddSubjectVictimAssociations(params SubjectVictimAssociation[] associations)
        {
            foreach (var association in associations)
                SubjectVictimAssocs.Add(association);
        }

        /// <summary>
        ///     To be called only after deserialization occurs. Because of the nature of deserialization, the full context of
        ///     the associations, arrestees, enforcement officials, subjects, and victims is missing. Person objects only
        ///     have the person ref and the association objects only have the refs of the objects they associate.
        /// </summary>
        public void RebuildCrossReferencedRelationships()
        {
            //Get all person victims so that these loops/lambdas do not try to match victims that are business, gov't, religious org, etc.
            var personVictims = Victims.Where(v => v.CategoryCode.MatchOne(VictimCategoryCode.INDIVIDUAL.NibrsCode(),
                VictimCategoryCode.LAW_ENFORCEMENT_OFFICER.NibrsCode())).ToArray();

            //Rebuild persons
            foreach (var arrestee in Arrestees)
            {
                arrestee.Person = Persons.First(p => p.Id == arrestee.Role.PersonId);
                arrestee.Person.PersonType = "Arrestee";
            }
            foreach (var leo in Officers)
            {
                leo.Person = Persons.First(p => p.Id == leo.Role.PersonId);
                leo.Person.PersonType = "Victim";
            }
            foreach (var subject in Subjects)
            {
                subject.Person = Persons.First(p => p.Id == subject.Role.PersonId);
                subject.Person.PersonType = "Offender";
            }
            foreach (var victim in personVictims)
            {
                victim.Person = Persons.First(p => p.Id == victim.Role.PersonId);
                victim.Person.PersonType = "Victim";
            }

            //Rebuild associations
            foreach (var assoc in ArrestSubjectAssocs)
            {
                assoc.RelatedArrest = Arrests.First(a => a.Id == assoc.ActivityRef.ArrestRef);
                assoc.RelatedArrestee = Arrestees.First(a => a.Id == assoc.SubjectRef.PersonRef);
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
                // Association doesnt make any sense 
                //assoc.RelatedVictim = personVictims.FirstOrDefault(v => v.Role.PersonId == assoc.VictimRef.VictimRef);
                assoc.RelatedVictim = Victims.First(v => v.Id == assoc.VictimRef.VictimRef);
                assoc.RelatedVictim.AssociatedOffense = assoc.RelatedOffense;          
                
            }

            foreach (var assoc in SubjectVictimAssocs)
            {
                assoc.RelatedSubject = Subjects.First(s => s.Id == assoc.SubjectRef.SubjectRef);
                assoc.RelatedVictim = Victims.First(v => v.Id == assoc.VictimRef.VictimRef);
            }
        }
    }
}