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
    }
}