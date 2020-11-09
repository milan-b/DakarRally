using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class VehicleStatisticRepository : RepositoryBase<VehicleStatistic>, IVehicleStatisticRepository
    { 
        public VehicleStatisticRepository(RepositoryContext repositoryContext) 
            : base(repositoryContext) 
        { 
        } 
    }
}
