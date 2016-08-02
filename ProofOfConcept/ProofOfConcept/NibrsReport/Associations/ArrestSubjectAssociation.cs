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
        public Arrest.Arrest activityRef { get; set; }

        [XmlElement("Subject", Namespace = Namespaces.justice, Order = 2)]
        public Arrestee.Arrestee arresteeRef { get; set; }

        public ArrestSubjectAssociation() { }

        public ArrestSubjectAssociation(Arrest.Arrest arrest, Arrestee.Arrestee arrestee)
        {
            this.activityRef = arrest.reference;
            this.arresteeRef = arrestee.reference;
        }
    }
}
