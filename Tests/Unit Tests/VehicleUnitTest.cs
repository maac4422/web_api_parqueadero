namespace Test.UnitTests
{
    using BusinessLayer.BusinessLogic;
    using BusinessLayer.Interfaces;
    using DataAccess.Interfaces;
    using DataAccess.Models;
    using NSubstitute;
    using ParqueaderoApi.Controllers;
    using System;
    using System.Collections.Generic;
    using Test.DataObjects;
    using Xunit;

    public class VehicleUnitTest
    {
        #region Attributes
        private readonly VehiclesController valuesController;
        private readonly IVehicleQuery vehicleQuery;
        private readonly IVehicleBusinessLogic vehicleBusinessLogic;
        private readonly IDateTimeHelper dateTimeHelper;
        private bool isRegisterSuccess;
        private string errorMessage;
        private DateTime fakeDate;
        #endregion

        #region Constructors
        public VehicleUnitTest() {
            valuesController = new VehiclesController();
            vehicleQuery = Substitute.For<IVehicleQuery>();
            dateTimeHelper = Substitute.For<IDateTimeHelper>();
            vehicleBusinessLogic = new VehicleBusinessLogic(vehicleQuery, dateTimeHelper);
            
            isRegisterSuccess = false;
            errorMessage = null;
        }
        #endregion

        #region Tests
        [Fact]
        public void VehicleLicencePlateStartWithALetterThuesday()
        {
            //Arrange
            fakeDate = new DateTime(2018, 05, 15);
            Vehicle vehicle = VehicleDataObject.CreateObjectVehicleLicencePlateWithA();

            //Act
            InitializerMockCheckInVehicle(new List<Vehicle>(), true);
            isRegisterSuccess = vehicleBusinessLogic.TryCheckInVehicle(out errorMessage,vehicle);

            //Assert
            Assert.True(isRegisterSuccess);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void VehicleLicencePlateStartWithALetterMonday()
        {
            //Arrange
            fakeDate = new DateTime(2018, 05, 14);
            Vehicle vehicle = VehicleDataObject.CreateObjectVehicleLicencePlateWithA();

            //Act
            InitializerMockCheckInVehicle(new List<Vehicle>(), false);
            isRegisterSuccess = vehicleBusinessLogic.TryCheckInVehicle(out errorMessage, vehicle);

            //Assert
            Assert.False(isRegisterSuccess);
            Assert.NotEmpty(errorMessage);
        }

        [Fact]
        public void VehicleLicencePlateStartWithALetterSunday()
        {
            //Arrange
            fakeDate = new DateTime(2018, 05, 13);
            Vehicle vehicle = VehicleDataObject.CreateObjectVehicleLicencePlateWithA();

            //Act
            InitializerMockCheckInVehicle(new List<Vehicle>(), false);
            isRegisterSuccess = vehicleBusinessLogic.TryCheckInVehicle(out errorMessage, vehicle);

            //Assert
            Assert.False(isRegisterSuccess);
            Assert.NotEmpty(errorMessage);
        }

        [Fact]
        public void VehicleLicencePlateDontStartWithALetterMonday()
        {
            //Arrange
            fakeDate = new DateTime(2018, 05, 15);
            Vehicle vehicle = VehicleDataObject.CreateCarVehicle();

            //Act
            InitializerMockCheckInVehicle(new List<Vehicle>(), true);
            isRegisterSuccess = vehicleBusinessLogic.TryCheckInVehicle(out errorMessage, vehicle);

            //Assert
            Assert.True(isRegisterSuccess);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void CarSpaceFull()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateCarVehicle();
            List<Vehicle> listCars = VehicleDataObject.CreateListCars();

            //Act
            vehicleQuery.GetAllActiveVehicles(Arg.Any<string>()).Returns(listCars);
            isRegisterSuccess = vehicleBusinessLogic.TryCheckInVehicle(out errorMessage, vehicle);

            //Assert
            Assert.False(isRegisterSuccess);
            Assert.NotEmpty(errorMessage);
        }

        [Fact]
        public void MotorcycleSpaceFull()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateMotorcycleVehicle();
            List<Vehicle> listMotorcycles = VehicleDataObject.CreateListMotorcycles();

            //Act
            vehicleQuery.GetAllActiveVehicles(Arg.Any<string>()).Returns(listMotorcycles);
            isRegisterSuccess = vehicleBusinessLogic.TryCheckInVehicle(out errorMessage, vehicle);

            //Assert
            Assert.False(isRegisterSuccess);
            Assert.NotEmpty(errorMessage);
        }

        [Fact]
        public void CalculatePaymentMotorcycle6Hours()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateMotorcycleActive(new DateTime(2018, 05, 15, 8, 0, 0));
            fakeDate = new DateTime(2018, 05, 15,14,0,0);

            //Act
            InitializerMockCheckOutVehicle(vehicle,true);
            isRegisterSuccess = vehicleBusinessLogic.GetVehicleCheckOut(ref vehicle,out errorMessage);

            //Assert
            Assert.True(isRegisterSuccess);
            Assert.Equal(3000,vehicle.Payment);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void CalculatePaymentMotorcycleHigher500Displacement6Hours()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateMotorcycleDisplacementHigher500Active(new DateTime(2018, 05, 15, 8, 0, 0));
            fakeDate = new DateTime(2018, 05, 15, 14, 0, 0);

            //Act
            InitializerMockCheckOutVehicle(vehicle,true);
            isRegisterSuccess = vehicleBusinessLogic.GetVehicleCheckOut(ref vehicle, out errorMessage);

            //Assert
            Assert.True(isRegisterSuccess);
            Assert.Equal(5000, vehicle.Payment);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void CalculatePaymentCar6Hours()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateCarActive(new DateTime(2018, 05, 15, 8, 0, 0));
            fakeDate = new DateTime(2018, 05, 15, 14, 0, 0);

            //Act
            InitializerMockCheckOutVehicle(vehicle,true);
            isRegisterSuccess = vehicleBusinessLogic.GetVehicleCheckOut(ref vehicle, out errorMessage);

            //Assert
            Assert.True(isRegisterSuccess);
            Assert.Equal(6000, vehicle.Payment);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void CalculatePaymentCar30Minutes()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateCarActive(new DateTime(2018, 05, 15, 8, 0, 0));
            fakeDate = new DateTime(2018, 05, 15, 8, 30, 0);

            //Act
            InitializerMockCheckOutVehicle(vehicle,true);
            isRegisterSuccess = vehicleBusinessLogic.GetVehicleCheckOut(ref vehicle, out errorMessage);

            //Assert
            Assert.True(isRegisterSuccess);
            Assert.Equal(1000, vehicle.Payment);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void CalculatePaymentMotorcycle30Minutes()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateMotorcycleActive(new DateTime(2018, 05, 15, 8, 0, 0));
            fakeDate = new DateTime(2018, 05, 15, 8, 30, 0);

            //Act
            InitializerMockCheckOutVehicle(vehicle,true);
            isRegisterSuccess = vehicleBusinessLogic.GetVehicleCheckOut(ref vehicle, out errorMessage);

            //Assert
            Assert.True(isRegisterSuccess);
            Assert.Equal(500, vehicle.Payment);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void CalculatePaymentMotorcycle8HalfHours()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateMotorcycleActive(new DateTime(2018, 05, 15, 8, 0, 0));
            fakeDate = new DateTime(2018, 05, 15, 16, 30, 0);

            //Act
            InitializerMockCheckOutVehicle(vehicle,true);
            isRegisterSuccess = vehicleBusinessLogic.GetVehicleCheckOut(ref vehicle, out errorMessage);

            //Assert
            Assert.True(isRegisterSuccess);
            Assert.Equal(4000, vehicle.Payment);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void CalculatePaymentCar8HalfHours()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateCarActive(new DateTime(2018, 05, 15, 8, 0, 0));
            fakeDate = new DateTime(2018, 05, 15, 16, 30, 0);

            //Act
            InitializerMockCheckOutVehicle(vehicle,true);
            isRegisterSuccess = vehicleBusinessLogic.GetVehicleCheckOut(ref vehicle, out errorMessage);

            //Assert
            Assert.True(isRegisterSuccess);
            Assert.Equal(8000, vehicle.Payment);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void CalculatePaymentMotorcycle20Hours()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateMotorcycleActive(new DateTime(2018, 05, 15, 0, 0, 0));
            fakeDate = new DateTime(2018, 05, 15, 20, 0, 0);

            //Act
            InitializerMockCheckOutVehicle(vehicle,true);
            isRegisterSuccess = vehicleBusinessLogic.GetVehicleCheckOut(ref vehicle, out errorMessage);

            //Assert
            Assert.True(isRegisterSuccess);
            Assert.Equal(4000, vehicle.Payment);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void CalculatePaymentMotorcycleHigher500Displacement20Hours()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateMotorcycleDisplacementHigher500Active(new DateTime(2018, 05, 15, 0, 0, 0));
            fakeDate = new DateTime(2018, 05, 15, 20, 0, 0);

            //Act
            InitializerMockCheckOutVehicle(vehicle,true);
            isRegisterSuccess = vehicleBusinessLogic.GetVehicleCheckOut(ref vehicle, out errorMessage);

            //Assert
            Assert.True(isRegisterSuccess);
            Assert.Equal(6000, vehicle.Payment);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void CalculatePaymentCar20Hours()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateCarActive(new DateTime(2018, 05, 15, 0, 0, 0));
            fakeDate = new DateTime(2018, 05, 15, 20, 0, 0);

            //Act
            InitializerMockCheckOutVehicle(vehicle,true);
            isRegisterSuccess = vehicleBusinessLogic.GetVehicleCheckOut(ref vehicle, out errorMessage);

            //Assert
            Assert.True(isRegisterSuccess);
            Assert.Equal(8000, vehicle.Payment);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void CalculatePaymentMotorcycle26Hours()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateMotorcycleActive(new DateTime(2018, 05, 15, 0, 0, 0));
            fakeDate = new DateTime(2018, 05, 16, 2, 0, 0);

            //Act
            InitializerMockCheckOutVehicle(vehicle,true);
            isRegisterSuccess = vehicleBusinessLogic.GetVehicleCheckOut(ref vehicle, out errorMessage);

            //Assert
            Assert.True(isRegisterSuccess);
            Assert.Equal(5000, vehicle.Payment);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void CalculatePaymentMotorcycleHigher500Displacement26Hours()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateMotorcycleDisplacementHigher500Active(new DateTime(2018, 05, 15, 0, 0, 0));
            fakeDate = new DateTime(2018, 05, 16, 2, 0, 0);

            //Act
            InitializerMockCheckOutVehicle(vehicle,true);
            isRegisterSuccess = vehicleBusinessLogic.GetVehicleCheckOut(ref vehicle, out errorMessage);

            //Assert
            Assert.True(isRegisterSuccess);
            Assert.Equal(7000, vehicle.Payment);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void CalculatePaymentCar26Hours()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateCarActive(new DateTime(2018, 05, 15, 0, 0, 0));
            fakeDate = new DateTime(2018, 05, 16, 2, 0, 0);

            //Act
            InitializerMockCheckOutVehicle(vehicle,true);
            isRegisterSuccess = vehicleBusinessLogic.GetVehicleCheckOut(ref vehicle, out errorMessage);

            //Assert
            Assert.True(isRegisterSuccess);
            Assert.Equal(10000, vehicle.Payment);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void CalculatePaymentMotorcycle35Hours()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateMotorcycleActive(new DateTime(2018, 05, 15, 0, 0, 0));
            fakeDate = new DateTime(2018, 05, 16, 11, 0, 0);

            //Act
            InitializerMockCheckOutVehicle(vehicle,true);
            isRegisterSuccess = vehicleBusinessLogic.GetVehicleCheckOut(ref vehicle, out errorMessage);

            //Assert
            Assert.True(isRegisterSuccess);
            Assert.Equal(8000, vehicle.Payment);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void CalculatePaymentMotorcycleHigher500Displacement35Hours()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateMotorcycleDisplacementHigher500Active(new DateTime(2018, 05, 15, 0, 0, 0));
            fakeDate = new DateTime(2018, 05, 16, 11, 0, 0);

            //Act
            InitializerMockCheckOutVehicle(vehicle,true);
            isRegisterSuccess = vehicleBusinessLogic.GetVehicleCheckOut(ref vehicle, out errorMessage);

            //Assert
            Assert.True(isRegisterSuccess);
            Assert.Equal(10000, vehicle.Payment);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void CalculatePaymentCar35Hours()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateCarActive(new DateTime(2018, 05, 15, 0, 0, 0));
            fakeDate = new DateTime(2018, 05, 16, 11, 0, 0);

            //Act
            InitializerMockCheckOutVehicle(vehicle,true);
            isRegisterSuccess = vehicleBusinessLogic.GetVehicleCheckOut(ref vehicle, out errorMessage);

            //Assert
            Assert.True(isRegisterSuccess);
            Assert.Equal(16000, vehicle.Payment);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void TryCheckOutInactiveVehicle()
        {
            //Arrange
            Vehicle vehicle = VehicleDataObject.CreateCarInactive(new DateTime(2018, 05, 15, 0, 0, 0));
            fakeDate = new DateTime(2018, 05, 16, 11, 0, 0);

            //Act
            InitializerMockCheckOutVehicle(vehicle,true);
            isRegisterSuccess = vehicleBusinessLogic.GetVehicleCheckOut(ref vehicle, out errorMessage);

            //Assert
            Assert.False(isRegisterSuccess);
            Assert.Equal(0, vehicle.Payment);
            Assert.NotEmpty(errorMessage);
        }

        #endregion

        #region Private Methods
        private void InitializerMockCheckOutVehicle(Vehicle vehicleToReturn, bool willCheckOut)
        {
            dateTimeHelper.GetDateTimeNow().Returns(fakeDate);
            vehicleQuery.GetVehicle(Arg.Any<int>()).Returns(vehicleToReturn);
            vehicleQuery.CheckOutVehicle(Arg.Any<Vehicle>()).Returns(willCheckOut);
        }

        private void InitializerMockCheckInVehicle(List<Vehicle> vehiclesToReturn,  bool willCheckIn)
        {
            dateTimeHelper.GetDateTimeNow().Returns(fakeDate);
            vehicleQuery.GetAllActiveVehicles(Arg.Any<string>()).Returns(vehiclesToReturn);
            vehicleQuery.CheckInVehicle(Arg.Any<Vehicle>()).Returns(willCheckIn);
        }
        #endregion
    }
}
