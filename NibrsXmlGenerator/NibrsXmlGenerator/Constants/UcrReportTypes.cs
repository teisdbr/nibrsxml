using NibrsXml.Utility;

namespace NibrsXml.Constants
{
    public enum UcrReportType
    {
        [UcrReport("NibrsXml.Ucr.Reports.ReturnA.returna.xsl","_ReturnA.xml","_ReturnA.html")]
        ReturnA,

        [UcrReport("NibrsXml.Ucr.Reports.ReturnA.returnasupp.xsl","_ReturnASupplement.xml","_ReturnASupplement.html")]
        ReturnASupplement,

        [UcrReport("NibrsXml.Ucr.Reports.Asre.asre.xsl","_Asre.xml","_Asre.html")]
        Asre,

        [UcrReport("NibrsXml.Ucr.Reports.Arson.arson.xsl","_Arson.xml","_Arson.html")]
        Arson,

        [UcrReport("NibrsXml.Ucr.Reports.HumanTrafficking.ht.xsl","_HumanTrafficking.xml","_HumanTrafficking.html")]
        HumanTrafficking,

        HateCrime,

        SupplementaryHomicide,

        [UcrReport("NibrsXml.Ucr.Reports.Leoka.leoka.xsl","_Leoka.xml","_Leoka.html")]
        Leoka
    }
}
