using System.Collections.Generic;
using NibrsXml.Constants;
using NibrsXml.Utility;

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
    }
}
