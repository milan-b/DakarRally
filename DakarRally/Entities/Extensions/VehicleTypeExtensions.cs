using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Extensions
{
    public static class VehicleTypeExtensions
    {
        public static void SeedVehicleTypes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleType>().HasData(
                new VehicleType
                {
                    Name = "sportsCar",
                    PercentageOfLightMalfunctionsPerHour = 12,
                    PercentageOfHeavyMalfunctionsPerHour = 2,
                    MaxSpeed = 140,
                    RepairmentTimeInHovers = 5,
                    SuperType = "car"
                },
                new VehicleType
                {
                    Name = "terrainCar",
                    PercentageOfLightMalfunctionsPerHour = 3,
                    PercentageOfHeavyMalfunctionsPerHour = 1,
                    MaxSpeed = 100,
                    RepairmentTimeInHovers = 5,
                    SuperType = "car"
                },
                new VehicleType
                {
                    Name = "truck",
                    PercentageOfLightMalfunctionsPerHour = 6,
                    PercentageOfHeavyMalfunctionsPerHour = 4,
                    MaxSpeed = 80,
                    RepairmentTimeInHovers = 7,
                    SuperType = "truck"
                },
                new VehicleType
                {
                    Name = "crossMotorcycle",
                    PercentageOfLightMalfunctionsPerHour = 3,
                    PercentageOfHeavyMalfunctionsPerHour = 2,
                    MaxSpeed = 85,
                    RepairmentTimeInHovers = 3,
                    SuperType = "motorcycle"
                },
                new VehicleType
                {
                    Name = "sportMotorcycle",
                    PercentageOfLightMalfunctionsPerHour = 18,
                    PercentageOfHeavyMalfunctionsPerHour = 10,
                    MaxSpeed = 130,
                    RepairmentTimeInHovers = 3,
                    SuperType = "motorcycle"
                }
                );
        }

    }
}
