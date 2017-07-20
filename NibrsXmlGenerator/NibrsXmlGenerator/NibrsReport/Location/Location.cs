using NibrsXml.Constants;
using System.Xml.Serialization;

namespace NibrsXml.NibrsReport.Location
{
    [XmlRoot("Location", Namespace = Namespaces.niemCore)]
    public class Location
    {
        /// <summary>
        /// This property is public only For serialization.
        /// It should only be set by using the Location(string) constructor and accessed using the reference property.
        /// </summary>
        [XmlAttribute("ref", Namespace = Namespaces.niemStructs)]
        public string LocationRef { get; set; }

        [XmlAttribute("id", Namespace = Namespaces.niemStructs)]
        public string Id { get; set; }

        [XmlElement("LocationCategoryCode", Namespace = Namespaces.justice)]
        public string CategoryCode { get; set; }

        [XmlIgnore]
        public Location Reference => new Location(Id);
        public Location() { }

        public Location(string locationRef)
        {
            this.LocationRef = locationRef;
        }

        public Location(string categoryCode, string id)
        {
            this.Id = id;
            this.CategoryCode = categoryCode;
        }
    }
}
