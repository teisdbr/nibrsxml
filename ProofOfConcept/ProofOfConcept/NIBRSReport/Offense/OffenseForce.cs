﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Offense
{
    [XmlRoot("OffenseForce", Namespace = Namespaces.justice)]
    public class OffenseForce
    {
        [XmlElement("ForceCategoryCode", Namespace = Namespaces.justice)]
        public string forceCategoryCode { get; set; }
        
        public OffenseForce() { }

        public OffenseForce(string forceCategoryCode)
        {
            this.forceCategoryCode = forceCategoryCode;
        }
    }
}
