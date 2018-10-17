using System;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Associations
{
    [XmlRoot("OffenseVictimAssociation", Namespace = Namespaces.justice)]
    public class OffenseVictimAssociation : IComparable<OffenseVictimAssociation>
    {
        [XmlElement("Offense", Namespace = Namespaces.justice, Order = 1)]
        public Offense.Offense OffenseRef { get; set; }

        [XmlElement("Victim", Namespace = Namespaces.justice, Order = 2)]
        public Victim.Victim VictimRef { get; set; }

        [XmlIgnore]
        public Offense.Offense RelatedOffense { get; set; }

        [XmlIgnore]
        public Victim.Victim RelatedVictim { get; set; }

        public OffenseVictimAssociation() { }

        public OffenseVictimAssociation(Offense.Offense offense, Victim.Victim victim)
        {
            this.OffenseRef = offense.Reference;
            this.VictimRef = victim.Reference;
            this.RelatedVictim = victim;
            this.RelatedOffense = offense;
        }

        public OffenseVictimAssociation(string offenseRef, string victimRef)
        {
            this.OffenseRef = new Offense.Offense(offenseRef);
            this.VictimRef = new Victim.Victim(victimRef);
        }


        public int CompareTo(OffenseVictimAssociation other)
        {
            if (OffenseRef.OffenseRef == other.OffenseRef.OffenseRef && VictimRef.VictimRef == other.VictimRef.VictimRef)
            {
                return 0;
            }

            return -1;
        }
    }
}
