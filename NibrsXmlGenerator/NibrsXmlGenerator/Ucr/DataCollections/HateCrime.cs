using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace NibrsXml.Ucr.DataCollections
{
    public class HateCrime : Data
    {
        public class Incident
        {
            public string Id { get; set; }
            public string Date { get; set; }
            public int AdultVictimCount { get; set; }
            public int JuvenileVictimCount { get; set; }
            public int TotalVictimCount { get; set; }
            public int? AdultOffenderCount { get; set; }
            public int? JuvenileOffenderCount { get; set; }
            public int TotalOffenderCount { get; set; }
            public string OffenderRace { get; set; }
            public string OffenderEthnicity { get; set; }
            public List<Offense> Offenses { get; set; }
        }

        public class Offense
        {
            public string Code { get; set; }
            public int VictimCount { get; set; }
            public string Location { get; set; }
            public string BiasMotivation1 { get; set; }
            public string BiasMotivation2 { get; set; }
            public string BiasMotivation3 { get; set; }
            public string BiasMotivation4 { get; set; }
            public string BiasMotivation5 { get; set; }
            public bool VictimTypeIndividual { get; set; }
            public bool VictimTypeBusiness { get; set; }
            public bool VictimTypeFinancialInstitution { get; set; }
            public bool VictimTypeGovernment { get; set; }
            public bool VictimTypeReligiousOrg { get; set; }
            public bool VictimTypeOther { get; set; }
            public bool VictimTypeUnknown { get; set; }
        }

        public List<Incident> Incidents { get; } = new List<Incident>();
        
        public XDocument Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
