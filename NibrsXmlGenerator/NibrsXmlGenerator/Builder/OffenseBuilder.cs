using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.Utility;
using LoadBusinessLayer;
using LoadBusinessLayer.LIBRSAdmin;
using LoadBusinessLayer.LIBRSOffender;
using LoadBusinessLayer.LIBRSOffense;
using LoadBusinessLayer.LIBRSErrorConstants;
using System.Text.RegularExpressions;
using TeUtil.Extensions;

namespace NibrsXml.Builder
{
    class OffenseBuilder
    {
        private static string offenseAttemptedCode = "A";
        private static Dictionary<string, string> biasMotivationCodeTranslations = new Dictionary<string, string>()
        {
            { "11", BiasMotivationCode.ANTIWHITE.NibrsCode() },
            { "12", BiasMotivationCode.ANTIBLACK_AFRICAN_AMERICAN.NibrsCode() },
            { "13", BiasMotivationCode.ANTIAMERICAN_INDIAN_ALASKAN_NATIVE.NibrsCode() },
            { "14", BiasMotivationCode.ANTIASIAN.NibrsCode() },
            { "15", BiasMotivationCode.ANTIMULTIRACIAL_GROUP.NibrsCode() },
            { "16", BiasMotivationCode.ANTINATIVEHAWAIIAN_OTHERPACIFICISLANDER.NibrsCode() },
            { "21", BiasMotivationCode.ANTIJEWISH.NibrsCode() },
            { "22", BiasMotivationCode.ANTICATHOLIC.NibrsCode() },
            { "23", BiasMotivationCode.ANTIPROTESTANT.NibrsCode() },
            { "24", BiasMotivationCode.ANTIISLAMIC.NibrsCode() },
            { "25", BiasMotivationCode.ANTIOTHER_RELIGION.NibrsCode() },
            { "26", BiasMotivationCode.ANTIMULTIRELIGIOUS_GROUP.NibrsCode() },
            { "27", BiasMotivationCode.ANTIATHEIST_AGNOSTIC.NibrsCode() },
            { "28", BiasMotivationCode.ANTIMORMON.NibrsCode() },
            { "29", BiasMotivationCode.ANTIJEHOVAHWITNESS.NibrsCode() },
            { "31", BiasMotivationCode.ANTIARAB.NibrsCode() },
            { "32", BiasMotivationCode.ANTIHISPANIC_LATINO.NibrsCode() },
            { "33", BiasMotivationCode.ANTIOTHER_ETHNICITY_NATIONAL_ORIGIN.NibrsCode() },
            { "41", BiasMotivationCode.ANTIMALE_HOMOSEXUAL.NibrsCode() },
            { "42", BiasMotivationCode.ANTIFEMALE_HOMOSEXUAL.NibrsCode() },
            { "43", BiasMotivationCode.ANTIHOMOSEXUAL.NibrsCode() },
            { "44", BiasMotivationCode.ANTIHETEROSEXUAL.NibrsCode() },
            { "45", BiasMotivationCode.ANTIBISEXUAL.NibrsCode() },
            { "51", BiasMotivationCode.ANTIPHYSICAL_DISABILITY.NibrsCode() },
            { "52", BiasMotivationCode.ANTIMENTAL_DISABILITY.NibrsCode() },
            { "61", BiasMotivationCode.ANTIMALE.NibrsCode() },
            { "62", BiasMotivationCode.ANTIFEMALE.NibrsCode() },
            { "70", BiasMotivationCode.UNKNOWN.NibrsCode() },
            { "71", BiasMotivationCode.UNKNOWN.NibrsCode() },
            { "72", BiasMotivationCode.UNKNOWN.NibrsCode() },
            { "73", BiasMotivationCode.UNKNOWN.NibrsCode() },
            { "74", BiasMotivationCode.UNKNOWN.NibrsCode() },
            { "81", BiasMotivationCode.ANTIEASTERNORTHODOX.NibrsCode() },
            { "82", BiasMotivationCode.ANTIOTHER_CHRISTIAN.NibrsCode() },
            { "83", BiasMotivationCode.ANTIBUDDHIST.NibrsCode() },
            { "84", BiasMotivationCode.ANTIHINDU.NibrsCode() },
            { "85", BiasMotivationCode.ANTISIKH.NibrsCode() },
            { "88", BiasMotivationCode.NONE.NibrsCode() },
            { "99", BiasMotivationCode.UNKNOWN.NibrsCode() }
        };

