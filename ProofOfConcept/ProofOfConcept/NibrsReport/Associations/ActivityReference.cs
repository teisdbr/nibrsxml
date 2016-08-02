using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Associations
{
    [XmlRoot("Activity", Namespace = Namespaces.niemCore)]
    public class ActivityReference
    {
        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string arrestId { get; set; }

        public ActivityReference() { }

        public ActivityReference(Arrest.Arrest arrest)
        {
            this.arrestId = arrest.arrestId;
        }
    }
}
