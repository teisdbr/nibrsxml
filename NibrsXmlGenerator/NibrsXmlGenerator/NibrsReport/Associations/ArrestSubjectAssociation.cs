using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Associations
{
    [XmlRoot("ArrestSubjectAssociation", Namespace = Namespaces.justice)]
    public class ArrestSubjectAssociation
    {
        public ArrestSubjectAssociation()
        {
        }

        public ArrestSubjectAssociation(Arrest.Arrest arrest, Arrestee.Arrestee arrestee)
        {
            ActivityRef = arrest.Reference;
            SubjectRef = arrestee.Reference;
            RelatedArrestee = arrestee;
            RelatedArrest = arrest;
        }

        public ArrestSubjectAssociation(string arrestRef, string arresteeRef)
        {
            ActivityRef = new Arrest.Arrest(arrestRef);
            SubjectRef = new Arrestee.Arrestee(arresteeRef);
        }

        [XmlElement("Activity", Namespace = Namespaces.niemCore, Order = 1)]
        public Arrest.Arrest ActivityRef { get; set; }

        [XmlElement("Subject", Namespace = Namespaces.justice, Order = 2)]
        public Arrestee.Arrestee SubjectRef { get; set; }

        [XmlIgnore] public Arrest.Arrest RelatedArrest { get; set; }

        [XmlIgnore] public Arrestee.Arrestee RelatedArrestee { get; set; }
    }
}