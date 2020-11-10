using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DataTransferObjects
{
    public class RaceDTO: IEntity
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Year is required")]
        public ushort? Year { get; set; }
    }
}
