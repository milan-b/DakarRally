using Contracts;
using Entities;
using Entities.DataTransferObjects;
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

        public IQueryable<Vehicle> FindVehicle(FindVehicleParams findVehicleParams)
        {
            return RepositoryContext.Vehicles.Where(v =>
                (string.IsNullOrEmpty(findVehicleParams.Team) || string.Equals(v.TeamName.ToLower(), findVehicleParams.Team.ToLower())) &&
                (string.IsNullOrEmpty(findVehicleParams.Model) || string.Equals(v.Model.ToLower(), findVehicleParams.Model.ToLower())) &&
                (string.IsNullOrEmpty(findVehicleParams.Status) || string.Equals(v.VehicleStatistic.Status.ToLower(), findVehicleParams.Status.ToLower())) &&
                (findVehicleParams.ManucaturingDate == null || v.ManucaturingDate == findVehicleParams.ManucaturingDate) &&
                (findVehicleParams.Distance == null || ((int)v.VehicleStatistic.Distance) == findVehicleParams.Distance)
            );
        }

    }
}
