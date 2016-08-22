using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Associations
{
    [XmlRoot("ArrestSubjectAssociation", Namespace = Namespaces.justice)]
    public class ArrestSubjectAssociation //: Association
    {
        [XmlElement("Activity", Namespace = Namespaces.niemCore, Order = 1)]
        public Arrest.Arrest ActivityRef { get; set; }

        [XmlElement("Subject", Namespace = Namespaces.justice, Order = 2)]
        public Arrestee.Arrestee SubjectRef { get; set; }

        public ArrestSubjectAssociation() { }

        public ArrestSubjectAssociation(Arrest.Arrest arrest, Arrestee.Arrestee arrestee)
        {
            this.ActivityRef = arrest.Reference;
            this.SubjectRef = arrestee.Reference;
        }

        public ArrestSubjectAssociation(string arrestRef, string arresteeRef)
        {
            this.ActivityRef = new Arrest.Arrest(arrestRef);
            this.SubjectRef = new Arrestee.Arrestee(arresteeRef);
        }
    }
}
