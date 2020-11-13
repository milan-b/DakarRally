using System;

namespace Entities.DataTransferObjects
{
    public class FindVehicleParams
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        public string Team { get; set; }
        public string Model { get; set; }
        public DateTime? ManucaturingDate { get; set; }
        public string Status { get; set; }

        public int? Distance { get; set; }
        public string OrderBy { get; set; } = "team";

        public string SortOrder { get; set; }
    }
}
