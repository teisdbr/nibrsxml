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
        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string id { get; set; }

        [XmlElement("LocationCategoryCode", Namespace = Namespaces.justice)]
        public string locationCategoryCode { get; set; }

        public Location() { }

        public Location(int id, string locationCategoryCode)
        {
            this.id = "Location" + id.ToString();
            this.locationCategoryCode = locationCategoryCode;
        }
    }
}
