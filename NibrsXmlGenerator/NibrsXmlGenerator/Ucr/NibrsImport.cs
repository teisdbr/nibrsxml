﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using NibrsXml.NibrsReport;
using NibrsXml.Ucr.DataCollections;
using NibrsXml.Ucr.DataMining;

namespace NibrsXml.Ucr
{
    class NibrsImport
    {
        public List<Report> Reports { get; private set; }
        public ConcurrentDictionary<string, ReportData> MontlyReportData { get; private set; }

        public NibrsImport(string xmlFilepath)
        {
            XmlValidator validator = new XmlValidator(xmlFilepath);
            if (!validator.HasErrors)
            {
                Reports = Submission.Deserialize(xmlFilepath).Reports;
                MontlyReportData = ReportDataMiner.Mine(Reports);
            }
        }
    }
}