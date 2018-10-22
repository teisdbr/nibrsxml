﻿using System.Collections.Generic;
using System.Linq;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.Utility;
using LoadBusinessLayer;
using LoadBusinessLayer.LIBRSOffender;
using LoadBusinessLayer.LIBRSOffense;
using LoadBusinessLayer.LibrsErrorConstants;
using TeUtil.Extensions;

namespace NibrsXml.Builder
{
    internal class OffenseBuilder
    {
        private static string offenseAttemptedCode = "A";
        private static Dictionary<string, string> biasMotivationCodeTranslations = new Dictionary<string, string>
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

        public static List<Offense> Build(List<LIBRSOffense> offenses, List<string> uniqueBiasMotivationCodes, List<string> uniqueSuspectedOfUsingCodes, string uniqueReportPrefix)
        {
            var offenseReports = new List<Offense>();

            // Unique UCR Codes of GROUP A offenses
            var uniqueOffenses = offenses.Where(o => o.OffenseGroup.Equals("A", System.StringComparison.OrdinalIgnoreCase)).GroupBy(lo => lo.AgencyAssignedNibrs);

            foreach (var offense in uniqueOffenses)
            {
                var offenseReport = new Offense();
                offenseReport.Id = ExtractOffenseId(uniqueReportPrefix: uniqueReportPrefix, offenseSeqNum: offense.First().OffenseSeqNum);
                offenseReport.UcrCode = ExtractNibrsCode(offense.First());
                offenseReport.CriminalActivityCategoryCodes = ExtractNibrsCriminalActivityCategoryCodes(offense.First());
                offenseReport.FactorBiasMotivationCodes = TranslateBiasMotivationCodes(uniqueBiasMotivationCodes);
                offenseReport.StructuresEnteredQuantity = offense.First().Premises.TrimStart('0').TrimNullIfEmpty();
                offenseReport.Factors = TranslateOffenseFactors(uniqueSuspectedOfUsingCodes);
                offenseReport.EntryPoint = offense.First().MethodOfEntry.TryBuild<OffenseEntryPoint>();
                offenseReport.Forces = ExtractNibrsOffenseForces(offense.First());
                offenseReport.AttemptedIndicator = ExtractNibrsAttemptedIndicator(offense.First());
                // todo: ??? Does the FBI want multiple category codes per location or multiple locations with distinct category codes?
                offenseReport.Location = new NibrsReport.Location.Location(categoryCode: offense.First().LocationType, id: uniqueReportPrefix);
                offenseReport.librsVictimSequenceNumber = offense.First().OffConnecttoVic.TrimStart('0');
                offenseReports.Add(offenseReport);
            }
            return offenseReports;
        }

        private static string ExtractOffenseId(string uniqueReportPrefix, string offenseSeqNum)
        {
            return uniqueReportPrefix + "Offense" + offenseSeqNum.Trim().TrimStart('0');
        }

        private static string ExtractNibrsCode(LIBRSOffense offense)
        {
            return offense.AgencyAssignedNibrs.HasValue(trim: true) ? offense.AgencyAssignedNibrs : LarsList.LarsDictionary[offense.LrsNumber.Trim()].Nibr;
        }

        private static List<string> TranslateBiasMotivationCodes(List<string> biasMotivationCodes)
        {
            return biasMotivationCodes.Select(code => biasMotivationCodeTranslations[code]).ToList();
        }

        private static List<string> ExtractNibrsCriminalActivityCategoryCodes(LIBRSOffense offense)
        {
            var nibrsCriminalActivityCategoryCodes = new List<string>();
            nibrsCriminalActivityCategoryCodes.TryAdd(
                TranslateCriminalActivityCategoryCode(offense.CriminalActivity1),
                TranslateCriminalActivityCategoryCode(offense.CriminalActivity2),
                TranslateCriminalActivityCategoryCode(offense.CriminalActivity3));

            // Had to do distinct.. 
            return nibrsCriminalActivityCategoryCodes.Distinct().ToList();
        }

        private static string TranslateCriminalActivityCategoryCode(string librsCriminalActivity)
        {
            if (librsCriminalActivity.Trim() == string.Empty)
                return null;
            //Other (X) and Possession with Intent to Sell (I) are only ones translated as P because they are LIBRS only.
            if (librsCriminalActivity.MatchOne(LibrsErrorConstants.OthCrim,LibrsErrorConstants.Int))
                return LibrsErrorConstants.Posses;
            return librsCriminalActivity;
        }

        private static string ExtractNibrsBiasMotivationCode(LIBRSOffender offender)
        {    
            var librsOnlyBiasMotivationCodes = new HashSet<string>
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
            var offenseFactors = new List<OffenseFactor>();
            foreach (var code in suspectedOfUsingCodes)
            {
                //Exception: If LIBRS is G, should be translated to N
                var translatedCode = code == LibrsErrorConstants.OffGaming ? LibrsErrorConstants.OffNotApp : code;
                offenseFactors.Add(translatedCode.TryBuild<OffenseFactor>());
            }
            return offenseFactors;
        }

        private static List<OffenseForce> ExtractNibrsOffenseForces(LIBRSOffense offense)
        {
            var nibrsOffenseForces = new List<OffenseForce>();
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
