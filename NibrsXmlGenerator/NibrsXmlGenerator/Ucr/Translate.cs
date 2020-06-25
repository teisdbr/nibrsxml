using System.Collections.Generic;
using NibrsModels.Constants;
using NibrsModels.Utility;
using NibrsXml.Constants;
using NibrsXml.Constants.Ucr;
using NibrsXml.Utility;
using Util.Extensions;

namespace NibrsXml.Ucr
{
    public static class Translate
    {
        public static readonly Dictionary<string, string> HateCrimeBiasMotivationTranslations = new Dictionary<string, string>
        {
            { BiasMotivationCode.ANTIAMERICAN_INDIAN_ALASKAN_NATIVE.NibrsCode(), "13" },
            { BiasMotivationCode.ANTIARAB.NibrsCode(), "31" },
            { BiasMotivationCode.ANTIASIAN.NibrsCode(), "14" },
            { BiasMotivationCode.ANTIATHEIST_AGNOSTIC.NibrsCode(), "27" },
            { BiasMotivationCode.ANTIBISEXUAL.NibrsCode(), "45" },
            { BiasMotivationCode.ANTIBLACK_AFRICAN_AMERICAN.NibrsCode(), "12" },
            { BiasMotivationCode.ANTIBUDDHIST.NibrsCode(), "83" },
            { BiasMotivationCode.ANTICATHOLIC.NibrsCode(), "22" },
            { BiasMotivationCode.ANTIEASTERNORTHODOX.NibrsCode(), "81" },
            { BiasMotivationCode.ANTIFEMALE.NibrsCode(), "62" },
            { BiasMotivationCode.ANTIFEMALE_HOMOSEXUAL.NibrsCode(), "42" },
            { BiasMotivationCode.ANTIGENDER_NONCONFORMING.NibrsCode(), "72" },
            { BiasMotivationCode.ANTIHETEROSEXUAL.NibrsCode(), "44" },
            { BiasMotivationCode.ANTIHINDU.NibrsCode(), "84" },
            { BiasMotivationCode.ANTIHISPANIC_LATINO.NibrsCode(), "32" },
            { BiasMotivationCode.ANTIHOMOSEXUAL.NibrsCode(), "43" },
            { BiasMotivationCode.ANTIISLAMIC.NibrsCode(), "24" },
            { BiasMotivationCode.ANTIJEHOVAHWITNESS.NibrsCode(), "29" },
            { BiasMotivationCode.ANTIJEWISH.NibrsCode(), "21" },
            { BiasMotivationCode.ANTIMALE.NibrsCode(), "61" },
            { BiasMotivationCode.ANTIMALE_HOMOSEXUAL.NibrsCode(), "41" },
            { BiasMotivationCode.ANTIMENTAL_DISABILITY.NibrsCode(), "52" },
            { BiasMotivationCode.ANTIMORMON.NibrsCode(), "28" },
            { BiasMotivationCode.ANTIMULTIRACIAL_GROUP.NibrsCode(), "15" },
            { BiasMotivationCode.ANTIMULTIRELIGIOUS_GROUP.NibrsCode(), "26" },
            { BiasMotivationCode.ANTINATIVEHAWAIIAN_OTHERPACIFICISLANDER.NibrsCode(), "16" },
            { BiasMotivationCode.ANTIOTHER_CHRISTIAN.NibrsCode(), "82" },
            { BiasMotivationCode.ANTIOTHER_ETHNICITY_NATIONAL_ORIGIN.NibrsCode(), "33" },
            { BiasMotivationCode.ANTIOTHER_RELIGION.NibrsCode(), "25" },
            { BiasMotivationCode.ANTIPHYSICAL_DISABILITY.NibrsCode(), "51" },
            { BiasMotivationCode.ANTIPROTESTANT.NibrsCode(), "23" },
            { BiasMotivationCode.ANTISIKH.NibrsCode(), "85" },
            { BiasMotivationCode.ANTITRANSGENDER.NibrsCode(), "71" },
            { BiasMotivationCode.ANTIWHITE.NibrsCode(), "11" }
        };

