using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Contracts
{
    public interface IVehicleRepository : IRepositoryBase<Vehicle>
    {
        void SoftDelete(Vehicle vehicle);
        IQueryable<Vehicle> LeaderboardForCondition(Expression<Func<Vehicle, bool>> expression);
        IQueryable<Vehicle> FindVehicle(FindVehicleParams findVehicleParams);
    }
}
