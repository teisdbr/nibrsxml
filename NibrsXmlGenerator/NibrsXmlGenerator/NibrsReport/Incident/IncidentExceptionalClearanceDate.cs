using System;
using System.Xml.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Incident
{
    [XmlRoot("IncidentExceptionalClearanceDate", Namespace = Namespaces.justice)]
    public class IncidentExceptionalClearanceDate
    {
        public IncidentExceptionalClearanceDate()
        {
        }

        public IncidentExceptionalClearanceDate(string date)
        {
            Date = date;

            DateTime realDate;
            if (DateTime.TryParse(date, out realDate)) RealDate = realDate;
        }

        [XmlElement("Date", Namespace = Namespaces.niemCore)]
        public string Date { get; set; }

        [BsonElement] [XmlIgnore] public DateTime? RealDate { get; set; }

        [BsonIgnore]
        [XmlIgnore]
        public string YearMonthDate
        {
            get
            {
                DateTime dt;
                return DateTime.TryParse(Date, out dt) ? dt.ToString("yyyy-MM") : null;
            }
        }
    }
}