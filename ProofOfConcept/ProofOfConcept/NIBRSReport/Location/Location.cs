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
        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string locationRef { get; set; }
        
        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string id { get; set; }

        [XmlElement("LocationCategoryCode", Namespace = Namespaces.justice)]
        public string locationCategoryCode { get; set; }

        [XmlIgnore]
        public Location reference { get { return new Location(id); } }

        public Location() { }

        private Location(string locationRef)
        {
            this.locationRef = locationRef;
        }

        public Location(int id, string locationCategoryCode)
        {
            this.id = "Location" + id.ToString();
            this.locationCategoryCode = locationCategoryCode;
        }
    }
}
