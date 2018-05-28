namespace Test.IntegrationTest.DataRequest
{
    using DataAccess.Enumerators;
    using DataAccess.Models;
    using System;

    public static class VehicleDataRequest
    {
        public static Vehicle CreateCarVehicle() {
            Vehicle vehicle = new Vehicle();
            vehicle.LicencePlate = "BCD" + (new Random()).Next(100, 1000);
            vehicle.VehicleType = VehicleEnumerators.VehicleTypes.car.ToString();
            return vehicle;
        }
    }
}
