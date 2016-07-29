using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Arrest
{
    [XmlRoot("Arrest", Namespace = Namespaces.justice)]
    [XmlType("Arrest")]
    public class ArrestList : List<Arrest>
    {
        public ArrestList() { }

        public ArrestList(params Arrest[] offenses)
        {
            foreach (Arrest offense in offenses)
                this.Add(offense);
        }
    }
}
