using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Extensions
{
    public static class RaceExtensions
    {
        public static RaceDTO ToDTO(this Race race)
        {
            return new RaceDTO
            {
                Id = race.Id,
                Year = race.Year
            };
        }

        public static Race ToDAO(this RaceDTO raceDTO)
        {
            return new Race
            {
                Id = (int)raceDTO.Id,
                Year = (ushort)raceDTO.Year
            };
        }
    }
}
