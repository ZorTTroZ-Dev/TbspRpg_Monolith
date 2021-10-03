using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Repositories;

namespace TbspRpgDataLayer.Services
{
    public interface ILocationsService : IBaseService
    {
        Task<Location> GetInitialLocationForAdventure(Guid adventureId);
        Task<List<Location>> GetLocationsForAdventure(Guid adventureId);
        Task<Location> GetLocationById(Guid locationId);
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

        public async Task SaveChanges()
        {
            await _locationsRepository.SaveChanges();
        }
    }
}