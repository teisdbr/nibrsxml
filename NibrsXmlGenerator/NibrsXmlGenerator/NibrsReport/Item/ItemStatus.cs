﻿using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Item
{
    [XmlRoot("ItemStatus", Namespace = Namespaces.niemCore)]
    public class ItemStatus
    {
        [XmlElement("ItemStatusCode", Namespace = Namespaces.cjis)]
        public string Code { get; set; }

        public ItemStatus() { }

        public ItemStatus(string code)
        {
            this.Code = code;
        }
    }
}