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

        //[XmlElement("Offense", Namespace = Namespaces.justice)]
        //public Offense.OffenseList offenses { get; set; }

        [XmlElement("Offense", Namespace = Namespaces.justice, Order = 3)]
        public List<Offense.Offense> offenses = new List<Offense.Offense>();

        [XmlElement("Location", Namespace = Namespaces.niemCore, Order = 4)]
        public LocationList locations { get; set; }

        [XmlElement("Item", Namespace = Namespaces.niemCore, Order = 5)]
        public ItemList items { get; set; }

        [XmlElement("Substance", Namespace = Namespaces.niemCore, Order = 6)]
        public SubstanceList substances { get; set; }

        [XmlElement("Person", Namespace = Namespaces.niemCore, Order = 7)]
        public static PersonList persons;

        [XmlElement("EnforcementOfficial", Namespace = Namespaces.justice, Order = 8)]
        public PersonList officers { get; set; }

        [XmlElement("Victim", Namespace = Namespaces.justice, Order = 9)]
        public PersonList victims { get; set; }

        [XmlElement("Subject", Namespace = Namespaces.justice, Order = 10)]
        public PersonList subjects { get; set; }

        [XmlElement("Arrestee", Namespace = Namespaces.justice, Order = 11)]
        public PersonList arrestees { get; set; }

        [XmlElement("Arrest", Namespace = Namespaces.justice, Order = 12)]
        public ArrestList arrests { get; set; }

        [XmlElement("ArrestSubjectAssociation", Namespace = Namespaces.justice, Order = 13)]
        public AssociationList arrestSubjectAssocs { get; set; }

        [XmlElement("OffenseLocationAssociation", Namespace = Namespaces.justice, Order = 14)]
        public AssociationList offenseLocationAssocs { get; set; }

        [XmlElement("OffenseVictimAssociation", Namespace = Namespaces.justice, Order = 15)]
        public AssociationList offenseVictimAssocs { get; set; }

        [XmlElement("SubjectVictimAssociation", Namespace = Namespaces.justice, Order = 16)]
        public AssociationList subjectVictimAssocs { get; set; }

        public Report() { }

        public Report(
            ReportHeader.ReportHeader header,
            Incident.Incident incident,
            //Offense.OffenseList offenses,
            LocationList locations,
            ItemList items,
            SubstanceList substances,
            PersonList officers,
            PersonList victims,
            PersonList subjects,
            PersonList arrestees,
            ArrestList arrests,
            AssociationList arrestSubjectAssocs,
            AssociationList offenseLocationAssocs,
            AssociationList offenseVictimAssocs,
            AssociationList subjectVictimAssocs)
        {
            this.header = header;
            this.incident = incident;
            //this.offenses = offenses;
            this.locations = locations;
            this.items = items;
            this.substances = substances;
            this.officers = officers;
            this.victims = victims;
            this.subjects = subjects;
            this.arrestees = arrestees;
            this.arrests = arrests;
            this.arrestSubjectAssocs = arrestSubjectAssocs;
            this.offenseLocationAssocs = offenseLocationAssocs;
            this.offenseVictimAssocs = offenseVictimAssocs;
            this.subjectVictimAssocs = subjectVictimAssocs;
        }

        public void addOffenses(params Offense.Offense[] offenses) {
            foreach (Offense.Offense offense in offenses)
                this.offenses.Add(offense);
        }
    }
}
