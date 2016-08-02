using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Misc;

namespace NibrsXml.NibrsReport.Arrest
{
    [XmlRoot("Arrest", Namespace = Namespaces.justice)]
    public class Arrest
    {
        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string arrestId { get; set; }

        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string arrestRef { get; set; }

        [XmlElement("ActivityIdentification", Namespace = Namespaces.niemCore, Order = 1)]
        public ActivityIdentification activityId { get; set; }

        [XmlElement("ActivityDate", Namespace = Namespaces.niemCore, Order = 2)]
        public ActivityDate date { get; set; }

        [XmlElement("ArrestCharge", Namespace = Namespaces.justice, Order = 3)]
        public ArrestCharge charge { get; set; }

        [XmlElement("ArrestCategoryCode", Namespace = Namespaces.justice, Order = 4)]
        public string categoryCode { get; set; }

        [XmlElement("ArrestSubjectCountCode", Namespace = Namespaces.justice, Order = 5)]
        public string subjectCountCode { get; set; }

        [XmlIgnore]
        public Arrest reference { get { return new Arrest(this.arrestId); } }

        public Arrest() { }

        private Arrest(string arrestId)
        {
            this.arrestId = arrestId;
        }

        public Arrest(int arrestId, ActivityIdentification activityId, ActivityDate date, ArrestCharge charge, string categoryCode, string subjectCountCode)
        {
            this.arrestId = "Arrest" + arrestId.ToString();
            this.activityId = activityId;
            this.date = date;
            this.charge = charge;
            this.categoryCode = categoryCode;
            this.subjectCountCode = subjectCountCode;
        }
    }
}
