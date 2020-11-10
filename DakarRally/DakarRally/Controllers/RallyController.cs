using System;
using System.Collections.Generic;
using System.Linq;
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
            await _repository.Save();
            return Created("",race.ToDTO());
        }

        [HttpPost]
        public async Task<IActionResult> AddVehicleToRace([FromBody] VehicleDTO vehicleDTO)
        {
            if(!_repository.Race.FindAll().Any(o => o.Id == vehicleDTO.RaceId))
            {
                return BadRequest($"Race with id:{vehicleDTO.RaceId} does not exist.");
            }
            if(_repository.Simulation.FindAll().Any(o => o.RaceId == vehicleDTO.RaceId))
            {
                return BadRequest("Vehicle can be added only to races that are not started.");
            }
            var vehicleTypes = _repository.VehicleType.FindAll().Select(o => o.Name);
            if (!vehicleTypes.Any(n => n == vehicleDTO.VehicleType))
            {
                return BadRequest($"Bad vehicle type! \nAvailable vehicle types are:\n\n{vehicleTypes.Join(",\n")}");
            }

            var vehicle = vehicleDTO.ToDAO();
            _repository.Vehicle.Create(vehicle);
            await _repository.Save();
            return Created("", vehicle.ToDTO());
        }


    }
}
