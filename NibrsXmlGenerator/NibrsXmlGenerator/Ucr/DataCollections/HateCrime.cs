using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Victim;
using NibrsXml.NibrsReport.Subject;
using NibrsXml.Utility;
using TeUtil.Extensions;

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
            
            public void AddOffense(Offense offense, List<Subject> offenders)
            {

            }
        }

        public class Offense
        {
            public string Code { get; set; }
            public int AdultVictimCount { get; set; }
            public int JuvenileVictimCount { get; set; }
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

            public Offense(string code, List<Victim> victims, string location, List<string> biases)
            {
                var victimTypes = victims
                    .Select(v => v.CategoryCode)
                    .Distinct()
                    .ToList();

                Code = code;
                AdultVictimCount = victims.Count(v => v.Person != null && !v.Person.AgeMeasure.IsJuvenile);
                JuvenileVictimCount = victims.Count(v => v.Person != null && v.Person.AgeMeasure.IsJuvenile);
                Location = location;
                BiasMotivation1 = Translate.HateCrimeBiasMotivationTranslations.TryGet(biases.ElementAtOrDefault(0));
                BiasMotivation2 = Translate.HateCrimeBiasMotivationTranslations.TryGet(biases.ElementAtOrDefault(1));
                BiasMotivation3 = Translate.HateCrimeBiasMotivationTranslations.TryGet(biases.ElementAtOrDefault(2));
                BiasMotivation4 = Translate.HateCrimeBiasMotivationTranslations.TryGet(biases.ElementAtOrDefault(3));
                BiasMotivation5 = Translate.HateCrimeBiasMotivationTranslations.TryGet(biases.ElementAtOrDefault(4));
                VictimTypeIndividual = victimTypes.Contains(VictimCategoryCode.INDIVIDUAL.NibrsCode()) || victimTypes.Contains(VictimCategoryCode.LAW_ENFORCEMENT_OFFICER.NibrsCode());
                VictimTypeBusiness = victimTypes.Contains(VictimCategoryCode.BUSINESS.NibrsCode());
                VictimTypeFinancialInstitution = victimTypes.Contains(VictimCategoryCode.FINANCIAL_INSTITUTION.NibrsCode());
                VictimTypeGovernment = victimTypes.Contains(VictimCategoryCode.GOVERNMENT.NibrsCode());
                VictimTypeReligiousOrg = victimTypes.Contains(VictimCategoryCode.RELIGIOUS_ORGANIZATION.NibrsCode());
                VictimTypeOther = victimTypes.Contains(VictimCategoryCode.OTHER.NibrsCode());
                VictimTypeUnknown = victimTypes.Contains(VictimCategoryCode.UNKNOWN.NibrsCode());
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
    }
}
