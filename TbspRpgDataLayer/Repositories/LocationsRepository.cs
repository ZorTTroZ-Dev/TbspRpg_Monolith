using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;

namespace TbspRpgDataLayer.Repositories
{
    public interface ILocationsRepository : IBaseRepository
    {
        Task<Location> GetInitialForAdventure(Guid adventureId);
        Task<List<Location>> GetLocationsForAdventure(Guid adventureId);
        Task<Location> GetLocationById(Guid locationId);
        Task<Location> GetLocationByIdWithRoutes(Guid locationId);
        Task AddLocation(Location location);
        void RemoveLocation(Location location);
        void RemoveLocations(ICollection<Location> locations);
        Task<List<Location>> GetLocationsWithScript(Guid scriptId);
        Task<List<Location>> GetAdventureLocationsWithSource(Guid adventureId, Guid sourceKey);
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
                .Include(location => location.Adventure)
                .FirstOrDefaultAsync(location => location.Id == locationId);
        }

        public Task<Location> GetLocationByIdWithRoutes(Guid locationId)
        {
            return _databaseContext.Locations.AsQueryable()
                .Include(location => location.Adventure)
                .Include(location => location.Routes)
                .FirstOrDefaultAsync(location => location.Id == locationId);
        }

        public async Task AddLocation(Location location)
        {
            await _databaseContext.AddAsync(location);
        }

        public void RemoveLocation(Location location)
        {
            _databaseContext.Remove(location);
        }

        public void RemoveLocations(ICollection<Location> locations)
        {
            _databaseContext.RemoveRange(locations);
        }

        public Task<List<Location>> GetLocationsWithScript(Guid scriptId)
        {
            return _databaseContext.Locations.AsQueryable()
                .Where(location => location.EnterScriptId == scriptId || location.ExitScriptId == scriptId)
                .ToListAsync();
        }

        public Task<List<Location>> GetAdventureLocationsWithSource(Guid adventureId, Guid sourceKey)
        {
            return _databaseContext.Locations.AsQueryable()
                .Where(location => location.AdventureId == adventureId && location.SourceKey == sourceKey)
                .ToListAsync();
        }

        public async Task SaveChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}