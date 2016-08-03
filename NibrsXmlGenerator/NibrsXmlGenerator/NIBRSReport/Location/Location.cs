using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Location
{
    [XmlRoot("Location", Namespace = Namespaces.niemCore)]
    public class Location
    {
        /// <summary>
        /// This property is public only for serialization.
        /// It should only be set by using the Location(string) constructor and accessed using the reference property.
        /// </summary>
        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string locationRef { get; set; }
        
        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string id { get; set; }

        [XmlElement("LocationCategoryCode", Namespace = Namespaces.justice)]
        public string categoryCode { get; set; }

        [XmlIgnore]
        public Location reference { get { return new Location(id); } }

        public Location() { }

        public Location(string locationRef)
        {
            this.locationRef = locationRef;
        }

        public Location(int id, string categoryCode)
        {
            this.id = "Location" + id.ToString();
            this.categoryCode = categoryCode;
        }
    }
}
