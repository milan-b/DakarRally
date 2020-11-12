using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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
        public readonly SimulationConfiguration _simulationConfiguration;
        public SimulationWorker(ILogger<SimulationWorker> logger, IServiceProvider services, IConfiguration configuration)
        {
            _logger = logger;
            _services = services;
            configuration.GetSection(SimulationConfiguration.Simulation).Bind(_simulationConfiguration);
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
                        var iterationStarted = DateTime.Now;

                        //do work

                        var executionTime = (DateTime.Now - iterationStarted).TotalMilliseconds;
                        if (executionTime > _simulationConfiguration.DeadlineForRealTime)
                        {
                            _logger.LogError($"Real-time deadline is not meet.\n" +
                                $"Current iteration lasted for {executionTime}ms.\n" +
                                $"Real-time deadline is {_simulationConfiguration.DeadlineForRealTime}");
                        }
                        else
                        {
                            await Task.Delay(_simulationConfiguration.DeadlineForRealTime - (int)Math.Ceiling(executionTime));
                        }
                    }
                }
                
            }
        }
    }
}
