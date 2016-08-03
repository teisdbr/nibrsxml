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
        public string dateTime { 
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

        [XmlElement("Date", Namespace = Namespaces.niemCore)]
        public string date
        { 
            get 
            { 
                return _date;
            }
            set 
            { 
               if (_dateTime != null)
                    this._dateTime = null;
                this._date = value; 
            }
        }

        public string time { get { return dateTime.Substring(dateTime.IndexOf("T") + 1); } }

        public ActivityDate() { }

        public ActivityDate(string date)
        {
            this.date = date;
        }

        public ActivityDate(string date, string time)
        {
            this.dateTime = date + "T" + time;
        }
    }
}
