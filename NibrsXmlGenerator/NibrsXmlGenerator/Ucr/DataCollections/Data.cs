using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.NibrsReport;
using System.Xml.Linq;

namespace NibrsXml.Ucr.DataCollections
{
    interface Data
    {
        XDocument Serialize();
    }
}
