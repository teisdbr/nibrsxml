using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsSerializer;

namespace NibrsXml.NibrsReport
{
    [XmlRoot("Submission", Namespace = Namespaces.cjisNibrs)]
    public class Submission : NibrsSerializable
    {
        [XmlAttribute("schemaLocation", Namespace = System.Xml.Schema.XmlSchema.InstanceNamespace)]
        public string xsiSchemaLocation = NibrsXml.Constants.Misc.schemaLocation;

        [XmlElement("Report")]
        public List<Report> reports = new List<Report>();

        [XmlIgnore]
        public string xml { get { return new NibrsSerializer.NibrsSerializer(typeof(Submission)).Serialize(this); } }

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