        public static readonly Dictionary<string, string> HateCrimeOffenseCodeTranslations = new Dictionary<string, string>
        {
            { OffenseCode.MURDER_NONNEGLIGENT.NibrsCode(), "01" },
            { OffenseCode.RAPE.NibrsCode(), "02" },
            { OffenseCode.ROBBERY.NibrsCode(), "03" },
            { OffenseCode.AGGRAVATED_ASSAULT.NibrsCode(), "04" },
            { OffenseCode.BURGLARY_BREAKING_AND_ENTERING.NibrsCode(), "05" },
            { OffenseCode.PICKPOCKETING.NibrsCode(), "06" },
            { OffenseCode.PURSE_SNATCHING.NibrsCode(), "06" },
            { OffenseCode.SHOPLIFTING.NibrsCode(), "06" },
            { OffenseCode.THEFT_FROM_BUILDING.NibrsCode(), "06" },
            { OffenseCode.THEFT_FROM_COIN_OPERATED_MACHINE.NibrsCode(), "06" },
            { OffenseCode.THEFT_FROM_MOTOR_VEHICLE.NibrsCode(), "06" },
            { OffenseCode.THEFT_OF_MOTOR_VEHICLE_PARTS_OR_ACCESSORIES.NibrsCode(), "06" },
            { OffenseCode.MOTOR_VEHICLE_THEFT.NibrsCode(), "07" },
            { OffenseCode.ARSON.NibrsCode(), "08" },
            { OffenseCode.SIMPLE_ASSAULT.NibrsCode(), "09" },
            { OffenseCode.INTIMIDATION.NibrsCode(), "10" },
            { OffenseCode.DESTRUCTION_DAMAGE_VANDALISM_OR_PROPERTY.NibrsCode(), "11" },
            { OffenseCode.HUMAN_TRAFFICKING_COMMERCIAL_SEX_ACTS.NibrsCode(), "12" },
            { OffenseCode.HUMAN_TRAFFICKING_INVOLUNTARY_SERVITUDE.NibrsCode(), "13" }
        };

