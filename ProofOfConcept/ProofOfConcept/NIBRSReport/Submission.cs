using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NIBRSXML.Constants;

namespace NIBRSReport
{
    [XmlRoot(Namespace = Namespaces.cjisNibrs)]
    public class Submission
    {   
        public Report Report { get; set; }

        [XmlAttribute("schemaLocation", Namespace = System.Xml.Schema.XmlSchema.InstanceNamespace)]
        public string xsiSchemaLocation = NIBRSXML.Constants.Misc.schemaLocation;

        public Submission() { }

        public Submission(Report report)
        {
            this.Report = report;
        }
    }
}