        public static List<Offense> Build(List<LIBRSOffense> offenses, List<string> uniqueBiasMotivationCodes, List<string> uniqueSuspectedOfUsingCodes, String uniqueReportPrefix)
        {
            List<Offense> offenseReports = new List<Offense>();
            foreach (LIBRSOffense offense in offenses)
            {
                Offense offenseReport = new Offense();
                offenseReport.Id = ExtractOffenseId(uniqueReportPrefix: uniqueReportPrefix, offenseSeqNum: offense.OffenseSeqNum);
                offenseReport.UcrCode = ExtractNibrsCode(offense);
                offenseReport.CriminalActivityCategoryCodes = ExtractNibrsCriminalActivityCategoryCodes(offense);
                offenseReport.FactorBiasMotivationCodes = TranslateBiasMotivationCodes(uniqueBiasMotivationCodes);
                offenseReport.StructuresEnteredQuantity = offense.Premises.TrimStart('0').TrimNullIfEmpty();
                offenseReport.Factors = TranslateOffenseFactors(uniqueSuspectedOfUsingCodes);
                offenseReport.EntryPoint = offense.MethodOfEntry.TryBuild<OffenseEntryPoint>();
                offenseReport.Forces = ExtractNibrsOffenseForces(offense);
                offenseReport.AttemptedIndicator = ExtractNibrsAttemptedIndicator(offense);
                // todo: ??? Does the FBI want multiple category codes per location or multiple locations with distinct category codes?
                offenseReport.Location = new NibrsReport.Location.Location(categoryCode: offense.LocationType, id: uniqueReportPrefix);
                offenseReport.librsVictimSequenceNumber = offense.OffConnecttoVic.TrimStart('0');
                offenseReports.Add(offenseReport);
            }
            return offenseReports;
        }

        private static string ExtractOffenseId(String uniqueReportPrefix, string offenseSeqNum)
        {
            return uniqueReportPrefix + "Offense" + offenseSeqNum.Trim().TrimStart('0');
        }

        private static string ExtractNibrsCode(LIBRSOffense offense)
        {
            return offense.AgencyAssignedNibrs.HasValue(trim: true) ? offense.AgencyAssignedNibrs : LarsList.LarsDictionary[offense.LrsNumber.Trim()].nibr;
        }

        private static List<string> TranslateBiasMotivationCodes(List<string> biasMotivationCodes)
        {
            return biasMotivationCodes.Select(code => biasMotivationCodeTranslations[code]).ToList();
        }

        private static List<string> ExtractNibrsCriminalActivityCategoryCodes(LIBRSOffense offense)
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
            if (librsCriminalActivity.Trim() == String.Empty)
                return null;
            //Other (X) and Possession with Intent to Sell (I) are only ones translated as P because they are LIBRS only.
            if (librsCriminalActivity.MatchOne(LIBRSErrorConstants.OthCrim,LIBRSErrorConstants.Int))
                return LIBRSErrorConstants.Posses;
            return librsCriminalActivity;
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
            // 99 is the default bias code 
            return librsOnlyBiasMotivationCodes.Contains(offender.BiasMotivation) ? "99" : offender.BiasMotivation;
        }

        private static List<OffenseFactor> TranslateOffenseFactors(List<string> suspectedOfUsingCodes)
        {
            List<OffenseFactor> offenseFactors = new List<OffenseFactor>();
            foreach (string code in suspectedOfUsingCodes)
            {
                //Exception: If LIBRS is G, should be translated to N
                var translatedCode = code == LIBRSErrorConstants.OffGaming ? LIBRSErrorConstants.OffNotApp : code;
                offenseFactors.Add(translatedCode.TryBuild<OffenseFactor>());
            }
            return offenseFactors;
        }

        private static List<OffenseForce> ExtractNibrsOffenseForces(LIBRSOffense offense)
        {
            List<OffenseForce> nibrsOffenseForces = new List<OffenseForce>();
            nibrsOffenseForces.TryAdd(
                offense.WeaponForce1.Trim().TryBuild<OffenseForce>(),
                offense.WeaponForce2.Trim().TryBuild<OffenseForce>(),
                offense.WeaponForce3.Trim().TryBuild<OffenseForce>());
            return nibrsOffenseForces;
        }

        private static string ExtractNibrsAttemptedIndicator(LIBRSOffense offense)
        {
            return offense.AttemptedCompleted == offenseAttemptedCode ? true.ToString().ToLower() : false.ToString().ToLower();
        }
    }
}
