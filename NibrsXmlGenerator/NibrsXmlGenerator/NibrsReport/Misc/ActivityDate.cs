using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NibrsXml.Constants;

namespace NibrsXml.NibrsReport.Misc
{
    /// <summary>
    /// This class contains mechanisms that make its dateTime and date properties mutually exclusive
    /// so that only one can get serialized at a time
    /// </summary>
    [XmlRoot("ActivityDate", Namespace = Namespaces.niemCore)]
    public class ActivityDate
    {
        
        private string _dateTime;
        private string _date;
        
        [XmlElement("DateTime", Namespace = Namespaces.niemCore)]
        public string DateTime { 
            get
            { 
                return _dateTime;
            }
            set
            {
                if (_date != null)
                    _date = null;
                this._dateTime = value; 
            } 
        }

        [XmlIgnore]
        public string YearMonthDate
        {
            get
            {
                DateTime dt;
                if (System.DateTime.TryParse(this.DateTime, out dt))
                    return dt.ToString("yyyy-MM");
                return null;
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
                    this._dateTime = null;
                this._date = value; 
            }
        }

        public string Time { get { return _dateTime.Substring(_dateTime.IndexOf("T") + 1); } }

        public ActivityDate() { }

        public ActivityDate(string date)
        {
            this.Date = date;
        }

        public ActivityDate(string date, string time)
        {
            this.DateTime = date + "T" + time;
        }
    }
}
