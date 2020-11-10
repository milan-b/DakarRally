using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("vehicle")] 
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Team name is required")]
        public string TeamName { get; set; }

        [Required(ErrorMessage = "Model is required")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Manucaturing date is required")]
        public DateTime ManucaturingDate { get; set; }

        [Required(ErrorMessage = "Date created is required")] 
        public DateTime DateCreated { get; set; }

        [Required]
        [ForeignKey(nameof(VehicleType))]
        public int VehicleTypeId { get; set; }
        [Required]
        public VehicleType VehicleType { get; set; }

        [Required]
        [ForeignKey(nameof(Race))]
        public int RaceId { get; set; }
        [Required]
        public Race Race { get; set; }

        public VehicleStatistic VehicleStatistic { get; set; }


        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
