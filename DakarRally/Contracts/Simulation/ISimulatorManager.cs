using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Contracts.Simulation
{
    public interface ISimulatorManager
    {
        bool StartSimulation(int raceId, out string message);

        Task SignalStart(CancellationToken cancellationToken);
    }

}
