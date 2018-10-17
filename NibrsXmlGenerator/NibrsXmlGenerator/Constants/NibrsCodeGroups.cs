using NibrsXml.Utility;

namespace NibrsXml.Constants
{
    public static class NibrsCodeGroups
    {
        public static readonly string[] VehicleProperties =
        {
            PropertyCategoryCode.AUTOMOBILE.NibrsCode(),
            PropertyCategoryCode.TRUCKS.NibrsCode(),
            PropertyCategoryCode.RECREATIONAL_VEHICLES.NibrsCode(),
            PropertyCategoryCode.BUSES.NibrsCode(),
            PropertyCategoryCode.OTHER_MOTOR_VEHICLES.NibrsCode()
        };

        public static readonly string[] TheftOffenseFactorCodes =
        {
            OffenseCode.ROBBERY.NibrsCode(),
            OffenseCode.EXTORTION_BLACKMAIL.NibrsCode(),
            OffenseCode.BURGLARY_BREAKING_AND_ENTERING.NibrsCode(),
            OffenseCode.THEFT_FROM_BUILDING.NibrsCode(),
            OffenseCode.THEFT_FROM_MOTOR_VEHICLE.NibrsCode(),
            OffenseCode.OTHER_LARCENY.NibrsCode(),
            OffenseCode.MOTOR_VEHICLE_THEFT.NibrsCode(),
            OffenseCode.FALSE_PRETENSES_SWINDLE_CONFIDENCE_GAME.NibrsCode(),
            OffenseCode.CREDIT_CARD_FRAUD.NibrsCode(),
            OffenseCode.IMPERSONATION.NibrsCode(),
            OffenseCode.WIRE_FRAUD.NibrsCode(),
            OffenseCode.IDENTITY_THEFT.NibrsCode(),
            OffenseCode.HACKING_COMPUTER_INVASION.NibrsCode(),
            OffenseCode.EMBEZZLEMENT.NibrsCode(),
            OffenseCode.BRIBERY.NibrsCode()
        };
    }
}