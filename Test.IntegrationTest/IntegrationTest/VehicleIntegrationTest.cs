namespace Test.IntegrationTest
{
    using DataAccess.Enumerators;
    using DataAccess.Models;
    using Newtonsoft.Json;
    using ParqueaderoApi.Controllers;
    using Test.IntegrationTest.DataRequest;
    using Xunit;

    public class VehicleIntegrationTest
    {
        private readonly VehiclesController valuesController;

        public VehicleIntegrationTest()
        {
            //Arrange
            valuesController = new VehiclesController();
        }

        //[Fact]
        public void GetAllVehicles()
        {
            //Arrange
            var result = valuesController.Get();
            //Act
            string json = JsonConvert.SerializeObject(result);
            Response response = JsonConvert.DeserializeObject<Response>(json);
            //Assert
            Assert.Equal(200,response.StatusCode);
        }

        //[Fact]
        public void GetAllCarActives()
        {
            //Arrange
            var result = valuesController.Get(VehicleEnumerators.VehicleTypes.car.ToString());
            //Act
            string json = JsonConvert.SerializeObject(result);
            Response response = JsonConvert.DeserializeObject<Response>(json);
            //Assert
            Assert.Equal(200, response.StatusCode);
        }

        //[Fact]
        public void GetAllMotorcyclesActives()
        {
            //Arrange
            var result = valuesController.Get(VehicleEnumerators.VehicleTypes.motorcycle.ToString());
            //Act
            string json = JsonConvert.SerializeObject(result);
            Response response = JsonConvert.DeserializeObject<Response>(json);
            //Assert
            Assert.Equal(200, response.StatusCode);
        }

        //[Fact]
        public void RegisterVehicle()
        {
            //Arrange
            Vehicle vehicle = VehicleDataRequest.CreateCarVehicle();

            //Act
            var result = valuesController.Post(vehicle);
            string json = JsonConvert.SerializeObject(result);
            Response response = JsonConvert.DeserializeObject<Response>(json);
            //Assert
            Assert.Equal(200, response.StatusCode);
        }

        //[Fact]
        public void GetVehicle()
        {
            //Act
            var result = valuesController.Get(1);
            string json = JsonConvert.SerializeObject(result);
            Response response = JsonConvert.DeserializeObject<Response>(json);
            //Assert
            Assert.Equal(200, response.StatusCode);
        }

        //[Fact]
        public void CheckOutVehicle()
        {
            //Act
            var result = valuesController.Put(1);
            string json = JsonConvert.SerializeObject(result);
            Response response = JsonConvert.DeserializeObject<Response>(json);
            //Assert
            Assert.Equal(200, response.StatusCode);
        }
    }
}
