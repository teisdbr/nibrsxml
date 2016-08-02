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
    public class ArrestSubjectAssociation : Association
    {
        [XmlElement("Activity", Namespace = Namespaces.niemCore, Order = 1)]
        public ActivityReference activityRef { get; set; }

        [XmlElement("Subject", Namespace = Namespaces.justice, Order = 2)]
        public SubjectReference subjectRef { get; set; }

        public ArrestSubjectAssociation() { }

        public ArrestSubjectAssociation(Arrest.Arrest arrest, Subject.Subject subject)
        {
            this.activityRef = new ActivityReference(arrest);
            this.subjectRef = new SubjectReference(subject);
        }
    }
}
