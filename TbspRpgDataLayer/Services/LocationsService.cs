using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Repositories;

namespace TbspRpgDataLayer.Services
{
    public interface ILocationsService : IBaseService
    {
        Task<Location> GetInitialLocationForAdventure(Guid adventureId);
        Task<List<Location>> GetLocationsForAdventure(Guid adventureId);
        Task<Location> GetLocationById(Guid locationId);
        Task<Location> GetLocationByIdWithRoutes(Guid locationId);
        Task AddLocation(Location location);
        void RemoveLocation(Location location);
        void RemoveLocations(ICollection<Location> locations);
        Task RemoveScriptFromLocations(Guid scriptId);
        Task<bool> DoesAdventureLocationUseSource(Guid adventureId, Guid sourceKey);
        Task<List<Location>> GetAdventureLocationsWithSource(Guid adventureId, Guid sourceKey);
        void AttachLocation(Location location);
        Task<Location> GetLocationByIdWithObjects(Guid locationId);
    }
    
    public class LocationsService : ILocationsService
    {
        private readonly ILocationsRepository _locationsRepository;
        private readonly ILogger<LocationsService> _logger;

        public LocationsService(
            ILocationsRepository locationsRepository,
            ILogger<LocationsService> logger)
        {
            _locationsRepository = locationsRepository;
            _logger = logger;
        }

        public Task<Location> GetInitialLocationForAdventure(Guid adventureId)
        {
            return _locationsRepository.GetInitialForAdventure(adventureId);
        }

        public Task<List<Location>> GetLocationsForAdventure(Guid adventureId)
        {
            return _locationsRepository.GetLocationsForAdventure(adventureId);
        }

        public Task<Location> GetLocationById(Guid locationId)
        {
            return _locationsRepository.GetLocationById(locationId);
        }

        public Task<Location> GetLocationByIdWithRoutes(Guid locationId)
        {
            return _locationsRepository.GetLocationByIdWithRoutes(locationId);
        }

        public async Task AddLocation(Location location)
        {
            await _locationsRepository.AddLocation(location);
        }

        public void RemoveLocation(Location location)
        {
            _locationsRepository.RemoveLocation(location);
        }

        public void RemoveLocations(ICollection<Location> locations)
        {
            _locationsRepository.RemoveLocations(locations);
        }

        public async Task RemoveScriptFromLocations(Guid scriptId)
        {
            var locations = await _locationsRepository.GetLocationsWithScript(scriptId);
            foreach (var location in locations)
            {
                if (location.EnterScriptId == scriptId)
                    location.EnterScriptId = null;
                if (location.ExitScriptId == scriptId)
                    location.ExitScriptId = null;
            }
        }

        public async Task<bool> DoesAdventureLocationUseSource(Guid adventureId, Guid sourceKey)
        {
            var locations = await GetAdventureLocationsWithSource(adventureId, sourceKey);
            return locations.Any();
        }

        public Task<List<Location>> GetAdventureLocationsWithSource(Guid adventureId, Guid sourceKey)
        {
            return _locationsRepository.GetAdventureLocationsWithSource(adventureId, sourceKey);
        }

        public void AttachLocation(Location location)
        {
            _locationsRepository.AttachLocation(location);
        }

        public Task<Location> GetLocationByIdWithObjects(Guid locationId)
        {
            return _locationsRepository.GetLocationByIdWithObjects(locationId);
        }

        public async Task SaveChanges()
        {
            await _locationsRepository.SaveChanges();
        }
    }
}