        private static readonly Dictionary<string, string> SupplementaryHomicideRelationshipDirectTranslations = new Dictionary<string, string>
        {
            { VictimToSubjectRelationshipCode.Family_Member.NibrsCode(), VictimOffenderRelationship.OtherFamily },
            { VictimToSubjectRelationshipCode.Child_of_Boyfriend_Girlfriend.NibrsCode(), VictimOffenderRelationship.OtherFamily },
            { VictimToSubjectRelationshipCode.Family_Member_Uncle.NibrsCode(), VictimOffenderRelationship.OtherFamily },
            { VictimToSubjectRelationshipCode.Family_Member_Aunt.NibrsCode(), VictimOffenderRelationship.OtherFamily },
            { VictimToSubjectRelationshipCode.Family_Member_Foster_Child.NibrsCode(), VictimOffenderRelationship.OtherFamily },
            { VictimToSubjectRelationshipCode.Family_Member_Foster_Parent.NibrsCode(), VictimOffenderRelationship.OtherFamily },
            { VictimToSubjectRelationshipCode.Family_Member_Cousin.NibrsCode(), VictimOffenderRelationship.OtherFamily },
            { VictimToSubjectRelationshipCode.Family_Member_Grandchild.NibrsCode(), VictimOffenderRelationship.OtherFamily },
            { VictimToSubjectRelationshipCode.Family_Member_Grandparent.NibrsCode(), VictimOffenderRelationship.OtherFamily },
            { VictimToSubjectRelationshipCode.Family_Member_Nephew.NibrsCode(), VictimOffenderRelationship.OtherFamily },
            { VictimToSubjectRelationshipCode.Family_Member_Niece.NibrsCode(), VictimOffenderRelationship.OtherFamily },
            { VictimToSubjectRelationshipCode.Family_Member_Stepsibling.NibrsCode(), VictimOffenderRelationship.OtherFamily },
            { VictimToSubjectRelationshipCode.Neighbor.NibrsCode(), VictimOffenderRelationship.Neighbor },
            { VictimToSubjectRelationshipCode.Acquaintance.NibrsCode(), VictimOffenderRelationship.Acquaintance },
            { VictimToSubjectRelationshipCode.Boyfriend.NibrsCode(), VictimOffenderRelationship.Boyfriend },
            { VictimToSubjectRelationshipCode.Girlfriend.NibrsCode(), VictimOffenderRelationship.Girlfriend },
            { VictimToSubjectRelationshipCode.Employee.NibrsCode(), VictimOffenderRelationship.Employee },
            { VictimToSubjectRelationshipCode.Employer.NibrsCode(), VictimOffenderRelationship.Employer },
            { VictimToSubjectRelationshipCode.Friend.NibrsCode(), VictimOffenderRelationship.Friend },
            { VictimToSubjectRelationshipCode.Homosexual_relationship.NibrsCode(), VictimOffenderRelationship.HomosexualRelationship },
            { VictimToSubjectRelationshipCode.NonFamily_Otherwise_Known.NibrsCode(), VictimOffenderRelationship.OtherKnown },
            { VictimToSubjectRelationshipCode.Accomplice.NibrsCode(), VictimOffenderRelationship.OtherKnown },
            { VictimToSubjectRelationshipCode.Babysittee.NibrsCode(), VictimOffenderRelationship.OtherKnown },
            { VictimToSubjectRelationshipCode.Babysitter.NibrsCode(), VictimOffenderRelationship.OtherKnown },
            { VictimToSubjectRelationshipCode.Caregiver.NibrsCode(), VictimOffenderRelationship.OtherKnown },
            { VictimToSubjectRelationshipCode.Client.NibrsCode(), VictimOffenderRelationship.OtherKnown },
            { VictimToSubjectRelationshipCode.Cohabitant.NibrsCode(), VictimOffenderRelationship.OtherKnown },
            { VictimToSubjectRelationshipCode.Authority_Figure.NibrsCode(), VictimOffenderRelationship.OtherKnown },
            { VictimToSubjectRelationshipCode.Former_Employee.NibrsCode(), VictimOffenderRelationship.OtherKnown },
            { VictimToSubjectRelationshipCode.Former_Employer.NibrsCode(), VictimOffenderRelationship.OtherKnown },
            { VictimToSubjectRelationshipCode.Guardian.NibrsCode(), VictimOffenderRelationship.OtherKnown },
            { VictimToSubjectRelationshipCode.Patient.NibrsCode(), VictimOffenderRelationship.OtherKnown },
            { VictimToSubjectRelationshipCode.Student.NibrsCode(), VictimOffenderRelationship.OtherKnown },
            { VictimToSubjectRelationshipCode.Teacher.NibrsCode(), VictimOffenderRelationship.OtherKnown },
            { VictimToSubjectRelationshipCode.Victim_Was_Offender.NibrsCode(), VictimOffenderRelationship.OtherKnown },
            { VictimToSubjectRelationshipCode.Stranger.NibrsCode(), VictimOffenderRelationship.Stranger },
            { VictimToSubjectRelationshipCode.Relationship_Unknown.NibrsCode(), VictimOffenderRelationship.UnknownRelationship },
            { VictimToSubjectRelationshipCode.Delivery_Person.NibrsCode(), VictimOffenderRelationship.UnknownRelationship }
        };

