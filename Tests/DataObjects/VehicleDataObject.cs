namespace Test.DataObjects
{
    using DataAccess.Enumerators;
    using DataAccess.Models;
    using System;
    using System.Collections.Generic;

    public static class VehicleDataObject
    {
        #region CarMethods

        public static Vehicle CreateCarVehicle()
        {
            Vehicle vehicle = new Vehicle();
            vehicle.Id = 1;
            vehicle.LicencePlate = "BCS523";
            vehicle.VehicleType = VehicleEnumerators.VehicleTypes.car.ToString();

            return vehicle;
        }

        public static Vehicle CreateCarActive(DateTime entryTime)
        {
            Vehicle vehicle = new Vehicle();
            vehicle.Id = 1;
            vehicle.LicencePlate = "BCS523";
            vehicle.VehicleType = VehicleEnumerators.VehicleTypes.car.ToString();
            vehicle.EntryTime = entryTime;
            vehicle.State = 0;

            return vehicle;
        }

        public static Vehicle CreateCarInactive(DateTime entryTime)
        {
            Vehicle vehicle = new Vehicle();
            vehicle.Id = 1;
            vehicle.LicencePlate = "BCS523";
            vehicle.VehicleType = VehicleEnumerators.VehicleTypes.car.ToString();
            vehicle.EntryTime = entryTime;
            vehicle.DepartureTime = entryTime;
            vehicle.State = 1;

            return vehicle;
        }

        public static Vehicle CreateObjectVehicleLicencePlateWithA()
        {
            Vehicle vehicle = new Vehicle();
            vehicle.Id = 1;
            vehicle.LicencePlate = "ACS523";
            vehicle.VehicleType = VehicleEnumerators.VehicleTypes.car.ToString();

            return vehicle;
        }

        public static List<Vehicle> CreateListCars()
        {
            List<Vehicle> listCars = new List<Vehicle>();
            for (int i = 0; i < 20; i++)
            {
                int licencePlateNumber = (new Random()).Next(100, 1000);
                Vehicle carToAdd = CreateDynamicVehicle(
                    "ABC" + licencePlateNumber,
                    VehicleEnumerators.VehicleTypes.car.ToString()
                    );
                listCars.Add(carToAdd);
            }
            return listCars;
        }

        #endregion

        #region MotorycleMethods
        public static Vehicle CreateMotorcycleVehicle()
        {
            Vehicle vehicle = new Vehicle();
            vehicle.Id = 1;
            vehicle.LicencePlate = "BCS523";
            vehicle.VehicleType = VehicleEnumerators.VehicleTypes.motorcycle.ToString();

            return vehicle;
        }

        public static Vehicle CreateMotorcycleActive(DateTime entryTime)
        {
            Vehicle vehicle = new Vehicle();
            vehicle.Id = 1;
            vehicle.LicencePlate = "BCS523";
            vehicle.VehicleType = VehicleEnumerators.VehicleTypes.motorcycle.ToString();
            vehicle.EntryTime = entryTime;
            vehicle.State = 0;
            vehicle.Displacement = 250;

            return vehicle;
        }

        public static Vehicle CreateMotorcycleDisplacementHigher500Active(DateTime entryTime)
        {
            Vehicle vehicle = new Vehicle();
            vehicle.Id = 1;
            vehicle.LicencePlate = "BCS523";
            vehicle.VehicleType = VehicleEnumerators.VehicleTypes.motorcycle.ToString();
            vehicle.EntryTime = entryTime;
            vehicle.State = 0;
            vehicle.Displacement = 650;

            return vehicle;
        }

        public static List<Vehicle> CreateListMotorcycles()
        {
            List<Vehicle> listMotorcycles = new List<Vehicle>();
            for (int i = 0; i < 10; i++)
            {
                int licencePlateNumber = (new Random()).Next(10, 99);
                Vehicle motorcycleToAdd = CreateDynamicVehicle(
                    "ABC" + licencePlateNumber + "D",
                    VehicleEnumerators.VehicleTypes.motorcycle.ToString()
                    );
                listMotorcycles.Add(motorcycleToAdd);
            }
            return listMotorcycles;
        }

        #endregion

        #region Transversal Methods
        private static Vehicle CreateDynamicVehicle(string licencePlate, string vehicleType) {
            Vehicle vehicle = new Vehicle();
            vehicle.LicencePlate = licencePlate;
            vehicle.VehicleType = vehicleType;

            return vehicle;
        }
        #endregion
    }
}
