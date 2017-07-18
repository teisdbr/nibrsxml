using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NibrsXml.Constants;
using NibrsXml.NibrsReport;
using NibrsXml.NibrsReport.Incident;
using NibrsXml.NibrsReport.Misc;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.NibrsReport.ReportHeader;


namespace NibrsXml.NibrsSerializer
{
    sealed internal class NibrsSerializer : XmlSerializer
    {
        /// <summary>
        /// This class allows the serializer to write in UTF-8 formatting
        /// </summary>
        internal class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }

        // Declare the types of all the objects that shall be serialized by NibrsSerializer
        // The root type is separate from the others because it is its own parameter in the base class's constructor
        private static Type[] nonRootTypes = 
            {
                // NIBRSReport namespace
                typeof(Report), 
                
                // Incident namespace
                typeof(CjisIncidentAugmentation),
                typeof(Incident),
                typeof(IncidentExceptionalClearanceDate),
                typeof(JxdmIncidentAugmentation),

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

        public NibrsSerializer(Type type) : base(type, nonRootTypes)
        {
            // Constrain the argument to be either of type Submission or Report
            if (type != typeof(Submission) && type != typeof(Report))
                throw new ArgumentException(
                    "There was an error constructing the NibrsSerializer object." +
                    " Requires first argument to be either typeof(Submission) or typeof(Report)");
        }

        /// <summary>
        /// Writes an XML representation of the given NibrsSerializable object with the statically defined namespaces within this class
        /// </summary>
        /// <param name="serializee">The Submission or Report object to serialize</param>
        /// <returns>An XML representation of the argument object using NIBRS XML schema definitions</returns>
        public string Serialize(NibrsSerializable serializee)
        { 
            string xml = "";
            using (StringWriter xmlWriter = new Utf8StringWriter())
            {
                if (serializee.GetType() == typeof(Submission) || serializee.GetType() == typeof(Report))
                    base.Serialize(xmlWriter, serializee, namespaces);
                else
                    throw new ArgumentException("The object provided must be of type Submission or Report.");
                xml = xmlWriter.ToString() + "\r\n";
            }
            return xml;
        }

        private static void Main(string[] args)
        {
            //ReportHeader reportHeader = new ReportHeader(
            //                "GROUP A INCIDENT REPORT",
            //                "I",
            //                new ReportDate("2016-02"),
            //                new ReportingAgency(
            //                    new OrganizationAugmentation(
            //                        new OrganizationORIIdentification("WVNDX01"))));
            
            //Incident incident = new Incident(
            //                new ActivityIdentification("54236732"),
            //                new ActivityDate("2016-02-19", "10:00:00"),
            //                new CjisIncidentAugmentation(false, true),
            //                new JxdmIncidentAugmentation(
            //                    "A",
            //                    new IncidentExceptionalClearanceDate("2016-02-25")));

            //Offense offense1 =
            //    new Offense(
            //        1,
            //        "64A",
            //        "N",
            //        "NONE",
            //        1,
            //        new OffenseFactor("N"),
            //        new OffenseEntryPoint("F"),
            //        new OffenseForce("11A"),
            //        false);

            //Location location1 =
            //    new Location(
            //       "13",
            //       1);

            //EnforcementOfficial officer =
            //    new EnforcementOfficial(
            //        new Person(
            //            new PersonAgeMeasure(32),
            //            "N",
            //            new PersonInjury("N"),
            //            "B",
            //            "R",
            //            "M",
            //            null),
            //        1,
            //        "10",
            //        "G",
            //        new EnforcementOfficialUnit(
            //            new OrganizationAugmentation(
            //                new OrganizationORIIdentification("WVNDX01"))));

            //Victim 
            //    victim1 =
            //        new Victim(
            //            officer,
            //            "L",
            //            "01",
            //            "C"),
            //    victim2 =
            //        new Victim(
            //            new Person(
            //                null,
            //                "U",
            //                new PersonInjury("N"),
            //                "W",
            //                "R",
            //                "M",
            //                new PersonAugmentation("BB")),
            //            2,
            //            "I",
            //            "10",
            //            "G");
            
            //Subject subject1 =
            //    new Subject(
            //        new Person(
            //            new PersonAgeMeasure(30, 25),
            //            "N",
            //            null,
            //            "W",
            //            null,
            //            "F",
            //            null),
            //        1);

            //Arrestee arrestee1 =
            //    new Arrestee(
            //        new Person(
            //            new PersonAgeMeasure(25),
            //            "N",
            //            null,
            //            "W",
            //            "R",
            //            "F",
            //            null),
            //        1,
            //        true,
            //        "12",
            //        "H");

            //Arrest arrest1 =
            //    new Arrest(
            //        1,
            //        new ActivityIdentification("12345"),
            //        new ActivityDate("2016-02-28"),
            //        new ArrestCharge("64A"),
            //        "O",
            //        "N");
            
            //Report report = new Report(reportHeader, incident);
            ////report.AddOffenses(offense1);
            //report.AddLocations(location1);
            //report.AddItems(
            //    new Item(
            //        new ItemStatus("NONE"),
            //        new ItemValue(
            //            new ItemValueAmount(12000),
            //            new ItemValueDate("2016-02-24")),
            //        "01",
            //        1));

            //report.AddSubstances(
            //    new Substance(
            //        "X",
            //        new SubstanceQuantityMeasure(
            //            001.500,
            //            "XX")));
            //report.AddEnforcementOfficials(officer);
            //report.AddVictims(victim1, victim2);
            //report.AddSubjects(subject1);
            //report.AddArrestees(arrestee1);
            //report.AddArrests(arrest1);
            //report.AddArrestSubjectAssociations(new ArrestSubjectAssociation(arrest1, arrestee1));
            ////report.AddOffenseLocationAssociations(new OffenseLocationAssociation(offense1, location1));
            ////report.AddOffenseVictimAssociations(
            ////    new OffenseVictimAssociation(offense1, victim1),
            ////    new OffenseVictimAssociation(offense1, victim2));
            //report.AddSubjectVictimAssociations(
            //    new SubjectVictimAssociation(1, subject1, victim1, "Acquaintance"),
            //    new SubjectVictimAssociation(2, subject1, victim2, "Stranger"));

            //Submission submission = new Submission(report);
            //string x = submission.Xml;
            //string y = report.Xml;
            //Console.WriteLine(x);
            //Console.ReadLine();
        }
    }
}
