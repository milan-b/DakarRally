using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
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
    }
}
