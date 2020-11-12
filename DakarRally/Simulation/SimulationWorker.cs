using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simulation
{
    public interface ISimulationWorker
    {
        Task SimulateRally(CancellationToken stoppingToken);
    }

    public class SimulationWorker: ISimulationWorker
    {
        private readonly ILogger<SimulationWorker> _logger;
        private readonly IServiceProvider _services;
        public SimulationWorker(ILogger<SimulationWorker> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        public async Task SimulateRally(CancellationToken stoppingToken)
        {
            using (var scope = _services.CreateScope())
            {
                
                var repository = scope.ServiceProvider.GetRequiredService<IRepositoryWrapper>();
                var simulation = repository.Simulation.FindByCondition(o => o.EndTime == null).FirstOrDefault();
                if (simulation == null)
                {
                    _logger.LogInformation("There is no simulation to execute.");
                }
                else
                {
                    var vehicles = repository.Vehicle.FindByCondition(o => o.RaceId == simulation.RaceId)
                                .Include(o => o.VehicleStatistic)
                                .Include(o => o.VehicleType).ToList();
                    while (!stoppingToken.IsCancellationRequested)
                    {

                        _logger.LogInformation("working...");
                        await Task.Delay(2000);
                        
                    }
                }
                
            }
        }
    }
}
