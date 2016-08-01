using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Person
{
    [XmlInclude(typeof(PersonAgeMeasureRange))]
    [XmlInclude(typeof(PersonAgeMeasureValue))]
    [XmlRoot(Namespace = Namespaces.niemCore)]
    public abstract class PersonAgeMeasure
    {
    }
}
