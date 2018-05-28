namespace BusinessLayer.BusinessLogic
{
    using System;
    using System.Collections.Generic;
    using BusinessLayer.Helpers;
    using BusinessLayer.Interfaces;
    using DataAccess.Constants;
    using DataAccess.Enumerators;
    using DataAccess.Interfaces;
    using DataAccess.Models;
    using DataAccess.Queries;

    public class VehicleBusinessLogic : IVehicleBusinessLogic
    {
        #region Attributes
        private readonly IVehicleQuery vehicleQuery;
        private readonly IDateTimeHelper dateTimeHelper;
        private string errorMessage;
        #endregion

        #region Constructors
        public VehicleBusinessLogic()
        {
            vehicleQuery = new VehicleQuery();
            dateTimeHelper = new DateTimeHelper();
        }

        public VehicleBusinessLogic(IVehicleQuery vehicleQuery,IDateTimeHelper dateTimeHelper)
        {
            this.vehicleQuery = vehicleQuery;
            this.dateTimeHelper = dateTimeHelper;
        }
        #endregion

        #region Interface Methods 
        public List<Vehicle> GetAllActiveVehicles()
        {
            return vehicleQuery.GetAllActiveVehicles();
        }

        public List<Vehicle> GetAllActiveVehicles(string vehicleType)
        {
            return vehicleQuery.GetAllActiveVehicles(vehicleType);
        }

        public Vehicle GetVehicle(int id)
        {
            return vehicleQuery.GetVehicle(id);
        }

        public bool TryCheckInVehicle(out string errorMessage,Vehicle vehicleModel)
        {
            errorMessage = string.Empty;
            if (AreAvailableSpace(vehicleModel) && CanParkingToday(vehicleModel.LicencePlate))
            {
                vehicleModel.EntryTime = vehicleModel.EntryTime.Equals(DateTime.MinValue) ? dateTimeHelper.GetDateTimeNow() : vehicleModel.EntryTime;
                bool vehicleRegisteredSuccess = vehicleQuery.CheckInVehicle(vehicleModel);
                return vehicleRegisteredSuccess;
            }
            errorMessage = this.errorMessage;
            return false;
            
        }

        public bool GetVehicleCheckOut(ref Vehicle vehicle,out string errorMessage)
        {
            errorMessage = string.Empty;
            vehicle = GetVehicle(vehicle.Id);
            if (IsActiveVehicle(vehicle.Id))
            { 
                vehicle.Payment = CalculatePayment(vehicle);
                vehicle.DepartureTime = dateTimeHelper.GetDateTimeNow();
                vehicle.State = VehicleHelper.SetActive();
                bool vehicleRegisteredSuccess = vehicleQuery.CheckOutVehicle(vehicle);
                return vehicleRegisteredSuccess;
            }
            this.errorMessage = Messages.ErrorVehicleAlreadyCheckOut;
            errorMessage = this.errorMessage;
            return false;
        }
        #endregion

        #region Private Methods

        private bool AreAvailableSpace(Vehicle vehicle)
        {
            List<Vehicle> vehicles = vehicleQuery.GetAllActiveVehicles(vehicle.VehicleType);
            if ( (VehicleHelper.IsCar(vehicle) && vehicles.Count.Equals(20)) ||
                (VehicleHelper.IsMotorcycle(vehicle) && vehicles.Count.Equals(10)) )
            {
                errorMessage = Messages.ErrorSpaceFull;
                return false;
            }
            return true;
        }

        private bool CanParkingToday(string licencePlate)
        {
            if (
                (!LicencePlateStartWithA(licencePlate)) || 
                (LicencePlateStartWithA(licencePlate) && !IsTodaySundayOrMonday()) )
            {
                return true;
            }
            errorMessage = Messages.ErrorLicencePlateStartWithA;
            return false;
        }

        private static bool LicencePlateStartWithA(string licencePlate)
        {
            return licencePlate.ToUpperInvariant()[0].Equals('A');
        }

        private bool IsTodaySundayOrMonday()
        {
            DayOfWeek numberDay = dateTimeHelper.GetDateTimeNow().DayOfWeek;
            if (numberDay.Equals(DayOfWeek.Sunday) || numberDay.Equals(DayOfWeek.Monday)) {
                return true;
            }
            return false;
        }

        private int CalculatePayment(Vehicle vehicle)
        {
            int payment = 0;
            
            int differenceHours = Convert.ToInt32(Math.Ceiling((dateTimeHelper.GetDateTimeNow() - vehicle.EntryTime).TotalHours));
            if (differenceHours >= 9)
            {
                payment = CalculatePaymentParkingDays(vehicle,differenceHours);
            }
            else
            {
                payment = GetPaymentParkingHours(vehicle,differenceHours);
            }
            payment = VehicleHelper.IsMotorcycle(vehicle) && vehicle.Displacement > 500 ?
                    payment + VehicleHelper.GetExtraCostMotorcycleHigher500Displacement() :
                    payment;
            return payment;
        }

        private int CalculatePaymentParkingDays(Vehicle vehicle, int differenceHours)
        {
            int totalDays = differenceHours / 24;
            int extraHours = differenceHours % 24;
            int payment = 0;
            if (totalDays > 0)
            {
                payment += GetPaymentParkingDays(vehicle, totalDays);
                if (extraHours >= 9)
                {
                    payment += GetPaymentParkingDays(vehicle, 1);
                }
                else
                {
                    payment += GetPaymentParkingHours(vehicle, extraHours);
                }
            }
            else
            {
                payment += GetPaymentParkingDays(vehicle, 1);
            }
            return payment;
        }

        private static int GetPaymentParkingDays(Vehicle vehicle, int days)
        {
            return days * VehicleHelper.GetCostPerDay(vehicle);
        }

        private static int GetPaymentParkingHours(Vehicle vehicle,int hours)
        {
            return  hours * VehicleHelper.GetCostPerHour(vehicle);
        }

        private bool IsActiveVehicle(int id)
        {
            Vehicle vehicle = GetVehicle(id);
            return vehicle != null && VehicleHelper.IsActive(vehicle);
        }
        #endregion
    }
}
