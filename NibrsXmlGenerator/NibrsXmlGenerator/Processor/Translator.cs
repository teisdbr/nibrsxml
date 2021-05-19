using System.Collections.Generic;
using System.Linq;
using NibrsModels.Constants;
using NibrsModels.NibrsReport;
using NibrsModels.Utility;
using NibrsXml.Utility.Extensions;
using TeUtil.Extensions;

namespace NibrsXml.Processor
{
    public class Translator
    {
        public static void TranslateAsReplaceSub(Submission sub)
        {
            if (sub.Reports.First().Header.ReportActionCategoryCode.MatchOne(ReportActionCategoryCode.R.NibrsCode(),ReportActionCategoryCode.D.NibrsCode()))
                return;
            
            // For the Group B Arrest Report Incident has to be null for the Replace Action Type.
            if (sub.ReportingCategory == NibrsReportCategoryCode.B.NibrsCode())
            {
                sub.Reports.First().Incident = null;
            }

            sub.Reports.First().Header.ReportActionCategoryCode = ReportActionCategoryCode.R.NibrsCode(); 

        }

        public static List<Submission> TransformIntoDeletes(
            IEnumerable<Submission> submissions)
        {

            var transformToDeletes = new List<Submission>();

            // make a copy of the list and transform the copied list
            transformToDeletes = submissions.Select(subs => subs.DeepClone()).ToList();

            transformToDeletes.Where(sub => sub.Reports[0].Header.ReportActionCategoryCode != "D")
                .ToList().ForEach(sub =>
                {
                    var report = sub.Reports[0];
                    var header = report.Header;

                    if (report.Header.NibrsReportCategoryCode == NibrsReportCategoryCode.B.NibrsCode())
                    {

                        var arresteess = report.Arrestees.ToList();
                        var arrests = report.Arrests.ToList();
                        var arrestSubjectAssocs = report.ArrestSubjectAssocs.ToList();


                        // new report with only arrestee, arrest and their associations
                        report = new Report
                        {
                            Arrests = arrests,
                            Header = header,
                            ArrestSubjectAssocs = arrestSubjectAssocs
                        };

                        arresteess.ForEach(arres =>
                        {
                            arres.Role = null;
                            arres.Person = null;
                        });

                        report.Arrestees = arresteess;
                    }
                    else
                    {
                        // Group A Incident Report only requires header and incident segement

                        var incident = report.Incident;

                        report = new Report
                        {
                            Incident = incident,
                            Header = header
                        };
                    }

                    // Overwrite the action type
                    report.Header.ReportActionCategoryCode = "D";
                    sub.Reports[0] = report;
                });

            return transformToDeletes;
        }
    }
}