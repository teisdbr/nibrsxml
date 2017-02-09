﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibrsXml.Ucr.DataCollections
{
    public class ReportData
    {
        public Asra AsraData { get; set; }
        public OffenseReturn OffenseReturnData { get; set; }
        public HumanTrafficking HumanTraffickingData { get; set; }

        public ReportData()
        {
            this.AsraData = new Asra();
            this.OffenseReturnData = new OffenseReturn();
            this.HumanTraffickingData = new HumanTrafficking();
        }
    }
}
