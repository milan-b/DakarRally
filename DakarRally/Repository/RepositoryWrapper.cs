using Contracts;
using Entities;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper 
    { 
        private RepositoryContext _repoContext; 
        private IVehicleRepository _vehicle; 
        private IVehicleTypeRepository _vehicleType;
        private IVehicleStatisticRepository _vehicleStatistic;
        private IRaceRepository _race;
        private ISimulationRepository _simulation;

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }


        public IVehicleRepository Vehicle 
        { 
            get 
            { 
                if (_vehicle == null) 
                {
                    _vehicle = new VehicleRepository(_repoContext); 
                } 
                return _vehicle; 
            } 
        }
        public IVehicleTypeRepository VehicleType
        {
            get
            {
                if (_vehicleType == null)
                {
                    _vehicleType = new VehicleTypeRepository(_repoContext);
                }
                return _vehicleType;
            }
        }
        public IVehicleStatisticRepository VehicleStatistic
        {
            get
            {
                if (_vehicleStatistic == null)
                {
                    _vehicleStatistic = new VehicleStatisticRepository(_repoContext);
                }
                return _vehicleStatistic;
            }
        }

        public IRaceRepository Race
        {
            get
            {
                if (_race == null)
                {
                    _race = new RaceRepository(_repoContext);
                }
                return _race;
            }
        }
        public ISimulationRepository Simulation
        {
            get
            {
                if (_simulation == null)
                {
                    _simulation = new SimulationRepository(_repoContext);
                }
                return _simulation;
            }
        }
        
        public async Task SaveAsync() 
        {
            await _repoContext.SaveChangesAsync();
        } 
    }
}
