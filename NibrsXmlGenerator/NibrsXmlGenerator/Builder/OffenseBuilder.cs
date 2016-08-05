using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.Utility;
using LoadBusinessLayer;
using LoadBusinessLayer.LIBRSAdmin;
using LoadBusinessLayer.LIBRSOffender;
using LoadBusinessLayer.LIBRSOffense;
using LoadBusinessLayer.LIBRSErrorConstants;

namespace NibrsXml.Builder
{
    class OffenseBuilder
    {
        private const string offenseAttemptedCode = "A";

        public static List<Offense> Build(List<LIBRSOffense> offenses)
        {
            List<Offense> offenseReports = new List<Offense>();
            foreach (LIBRSOffense offense in offenses)
            {
                Offense offenseReport = new Offense();
                offenseReport.UcrCode = ExtractNibrsCode(offense);
                offenseReport.CriminalActivityCategoryCodes = ExtractNibrsCriminalActivityCategoryCodes(offense);
                //todo: figure out how to relate the LibrsOffender's bias code to the NibrsOffense's bias code
                offenseReport.OffenseStructuresEnteredQuantity = offense.Premises.TryBuild<String>();
                //todo: figure out how to relate the LibrsOffender's suspected of using code to the NibrsOffense's suspected of using code
                offenseReport.OffenseEntryPoint = offense.MethodOfEntry.TryBuild<OffenseEntryPoint>();
                offenseReport.OffenseForces = ExtractNibrsOffenseForces(offense);
                offenseReport.OffenseAttemptedIndicator = ExtractNibrsAttemptedIndicator(offense);
                offenseReports.Add(offenseReport);
            }
            return offenseReports;
        }

        private static string ExtractNibrsCode(LIBRSOffense offense)
        {
            return LarsList.LarsDictionary[offense.LRSNumber].nibr;
        }

        private static List<String> ExtractNibrsCriminalActivityCategoryCodes(LIBRSOffense offense)
        {
            List<String> nibrsCriminalActivityCategoryCodes = new List<String>();
            nibrsCriminalActivityCategoryCodes.TryAdd(
                TranslateCriminalActivityCategoryCode(offense.CriminalActivity1),
                TranslateCriminalActivityCategoryCode(offense.CriminalActivity2),
                TranslateCriminalActivityCategoryCode(offense.CriminalActivity3));
            return nibrsCriminalActivityCategoryCodes;
        }

        private static string TranslateCriminalActivityCategoryCode(string librsCriminalActivity)
        {
            if (librsCriminalActivity == LIBRSErrorConstants.OthCrim)
                librsCriminalActivity = LIBRSErrorConstants.Posses;
            return librsCriminalActivity.TryBuild<String>();
        }

        private static string ExtractNibrsBiasMotivationCode(LIBRSOffender offender)
        {
            HashSet<String> librsOnlyBiasMotivationCodes = new HashSet<string>()
            {
                "70",   //Age
                "71",   //Ancestry
                "72",   //Creed
                "73",   //Gender
                "74"    //Organizational Affiliation
            };
            // 88 is the default bias code 
            return librsOnlyBiasMotivationCodes.Contains(offender.BiasMotivation) ? "88" : offender.BiasMotivation;
        }

        private static List<OffenseForce> ExtractNibrsOffenseForces(LIBRSOffense offense)
        {
            List<OffenseForce> nibrsOffenseForces = new List<OffenseForce>();
            nibrsOffenseForces.TryAdd(
                offense.WeaponForce1.TryBuild<OffenseForce>(),
                offense.WeaponForce2.TryBuild<OffenseForce>(),
                offense.WeaponForce3.TryBuild<OffenseForce>());
            return nibrsOffenseForces;
        }

        private static string ExtractNibrsAttemptedIndicator(LIBRSOffense offense)
        {
            return offense.AttemptedCompleted == offenseAttemptedCode ? true.ToString().ToLower() : false.ToString().ToLower();
        }

        //public static string toDate(this String librsDate)
        //{
        //    //todo for incident exceptional clearance date
        //    return "yyyy-MM-dd";
        //}
    }
}
