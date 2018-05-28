namespace DataAccess.Queries
{
    using DataAccess.Interfaces;
    using DataAccess.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class VehicleQuery : IVehicleQuery
    {

        #region Methods
        public bool CheckInVehicle(Vehicle vehicleModel)
        {
            try
            {
                using (var context = new ParkingContext())
                {
                    var register = new Vehicle
                    {
                        LicencePlate = vehicleModel.LicencePlate,
                        Displacement = vehicleModel.Displacement,
                        VehicleType = vehicleModel.VehicleType,
                        EntryTime = DateTime.Now
                    };
                    context.Vehicles.Add(register);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Vehicle> GetAllActiveVehicles()
        {
            try
            {
                using (var context = new ParkingContext())
                {
                    return context.Vehicles.Where(v => v.State.Equals(0)).ToList();
                }
            }
            catch (Exception)
            {
                return new List<Vehicle>();
            }
        }
        
        public List<Vehicle> GetAllActiveVehicles(string vehicleType)
        {
            try
            {
                using (var context = new ParkingContext())
                {
                    return context.Vehicles.Where(v => 
                                                v.VehicleType.Equals(vehicleType) && 
                                                v.State.Equals(0)).ToList();
                }
            }
            catch (Exception)
            {
                return new List<Vehicle>();
            }
        }

        public Vehicle GetVehicle(int id)
        {
            try
            {
                using (var context = new ParkingContext())
                {
                    return context.Vehicles.FirstOrDefault(v => v.Id == id);
                }
            }
            catch (Exception)
            {
                return new Vehicle();
            }
        }

        public bool CheckOutVehicle(Vehicle vehicle)
        {
            try
            {
                using (var context = new ParkingContext())
                {
                    context.Vehicles.Update(vehicle);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
