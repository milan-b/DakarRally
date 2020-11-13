using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class VehicleRepository : RepositoryBase<Vehicle>, IVehicleRepository 
    { 
        public VehicleRepository(RepositoryContext repositoryContext) 
            : base(repositoryContext) 
        { 
        }

        public void SoftDelete(Vehicle vehicle)
        {
            vehicle.IsDeleted = true;
            RepositoryContext.Vehicles.Update(vehicle);
        }

        public IQueryable<Vehicle> LeaderboardForCondition(Expression<Func<Vehicle, bool>> expression)
        {
            return RepositoryContext.Vehicles.Where(expression).Include(o => o.VehicleStatistic)
                .OrderBy(o => o.VehicleStatistic.FinishTime).ThenByDescending(o => o.VehicleStatistic.Distance).AsNoTracking();
        }
    }
}
