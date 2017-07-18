using System.Xml.Linq;

namespace NibrsXml.Ucr.DataCollections
{
    internal interface Data
    {
        XDocument Serialize();
    }
}
