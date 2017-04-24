using NibrsXml.NibrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NibrsXml.NibrsReport.Victim;

namespace NibrsXml.Ucr.DataCollections
{
    public class HateCrime : Data
    {
        public class Incident
        {
            public string Id { get; set; }
            public string Date { get; set; }
            public int AdultOffenderCount { get; set; }
            public int JuvenileOffenderCount { get; set; }
            public char OffenderRace { get; set; }
            public char OffenderEthnicity { get; set; }
            public List<Offense> Offenses { get; set; }
            
            public void AddOffense(Offense offense)
            {

            }
        }

        public class Offense
        {
            public string Code { get; set; }
            public int AdultVictimCount { get; set; }
            public int JuvenileVictimCount { get; set; }
            public string Location { get; set; }
            public char? BiasMotivation1 { get; set; }
            public char? BiasMotivation2 { get; set; }
            public char? BiasMotivation3 { get; set; }
            public char? BiasMotivation4 { get; set; }
            public char? BiasMotivation5 { get; set; }
            public bool VictimTypeIndividual { get; set; }
            public bool VictimTypeBusiness { get; set; }
            public bool VictimTypeFinancialInstitution { get; set; }
            public bool VictimTypeGovernment { get; set; }
            public bool VictimTypeReligiousOrg { get; set; }
            public bool VictimTypeOther { get; set; }
            public bool VictimTypeUnknown { get; set; }
            public List<Report.Subject> Subjects { get; set; }

            public Offense(string code, List<Victim> victims, string location, List<string> biases, List<Report.Subject> offenders)
            {
                Code = code;
                AdultVictimCount = victims.Count(v => v.Person != null && !v.Person.AgeMeasure.IsJuvenile);
                JuvenileVictimCount = victims.Count(v => v.Person != null && v.Person.AgeMeasure.IsJuvenile);
                Location = location;
                BiasMotivation1 = TranslateBias(biases.ElementAtOrDefault(0));
                BiasMotivation2 = TranslateBias(biases.ElementAtOrDefault(1));
                BiasMotivation3 = TranslateBias(biases.ElementAtOrDefault(2));
                BiasMotivation4 = TranslateBias(biases.ElementAtOrDefault(3));
                BiasMotivation5 = TranslateBias(biases.ElementAtOrDefault(4));
            }
        }

        public List<Incident> Incidents { get; private set; }

        public HateCrime()
        {
            Incidents = new List<Incident>();
        }

        public void AddIncident(Incident incident)
        {
            incident.Id = GetNewIncidentId();
        }

        private string GetNewIncidentId()
        {
            return Incidents.Count.ToString().PadLeft(3, '0');
        }

        public XDocument Serialize()
        {
            throw new NotImplementedException();
        }

        private static char? TranslateBias(string bias)
        {
            if (bias == null)
                return null;
            
            return null;
        }
    }
}
