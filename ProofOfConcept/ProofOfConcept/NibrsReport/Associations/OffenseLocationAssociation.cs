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
        public Offense.Offense offenseRef { get; set; }

        [XmlElement("Location", Namespace = Namespaces.niemCore, Order = 2)]
        public Location.Location locationRef { get; set; }

        public OffenseLocationAssociation() { }

        public OffenseLocationAssociation(Offense.Offense offense, Location.Location location)
        {
            this.offenseRef = offense.reference;
            this.locationRef = location.reference;
        }
    }
}
