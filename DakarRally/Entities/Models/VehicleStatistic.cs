﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("vehicle_statistic")] 
    public class VehicleStatistic : IEntity
    {
        public int Id { get; set; }

        [DefaultValue(0)]
        public double Distance { get; set; }

        [DefaultValue(0)]
        public ushort Malfunctions { get; set; }

        public string Status { get; set; }

        [ForeignKey(nameof(Vehicle))]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        
    }
}