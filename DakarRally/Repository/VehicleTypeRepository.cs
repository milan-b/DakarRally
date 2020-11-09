using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class VehicleTypeRepository : RepositoryBase<VehicleType>, IVehicleTypeRepository 
    { 
        public VehicleTypeRepository(RepositoryContext repositoryContext) 
            : base(repositoryContext) 
        { 
        } 
    }
}
