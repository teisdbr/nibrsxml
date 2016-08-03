using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Associations
{
    [XmlRoot("OffenseVictimAssociation", Namespace = Namespaces.justice)]
    public class OffenseVictimAssociation// : Association
    {
        [XmlElement("Offense", Namespace = Namespaces.justice, Order = 1)]
        public Offense.Offense offenseRef { get; set; }

        [XmlElement("Victim", Namespace = Namespaces.justice, Order = 2)]
        public Victim.Victim victimRef { get; set; }

        public OffenseVictimAssociation() { }

        public OffenseVictimAssociation(Offense.Offense offense, Victim.Victim victim)
        {
            this.offenseRef = offense.reference;
            this.victimRef = victim.reference;
        }

        public OffenseVictimAssociation(string offenseRef, string victimRef)
        {
            this.offenseRef = new Offense.Offense(offenseRef);
            this.victimRef = new Victim.Victim(victimRef);
        }
    }
}
