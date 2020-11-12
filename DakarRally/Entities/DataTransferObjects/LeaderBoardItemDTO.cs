using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DataTransferObjects
{
    public class LeaderbordItemDTO
    {
        public int Position { get; set; }
        public double Distance { get; set; }

        public ushort Malfunctions { get; set; }

        public string Status { get; set; }

        public DateTime? FinishTime { get; set; }

        public string TeamName { get; set; }

        public string Model { get; set; }

        public string Type { get; set; }

    }
}
