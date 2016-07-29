using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Arrest;
using NibrsXml.NibrsReport.Arrestee;
using NibrsXml.NibrsReport.Association;
using NibrsXml.NibrsReport.EnforcementOfficial;
using NibrsXml.NibrsReport.Incident;
using NibrsXml.NibrsReport.Item;
using NibrsXml.NibrsReport.Location;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.NibrsReport.Person;
using NibrsXml.NibrsReport.ReportHeader;
using NibrsXml.NibrsReport.Subject;
using NibrsXml.NibrsReport.Substance;
using NibrsXml.NibrsReport.Victim;

namespace NibrsXml.NibrsReport
{
    [XmlInclude(typeof(Arrest.Arrest))]
    [XmlInclude(typeof(Arrestee.Arrestee))]
    [XmlInclude(typeof(Association.Association))]
    [XmlInclude(typeof(EnforcementOfficial.EnforcementOfficial))]
    [XmlInclude(typeof(Incident.Incident))]
    [XmlInclude(typeof(Item.Item))]
    [XmlInclude(typeof(Location.Location))]
    [XmlInclude(typeof(Offense.Offense))]
    [XmlInclude(typeof(ReportHeader.ReportHeader))]
    [XmlInclude(typeof(Subject.Subject))]
    [XmlInclude(typeof(Substance.Substance))]
    [XmlInclude(typeof(Victim.Victim))]
    [XmlRoot(Namespace = Namespaces.cjisNibrs)]
    public abstract class ReportElement
    {
    }
}
