using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport;
using NibrsXml.NibrsReport.Arrest;
using NibrsXml.NibrsReport.Arrestee;
using NibrsXml.NibrsReport.Associations;
using NibrsXml.NibrsReport.EnforcementOfficial;
using NibrsXml.NibrsReport.Incident;
using NibrsXml.NibrsReport.Item;
using NibrsXml.NibrsReport.Location;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.NibrsReport.Person;
using NibrsXml.NibrsReport.ReportHeader;
using NibrsXml.NibrsReport.Subject;
using NibrsXml.NibrsReport.Substance;
using NibrsXml.NibrsReport.Victim;


namespace NibrsXml.NibrsSerializer
{
    public class NibrsSerializer : XmlSerializer
    {
        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding { get { return Encoding.UTF8; } }
        }

        // Declare the types of all the objects that shall be serialized by NibrsSerializer
        // The root type is separate from the others because it is its own parameter in the base class's constructor
        private static Type[] nonRootTypes = 
            {
                // NIBRSReport namespace
                typeof(Report), 
                
                // Incident namespace
                typeof(cjisIncidentAugmentation),
                typeof(Incident),
                typeof(IncidentExceptionalClearanceDate),
                typeof(jxdmIncidentAugmentation),

                // Offense namespace
                typeof(Offense),
                typeof(OffenseEntryPoint),
                typeof(OffenseFactor),
                typeof(OffenseForce),

                // Misc namespace
                typeof(ActivityDate),
                typeof(ActivityIdentification),
                typeof(RoleOfPerson),
                typeof(OrganizationAugmentation),
                typeof(OrganizationORIIdentification),
                
                // ReportHeader namespace
                typeof(ReportDate), 
                typeof(ReportHeader), 
                typeof(ReportingAgency)
            };

        // Declare the namespaces that will be used within the report
        private static XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(
            new XmlQualifiedName[] 
            {
                new XmlQualifiedName(Aliases.nibrs, Namespaces.cjisNibrs),
                new XmlQualifiedName(Aliases.cjis, Namespaces.cjis),
                new XmlQualifiedName(Aliases.cjiscodes, Namespaces.cjisCodes),
                new XmlQualifiedName(Aliases.i, Namespaces.appInfo),
                new XmlQualifiedName(Aliases.ucr, Namespaces.fbiUcr),
                new XmlQualifiedName(Aliases.j, Namespaces.justice),
                new XmlQualifiedName(Aliases.term, Namespaces.niemTerminology),
                new XmlQualifiedName(Aliases.nc, Namespaces.niemCore),
                new XmlQualifiedName(Aliases.niemXsd, Namespaces.niemXsd),
                new XmlQualifiedName(Aliases.s, Namespaces.niemStructs),
                new XmlQualifiedName(Aliases.xsi, Namespaces.xsi),
                new XmlQualifiedName(Aliases.xsd, Namespaces.xsd),
                new XmlQualifiedName(Aliases.nibrscodes, Namespaces.cjisNibrsCodes)
            });

        //private static NibrsSerializer submissionSerializer = new NibrsSerializer(typeof(Submission));
        //private static NibrsSerializer reportSerializer = new NibrsSerializer(typeof(Report));

        public NibrsSerializer(Type type) : base(type, nonRootTypes)
        {
            if (type != typeof(Submission) && type != typeof(Report))
                throw new ArgumentException(
                    "There was an error constructing the NibrsSerializer object." +
                    " Requires first argument to be either typeof(Submission) or typeof(Report)");
        }

        public string Serialize(NibrsSerializable serializee)
        {
            // Initialize the XML string to be returned
            string xml = "";
            using (StringWriter writer = new Utf8StringWriter())
            {
                try
                {
                    if (serializee.GetType() == typeof(Submission) || serializee.GetType() == typeof(Report))
                        base.Serialize(writer, serializee, namespaces);
                    else
                        throw new ArgumentException("The object provided must be of type Submission or Report.");
                    xml = writer.ToString();
                }
                catch (InvalidOperationException e)
                {
                    Exception inner = e.InnerException;
                    string innerMessage = inner.Message;
                }
            }
            return xml;
        }

