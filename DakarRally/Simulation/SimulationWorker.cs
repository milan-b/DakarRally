using Contracts;
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
                while (!stoppingToken.IsCancellationRequested)
                {

                    _logger.LogInformation("working...");
                    await Task.Delay(2000);
                    //repository.Simulation.FindByCondition(o => o.EndTime == null).FirstOrDefault().EndTime = DateTime.Now;
                    //return;
                }
            }
        }
    }
}
