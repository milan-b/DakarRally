using Entities.Enums;
using Entities.Validation;
using System;
using System.Linq;

namespace Entities.DataTransferObjects
{
    public class FindVehicleParams
    {
        public string Team { get; set; }
        public string Model { get; set; }
        public DateTime? ManucaturingDate { get; set; }

        [StringRange(AllowableValues = new[] { "Running", "Broken", "Finished", "ReadyToStart", "UnderRepair" })]
        public string Status { get; set; }

        public int? Distance { get; set; }

        public string OrderBy { get; set; } = "Model";

    }
}
