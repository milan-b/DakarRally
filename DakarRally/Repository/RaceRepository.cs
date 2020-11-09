using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class RaceRepository : RepositoryBase<Race>, IRaceRepository 
    { 
        public RaceRepository(RepositoryContext repositoryContext) 
            : base(repositoryContext) 
        { 
        } 
    }
}
