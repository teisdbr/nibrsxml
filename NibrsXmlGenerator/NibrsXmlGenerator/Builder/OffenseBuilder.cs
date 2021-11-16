using System;
using System.Collections.Generic;
using System.Linq;
using LoadBusinessLayer;
using LoadBusinessLayer.LibrsErrorConstants;
using LoadBusinessLayer.LIBRSOffender;
using LoadBusinessLayer.LIBRSOffense;
using NibrsModels.Constants;
using NibrsModels.NibrsReport.Location;
using NibrsModels.NibrsReport.Offense;
using NibrsModels.Utility;
using TeUtil.Extensions;


namespace NibrsXml.Builder
{
    internal class OffenseBuilder
    {
        private static readonly string offenseAttemptedCode = "A";

        private static readonly Dictionary<string, string> biasMotivationCodeTranslations =
            new Dictionary<string, string>
            {
                {"11", BiasMotivationCode.ANTIWHITE.NibrsCode()},
                {"12", BiasMotivationCode.ANTIBLACK_AFRICAN_AMERICAN.NibrsCode()},
                {"13", BiasMotivationCode.ANTIAMERICAN_INDIAN_ALASKAN_NATIVE.NibrsCode()},
                {"14", BiasMotivationCode.ANTIASIAN.NibrsCode()},
                {"15", BiasMotivationCode.ANTIMULTIRACIAL_GROUP.NibrsCode()},
                {"16", BiasMotivationCode.ANTINATIVEHAWAIIAN_OTHERPACIFICISLANDER.NibrsCode()},
                {"21", BiasMotivationCode.ANTIJEWISH.NibrsCode()},
                {"22", BiasMotivationCode.ANTICATHOLIC.NibrsCode()},
                {"23", BiasMotivationCode.ANTIPROTESTANT.NibrsCode()},
                {"24", BiasMotivationCode.ANTIISLAMIC.NibrsCode()},
                {"25", BiasMotivationCode.ANTIOTHER_RELIGION.NibrsCode()},
                {"26", BiasMotivationCode.ANTIMULTIRELIGIOUS_GROUP.NibrsCode()},
                {"27", BiasMotivationCode.ANTIATHEIST_AGNOSTIC.NibrsCode()},
                {"28", BiasMotivationCode.ANTIMORMON.NibrsCode()},
                {"29", BiasMotivationCode.ANTIJEHOVAHWITNESS.NibrsCode()},
                {"31", BiasMotivationCode.ANTIARAB.NibrsCode()},
                {"32", BiasMotivationCode.ANTIHISPANIC_LATINO.NibrsCode()},
                {"33", BiasMotivationCode.ANTIOTHER_ETHNICITY_NATIONAL_ORIGIN.NibrsCode()},
                {"41", BiasMotivationCode.ANTIMALE_HOMOSEXUAL.NibrsCode()},
                {"42", BiasMotivationCode.ANTIFEMALE_HOMOSEXUAL.NibrsCode()},
                {"43", BiasMotivationCode.ANTIHOMOSEXUAL.NibrsCode()},
                {"44", BiasMotivationCode.ANTIHETEROSEXUAL.NibrsCode()},
                {"45", BiasMotivationCode.ANTIBISEXUAL.NibrsCode()},
                {"51", BiasMotivationCode.ANTIPHYSICAL_DISABILITY.NibrsCode()},
                {"52", BiasMotivationCode.ANTIMENTAL_DISABILITY.NibrsCode()},
                {"61", BiasMotivationCode.ANTIMALE.NibrsCode()},
                {"62", BiasMotivationCode.ANTIFEMALE.NibrsCode()},
                {"70", BiasMotivationCode.UNKNOWN.NibrsCode()},
                {"71", BiasMotivationCode.UNKNOWN.NibrsCode()},
                {"72", BiasMotivationCode.UNKNOWN.NibrsCode()},
                {"73", BiasMotivationCode.UNKNOWN.NibrsCode()},
                {"74", BiasMotivationCode.UNKNOWN.NibrsCode()},
                {"81", BiasMotivationCode.ANTIEASTERNORTHODOX.NibrsCode()},
                {"82", BiasMotivationCode.ANTIOTHER_CHRISTIAN.NibrsCode()},
                {"83", BiasMotivationCode.ANTIBUDDHIST.NibrsCode()},
                {"84", BiasMotivationCode.ANTIHINDU.NibrsCode()},
                {"85", BiasMotivationCode.ANTISIKH.NibrsCode()},
                {"88", BiasMotivationCode.NONE.NibrsCode()},
                {"99", BiasMotivationCode.UNKNOWN.NibrsCode()}
            };

