﻿using System;
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
        public string Id { get; set; }

        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string ArrestRef { get; set; }

        [XmlElement("ActivityIdentification", Namespace = Namespaces.niemCore, Order = 1)]
        public ActivityIdentification ActivityId { get; set; }

        [XmlElement("ActivityDate", Namespace = Namespaces.niemCore, Order = 2)]
        public ActivityDate Date { get; set; }

        [XmlElement("ArrestCharge", Namespace = Namespaces.justice, Order = 3)]
        public ArrestCharge Charge { get; set; }

        [XmlElement("ArrestCategoryCode", Namespace = Namespaces.justice, Order = 4)]
        public string CategoryCode { get; set; }

        [XmlElement("ArrestSubjectCountCode", Namespace = Namespaces.justice, Order = 5)]
        public string SubjectCountCode { get; set; }

        [XmlIgnore]
        public Arrest Reference { get { return new Arrest(this.Id); } }

        public Arrest() { }

        public Arrest(string arrestId)
        {
            this.ArrestRef = arrestId;
        }

        public Arrest(int arrestId, ActivityIdentification activityId, ActivityDate date, ArrestCharge charge, string categoryCode, string subjectCountCode)
        {
            this.Id = "Arrest" + arrestId.ToString();
            this.ActivityId = activityId;
            this.Date = date;
            this.Charge = charge;
            this.CategoryCode = categoryCode;
            this.SubjectCountCode = subjectCountCode;
        }
    }
}
