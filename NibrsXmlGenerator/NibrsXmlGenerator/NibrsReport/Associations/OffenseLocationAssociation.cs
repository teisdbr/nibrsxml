using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Associations
{
    [XmlRoot("OffenseLocationAssociation", Namespace = Namespaces.justice)]
    public class OffenseLocationAssociation
    {
        public OffenseLocationAssociation()
        {
        }

        public OffenseLocationAssociation(Offense.Offense offense, Location.Location location)
        {
            OffenseRef = offense.Reference;
            LocationRef = location.Reference;
            RelatedOffense = offense;
            RelatedLocation = location;
        }

        public OffenseLocationAssociation(string offenseRef, string locationRef)
        {
            OffenseRef = new Offense.Offense(offenseRef);
            LocationRef = new Location.Location(locationRef);
        }

        [XmlElement("Offense", Namespace = Namespaces.justice, Order = 1)]
        public Offense.Offense OffenseRef { get; set; }

        [XmlElement("Location", Namespace = Namespaces.niemCore, Order = 2)]
        public Location.Location LocationRef { get; set; }

        [XmlIgnore] public Offense.Offense RelatedOffense { get; set; }

        [XmlIgnore] public Location.Location RelatedLocation { get; set; }
    }
}