        public static List<Offense> Build(List<LIBRSOffense> offenses, List<string> uniqueBiasMotivationCodes,
            List<string> uniqueSuspectedOfUsingCodes, string uniqueReportPrefix)
        {
            var offenseReports = new List<Offense>();

            // Unique UCR Codes of GROUP A offenses
            var uniqueOffenses = offenses.Where(o => o.OffenseGroup.Equals("A", StringComparison.OrdinalIgnoreCase))
                .GroupBy(lo => lo.AgencyAssignedNibrs);

            foreach (var offense in uniqueOffenses)
            {
                var offenseReport = new Offense();
                offenseReport.Id = ExtractOffenseId(uniqueReportPrefix, offense.First().OffenseSeqNum);
                offenseReport.UcrCode = ExtractNibrsCode(offense.First());
                offenseReport.CriminalActivityCategoryCodes =
                    ExtractNibrsCriminalActivityCategoryCodes(offenses, offenseReport.UcrCode);
                offenseReport.FactorBiasMotivationCodes = TranslateBiasMotivationCodes(uniqueBiasMotivationCodes);
                offenseReport.StructuresEnteredQuantity = offense.First().Premises.TrimStart('0').TrimNullIfEmpty();
                offenseReport.Factors = TranslateOffenseFactors(uniqueSuspectedOfUsingCodes);
                offenseReport.EntryPoint = offense.First().MethodOfEntry.TryBuild<OffenseEntryPoint>();
                offenseReport.Forces = ExtractNibrsOffenseForces(offenses, offenseReport.UcrCode);
                offenseReport.AttemptedIndicator = ExtractNibrsAttemptedIndicator(offense.First());
                // todo: ??? Does the FBI want multiple category codes per location or multiple locations with distinct category codes?
                offenseReport.Location = new Location(offense.First().LocationType, uniqueReportPrefix);
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
            return offense.AgencyAssignedNibrs.HasValue(true)
                ? offense.AgencyAssignedNibrs
                : LarsList.LarsDictionaryBuildNibrsXmlForUcrExtract[offense.LrsNumber.Trim()].Nibr;
        }

        private static List<string> TranslateBiasMotivationCodes(List<string> biasMotivationCodes)
        {
            //todo: what if None (88) or Unknown (99) exists together (Mutually Exclusive), return Unknown (99) for now. 
            if (biasMotivationCodes.Any(code => code == "88") && biasMotivationCodes.Any(code => code == "99")) return new List<string> { biasMotivationCodeTranslations["99"] };
            else if (biasMotivationCodes.Any(code => code == "88")) return new List<string> { biasMotivationCodeTranslations["88"] };
            else if ((biasMotivationCodes.Any(code => code == "99"))) return new List<string> { biasMotivationCodeTranslations["99"] };

            return biasMotivationCodes.Select(code => biasMotivationCodeTranslations[code]).ToList();
        }

        private static List<string> ExtractNibrsCriminalActivityCategoryCodes(List<LIBRSOffense> offenses, string nibrsCode)
        {
            var nibrsCriminalActivityCategoryCodes = new List<string>();
            var forces = new List<string>();
            offenses = offenses.Where(of => of.AgencyAssignedNibrs == nibrsCode).ToList();
            offenses.ForEach(off => forces.UniqueAdd(off.CriminalActivity1.Trim()));
            offenses.ForEach(off => forces.UniqueAdd(off.CriminalActivity2.Trim()));
            offenses.ForEach(off => forces.UniqueAdd(off.CriminalActivity3.Trim()));
            nibrsCriminalActivityCategoryCodes.AddRange(forces.Take(3).Select(cr => TranslateCriminalActivityCategoryCode(cr)));
            // Had to do distinct.. 
            return nibrsCriminalActivityCategoryCodes.Distinct().ToList();
        }

        private static string TranslateCriminalActivityCategoryCode(string librsCriminalActivity)
        {
            if (librsCriminalActivity.Trim() == string.Empty)
                return null;
            //Other (X) and Possession with Intent to Sell (I) are only ones translated as P because they are LIBRS only.
            if (librsCriminalActivity.MatchOne(LibrsErrorConstants.OthCrim, LibrsErrorConstants.Int))
                return LibrsErrorConstants.Posses;
            return librsCriminalActivity;
        }

        private static string ExtractNibrsBiasMotivationCode(LIBRSOffender offender)
        {
            var librsOnlyBiasMotivationCodes = new HashSet<string>
            {
                "70", //Age
                "71", //Ancestry
                "72", //Creed
                "73", //Gender
                "74" //Organizational Affiliation
            };
            // 99 is the default bias code 
            return librsOnlyBiasMotivationCodes.Contains(offender.BiasMotivation) ? "99" : offender.BiasMotivation;
        }

        private static List<OffenseFactor> TranslateOffenseFactors(List<string> suspectedOfUsingCodes)
        {
            var offenseFactors = new List<OffenseFactor>();

            //Exception: If LIBRS is G, should be translated to N.
            // As N is mutually Exclusive with all other codes, if G or N are appread return N.
            if (suspectedOfUsingCodes.Any(code => code == LibrsErrorConstants.OffGaming || code == LibrsErrorConstants.OffNotApp))
            {
                offenseFactors.Add(LibrsErrorConstants.OffNotApp.TryBuild<OffenseFactor>());
                return offenseFactors;
            }

            foreach (var code in suspectedOfUsingCodes)
            {

                offenseFactors.Add(code.TryBuild<OffenseFactor>());
            }

            return offenseFactors;
        }

        private static List<OffenseForce> ExtractNibrsOffenseForces(List<LIBRSOffense> offenses, string nibrsCode)
        {
            // Librs accepts force code "99" for the offenses that doesn't require Weapon/Force code. But FBI (Nibrs) expects the code to be empty.
            // So created a list for the offenses that require force codes, to omit out force code that doesn't require one.
            var offensesRequireForceCode = new HashSet<string>
            {
                 "09A", "09B", "09C", "100", "11A", "11B", "11C", "11D", "120", "13A", "13B", "210", "520", "64A", "64B"

            };

            var nibrsOffenseForces = new List<OffenseForce>();
            if (offensesRequireForceCode.Contains(nibrsCode))
            {
                var forces = new List<string>();
                offenses = offenses.Where(of => of.AgencyAssignedNibrs == nibrsCode).ToList();
                offenses.ForEach(off => forces.UniqueAdd(off.WeaponForce1.Trim()));
                offenses.ForEach(off => forces.UniqueAdd(off.WeaponForce2.Trim()));
                offenses.ForEach(off => forces.UniqueAdd(off.WeaponForce3.Trim()));
                nibrsOffenseForces.AddRange(forces.Take(3).Select(wp => wp.TryBuild<OffenseForce>()));
                return nibrsOffenseForces;
            }
            return null;
        }

        private static string ExtractNibrsAttemptedIndicator(LIBRSOffense offense)
        {
            return offense.AttemptedCompleted == offenseAttemptedCode
                ? true.ToString().ToLower()
                : false.ToString().ToLower();
        }


    }
}