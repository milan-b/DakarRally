using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryWrapper 
    { 
        IVehicleRepository Vehicle { get; } 
        IVehicleTypeRepository VehicleType { get; }
        IVehicleStatisticRepository VehicleStatistic { get; }
        IRaceRepository Race { get; }
        ISimulationRepository Simulation { get; }

        Task SaveAsync(); 
    }
}
