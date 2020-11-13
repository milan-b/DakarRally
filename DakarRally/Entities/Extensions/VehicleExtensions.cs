using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Entities.Extensions
{
    public static class VehicleExtensions
    {
        public static VehicleDTO ToDTO(this Vehicle vehicle)
        {
            return new VehicleDTO
            {
                Id = vehicle.Id,
                RaceId = vehicle.RaceId,
                ManucaturingDate = vehicle.ManucaturingDate,
                Model = vehicle.Model,
                TeamName = vehicle.TeamName,
                VehicleType = vehicle.VehicleTypeName
            };
        }

        public static Vehicle ToDAO(this VehicleDTO vehicleDTO)
        {
            return new Vehicle
            {
                Id = vehicleDTO.Id,
                RaceId = (int)vehicleDTO.RaceId,
                ManucaturingDate = vehicleDTO.ManucaturingDate,
                Model = vehicleDTO.Model,
                TeamName = vehicleDTO.TeamName,
                VehicleTypeName = vehicleDTO.VehicleType
            };
        }

        public static void Map(this Vehicle dbVehicle, Vehicle vehicle)
        {
            dbVehicle.Model = vehicle.Model;
            dbVehicle.RaceId = vehicle.RaceId;
            dbVehicle.VehicleTypeName = vehicle.VehicleTypeName;
            dbVehicle.TeamName = vehicle.TeamName;
            dbVehicle.ManucaturingDate = vehicle.ManucaturingDate;
        }

        public static VehicleStatisticDTO ToStatisticDTO(this Vehicle vehicle)
        {
            return new VehicleStatisticDTO
            {
                Distance = vehicle.VehicleStatistic.Distance,
                Malfunctions = vehicle.VehicleStatistic.Malfunctions,
                Status = vehicle.VehicleStatistic.Status,
                FinishTime = vehicle.VehicleStatistic.FinishTime
            };
        }

        public static LeaderbordItemDTO ToLeaderboardDTO(this Vehicle vehicle, int position)
        {
            return new LeaderbordItemDTO
            {
                Position = position,
                Distance = vehicle.VehicleStatistic.Distance,
                Malfunctions = vehicle.VehicleStatistic.Malfunctions,
                Status = vehicle.VehicleStatistic.Status,
                FinishTime = vehicle.VehicleStatistic.FinishTime,
                TeamName = vehicle.TeamName,
                Model = vehicle.Model,
                Type = vehicle.VehicleTypeName
            };
        }

        public static IQueryable<Vehicle> Sort(this IQueryable<Vehicle> vehicles, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return vehicles.OrderBy(e => e.Model);

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Vehicle).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name} {direction}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            if (string.IsNullOrWhiteSpace(orderQuery))
                return vehicles.OrderBy(e => e.Model);

            return vehicles.OrderBy(orderQuery);
        }



    }
}
