using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NibrsXml.Constants.Ucr;

namespace NibrsXml.Ucr.DataCollections
{
    public class HateCrime : Data
    {
        public class Incident
        {
            public string Id { get; set; }
            public string Date { get; set; }
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
            public int AdultVictimCount { get; set; }
            public int JuvenileVictimCount { get; set; }
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

        public List<Incident> Incidents { get; set; }

        public HateCrime()
        {
            Incidents = new List<Incident>();
        }

        public XDocument Serialize()
        {
            return new XDocument(
                new XProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"hcr.xslt\""),
                new XElement("HCR",
                    new XElement("INCIDENTS",
                        Incidents.Select(i => new XElement("INCIDENT",
                            new XElement("INCIDENTNUM", i.Id),
                            new XElement("INCIDENTDATE", i.Date),
                            new XElement("FILINGTYPE", FilingType.Initial), //todo: The filing type of HCRs need to be determined ??? how? i don't really know yet
                            new XElement("ADULTOFFENDERSCOUNT", i.AdultOffenderCount),
                            new XElement("JUVENILEOFFENDERSCOUNT", i.JuvenileOffenderCount),
                            new XElement("OFFENDERRACE", i.OffenderRace),
                            new XElement("OFFENDERETHNICITY", i.OffenderEthnicity),
                            new XElement("OFFENSES", i.Offenses.Select(o =>
                            {
                                var biases = new[] { o.BiasMotivation1, o.BiasMotivation2, o.BiasMotivation3, o.BiasMotivation4, o.BiasMotivation5 }
                                    .Where(b => b != null)
                                    .Select(b => new XElement("BIASMOTIVE",
                                        new XAttribute("CODE", b)));

                                return new XElement("OFFENSE",
                                    new XElement("OFFENSECODE", o.Code),
                                    new XElement("LOCATIONCODE", o.Location),
                                    new XElement("ADULTVICTIMSCOUNT", o.AdultVictimCount),
                                    new XElement("JUVENILEVICTIMSCOUNT", o.JuvenileVictimCount),
                                    new XElement("VICTIMTYPE",
                                        new XElement("INDIVIDUAL", Convert.ToInt32(o.VictimTypeIndividual)),
                                        new XElement("BUSINESS", Convert.ToInt32(o.VictimTypeBusiness)),
                                        new XElement("FINANCIAL", Convert.ToInt32(o.VictimTypeFinancialInstitution)),
                                        new XElement("GOVERNMENT", Convert.ToInt32(o.VictimTypeGovernment)),
                                        new XElement("RELIGIOUS", Convert.ToInt32(o.VictimTypeReligiousOrg)),
                                        new XElement("OTHER", Convert.ToInt32(o.VictimTypeOther)),
                                        new XElement("UNKNOWN", Convert.ToInt32(o.VictimTypeUnknown))),
                                    new XElement("BIASMOTIVES", biases));
                            })))))));
        }
    }
}
