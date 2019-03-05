namespace NibrsXml.Constants
{
    /// <summary>
    /// A listing of miscellaneous constants used for generating NIBRSXML reports
    /// </summary>
    public class Misc
    {
        /// <summary>
        /// "http://fbi.gov/cjis/nibrs/4.2 ../xsd/nibrs/4.2/nibrs.xsd"
        /// </summary>
        public const string schemaLocation = "http://fbi.gov/cjis/nibrs/4.2 ../xsd/nibrs/4.2/nibrs.xsd";

        //To be used only by the individual ORI xml generation.
        public const string rootNibrsSchemaLocation = "http://fbi.gov/cjis/nibrs/4.2 ../../xsd/nibrs/4.2/nibrs.xsd";
    }

    /// <summary>
    /// A listing of XML namespaces that will be used for generating NIBRSXML reports
    /// </summary>
    internal class Namespaces
    {
        /// <summary>
        /// "http://fbi.gov/cjis/nibrs/4.2"
        /// </summary>
        public const string cjisNibrs = "http://fbi.gov/cjis/nibrs/4.2";

        /// <summary>
        /// "http://fbi.gov/cjis/1.0"
        /// </summary>
        public const string cjis = "http://fbi.gov/cjis/1.0";

        /// <summary>
        /// "http://fbi.gov/cjis/cjis-codes/1.0"
        /// </summary>
        public const string cjisCodes = "http://fbi.gov/cjis/cjis-codes/1.0";

        /// <summary>
        /// "http://release.niem.gov/niem/appinfo/3.0/"
        /// </summary>
        public const string appInfo = "http://release.niem.gov/niem/appinfo/3.0/";

        /// <summary>
        /// "http://release.niem.gov/niem/codes/fbi_ucr/3.2/"
        /// </summary>
        public const string fbiUcr = "http://release.niem.gov/niem/codes/fbi_ucr/3.2/";

        /// <summary>
        /// "http://release.niem.gov/niem/domains/jxdm/5.2/"
        /// </summary>
        public const string justice = "http://release.niem.gov/niem/domains/jxdm/5.2/";

        /// <summary>
        /// "http://release.niem.gov/niem/localTerminology/3.0/"
        /// </summary>
        public const string niemTerminology = "http://release.niem.gov/niem/localTerminology/3.0/";

        /// <summary>
        /// "http://release.niem.gov/niem/niem-core/3.0/"
        /// </summary>
        public const string niemCore = "http://release.niem.gov/niem/niem-core/3.0/";

        /// <summary>
        /// "http://release.niem.gov/niem/proxy/xsd/3.0/"
        /// </summary>
        public const string niemXsd = "http://release.niem.gov/niem/proxy/xsd/3.0/";

        /// <summary>
        /// "http://release.niem.gov/niem/structures/3.0/"
        /// </summary>
        public const string niemStructs = "http://release.niem.gov/niem/structures/3.0/";

        /// <summary>
        /// "http://www.w3.org/2001/XMLSchema-instance"
        /// </summary>
        public const string xsi = "http://www.w3.org/2001/XMLSchema-instance";

        /// <summary>
        /// "http://www.w3.org/2001/XMLSchema"
        /// </summary>
        public const string xsd = "http://www.w3.org/2001/XMLSchema";

        /// <summary>
        /// "http://fbi.gov/cjis/nibrs/nibrs-codes/4.2"
        /// </summary>
        public const string cjisNibrsCodes = "http://fbi.gov/cjis/nibrs/nibrs-codes/4.2";


        /// <summary>
        /// "http://schemas.xmlsoap.org/soap/envelope/"
        /// </summary>
        public const string soapenv = "http://schemas.xmlsoap.org/soap/envelope/";


        /// <summary>
        /// "http://ws.nibrs.ucr.cjis.fbi.gov/"
        /// </summary>
        public const string ws = "http://ws.nibrs.ucr.cjis.fbi.gov/";


    }

    /// <summary>
    /// A listing of aliases to use for referencing XML namespaces when generating NIBRSXML reports
    /// Each string constant contains a string of its variable name. Underscores in variable names represent hyphens in its contained string.
    /// </summary>
    internal class Aliases
    {
        /// <summary>
        /// "nibrs"
        /// </summary>
        public const string nibrs = "nibrs";

        /// <summary>
        /// "cjis"
        /// </summary>
        public const string cjis = "cjis";

        /// <summary>
        /// "cjiscodes"
        /// </summary>
        public const string cjiscodes = "cjiscodes";

        /// <summary>
        /// "i"
        /// </summary>
        public const string i = "i";

        /// <summary>
        /// "ucr"
        /// </summary>
        public const string ucr = "ucr";

        /// <summary>
        /// "j"
        /// </summary>
        public const string j = "j";

        /// <summary>
        /// "term"
        /// </summary>
        public const string term = "term";

        /// <summary>
        /// "nc"
        /// </summary>
        public const string nc = "nc";

        /// <summary>
        /// "niem-xsd"
        /// </summary>
        public const string niemXsd = "niem-xsd";

        /// <summary>
        /// "s"
        /// </summary>
        public const string s = "s";

        /// <summary>
        /// "xsi"
        /// </summary>
        public const string xsi = "xsi";

        /// <summary>
        /// "xsd"
        /// </summary>
        public const string xsd = "xsd";

        /// <summary>
        /// "nibrscodes"
        /// </summary>
        public const string nibrscodes = "nibrscodes";

        /// <summary>
        /// "soapenv"
        /// </summary>
        public const string soapenv = "soapenv";


        /// <summary>
        /// "ws"
        /// </summary>
        public const string ws = "ws";

        

    }
}
