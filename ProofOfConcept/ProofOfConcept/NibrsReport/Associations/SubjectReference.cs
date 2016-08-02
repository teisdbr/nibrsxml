using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Associations
{
    [XmlRoot("Subject", Namespace = Namespaces.justice)]
    public class SubjectReference
    {
        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string subjectId { get; set; }

        public SubjectReference() { }

        public SubjectReference(Subject.Subject subject)
        {
            //this.subjectId = subject.personId;
        }
    }
}
