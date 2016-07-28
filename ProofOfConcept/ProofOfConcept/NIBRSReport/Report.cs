using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NIBRSXML.Constants;
using NIBRSXML.NIBRSReport.Arrest;
using NIBRSXML.NIBRSReport.Arrestee;
using NIBRSXML.NIBRSReport.Association;
using NIBRSXML.NIBRSReport.EnforcementOfficial;
using NIBRSXML.NIBRSReport.Incident;
using NIBRSXML.NIBRSReport.Item;
using NIBRSXML.NIBRSReport.Location;
using NIBRSXML.NIBRSReport.Misc;
using NIBRSXML.NIBRSReport.Offense;
using NIBRSXML.NIBRSReport.Person;
using NIBRSXML.NIBRSReport.ReportHeader;
using NIBRSXML.NIBRSReport.Subject;
using NIBRSXML.NIBRSReport.Substance;
using NIBRSXML.NIBRSReport.Victim;

namespace NIBRSXML.NIBRSReport
{
    [XmlRoot("Report", Namespace = Namespaces.cjisNibrs)]
    public class Report : List<IReportElement>
    {   
        //public ReportHeader.ReportHeader reportHeader { get; set; }
        //public Incident.Incident incident { get; set; }
        //public OffenseList offenses { get; set; }
        //public LocationList locations { get; set; }
        //public ItemList items { get; set; }
        //public SubstanceList substances { get; set; }
        //public static PersonList persons;
        //public PersonList officers { get; set; }
        //public PersonList victims { get; set; }
        //public PersonList subjects { get; set; }
        //public PersonList arrestees { get; set; }
        //public AssociationList arrestSubjectAssocs { get; set; }
        //public AssociationList offenseLocationAssocs { get; set; }
        //public AssociationList offenseVictimAssocs { get; set; }
        //public AssociationList subjectVictimAssocs { get; set; }

        public Report() { }

        public Report(params IReportElement[] elements)
        {
            foreach (IReportElement element in elements)
                this.Add(element);
        }
    }
}
