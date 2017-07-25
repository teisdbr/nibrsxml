using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NibrsXml.Ucr.DataCollections
{
    public class SupplementaryHomicide : Data
    {
        /// <summary>
        ///     Keep this property private so that the only way to interact with it is via the AddIncident and AddIncidents
        ///     functions
        /// </summary>
        private List<Incident> Incidents { get; set; }

        public SupplementaryHomicide()
        {
            Incidents = new List<Incident>();
        }

        public XDocument Serialize()
        {
            return new XDocument(
                new XProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"shr.xsl\""),
                new XElement("SHR",
                    new XElement("INCIDENTS",
                        Incidents.Select(i => new XElement("INCIDENT",
                            new XElement("MANSLAUGHTERNEGLIGENT", i.IsNegligent ? 1 : 0,
                            new XElement("MANSLAUGTERNOTNEGLIGENT", i.IsNegligent ? 1 : 0,
                            new XElement("SITUATION", i.Situation),
                            new XElement("VICTIMS",
                                i.Victims.GroupJoin(i.Relationships, v => v.SequenceNumber, r => r.VictimSequenceNumber, (victim, relationships) =>
                                {
                                    return new XElement("VICTIM",
                                        new XElement("AGE", victim.Age),
                                        new XElement("SEX", victim.Sex),
                                        new XElement("ETHNICITY", victim.Ethnicity),
                                        new XElement("RACE", victim.Race),
                                        new XElement("OFFENDERS",
                                            relationships.Join(i.Offenders, r => r.OffenderSequenceNumber, o => o.SequenceNumber, (relationship, offender) =>
                                            new XElement("OFFENDER",
                                                new XElement("AGE", offender.Age),
                                                new XElement("SEX", offender.Sex),
                                                new XElement("ETHNICITY", offender.Ethnicity),
                                                new XElement("RACE", offender.Race),
                                                new XElement("WEAPONUSED", offender.WeaponUsed),
                                                new XElement("RELATIONSHIP", relationship.RelationshipOfVictimToOffender),
                                                new XElement("CIRCUMSTANCE", victim.Circumstance),
                                                new XElement("SUBCIRCUMSTANCE", victim.Subcircumstance)
                                                ))));
                                })))))))));
        }

        /// <summary>
        ///     Assigns a unique incident number for the incident to be added, then adds it to this report's list of incidents
        ///     while there are less than 1000 incidents.
        /// </summary>
        /// <param name="incident">The homicide incident to add to the SHR</param>
        public void TryAddIncident(Incident incident)
        {
            //Define the incident sequence number based on how many homicides have already been recorded
            incident.SequenceNumber = (Incidents.Count + 1).ToString().PadLeft(3, '0');

            if (Incidents.Count < 999)
                Incidents.Add(incident);
        }

        public class Victim
        {
            public string SequenceNumber { get; set; }
            public string Age { get; set; }
            public string Sex { get; set; }
            public string Race { get; set; }
            public string Ethnicity { get; set; }
            public bool WasKilledByNegligence { get; set; }
            public string Circumstance { get; set; }
            public string Subcircumstance { get; set; }
        }

        public class Offender
        {
            public string SequenceNumber { get; set; }
            public string Age { get; set; }
            public string Sex { get; set; }
            public string Race { get; set; }
            public string Ethnicity { get; set; }
            public string WeaponUsed { get; set; }
        }

        public class Relationship
        {
            public string VictimSequenceNumber { get; set; }
            public string OffenderSequenceNumber { get; set; }
            public string RelationshipOfVictimToOffender { get; set; }
        }

        public class Incident
        {
            public string SequenceNumber { get; set; }
            public bool IsNegligent { get; set; }
            public List<Victim> Victims { get; set; }
            public List<Offender> Offenders { get; set; }
            public List<Relationship> Relationships { get; set; }
            public string Situation { get; set; }
        }
    }
}