using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport
{
    [XmlRoot("Submission", Namespace = Namespaces.cjisNibrs)]
    public class Submission : NibrsSerializable
    {
        [XmlAttribute("schemaLocation", Namespace = System.Xml.Schema.XmlSchema.InstanceNamespace)]
        private string xsiSchemaLocation = NibrsXml.Constants.Misc.schemaLocation;

        [XmlElement("Report")]
        public List<Report> reports = new List<Report>();

        public Submission() { }

        public Submission(params Report[] reports)
        {
            foreach (Report r in reports)
            {
                this.reports.Add(r);
            }
        }
    }
}
