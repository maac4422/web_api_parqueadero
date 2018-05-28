namespace BusinessLayer.Interfaces
{
    using DataAccess.Models;
    using System.Collections.Generic;

    public interface IVehicleBusinessLogic
    {
        bool TryCheckInVehicle(out string errorMessage,Vehicle vehicleModel);

        List<Vehicle> GetAllActiveVehicles();

        List<Vehicle> GetAllActiveVehicles(string vehicleType);

        Vehicle GetVehicle(int id);

        bool GetVehicleCheckOut(ref Vehicle vehicle,out string errorMessage );
    }
}
