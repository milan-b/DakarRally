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
                VehicleType = vehicle.VehicleTypeId
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
                VehicleTypeId = vehicleDTO.VehicleType
            };
        }
    }
}
