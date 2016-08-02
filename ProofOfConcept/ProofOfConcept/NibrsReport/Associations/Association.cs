using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Associations
{
    [XmlInclude(typeof(ArrestSubjectAssociation))]
    [XmlInclude(typeof(OffenseLocationAssociation))]
    [XmlInclude(typeof(OffenseVictimAssociation))]
    [XmlInclude(typeof(SubjectVictimAssociation))]
    [Serializable]
    public abstract class Association
    {
    }
}
