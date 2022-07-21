using System;
using System.Collections.Generic;
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
        void RemoveScriptFromLocations(Guid scriptId);
        Task<bool> DoesAdventureLocationUseSource(Guid adventureId, Guid sourceKey);
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

        public async void RemoveScriptFromLocations(Guid scriptId)
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

        public Task<bool> DoesAdventureLocationUseSource(Guid adventureId, Guid sourceKey)
        {
            throw new NotImplementedException();
        }

        public async Task SaveChanges()
        {
            await _locationsRepository.SaveChanges();
        }
    }
}