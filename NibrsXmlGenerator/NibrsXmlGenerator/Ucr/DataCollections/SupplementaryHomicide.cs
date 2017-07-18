using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NibrsXml.Ucr.DataCollections
{
    [XmlRoot]
    public class SupplementaryHomicide : Data
    {
        /// <summary>
        ///     Keep this property private so that the only way to interact with it is via the AddIncident and AddIncidents
        ///     functions
        /// </summary>
        [XmlElement]
        private List<Incident> Incidents { get; set; }

        public XDocument Serialize()
        {
            //todo: serialize this
            var serializer = new XmlSerializer(typeof(SupplementaryHomicide), new []{typeof(Victim), typeof(Offender), typeof(Relationship), typeof(Incident)});
            string xml = "";
            using (StringWriter xmlWriter = new NibrsSerializer.NibrsSerializer.Utf8StringWriter())
            {
                serializer.Serialize(xmlWriter, this);
                xml = xmlWriter.ToString() + "\r\n";
            }
            //return xml;
            return null;
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

            if (Incidents == null)
                Incidents = new List<Incident> {incident};
            if (Incidents.Count == 999)
                return;
            Incidents.Add(incident);
        }

        [XmlRoot]
        public class Victim
        {
            [XmlElement]
            public string Age { get; set; }

            [XmlElement]
            public string Sex { get; set; }

            [XmlElement]
            public string Race { get; set; }

            [XmlElement]
            public string Ethnicity { get; set; }

            [XmlElement]
            public bool WasKilledByNegligence { get; set; }

            [XmlElement]
            public string Circumstance { get; set; }

            [XmlElement]
            public string Subcircumstance { get; set; }
        }

        [XmlRoot]
        public class Offender
        {
            [XmlElement]
            public string SequenceNumber { get; set; }

            [XmlElement]
            public string Age { get; set; }

            [XmlElement]
            public string Sex { get; set; }

            [XmlElement]
            public string Race { get; set; }

            [XmlElement]
            public string Ethnicity { get; set; }
        }

        [XmlRoot]
        public class Relationship
        {
            [XmlElement]
            public string VictimSequenceNumber { get; set; }

            [XmlElement]
            public string OffenderSequenceNumber { get; set; }

            [XmlElement]
            public string RelationshipOfVictimToOffender { get; set; }
        }

        [XmlRoot]
        public class Incident
        {
            [XmlElement]
            public string SequenceNumber { get; set; }

            [XmlElement]
            public List<Victim> Victims { get; set; }

            [XmlElement]
            public List<Offender> Offenders { get; set; }

            [XmlElement]
            public List<Relationship> Relationships { get; set; }

            [XmlElement]
            public string Situation { get; set; }
        }
    }
}