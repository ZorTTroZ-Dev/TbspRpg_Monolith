using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Entities;

namespace TbspRpgDataLayer.Repositories
{
    public interface ILocationsRepository : IBaseRepository
    {
        Task<Location> GetInitialForAdventure(Guid adventureId);
        Task<List<Location>> GetLocationsForAdventure(Guid adventureId);
        Task<Location> GetLocationById(Guid locationId);
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

        public Task<List<Location>> GetLocationsForAdventure(Guid adventureId)
        {
            return _databaseContext.Locations.AsQueryable()
                .Where(location => location.AdventureId == adventureId)
                .ToListAsync();
        }

        public Task<Location> GetLocationById(Guid locationId)
        {
            return _databaseContext.Locations.AsQueryable()
                .FirstOrDefaultAsync(location => location.Id == locationId);
        }

        public async Task SaveChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}