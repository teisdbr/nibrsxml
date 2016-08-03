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
    /// <summary>
    /// In the NibrsReport namespace, all XML elements that must be serialized must be given public access in order for
    /// NibrsSerializer to print them accordingly. This also gives full freedom for NibrsReportBuilder to build reports
    /// however it sees fit.
    /// </summary>
    [XmlRoot("Submission", Namespace = Namespaces.cjisNibrs)]
    public class Submission : NibrsSerializable
    {
        [XmlAttribute("schemaLocation", Namespace = System.Xml.Schema.XmlSchema.InstanceNamespace)]
        public string xsiSchemaLocation = NibrsXml.Constants.Misc.schemaLocation;

        [XmlElement("Report")]
        public List<Report> reports = new List<Report>();

        [XmlIgnore]
        private static NibrsSerializer.NibrsSerializer serializer = new NibrsSerializer.NibrsSerializer(typeof(Submission));

        [XmlIgnore]
        public string xml { get { return serializer.Serialize(this); } }

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
