using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("race")] 
    public class Race: IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Year is required")]
        public ushort Year { get; set; }
    }
}
