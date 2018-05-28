namespace DataAccess.Enumerators
{
    public class VehicleEnumerators
    {
        public  enum VehicleTypes
        {
            car,
            motorcycle
        }

        public  enum VehicleCostPerHour
        {
            car = 1000,
            motorcycle = 500
        }

        public  enum VehicleCostPerDay
        {
            car = 8000,
            motorcycle = 4000
        }

        public enum VehicleExtraCost
        {
            motorcycleHigher500Displacement = 2000
        }
    }
}
