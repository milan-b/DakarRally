using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Simulation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Simulation
{
    public class HostedService : BackgroundService
    {
        private readonly ILogger<HostedService> _logger;
        private readonly ISimulatorManager _simulationManager;
        private readonly ISimulationWorker _simulationWorker;

        public HostedService(ISimulatorManager simulationManager, ISimulationWorker simulationWorker, ILogger<HostedService> logger)
        {
            _simulationManager = simulationManager;
            _simulationWorker = simulationWorker;
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await SimulateRally(stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                await _simulationManager.SignalStart(stoppingToken);

                await SimulateRally(stoppingToken);
            }
        }

        private async Task SimulateRally(CancellationToken stoppingToken)
        {
            try
            {
                await _simulationWorker.SimulateRally(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing simulation.");
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
