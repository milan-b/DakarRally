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


    }
}
