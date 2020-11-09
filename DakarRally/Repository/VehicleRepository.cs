using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class VehicleRepository : RepositoryBase<Vehicle>, IVehicleRepository 
    { 
        public VehicleRepository(RepositoryContext repositoryContext) 
            : base(repositoryContext) 
        { 
        } 
    }
}
