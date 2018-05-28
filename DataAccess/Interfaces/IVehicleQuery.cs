using DataAccess.Models;
using System.Collections.Generic;

namespace DataAccess.Interfaces
{

    public interface IVehicleQuery
    {
        bool CheckInVehicle(Vehicle vehicleModel);

        List<Vehicle> GetAllActiveVehicles(string vehicleType);

        List<Vehicle> GetAllActiveVehicles();

        Vehicle GetVehicle(int id);

        bool CheckOutVehicle(Vehicle vehicle);
        
    }
}