        private static readonly Dictionary<string, Dictionary<string, string>> SupplementaryHomicideRelationshipComplexTranslations = new Dictionary<string, Dictionary<string, string>>
        {
            { SexCode.MALE.NibrsCode(), new Dictionary<string, string>
            {
                { VictimToSubjectRelationshipCode.Family_Member_Spouse.NibrsCode(), VictimOffenderRelationship.Husband },
                { VictimToSubjectRelationshipCode.Family_Member_Spouse_Common_Law.NibrsCode(), VictimOffenderRelationship.CommonLawHusband },
                { VictimToSubjectRelationshipCode.Family_Member_Sibling.NibrsCode(), VictimOffenderRelationship.Brother },
                { VictimToSubjectRelationshipCode.Family_Member_Parent.NibrsCode(), VictimOffenderRelationship.Father },
                { VictimToSubjectRelationshipCode.Family_Member_Child.NibrsCode(), VictimOffenderRelationship.Son },
                { VictimToSubjectRelationshipCode.Family_Member_Stepparent.NibrsCode(), VictimOffenderRelationship.Stepfather },
                { VictimToSubjectRelationshipCode.Ex_Spouse.NibrsCode(), VictimOffenderRelationship.ExHusband }
            }},
            { SexCode.FEMALE.NibrsCode(), new Dictionary<string, string>
            {
                { VictimToSubjectRelationshipCode.Family_Member_Spouse.NibrsCode(), VictimOffenderRelationship.Wife },
                { VictimToSubjectRelationshipCode.Family_Member_Spouse_Common_Law.NibrsCode(), VictimOffenderRelationship.CommonLawHusband },
                { VictimToSubjectRelationshipCode.Family_Member_Sibling.NibrsCode(), VictimOffenderRelationship.Sister },
                { VictimToSubjectRelationshipCode.Family_Member_Parent.NibrsCode(), VictimOffenderRelationship.Mother },
                { VictimToSubjectRelationshipCode.Family_Member_Child.NibrsCode(), VictimOffenderRelationship.Daughter },
                { VictimToSubjectRelationshipCode.Family_Member_Stepparent.NibrsCode(), VictimOffenderRelationship.Stepmother },
                { VictimToSubjectRelationshipCode.Ex_Spouse.NibrsCode(), VictimOffenderRelationship.ExWife }
            }}
        };

        public static string TranslateHateCrimeLocationCode(string locationCategoryCode)
        {
            //22 in NIBRS is "SCHOOL_COLLEGE", 52 "SCHOOL_COLLEGE_UNIVERSITY"
            //Ucr does not have code 22 in its scope, but it does for 52
            //All other codes are consistent for both systems
            return locationCategoryCode == "22" ? "52" : locationCategoryCode;
        }

        public static string TranslateSupplementaryHomicideRelationship(string nibrsRelationshipCode, string nibrsSexCode)
        {
            if (nibrsSexCode == SexCode.UNKNOWN.NibrsCode() || nibrsRelationshipCode == null)
                return VictimOffenderRelationship.UnknownRelationship;

            return SupplementaryHomicideRelationshipDirectTranslations.TryGet(nibrsRelationshipCode) ??
                SupplementaryHomicideRelationshipComplexTranslations[nibrsSexCode]
                    .TryGet(nibrsRelationshipCode) ?? VictimOffenderRelationship.UnknownRelationship;
        }

        public static string TranslateSupplementaryHomicideWeaponForceCode(string forceCategoryCode)
        {
            //Ucr force codes have no concept of automatic firearms
            forceCategoryCode = forceCategoryCode.Replace("A", "");

            var nibrsOnlyForceCodes = new[]
            {
                ForceCategoryCode.MOTOR_VEHICLE.NibrsCode(),
                ForceCategoryCode.UNKNOWN.NibrsCode(),
                ForceCategoryCode.NONE.NibrsCode()
            };

            return forceCategoryCode.MatchOne(nibrsOnlyForceCodes)
                ? ForceCategoryCode.OTHER.NibrsCode()
                : forceCategoryCode;
        }
    }
}