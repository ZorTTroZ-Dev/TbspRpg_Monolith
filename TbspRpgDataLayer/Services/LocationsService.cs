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
        Task AddLocation(Location location);
        void RemoveLocation(Location location);
        void RemoveLocations(ICollection<Location> locations);
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

        public async Task SaveChanges()
        {
            await _locationsRepository.SaveChanges();
        }
    }
}