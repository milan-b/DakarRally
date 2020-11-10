using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Contracts;
using DakarRally.ActionFilters;
using Entities.DataTransferObjects;
using Entities.Extensions;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace DakarRally.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RallyController : ControllerBase
    {

        private IRepositoryWrapper _repository;
        private readonly ILogger<RallyController> _logger;

        public RallyController(ILogger<RallyController> logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRace([FromBody] RaceDTO raceDTO)
        {
            var race = raceDTO.ToDAO();

            _repository.Race.Create(race);
            await _repository.SaveAsync();
            return Created("",race.ToDTO());
        }

        [HttpPost]
        [ServiceFilter(typeof(VehicleValidationFilter))]
        public async Task<IActionResult> AddVehicleToRace([FromBody] VehicleDTO vehicleDTO)
        {
            var vehicle = vehicleDTO.ToDAO();
            _repository.Vehicle.Create(vehicle);
            await _repository.SaveAsync();
            return Created("", vehicle.ToDTO());
        }

        [HttpPut]
        [ServiceFilter(typeof(VehicleValidationFilter))]
        public async Task<IActionResult> UpdateVehicle([FromBody] VehicleDTO vehicleDTO)
        {
            var vehicle = _repository.Vehicle.FindByCondition(o => o.Id == vehicleDTO.Id).FirstOrDefault();
            if(vehicle == null)
            {
                return NotFound($"Vehicle with id:{vehicleDTO.Id} does not exist.");
            }
            vehicle.Map(vehicleDTO.ToDAO());
            _repository.Vehicle.Update(vehicle);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> RemoveVehicle(int id)
        {
            var vehicle = _repository.Vehicle.FindByCondition(o => o.Id == id).FirstOrDefault();
            if (vehicle == null)
            {
                return NotFound($"Vehicle with id:{id} does not exist.");
            }
            _repository.Vehicle.SoftDelete(vehicle);
            await _repository.SaveAsync();
            return NoContent();
        }


    }
}
