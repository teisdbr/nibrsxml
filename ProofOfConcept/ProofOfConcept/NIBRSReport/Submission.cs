using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NIBRSXML.Constants;

namespace NIBRSXML.NIBRSReport
{
    [XmlRoot("Submission", Namespace = Namespaces.cjisNibrs)]
    public class Submission : List<Report>
    {
        [XmlAttribute("schemaLocation", Namespace = System.Xml.Schema.XmlSchema.InstanceNamespace)]
        public string xsiSchemaLocation = NIBRSXML.Constants.Misc.schemaLocation;

        public Submission() { }

        public Submission(params Report[] reports)
        {
            foreach (Report r in reports)
            {
                this.Add(r);
            }
        }
    }
}
