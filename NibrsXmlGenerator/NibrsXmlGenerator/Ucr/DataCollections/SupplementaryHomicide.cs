using System;
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
                                i.Victims.Select(v => new XElement("VICTIM",
                                    new XElement("AGE", v.Age),
                                    new XElement("SEX", v.Sex),
                                    new XElement("ETHNICITY", v.Ethnicity),
                                    new XElement("RACE", v.Race),
                                    new XElement("OFFENDERS",
                                        i.Offenders.Select(o => new XElement("OFFENDER",
                                            new XElement("AGE", o.Age),
                                            new XElement("SEX", o.Sex),
                                            new XElement("ETHNICITY", o.Ethnicity),
                                            new XElement("RACE", o.Race),
                                            new XElement("WEAPONUSED"),
                                            new XElement("RELATIONSHIP"),
                                            new XElement("CIRCUMSTANCE"),
                                            new XElement("SUBCIRCUMSTANCE"))))))))))))));
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