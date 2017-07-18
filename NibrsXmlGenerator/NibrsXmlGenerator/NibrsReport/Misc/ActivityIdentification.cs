﻿using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Misc
{
    [XmlRoot("ActivityIdentification", Namespace = Namespaces.niemCore)]
    public class ActivityIdentification
    {
        [XmlElement("IdentificationID", Namespace = Namespaces.niemCore)]
        public string Id { get; set; }

        public ActivityIdentification() { }

        public ActivityIdentification(string id)
        {
            this.Id = id;
        }
    }
}
