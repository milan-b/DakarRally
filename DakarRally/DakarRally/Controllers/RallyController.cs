using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Contracts;
using Contracts.Simulation;
using DakarRally.ActionFilters;
using Entities.DataTransferObjects;
using Entities.Extensions;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Simulation;

namespace DakarRally.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RallyController : ControllerBase
    {

        private IRepositoryWrapper _repository;
        private readonly ILogger<RallyController> _logger;
        private readonly ISimulatorManager _simulationManager;

        public RallyController(ILogger<RallyController> logger, IRepositoryWrapper repository, ISimulatorManager simulationManager)
        {
            _logger = logger;
            _repository = repository;
            _simulationManager = simulationManager;
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
            vehicle.VehicleStatistic = new VehicleStatistic();
            _repository.Vehicle.Create(vehicle);
            await _repository.SaveAsync();
            return Created("", vehicle.ToDTO());
        }

        [HttpPut]
        [ServiceFilter(typeof(VehicleValidationFilter))]
        public async Task<IActionResult> UpdateVehicle([FromBody] VehicleDTO vehicleDTO)
        {
            var vehicle = await _repository.Vehicle.FindByCondition(o => o.Id == vehicleDTO.Id).FirstOrDefaultAsync();
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
            var vehicle = await _repository.Vehicle.FindByCondition(o => o.Id == id).FirstOrDefaultAsync();
            if (vehicle == null)
            {
                return NotFound($"Vehicle with id:{id} does not exist.");
            }
            _repository.Vehicle.SoftDelete(vehicle);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> StartRace(int id)
        {
            
            var race = await _repository.Race.FindByCondition(o => o.Id == id).FirstOrDefaultAsync();
            if (race == null)
            {
                return NotFound($"Race with id:{id} does not exist.");
            }
            if (_repository.Simulation.FindAll().Any(o => o.RaceId == id))
            {
                return BadRequest("This race was already simulated.");
            }
            string message;
            if (_simulationManager.StartSimulation(id, out message))
            {
                return Ok(message);
            }
            else
            {
                return BadRequest(message);
            };
        }

        [HttpGet]
        [ServiceFilter(typeof(ActiveRaceValidationFilter))]
        public async Task<IActionResult> GetLeaderboard()
        {
            var simulation = HttpContext.Items["simulation"] as Entities.Models.Simulation;
            var vehicles = await _repository.Vehicle.FindByCondition(o => o.RaceId == simulation.RaceId).Include(o => o.VehicleStatistic)
                .OrderBy(o => o.VehicleStatistic.FinishTime).ThenByDescending(o => o.VehicleStatistic.Distance).ToListAsync();
            return Ok(GetLeaderbordPresentationHelper(vehicles));
        }

        [HttpGet("{superType}")]
        [ServiceFilter(typeof(ActiveRaceValidationFilter))]
        public async Task<IActionResult> GetLeaderboard(string superType)
        {
            var superTypes = await _repository.VehicleType.FindAll().Select(o => o.SuperType).Distinct().ToListAsync();
            if (!superTypes.Any(n => n == superType))
            {
                return BadRequest($"Bad vehicle super type! \nAvailable vehicle super types are:\n\n{superTypes.Join(",\n")}");
            }
            var simulation = HttpContext.Items["simulation"] as Entities.Models.Simulation;
            var vehicles = await _repository.Vehicle.FindByCondition(o => o.RaceId == simulation.RaceId && o.VehicleType.SuperType == superType)
                .Include(o => o.VehicleStatistic).OrderBy(o => o.VehicleStatistic.FinishTime).ThenByDescending(o => o.VehicleStatistic.Distance)
                .ToListAsync();
            return Ok(GetLeaderbordPresentationHelper(vehicles));
        }

        [HttpGet("{vehicleId:int:min(1)}")]
        public async Task<IActionResult> GetVehicleStatistics(int vehicleId)
        {
            var vehicle = await _repository.Vehicle.FindByCondition(o => o.Id == vehicleId).Include(o => o.VehicleStatistic).FirstOrDefaultAsync();
            if(vehicle == null)
            {
                return BadRequest($"Vehicle with id:{vehicleId} does not exist.");
            }
            
            return Ok(vehicle.ToStatisticDTO());
        }


        #region Helpers

        private List<LeaderbordItemDTO> GetLeaderbordPresentationHelper(List<Vehicle> vehicles)
        {
            return vehicles.Select((item, index) => item.ToLeaderboardDTO(index + 1)).ToList();
        }
        #endregion


    }
}
