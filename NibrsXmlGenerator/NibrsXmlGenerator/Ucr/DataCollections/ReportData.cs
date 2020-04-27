using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NibrsXml.Ucr.DataCollections
{
    public class ReportData
    {
        #region Stored Properties

        public Asre AsreData { get; set; }
        public ReturnA ReturnAData { get; set; }
        public HumanTrafficking HumanTraffickingData { get; set; }
        public Arson ArsonData { get; set; }
        public Leoka LeokaData { get; set; }
        public SupplementaryHomicide SupplementaryHomicideData { get; set; }
        public HateCrime HateCrimeData { get; set; }
        public List<string> AcceptedIncidents { get; set; }
        public List<Tuple<string, bool>> RejectedIncidents { get; set; }

        #endregion
        
        #region Computed Properties

        public ReturnASupplement ReturnASupplementData
        {
            get
            {
                return ReturnAData.Supplement;
            }
        }

        #endregion

        public ReportData()
        {
            AsreData = new Asre();
            ReturnAData = new ReturnA();
            HumanTraffickingData = new HumanTrafficking();
            ArsonData = new Arson();
            LeokaData = new Leoka();
            SupplementaryHomicideData = new SupplementaryHomicide();
            HateCrimeData = new HateCrime();
            AcceptedIncidents = new List<string>();
            RejectedIncidents = new List<Tuple<string, bool>>();
        }

        public XDocument Serialize()
        {
            return new XDocument(
                new XElement("UcrReports",
                    ReturnAData.Serialize().Root,
                    ReturnASupplementData.Serialize().Root,
                    HumanTraffickingData.Serialize().Root,
                    ArsonData.Serialize().Root,
                    AsreData.Serialize().Root,
                    LeokaData.Serialize().Root,
                    HateCrimeData.Serialize().Root,
                    SupplementaryHomicideData.Serialize().Root,
                    SerializeAcceptedAndRejectedIncidents()));
        }

        public XDocument Serialize(string agency, string ori, int year, int month)
        {
            return new XDocument(
                new XElement("UcrReports",
                    new XAttribute("agency", agency),
                    new XAttribute("ori", ori),
                    new XAttribute("year", year),
                    new XAttribute("month", month),
                    ReturnAData.Serialize().Root,
                    ReturnASupplementData.Serialize().Root,
                    HumanTraffickingData.Serialize().Root,
                    ArsonData.Serialize().Root,
                    AsreData.Serialize().Root,
                    LeokaData.Serialize().Root,
                    HateCrimeData.Serialize().Root,
                    SupplementaryHomicideData.Serialize().Root,
                    SerializeAcceptedAndRejectedIncidents()));
        }

        public XElement SerializeAcceptedAndRejectedIncidents()
        {
            return new XElement("IncidentsAcceptedOrRejected",
                AcceptedIncidents.Select(i => new XElement("Incident",
                    new XAttribute("id", i),
                    new XAttribute("accepted", 1)
                    )),
                RejectedIncidents.Where(rj => rj.Item2 == false).Select(i => new XElement("Incident",
                    new XAttribute("id", i.Item1),
                    new XAttribute("accepted", 0))),
                 RejectedIncidents.Where(rj => rj.Item2 == true).Select(i => new XElement("Incident",
                    new XAttribute("id", i.Item1),
                    new XAttribute("failed", 1)))
                );;
        }
    }
}