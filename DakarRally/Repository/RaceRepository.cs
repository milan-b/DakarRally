using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Repository
{
    public class RaceRepository : RepositoryBase<Race>, IRaceRepository 
    { 
        public RaceRepository(RepositoryContext repositoryContext) 
            : base(repositoryContext) 
        { 

        }
        public async Task<RaceStatusDTO> GetRaceStatus(int raceId)
        {
            var raceStatus = "Running";
            var simulation = await RepositoryContext.Simulations.Where(o => o.RaceId == raceId).FirstOrDefaultAsync();
            if(simulation == null)
            {
                raceStatus = "Pending";
            }else if(simulation.EndTime != null)
            {
                raceStatus = "Finished";
            }
            
            var vehicles = RepositoryContext.Vehicles.Where(v => v.RaceId == raceId);

            var vehiclesBroupedByStatus = await vehicles.GroupBy(o => o.VehicleStatistic.Status)
                .Select(o => new KeyValueDTO { Key = o.Key, Value = o.Count()})
                .OrderBy(o => o.Value).ToListAsync();

            var vehiclesBroupedByType = await vehicles.GroupBy(o => o.VehicleTypeName)
                .Select(o => new KeyValueDTO { Key = o.Key, Value = o.Count() })
                .OrderBy(o => o.Value).ToListAsync();


            return new RaceStatusDTO {
                Status = raceStatus,
                VeciclesGoupedByStatus = vehiclesBroupedByStatus,
                VeciclesGoupedByType = vehiclesBroupedByType
            };
        }

    }
}
