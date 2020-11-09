using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class SimulationRepository : RepositoryBase<Simulation>, ISimulationRepository
    { 
        public SimulationRepository(RepositoryContext repositoryContext) 
            : base(repositoryContext) 
        { 
        } 
    }
}