        //private  string SerializeSubmission(Submission submission)
        //{
        //    return submissionSerializer.Serialize(submission);
        //}

        //private string SerializeReport(Report report)
        //{
        //    return Regex.Replace(reportSerializer.Serialize(report), ".*\\n<nibrs:Report [\\w\\s\"/\\.:=\\-\\d_]+\">", "<nibrs:Report>");
        //}

        static void Main(string[] args)
        {
            //Initialize the test submission object whose XML shall be rendered
            //Submission submission =
            //new Submission(
            //    new Report(
            //        new ReportHeader(
            //            "GROUP A INCIDENT REPORT",
            //            "I",
            //            new ReportDate("2016-02"),
            //            new ReportingAgency(
            //                new OrganizationAugmentation(
            //                    new OrganizationORIIdentification("WVNDX01")))),
            //        new Incident(
            //            new ActivityIdentification("54236732"),
            //            new ActivityDate("2016-02-19T10:00:00"),
            //            new cjisIncidentAugmentation(false, true),
            //            new jxdmIncidentAugmentation(
            //                "A",
            //                new IncidentExceptionalClearanceDate("2016-02-25")))));

            ReportHeader reportHeader = new ReportHeader(
                            "GROUP A INCIDENT REPORT",
                            "I",
                            new ReportDate("2016-02"),
                            new ReportingAgency(
                                new OrganizationAugmentation(
                                    new OrganizationORIIdentification("WVNDX01"))));
            
            Incident incident = new Incident(
                            new ActivityIdentification("54236732"),
                            new ActivityDate("2016-02-19T10:00:00"),
                            new cjisIncidentAugmentation(false, true),
                            new jxdmIncidentAugmentation(
                                "A",
                                new IncidentExceptionalClearanceDate("2016-02-25")));

            EnforcementOfficial officer =
                new EnforcementOfficial(
                    new Person(
                        new PersonAgeMeasureValue(32),
                        "N",
                        new PersonInjury("N"),
                        "B",
                        "R",
                        "M",
                        null),
                    1,
                    "10",
                    "G",
                    new EnforcementOfficialUnit(
                        new OrganizationAugmentation(
                            new OrganizationORIIdentification("WVNDX01"))));
            
            Report report = new Report(
                        reportHeader,
                        incident);

            report.AddOffenses(
                    new Offense(
                        1,
                        "64A",
                        "N",
                        "NONE",
                        1,
                        new OffenseFactor("N"),
                        new OffenseEntryPoint("F"),
                        new OffenseForce("11A"),
                        false));

            report.AddLocations(
                new Location(
                   1,
                   "13"));

            report.AddItems(
                new Item(
                    new ItemStatus("NONE"),
                    new ItemValue(
                        new ItemValueAmount(12000),
                        new ItemValueDate("2016-02-24")),
                    "01",
                    1));

            report.AddSubstances(
                new Substance(
                    "X",
                    new SubstanceQuantityMeasure(
                        "1.5",
                        "XX")));

            report.AddEnforcementOfficials(officer);

            report.AddVictims(
                new Victim(
                    officer,
                    "L",
                    "01",
                    "C"),
                new Victim(
                    new Person(
                        null,
                        "U",
                        new PersonInjury("N"),
                        "W",
                        "R",
                        "M",
                        new PersonAugmentation("BB")),
                    2,
                    "I",
                    "10",
                    "G"));

            report.AddSubjects(
                new Subject(
                    new Person(
                        new PersonAgeMeasureRange(
                            new MeasureIntegerRange(
                                30,
                                25)),
                        "N",
                        null,
                        "W",
                        null,
                        "F",
                        null),
                    1));

            //Submission submission = new Submission(report);

            Console.WriteLine(report.xml);
            Console.ReadLine();
        }
    }
}
