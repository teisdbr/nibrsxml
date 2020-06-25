
using NibrsXml.Utility;

namespace NibrsXml.Constants.Ucr
{
    public enum UcrReportType
    {
        [UcrReport("NibrsModels.Ucr.Reports.ReturnA.returna.xsl","_ReturnA.xml","_ReturnA.html")]
        ReturnA,

        [UcrReport("NibrsModels.Ucr.Reports.ReturnA.returnasupp.xsl","_ReturnASupplement.xml","_ReturnASupplement.html")]
        SupplementToReturnA,

        [UcrReport("NibrsModels.Ucr.Reports.Asre.asre.xsl","_Asre.xml","_Asre.html")]
        Asre,

        [UcrReport("NibrsModels.Ucr.Reports.Arson.arson.xsl","_Arson.xml","_Arson.html")]
        Arson,

        [UcrReport("NibrsModels.Ucr.Reports.HumanTrafficking.ht.xsl","_HumanTrafficking.xml","_HumanTrafficking.html")]
        HumanTrafficking,

        [UcrReport("NibrsModels.Ucr.Reports.HateCrime.hcr.xslt", "_HateCrime.xml", "_HateCrime.html")]
        HateCrime,

        [UcrReport("NibrsModels.Ucr.Reports.SupplementaryHomicide.shr.xslt", "_SupplementaryHomicide.xml", "_SupplementaryHomicide.html")]
        SupplementaryHomicide,

        [UcrReport("NibrsModels.Ucr.Reports.Leoka.leoka.xsl","_Leoka.xml","_Leoka.html")]
        Leoka,

        [UcrReport("NibrsModels.Ucr.Reports.IncidentsAcceptedOrRejected.incidents.xslt", "_IncidentsAcceptedOrRejected.xml", "_IncidentsAcceptedOrRejected.html")]
        IncidentsAcceptedOrRejected
    }
}
