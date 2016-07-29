using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport
{
    [XmlInclude(typeof(Submission))]
    [XmlInclude(typeof(Report))]
    [XmlRoot(Namespace = Namespaces.cjisNibrs)]
    public abstract class NibrsSerializable
    {
    }
}
