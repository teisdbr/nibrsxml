﻿namespace NibrsXml.Processor
{
    public class SubmissionBatchStatus
    {
        public string RunNumber { get; set; }

        public int NoOfSubmissions { get; set; }

        public string Ori { get; set; }

        public string Environment { get; set; }

        public bool HasErrorOccured { get; set; }

    }
}