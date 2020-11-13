using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace Entities.DataTransferObjects
{
    public class RaceStatusDTO
    {
        public string Status { get; set; }

        public List<KeyValueDTO> VeciclesGoupedByType { get; set; }
        public List<KeyValueDTO> VeciclesGoupedByStatus { get; set; }
    }
}
