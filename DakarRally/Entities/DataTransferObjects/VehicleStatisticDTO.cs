using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DataTransferObjects
{
    public class VehicleStatisticDTO
    {
        public double Distance { get; set; }

        public ushort Malfunctions { get; set; }

        public string Status { get; set; }

        public DateTime? FinishTime { get; set; }

    }
}
