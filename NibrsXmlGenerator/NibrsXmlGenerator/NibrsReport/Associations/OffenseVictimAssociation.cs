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
        public Offense.Offense OffenseRef { get; set; }

        [XmlElement("Victim", Namespace = Namespaces.justice, Order = 2)]
        public Victim.Victim VictimRef { get; set; }

        public OffenseVictimAssociation() { }

        public OffenseVictimAssociation(Offense.Offense offense, Victim.Victim victim)
        {
            this.OffenseRef = offense.Reference;
            this.VictimRef = victim.Reference;
        }

        public OffenseVictimAssociation(string offenseRef, string victimRef)
        {
            this.OffenseRef = new Offense.Offense(offenseRef);
            this.VictimRef = new Victim.Victim(victimRef);
        }
    }
}
