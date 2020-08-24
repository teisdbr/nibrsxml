﻿using NibrsInterface;
using NibrsModels.Constants;
using NibrsModels.NibrsReport;
using NibrsModels.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using LoadBusinessLayer;
using NibrsXml.Builder;
using NibrsXml.Utility.Extensions;

namespace NibrsXml.Processor
{
    public class DeleteTransformer
    {

        public static List<NibrsXmlTransaction> TransformIntoDeletes(
            IEnumerable<NibrsXmlTransaction> nibrsXmlTransactions)
        {

            var transformToDeletes = new List<NibrsXmlTransaction>();

            // make a copy of the list and transform the copied list
            transformToDeletes = nibrsXmlTransactions.Select(trans => trans.DeepClone()).ToList();

            transformToDeletes.Where(trans => trans.Submission.Reports[0].Header.ReportActionCategoryCode != "D")
                .ToList().ForEach(
                    trans =>
                    {
                        var report = trans.Submission.Reports[0];
                        var header = report.Header;

                        if (report.Header.NibrsReportCategoryCode == NibrsReportCategoryCode.B.NibrsCode())
                        {

                            var arresteess = report.Arrestees.ToList();
                            var arrests = report.Arrests.ToList();
                            var arrestSubjectAssocs = report.ArrestSubjectAssocs.ToList();


                            // new report with only arrestee, arrest and thier associations
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
                        trans.Submission.Reports[0] = report;

                        trans.NibrsSubmissionResponse.NibrsResponse = null;

                    });
            return transformToDeletes;

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

