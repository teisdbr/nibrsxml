using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibrsXml.Utility
{
    public class UcrReportAttribute : DescriptionAttribute
    {
        public string Assembly { get; set; }
        public string HtmlOutputName { get; set; }
        public string XmlOutputName { get; set; }

        public UcrReportAttribute(string assembly, string xmlOutputName, string htmlOutputName)
        {
            Assembly = assembly;
            XmlOutputName = xmlOutputName;
            HtmlOutputName = htmlOutputName;
        }
    }
}
