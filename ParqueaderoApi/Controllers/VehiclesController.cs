namespace ParqueaderoApi.Controllers
{
    using BusinessLayer.BusinessLogic;
    using BusinessLayer.Interfaces;
    using DataAccess.Models;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Route("api/[controller]")]
    public class VehiclesController : Controller
    {
        #region Attributes
        private IVehicleBusinessLogic vehicleBusinessLogic;
        private Vehicle vehicleModel;
        private Response response;
        private string errorMessage;
        #endregion

        #region Inicializer
        private void Inicializer()
        {
            vehicleModel = new Vehicle();
            vehicleBusinessLogic = new VehicleBusinessLogic();
            response = new Response();
            errorMessage = null;
        }
        #endregion

        #region EndPoints
        // GET api/vehicles
        [HttpGet]
        public IActionResult Get()
        {
            Inicializer();
            var allVehicles = vehicleBusinessLogic.GetAllActiveVehicles();
            if (allVehicles != null)
            {
                return Ok(JsonConvert.SerializeObject(allVehicles, Formatting.Indented));
            }
            else
            {
                response.BadRequest(errorMessage);
                return BadRequest(JsonConvert.SerializeObject(response, Formatting.Indented));
            }
        }

        //GET api/vehicles/vehicle?type=car
        //GET api/vehicles/vehicle?type=motorcycle
        [HttpGet("vehicle")]
        public IActionResult Get(string type)
        {
            Inicializer();
            var allVehicles = vehicleBusinessLogic.GetAllActiveVehicles(type);
            if (allVehicles != null)
            {
                return Ok(JsonConvert.SerializeObject(allVehicles, Formatting.Indented));
            }
            else
            {
                response.BadRequest(errorMessage);
                return BadRequest(JsonConvert.SerializeObject(response, Formatting.Indented));
            }
        }

        // GET api/vehicles/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Inicializer();
            var vehicle = vehicleBusinessLogic.GetVehicle(id);
            if (vehicle != null)
            {
                return Ok(JsonConvert.SerializeObject(vehicle, Formatting.Indented));
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/vehicles
        [HttpPost]
        public IActionResult Post([FromBody] Vehicle vehicle)
        {
            Inicializer();
            if (vehicleBusinessLogic.TryCheckInVehicle(out errorMessage,vehicle))
            {
                return Ok();
            }
            else
            {
                response.BadRequest(errorMessage);
                return BadRequest(JsonConvert.SerializeObject(response, Formatting.Indented));
            }
        }

        // PUT api/vehicles/5
        [HttpPut("{id}")]
        public IActionResult Put(int id)
        {
            Inicializer();
            Vehicle vehicle = new Vehicle();
            vehicle.Id = id;
            
            if (vehicleBusinessLogic.GetVehicleCheckOut(ref vehicle,out errorMessage))
            {
                return Ok(JsonConvert.SerializeObject(vehicle, Formatting.Indented));
            }
            else
            {
                response.BadRequest(errorMessage);
                return BadRequest(JsonConvert.SerializeObject(response, Formatting.Indented));
            }
        }
        
        #endregion
    }
}
