using Entities.Models;

namespace Contracts
{
    public interface IVehicleRepository : IRepositoryBase<Vehicle>
    {
        void SoftDelete(Vehicle vehicle);
    }
}
