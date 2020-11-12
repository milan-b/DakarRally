using System.Threading;
using System.Threading.Tasks;

namespace Contracts.Simulation
{
    public interface ISimulationWorker
    {
        Task SimulateRally(CancellationToken stoppingToken);
    }
}
