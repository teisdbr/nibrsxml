using System;
using System.Xml.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Misc
{
    /// <summary>
    ///     This class contains mechanisms that make its dateTime and date properties mutually exclusive
    ///     so that only one can get serialized at a time
    /// </summary>
    [XmlRoot("ActivityDate", Namespace = Namespaces.niemCore)]
    public class ActivityDate
    {
        private string _date;

        private string _dateTime;

        private DateTime _realDateTime;

        public ActivityDate()
        {
        }

        public ActivityDate(string date)
        {
            Date = date;
        }

        public ActivityDate(string date, string time)
        {
            DateTime = date + "T" + time;
        }

        [BsonIgnore][XmlIgnore][JsonIgnore]
        public DateTime RealDateTime
        {
            get { return _realDateTime; }
            set
            {
                _realDateTime = value;

                if (!string.IsNullOrWhiteSpace(DateTime)) DateTime = value.ToString("yyyy-MM-ddThh:mm:ss");

                if (!string.IsNullOrWhiteSpace(Date)) Date = value.ToString("yyyy-MM-dd");
            }
        }

        [XmlElement("DateTime", Namespace = Namespaces.niemCore)]
        public string DateTime
        {
            get { return _dateTime; }
            set
            {
                if (_date != null)
                    _date = null;
                _dateTime = value;

                System.DateTime.TryParse(value, out _realDateTime);
            }
        }

        [XmlElement("Date", Namespace = Namespaces.niemCore)]
        public string Date
        {
            get
            {
                if (_date != null)
                    return _date;
                return DateTime == null ? _dateTime.Substring(0, _dateTime.IndexOf("T")) : null;
            }
            set
            {
                if (_dateTime != null)
                    _dateTime = null;
                _date = value;

                System.DateTime.TryParse(value, out _realDateTime);
            }
        }

        [BsonIgnore][JsonIgnore]
        public string Time
        {
            get { return _dateTime.Substring(_dateTime.IndexOf("T") + 1); }
        }
    }
}