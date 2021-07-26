using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Entities;

namespace TbspRpgApi.Repositories
{
    public interface ILocationsRepository
    {
        Task<Location> GetInitialForAdventure(Guid adventureId);
    }
    
    public class LocationsRepository: ILocationsRepository
    {
        private readonly DatabaseContext _databaseContext;

        public LocationsRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<Location> GetInitialForAdventure(Guid adventureId)
        {
            return _databaseContext.Locations.AsQueryable()
                .Where(location => location.AdventureId == adventureId && location.Initial)
                .FirstOrDefaultAsync();
        }
    }
}