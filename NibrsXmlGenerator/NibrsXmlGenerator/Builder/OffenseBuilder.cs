using System;
using System.Collections.Generic;
using System.Linq;
using LoadBusinessLayer;
using LoadBusinessLayer.LibrsErrorConstants;
using LoadBusinessLayer.LIBRSOffender;
using LoadBusinessLayer.LIBRSOffense;
using NibrsXml.Constants;
using NibrsXml.NibrsReport.Location;
using NibrsXml.NibrsReport.Offense;
using NibrsXml.Utility;
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
                offenseReport.UcrTags = ucrCodeTagsDictionary.TryGet(offenseReport.UcrCode);
                offenseReport.CriminalActivityCategoryCodes =
                    ExtractNibrsCriminalActivityCategoryCodes(offense.First());
                offenseReport.FactorBiasMotivationCodes = TranslateBiasMotivationCodes(uniqueBiasMotivationCodes);
                offenseReport.StructuresEnteredQuantity = offense.First().Premises.TrimStart('0').TrimNullIfEmpty();
                offenseReport.Factors = TranslateOffenseFactors(uniqueSuspectedOfUsingCodes);
                offenseReport.EntryPoint = offense.First().MethodOfEntry.TryBuild<OffenseEntryPoint>();
                offenseReport.Forces = ExtractNibrsOffenseForces(offense.First(), offenseReport.UcrCode);
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
            foreach (var code in suspectedOfUsingCodes)
            {
                //Exception: If LIBRS is G, should be translated to N
                var translatedCode = code == LibrsErrorConstants.OffGaming ? LibrsErrorConstants.OffNotApp : code;
                offenseFactors.Add(translatedCode.TryBuild<OffenseFactor>());
            }

            return offenseFactors;
        }

        private static List<OffenseForce> ExtractNibrsOffenseForces(LIBRSOffense offense, string nibrsCode)
        {
            // Librs accepts force code "99" for the offenses that doesn't require Weapon/Force code. But FBI (Nibrs) expects the code to be empty.
            // So created a list for the offenses that require force codes, to omit out force code that doesn't require one.
            var offensesRequireForceCode = new HashSet<string>
            {
                 "09A", "09B", "09C", "100", "11A", "11B", "11C", "11D", "120", "13A", "13B", "210", "520", "64A", "64B"                
                 
            };
           
           var nibrsOffenseForces = new List<OffenseForce>();
            if(offensesRequireForceCode.Contains(nibrsCode)){
                nibrsOffenseForces.TryAdd(
                offense.WeaponForce1.Trim().TryBuild<OffenseForce>(),
                offense.WeaponForce2.Trim().TryBuild<OffenseForce>(),
                offense.WeaponForce3.Trim().TryBuild<OffenseForce>());
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

        private static Dictionary<string, string[]> ucrCodeTagsDictionary = new Dictionary<string, string[]>
        {
            {"200", new[] {"Property", "Arson"}},
            {"13A", new[] {"Person", "Assault", "Aggravated"}},
            {"13B", new[] {"Person", "Assault", "Simple"}},
            {"13C", new[] {"Person", "Assault", "Intimidation"}},
            {"510", new[] {"Property", "Bribery"}},
            {"220", new[] {"Property", "Burglary", "Breaking", "Entering"}},
            {"250", new[] {"Property", "Forgery", "Counterfeiting"}},
            {"290", new[] {"Property", "Destruction", "Damage", "Vandalism"}},
            {"35A", new[] {"Society", "Drug", "Narcotic"}},
            {"35B", new[] {"Society", "Drug", "Equipment"}},
            {"270", new[] {"Property", "Embezzlement"}},
            {"210", new[] {"Property", "Extortion", "Blackmail"}},
            {"26A", new[] {"Property", "Fraud", "False Pretenses", "Swindle", "Confidence Game"}},
            {"26B", new[] {"Property", "Fraud", "Credit Card", "ATM"}},
            {"26C", new[] {"Property", "Fraud", "Impersonation"}},
            {"26D", new[] {"Property", "Fraud", "Welfare"}},
            {"26E", new[] {"Property", "Fraud", "Wire"}},
            {"39A", new[] {"Society", "Gambling", "Betting", "Wagering"}},
            {"39B", new[] {"Society", "Gambling", "Operating", "Promoting", "Assisting"}},
            {"39C", new[] {"Society", "Gambling", "Equipment"}},
            {"39D", new[] {"Society", "Gambling", "Sports", "Tampering"}},
            {"09A", new[] {"Person", "Murder", "Homicide", "Nonnegligent", "Manslaughter"}},
            {"09B", new[] {"Person", "Murder", "Homicide", "Negligent", "Manslaughter"}},
            {"09C", new[] {"Person", "Homicide", "Justifiable"}},
            {"100", new[] {"Person", "Kidnapping", "Abduction"}},
            {"23A", new[] {"Property", "Theft", "Pocket-picking"}},
            {"23B", new[] {"Property", "Theft", "Purse-snatching"}},
            {"23C", new[] {"Property", "Theft", "Shoplifting"}},
            {"23D", new[] {"Property", "Theft", "Theft From Building"}},
            {"23E", new[] {"Property", "Theft", "Theft From Coin-Operated Machine or Device"}},
            {"23F", new[] {"Property", "Theft", "Theft From Motor Vehicle"}},
            {"23G", new[] {"Property", "Theft", "Theft of Motor Vehicle Parts or Accessories"}},
            {"23H", new[] {"Property", "Theft", "Larceny"}},
            {"240", new[] {"Property", "Theft", "Motor", "Vehicle"}},
            {"370", new[] {"Society", "Pornography", "Obscene"}},
            {"40A", new[] {"Society", "Prostitution"}},
            {"40B", new[] {"Society", "Prostitution", "Promoting"}},
            {"120", new[] {"Property", "Robbery"}},
            {"11A", new[] {"Person", "Forcible", "Rape"}},
            {"11B", new[] {"Person", "Forcible", "Sodomy"}},
            {"11C", new[] {"Person", "Forcible", "Assault", "Sexual", "Object"}},
            {"11D", new[] {"Person", "Forcible", "Fondling"}},
            {"36A", new[] {"Person", "Incest", "Nonforcible"}},
            {"36B", new[] {"Person", "Rape", "Nonforcible", "Statutory"}},
            {"280", new[] {"Property", "Stolen"}},
            {"520", new[] {"Society", "Weapon"}},
            {"90A", new[] {"Property", "Bad Checks"}},
            {"90B", new[] {"Society", "Curfew", "Loitering", "Vagrancy"}},
            {"90C", new[] {"Society", "Disoderly Conduct"}},
            {"90D", new[] {"Society", "Driving Under Influence"}},
            {"90E", new[] {"Society", "Drunkenness"}},
            {"90F", new[] {"Society", "Family Offenses", "Nonviolent"}},
            {"90G", new[] {"Society", "Liquor Law Violations"}},
            {"90H", new[] {"Society", "Peeping Tom"}},
            {"90J", new[] {"Society", "Trespass Property"}}
        };
    }
}