using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("vehicle_type")] 
    public class VehicleType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public byte MaxSpeed { get; set; }

        [Required]
        public byte RepairmentTimeInHovers { get; set; }

        [Required]
        public byte PercentageOfLightMalfunctionsPerHour { get; set; }

        [Required]
        public byte PercentageOfHeavyMalfunctionsPerHour { get; set; }

        [Required]
        public string SuperType { get; set; }


    }
}
