using Entities.DataTransferObjects;
using Entities.Models;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRaceRepository : IRepositoryBase<Race>
    {
        Task<RaceStatusDTO> GetRaceStatus(int raceId);
    }
}
