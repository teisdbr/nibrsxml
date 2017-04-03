using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Associations
{
    [XmlRoot("OffenseLocationAssociation", Namespace = Namespaces.justice)]
    public class OffenseLocationAssociation// : Association
    {
        [XmlElement("Offense", Namespace = Namespaces.justice, Order = 1)]
        public Offense.Offense OffenseRef { get; set; }

        [XmlElement("Location", Namespace = Namespaces.niemCore, Order = 2)]
        public Location.Location LocationRef { get; set; }

        [XmlIgnore]
        public Offense.Offense RelatedOffense { get; set; }

        [XmlIgnore]
        public Location.Location RelatedLocation { get; set; }

        public OffenseLocationAssociation() { }

        public OffenseLocationAssociation(Offense.Offense offense, Location.Location location)
        {
            this.OffenseRef = offense.Reference;
            this.LocationRef = location.Reference;
            this.RelatedOffense = offense;
            this.RelatedLocation = location;
        }

        public OffenseLocationAssociation(string offenseRef, string locationRef)
        {
            this.OffenseRef = new Offense.Offense(offenseRef);
            this.LocationRef = new Location.Location(locationRef);
        }
    }
}
