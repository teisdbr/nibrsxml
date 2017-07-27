using NibrsXml.Utility;

namespace NibrsXml.Constants.Ucr
{
    public enum UcrReportType
    {
        [UcrReport("NibrsXml.Ucr.Reports.ReturnA.returna.xsl","_ReturnA.xml","_ReturnA.html")]
        ReturnA,

        [UcrReport("NibrsXml.Ucr.Reports.ReturnA.returnasupp.xsl","_ReturnASupplement.xml","_ReturnASupplement.html")]
        SupplementToReturnA,

        [UcrReport("NibrsXml.Ucr.Reports.Asre.asre.xsl","_Asre.xml","_Asre.html")]
        Asre,

        [UcrReport("NibrsXml.Ucr.Reports.Arson.arson.xsl","_Arson.xml","_Arson.html")]
        Arson,

        [UcrReport("NibrsXml.Ucr.Reports.HumanTrafficking.ht.xsl","_HumanTrafficking.xml","_HumanTrafficking.html")]
        HumanTrafficking,

        [UcrReport("NibrsXml.Ucr.Reports.HateCrime.hcr.xslt", "_HateCrime.xml", "_HateCrime.html")]
        HateCrime,

        [UcrReport("NibrsXml.Ucr.Reports.SupplementaryHomicide.shr.xslt", "_SupplementaryHomicide.xml", "SupplementaryHomicide.html")]
        SupplementaryHomicide,

        [UcrReport("NibrsXml.Ucr.Reports.Leoka.leoka.xsl","_Leoka.xml","_Leoka.html")]
        Leoka
    }
}
