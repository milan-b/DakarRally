using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("simulation")]
    public class Simulation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Required]
        public DateTime LastUpdate { get; set; }



        [Required]
        [ForeignKey(nameof(Race))]
        public int RaceId { get; set; }
        [Required]
        public Race Race { get; set; }

    }
}
