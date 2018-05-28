namespace BusinessLayer.Helpers
{
    using DataAccess.Enumerators;
    using DataAccess.Models;

    public static class VehicleHelper
    {
        #region Methods
        public static bool IsActive(Vehicle vehicle)
        {
            return vehicle.State.Equals(0);
        }

        public static bool IsCar(Vehicle vehicle)
        {
            return vehicle.VehicleType.Equals(VehicleEnumerators.VehicleTypes.car.ToString());
        }

        public static bool IsMotorcycle(Vehicle vehicle)
        {
            return vehicle.VehicleType.Equals(VehicleEnumerators.VehicleTypes.motorcycle.ToString());
        }

        public static int GetCostPerDay(Vehicle vehicle)
        {
            return IsMotorcycle(vehicle) ? (int)VehicleEnumerators.VehicleCostPerDay.motorcycle : (int)VehicleEnumerators.VehicleCostPerDay.car;
        }

        public static int GetCostPerHour(Vehicle vehicle)
        {
            return IsMotorcycle(vehicle) ? (int)VehicleEnumerators.VehicleCostPerHour.motorcycle : (int)VehicleEnumerators.VehicleCostPerHour.car;
        }

        public static int GetExtraCostMotorcycleHigher500Displacement()
        {
            return (int)VehicleEnumerators.VehicleExtraCost.motorcycleHigher500Displacement;
        }

        public static int SetActive()
        {
            return 1;
        }
        #endregion
    }
}
