using Contracts;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Simulation
{
    public interface ISimulatorManager
    {
        bool StartSimulation(int raceId, out string message);

        Task SignalStart(CancellationToken cancellationToken);
    }

    public class SimulationManager : ISimulatorManager
    {
        private readonly IServiceProvider _services;
        private static readonly object _synchronizationObject = new object();
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        public SimulationManager(IServiceProvider services)
        {
            _services = services;
        }

        public bool StartSimulation(int raceId, out string message)
        {
            using (var scope = _services.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IRepositoryWrapper>();

                lock (_synchronizationObject)
                {
                    if (repository.Simulation.FindAll().Any(o => o.EndTime == null || o.RaceId == raceId))
                    {
                        message = "Simulation can not be started.\nThere is some simulation that's already running or simulation for this race is " +
                            "already executed.";
                        return false;
                    }
                    else
                    {
                        var simulation = new Entities.Models.Simulation
                        {
                            RaceId = raceId,
                            StartTime = DateTime.Now,
                            LastUpdate = DateTime.Now
                        };
                        repository.Simulation.Create(simulation);
                        repository.SaveAsync().Wait();
                        _signal.Release();

                        message = "Simulation started.";
                        return true;
                    }
                }
                
            }
        }

        public async Task SignalStart(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
        }
    }
}
