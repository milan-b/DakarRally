using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DataTransferObjects
{
    public class VehicleDTO : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Team name is required")]
        public string TeamName { get; set; }

        [Required(ErrorMessage = "Model is required")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Manucaturing date is required")]
        public DateTime ManucaturingDate { get; set; }

        [Required(ErrorMessage = "Vehicle type is required")]
        public string VehicleType { get; set; }

        [Required(ErrorMessage = "RaceId is required")]
        public int? RaceId { get; set; }

    }
}
